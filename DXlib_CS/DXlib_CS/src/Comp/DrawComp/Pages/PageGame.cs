using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;


using DXlib_CS.src.Comp.DrawComp.Image;
using DXlib_CS.src.Comp.DrawComp.Object;
using DXlib_CS.src.Comp.DrawComp.Object.Effect;

namespace DXlib_CS.src.Comp.DrawComp.Pages
{
    class PageGame : Page
    {

        /// <summary>
        /// ブロック崩しの棒
        /// </summary>
        GameObject bar;

        /// <summary>
        /// 棒の移動速度
        /// </summary>
        double barSpeed;

        //各種判定
        bool isBarMoveLeft;
        bool isBarMoveRight;

        /// <summary>
        /// ボール
        /// </summary>
        Ball ball;



        /// <summary>
        /// ブロック
        /// </summary>
        GameObject[,] block;

        /// <summary>
        /// 背景
        /// </summary>
        ImageBox back;

        /// <summary>
        /// ミノ。この配列の0番は必ずミノの中心（回転の起点）とする。
        /// </summary>
        Tetrimino[] mino;

        /// <summary>
        /// 現在のミノの形
        /// </summary>
        Tetrimino.Type nowType;

        /// <summary>
        /// ミノの色
        /// </summary>
        Color minoColor;


        /// <summary>
        /// 落下地点予測用のミノ
        /// </summary>
        Tetrimino[] ghostMino;


        /// <summary>
        /// ０：初期状態　１：右に90度　２：右に180度　３：右に270度
        /// </summary>
        int numMinoSpin;
        public int NumMinoSpin {
            get {
                return numMinoSpin;
            }

            set {
                if(value >= 4)
                    numMinoSpin = 0;
                else
                    numMinoSpin = value;
            }
        }

        //ブロックの最大値
        int maxBlockX = 10;
        int maxBlockY = 20;

        /// <summary>
        /// 乱数生成
        /// </summary>
        Random rand;

        //各種判定
        bool isMinoMoveLeft;
        bool isMinoMoveRight;
        bool isMinoSpin;
        bool isMinoQuickDrop;

        /// <summary>
        /// ゲームオーバーかどうか
        /// </summary>
        bool isGameOver;

        /// <summary>
        /// ゲームオーバー用のフォントハンドル
        /// </summary>
        int fontHandleGameOver;

        /// <summary>
        /// ゲームオーバーの時のスコア用のフォントハンドル
        /// </summary>
        int fontHandleGameOverScore;

        /// <summary>
        /// スコア用のフォントハンドル
        /// </summary>
        int fontHandleScore;

        /// <summary>
        /// せつめいのフォントハンドル
        /// </summary>
        int fontHandleHowToUse;

        int scoreTetris;
        int scoreBreakout;
        int comboBreakout;


        /// <summary>
        /// エフェクト表示
        /// </summary>
        List<EffectBreakBlock> effectBlock;
        

        public PageGame(){

        }

        ~PageGame(){

        }

        public override void Init() {

            base.Init();
            this.LoadResource();


            //難易度
            Difficulty.Init();


            
            /*ブロック関係*/
            double blockSize = 35;

            block = new Block[maxBlockY , maxBlockX];
            for(int y = 0 ; y < maxBlockY ; y++) {
                for(int x = 0 ; x < maxBlockX ; x++) {
                    DxImage image;
                    if(y == 0) {
                        image = new ImageBox(blockSize , blockSize , new Color(255 , 0 , 0) , new Color(0 , 0 , 0) , 1);
                    } else {
                        image = new ImageBox(blockSize , blockSize , new Color(255 , 255 , 255) , new Color(0 , 0 , 0) , 1);
                    }
                    //posX = 画面の半分　- ブロックの大きさ * 設置する個数の半分　+ 中心座標ぶん移動 + 描画座標
                    double blockX = 512 - 35 * 5 + 35 / 2 + x * blockSize;
                    double blockY = 768 - 35 * 20 + 35 / 2 + y * blockSize;
                    block[y , x] = new Block(blockX , blockY , image);
                }
            }



            /*背景*/
            back = new ImageBox(512, 768 / 2, blockSize * maxBlockX , 768 , new Color(255 , 255 , 255) , new Color(0 , 0 , 0) , 1);


            /*ミノ*/
            mino = new Tetrimino[4];
            ghostMino = new Tetrimino[4];
            for(int i = 0 ; i < mino.Length ; i++) {
                mino[i] = new Tetrimino();
                ghostMino[i] = new Tetrimino();

            }

            minoColor = new Color(0 , 0 , 0);

            rand = new Random(globalTimer.Elapsed.Seconds);


            minoGeneration();


            isMinoMoveLeft = false;
            isMinoMoveRight = false;
            isMinoSpin = false;
            isMinoQuickDrop = false;

            effectBlock = new List<EffectBreakBlock>();

            isGameOver = false;
            fontHandleGameOver = DX.CreateFontToHandle(null , 64 , 3);
            fontHandleGameOverScore = DX.CreateFontToHandle(null , 100 , 5);
            fontHandleScore = DX.CreateFontToHandle(null , 25 , 2);
            fontHandleHowToUse = DX.CreateFontToHandle(null , 32 , 2);

            scoreTetris = 0;
            scoreBreakout = 0;
            comboBreakout = 0;


            /*棒*/
            {
                DxImage image = new ImageBox(blockSize*3.0 , blockSize*0.7 , new Color(255 , 255 , 255) , new Color(0 , 0 , 0) , 2);               
                bar = new Bar(512,30,image,337,687);

                isBarMoveLeft = false;
                isBarMoveRight = false;
                barSpeed = 5;
            }

            /*たま*/
            {
                DxImage image = new ImageCircle(blockSize / 2.5 , new Color(255 , 255 , 0) , new Color(0 , 0 , 0) , 2);
                ball = new Ball(512 , 70 , image , 225 , Difficulty.BallPower);
                ball.MinX = 337;
                ball.MaxX = 687;
                ball.MaxY = 768;

            }


            //AI
            DifficultyAI.Init();

        }

        public override void UpData(){
            base.UpData();

            if(isGameOver == false) {
                isGameOver = false;

                /*操作関係*/
                isMinoMoveLeft = false;
                isMinoMoveRight = false;
                isMinoSpin = false;
                isBarMoveLeft = false;
                isBarMoveRight = false;

                if(keys.Pressed(DX.KEY_INPUT_LEFT)) {
                    isMinoMoveLeft = true;
                    //仮
                    DifficultyAI.MinoStress += 1;
                }
                if(keys.Pressed(DX.KEY_INPUT_RIGHT)) {
                    isMinoMoveRight = true;
                    //仮
                    DifficultyAI.MinoStress += 1;
                }
                if(keys.Pressed(DX.KEY_INPUT_UP)) {
                    isMinoSpin = true;
                    //仮
                    DifficultyAI.MinoStress+=2;
                }
                if(keys.Pressed(DX.KEY_INPUT_DOWN)) {
                    isMinoQuickDrop = true;
                    //仮
                    DifficultyAI.MinoStress -= 3;

                }
                if(keys.Pressing(DX.KEY_INPUT_A)) {
                    isBarMoveLeft = true;
                }
                if(keys.Pressing(DX.KEY_INPUT_D)) {
                    isBarMoveRight = true;
                }


                /*棒関係*/
                if(isBarMoveLeft == true) {
                    bar.PosX -= barSpeed;
                }
                if(isBarMoveRight == true) {
                    bar.PosX += barSpeed;
                }

                bar.UpData();

                if(ball.Reflection(bar)) {
                    Difficulty.DifficultyLevel++;
                    SetBallDiffeculty();
                }
                ball.Reflection(block);

                for(int blockY = 0 ; blockY < maxBlockY ; blockY++) {
                    for(int blockX = 0 ; blockX < maxBlockX ; blockX++) {
                        if(block[blockY , blockX].IsCollision == true && Hit.CheckBoxToCircle(block[blockY , blockX].Image , ball.Image) == true) {
                            effectBlock.Add(new EffectBreakBlock(block[blockY , blockX].PosX , block[blockY , blockX].PosY , block[blockY , blockX].Image.InsideColor));
                            block[blockY , blockX].Init();

                            /*スコア関係*/
                            comboBreakout++;
                            scoreBreakout += comboBreakout * 100;

                        }
                    }
                }

                if(Hit.CheckBoxToCircle(bar.Image, ball.Image) == true) {
                    comboBreakout = 0;
                }

                ball.UpData();


                /*ミノ関係*/

                if(isMinoSpin == true && isMinoQuickDrop == false) {
                    minoSpin();
                }


                //ミノが落ちれるか
                bool isCanDrop = true;
                for(int i = 0 ; i < mino.Length ; i++) {
                    //マップの一番下まで到達
                    //下にに固定されたブロックがあったら
                    if(mino[i].CellPosY + 1 >= maxBlockY || block[mino[i].CellPosY + 1 , mino[i].CellPosX].IsCollision == true)
                        isCanDrop = false;

                    //画面外には出れないように
                    //横に固定されたブロックがあったら移動できないように
                    if(mino[i].CellPosX - 1 < 0 || block[mino[i].CellPosY , mino[i].CellPosX - 1].IsCollision == true) {
                        isMinoMoveLeft = false;
                    }
                    if(mino[i].CellPosX + 1 >= maxBlockX || block[mino[i].CellPosY , mino[i].CellPosX + 1].IsCollision == true) {
                        isMinoMoveRight = false;
                    }

                }

                bool isMinoGeneration = false;
                for(int i = 0 ; i < mino.Length ; i++) {

                    //クイックドロップを行うか
                    mino[i].IsQuickDrop = isMinoQuickDrop;

                    //ミノが落ちれるか設定
                    mino[i].IsCanDrop = isCanDrop;

                    //更新処理
                    //ミノが落ちれれば落ちる
                    //落ちれなければ遊び時間
                    mino[i].UpData();


                    //移動処理
                    if(mino[i].IsCanPlay == true) {
                        if(isMinoMoveLeft)
                            mino[i].CellPosX--;
                        if(isMinoMoveRight)
                            mino[i].CellPosX++;
                    } else {
                        //遊び時間が終わったら固定
                        block[mino[i].CellPosY , mino[i].CellPosX].IsCollision = true;
                        //色の設定
                        block[mino[i].CellPosY , mino[i].CellPosX].Image.InsideColor = minoColor.Clone();
                        isMinoGeneration = true;
                    }
                }

                int ghostMinoY;
                for(int i = 0 ; i < ghostMino.Length ; i++) {
                    ghostMino[i].CellPosX = mino[i].CellPosX;
                    ghostMino[i].CellPosY = mino[i].CellPosY;
                }

                for(ghostMinoY = 0 ; ghostMinoY < maxBlockY ; ghostMinoY++) {
                    bool isGhostMostLowerLayer = false;
                    for(int i = 0 ; i < ghostMino.Length ; i++) {
                        //マップの一番下まで到達
                        //下にに固定されたブロックがあったら
                        if(mino[i].CellPosY + ghostMinoY + 1 >= maxBlockY || block[mino[i].CellPosY + ghostMinoY + 1 , mino[i].CellPosX].IsCollision == true)
                            isGhostMostLowerLayer = true;
                    }
                    if(isGhostMostLowerLayer == true) {
                        break;
                    }
                }

                for(int i = 0 ; i < ghostMino.Length ; i++) {
                    ghostMino[i].CellPosY += ghostMinoY;
                }


                /*ブロック関係*/
                //ブロックが並んでたら消す
                int delBlockY = 0;
                int comboLine = 0;
                while(delBlockY < maxBlockY) {

                    bool isDelBlock = true;
                    for(int x = 0 ; x < maxBlockX ; x++) {
                        if(block[delBlockY , x].IsCollision == false) {
                            isDelBlock = false;
                            break;
                        }
                    }

                    //消す列がある
                    if(isDelBlock == true) {
                        comboLine++;

                        for(int x = 0 ; x < maxBlockX ; x++) {
                            //エフェクトの追加
                            effectBlock.Add(new EffectBreakBlock(block[delBlockY , x].PosX , block[delBlockY , x].PosY , block[delBlockY , x].Image.InsideColor));
                        }

                        for(int y = delBlockY ; y > 0 ; y--) {
                            for(int x = 0 ; x < maxBlockX ; x++) {
                                block[y , x].Image.InsideColor = block[y - 1 , x].Image.InsideColor.Clone();
                                block[y , x].IsCollision = block[y - 1 , x].IsCollision;
                                block[0 , x].Init();
                            }
                        }
                    } else {
                        delBlockY++;
                    }
                }

                if(comboLine >= 1) {
                    for(int i = 1 ; i < comboLine+1 ; i++) {
                        scoreTetris += 1500 * i;
                    }
                }

                //ブロックの更新処理
                for(int y = 0 ; y < maxBlockY ; y++) {
                    for(int x = 0 ; x < maxBlockX ; x++) {
                        block[y , x].UpData();
                    }
                }

                //ミノの再生成
                bool isMinoGenerationFail = false;
                if(isMinoGeneration == true) {
                    isMinoQuickDrop = false;
                    isMinoGenerationFail = !minoGeneration();

                    //ミノの生成後に難易度の調整
                    SetMinoDiffeculty();

                }


                /*ゲームオーバー関係*/
                if(ball.PosY + ball.Image.Radius < 0 || isMinoGenerationFail == true) {
                    isGameOver = true;
                }

            }



            /*色関係*/


            for(int y = 0 ; y < maxBlockY ; y++) {
                for(int x = 0 ; x < maxBlockX ; x++) {
                    block[y , x].Image.InsideColor.Alpha = 255;
                    if(block[y , x].IsCollision == false) {
                        block[y , x].Image.InsideColor = new Color(255 , 255 , 255);
                    }
                    if(y == 0) block[y , x].Image.InsideColor = new Color(255 , 0 , 0);
                }
            }

            for(int i = 0 ; i < ghostMino.Length ; i++) {
                if(block[ghostMino[i].CellPosY , ghostMino[i].CellPosX].IsCollision == false) {                   
                    block[ghostMino[i].CellPosY , ghostMino[i].CellPosX].Image.InsideColor = minoColor.Clone();
                    block[ghostMino[i].CellPosY , ghostMino[i].CellPosX].Image.InsideColor.Alpha = 225 / 2;
                }
            }

            for(int i = 0 ; i < mino.Length ; i++) {
                block[mino[i].CellPosY , mino[i].CellPosX].Image.InsideColor = minoColor.Clone();
            }


            //テスト用。ミノの回転中心表示
            //block[mino[0].CellPosY , mino[0].CellPosX].Image.InsideColor = new Color(0 ,255 , 255);



            /*エフェクト関係*/
            if(effectBlock.Count > 0) {
                for(int i = 0 ; i < effectBlock.Count ; i++) {
                    //エフェクト更新
                    effectBlock[i].UpData();

                    //再生終了したエフェクトは消す
                    if(effectBlock[i].IsEnable == false) {
                        effectBlock.Remove(effectBlock[i]);
                    }
                }
            }

            //////////////////////////////////////////////////


            DifficultyAI.UpData();


            //escでめにゅー
            if (keys.Pressed(DX.KEY_INPUT_ESCAPE)){
                pageState = (int)Page.State.TITLE;
            }
        }

        public override void Draw(){
            //DX.DrawString(0 , 0 , "げーむ" , DX.GetColor(255 , 255 , 0));
            //DX.DrawString(0 , 15 , "globaltimer:"+globalTimer.Elapsed , DX.GetColor(255 , 255 , 0));
            //DX.DrawString(0 , 30 , "localtimer:" + localTimer.Elapsed , DX.GetColor(255 , 255 , 0));

            //int mouseX , mouseY;
            //DX.GetMousePoint(out mouseX , out mouseY);
            //DX.DrawString(0 , 45 , "mouseX:" + mouseX , DX.GetColor(255 , 255 , 0));
            //DX.DrawString(0 , 60 , "mouseY:" + mouseY , DX.GetColor(255 , 255 , 0));

            DX.DrawString(700 , 500 , "stress:" + DifficultyAI.MinoStress , DX.GetColor(255 , 255 , 0));
            DX.DrawString(700 , 520 , "stressLevel:" + DifficultyAI.MinoDifficulty , DX.GetColor(255 , 255 , 0));
            
            DX.DrawStringToHandle(15 , 45 , "*Control" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);

            DX.DrawStringToHandle(15 , 90 , " Breakout" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 90 + 35 , "  Bar" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 90 + 35 * 2 , "   move Left  :[A]" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 90 + 35 * 3 , "   move Right :[D]" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);

            DX.DrawStringToHandle(15 , 220 + 35 * 1 , " Tetris" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 220 + 35 * 2 , "  Mino" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 220 + 35 * 3 , "   move Left  :[←]" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 220 + 35 * 4 , "   move Right :[→]" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 220 + 35 * 5 , "   spin       :[↑]" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 220 + 35 * 6 , "   quick drop :[↓]" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);

            DX.DrawStringToHandle(15 , 500 , "*Game Over" , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 500 + 35 , " Mino on RED LINE." , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);
            DX.DrawStringToHandle(15 , 500 + 35 * 2 , " Ball over TOP." , DX.GetColor(255 , 255 , 0) , fontHandleHowToUse);




            DX.DrawStringToHandle(700 , 45 , "Tetris Score" , DX.GetColor(255 , 255 , 0),fontHandleScore);
            DX.DrawStringToHandle(700 , 75 , "  :" + scoreTetris , DX.GetColor(255 , 255 , 0) , fontHandleScore);
            DX.DrawStringToHandle(700 , 105 , " 1LINE =  1500 point", DX.GetColor(255 , 255 , 0) , fontHandleScore);
            DX.DrawStringToHandle(700 , 135 , " 2LINE =  4500 point" , DX.GetColor(255 , 255 , 0) , fontHandleScore);
            DX.DrawStringToHandle(700 , 165 , " 3LINE =  9000 point" , DX.GetColor(255 , 255 , 0) , fontHandleScore);
            DX.DrawStringToHandle(700 , 195 , " 4LINE = 15000 point" , DX.GetColor(255 , 255 , 0) , fontHandleScore);

            DX.DrawStringToHandle(700 , 255 , "Breakout Score" , DX.GetColor(255 , 255 , 0) , fontHandleScore);
            DX.DrawStringToHandle(700 , 285 , "  :" + scoreBreakout , DX.GetColor(255 , 255 , 0) , fontHandleScore);
            DX.DrawStringToHandle(700 , 315 , " 1BLOCK = 100 * Combo" , DX.GetColor(255 , 255 , 0) , fontHandleScore);
            DX.DrawStringToHandle(700 , 345 , "        = 100 * " + (comboBreakout+1) , DX.GetColor(255 , 255 , 0) , fontHandleScore);

            DX.DrawStringToHandle(700 , 400 , "Total Score", DX.GetColor(255 , 255 , 0) , fontHandleScore);
            DX.DrawStringToHandle(700 , 435 , "  :" + (scoreBreakout + scoreTetris) , DX.GetColor(255 , 255 , 0) , fontHandleScore);
            


            back.Draw();

            for(int y = 0 ; y < maxBlockY ; y++) {
                for(int x = 0 ; x < maxBlockX ; x++) {
                    block[y , x].Draw();
                }
            }

            bar.Draw();

            ball.Draw();

            if(effectBlock.Count > 0) {
                foreach(var effect in effectBlock) {
                    effect.Draw();
                }
            }

            if(isGameOver == true) {
                ImageBox ib = new ImageBox(1024 / 2 , 768 / 2 , 600 , 700 , new Color(255 , 255 , 255) , new Color(0 , 0 , 0) , 2);
                ib.Draw();
                DX.DrawStringToHandle(375 , 150, "Gameover!" , DX.GetColor(255 , 0 , 0) , fontHandleGameOver);
                DX.DrawStringToHandle(370 , 150 + 64 , "Your score" , DX.GetColor(255 , 0 , 0) , fontHandleGameOver);
                DX.DrawStringToHandle(480 , 150 + 64*2 , "is" , DX.GetColor(255 , 0 , 0) , fontHandleGameOver);
                string str = "" + (scoreTetris + scoreBreakout);
                int strLength = DX.GetDrawStringWidthToHandle(str, str.Length,fontHandleGameOverScore);
                DX.DrawStringToHandle(1024 / 2 - strLength/2 , 170 + 64 * 3 , str , DX.GetColor(255 , 0 , 0) , fontHandleGameOverScore);

                string eq = "";
                if(scoreTetris == scoreBreakout) {
                    eq = "=";
                } else if(scoreTetris > scoreBreakout){
                    eq = ">";
                } else {
                    eq = "<";
                }

                str = "Tetris " + eq + " Breakout";
                strLength = DX.GetDrawStringWidthToHandle(str , str.Length , fontHandleGameOver);
                DX.DrawStringToHandle(1024 / 2 - strLength / 2 , 280 + 64 * 4 , str , DX.GetColor(255 , 0 , 0) , fontHandleGameOver);
                str = "" + scoreTetris + ":" + scoreBreakout;
                strLength = DX.GetDrawStringWidthToHandle(str , str.Length , fontHandleGameOver);
                DX.DrawStringToHandle(1024 / 2 - strLength / 2 , 350 + 64 * 4 , str , DX.GetColor(255 , 0 , 0) , fontHandleGameOver);


                DX.DrawStringToHandle(350 , 700 , "Push [ESC]. Return Title." , DX.GetColor(255 , 0 , 0) , fontHandleScore);
            }

            
        }

        public override void LoadResource() {
        }


        /// <summary>
        /// ミノの回転処理
        /// </summary>
        private void minoSpin() {
            int minoType = (int)nowType;

            int[] spinPosX = new int[4];
            int[] spinPosY = new int[4];

            for(int i = 0 ; i < 4 ; i++) {
                spinPosX[i] = new int();
                spinPosX[i] = 0;
                spinPosY[i] = new int();
                spinPosY[i] = 0;
            }

            //回転できるかチェック
            bool isCanSpin = true;
            switch(minoType) {
                case (int)Tetrimino.Type.I:
                    switch(NumMinoSpin % 2){
                        case 0:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = 1;
                            spinPosY[1] = -1;
                            spinPosX[2] = -1;
                            spinPosY[2] = 1;
                            spinPosX[3] = -2;
                            spinPosY[3] = 2;
                            break;
                        case 1:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = 1;
                            spinPosY[1] = 1;
                            spinPosX[2] = -1;
                            spinPosY[2] = -1;
                            spinPosX[3] = -2;
                            spinPosY[3] = -2;
                            break;
                    }

                    break;
                case (int)Tetrimino.Type.O:
                    switch(NumMinoSpin % 2) {
                        case 0:
                            break;
                        case 1:
                            break;
                    }

                    break;
                case (int)Tetrimino.Type.S:
                    switch(NumMinoSpin % 2) {
                        case 0:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = 1;
                            spinPosY[1] = 1;
                            spinPosX[2] = 0;
                            spinPosY[2] = 2;
                            spinPosX[3] = 1;
                            spinPosY[3] = -1;
                            break;
                        case 1:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = -1;
                            spinPosY[1] = 1;
                            spinPosX[2] = -2;
                            spinPosY[2] = 0;
                            spinPosX[3] = 1;
                            spinPosY[3] = 1;
                            break;
                    }

                    break;
                case (int)Tetrimino.Type.Z:
                    switch(NumMinoSpin % 2) {
                        case 0:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = 1;
                            spinPosY[1] = 1;
                            spinPosX[2] = 2;
                            spinPosY[2] = 0;
                            spinPosX[3] = -1;
                            spinPosY[3] = 1;
                            break;
                        case 1:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = -1;
                            spinPosY[1] = 1;
                            spinPosX[2] = 0;
                            spinPosY[2] = 2;
                            spinPosX[3] = -1;
                            spinPosY[3] = -1;
                            break;
                    }

                    break;
                case (int)Tetrimino.Type.J:
                    switch(NumMinoSpin % 2) {
                        case 0:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = -1;
                            spinPosY[1] = 1;
                            spinPosX[2] = -2;
                            spinPosY[2] = 2;
                            spinPosX[3] = 1;
                            spinPosY[3] = 1;
                            break;
                        case 1:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = -1;
                            spinPosY[1] = -1;
                            spinPosX[2] = -2;
                            spinPosY[2] = -2;
                            spinPosX[3] = -1;
                            spinPosY[3] = 1;
                            break;
                    }

                    break;
                case (int)Tetrimino.Type.L:
                    switch(NumMinoSpin % 2) {
                        case 0:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = 1;
                            spinPosY[1] = -1;
                            spinPosX[2] = 2;
                            spinPosY[2] = -2;
                            spinPosX[3] = 1;
                            spinPosY[3] = 1;
                            break;
                        case 1:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = 1;
                            spinPosY[1] = 1;
                            spinPosX[2] = 2;
                            spinPosY[2] = 2;
                            spinPosX[3] = -1;
                            spinPosY[3] = 1;
                            break;
                    }

                    break;
                case (int)Tetrimino.Type.T:
                    switch(NumMinoSpin % 2) {
                        case 0:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = 1;
                            spinPosY[1] = -1;
                            spinPosX[2] = -1;
                            spinPosY[2] = 1;
                            spinPosX[3] = 1;
                            spinPosY[3] = 1;
                            break;
                        case 1:
                            spinPosX[0] = 0;
                            spinPosY[0] = 0;
                            spinPosX[1] = 1;
                            spinPosY[1] = 1;
                            spinPosX[2] = -1;
                            spinPosY[2] = -1;
                            spinPosX[3] = -1;
                            spinPosY[3] = 1;
                            break;
                    }

                    break;
            }

           


            //実際の座標に変換
            for(int i = 0 ; i < 4 ; i++) {

                if(numMinoSpin >= 2) {
                    spinPosX[i] *= -1;
                    spinPosY[i] *= -1;
                }

                spinPosX[i] += mino[i].CellPosX;
                spinPosY[i] += mino[i].CellPosY;
            }
            

            for(int i = 0 ; i < 4 ; i++) {
                if(spinPosX[i] < 0) {
                    //横に壁があったら回転できないように
                    isCanSpin = false;
                    break;
                }
                if(spinPosX[i] >= maxBlockX) {
                    isCanSpin = false;
                    break;
                }
                if(spinPosY[i] < 0 ) {
                    // 縦に壁があったら回転できないように
                    isCanSpin = false;
                    break;
                }
                if(spinPosY[i] >= maxBlockY) {
                    isCanSpin = false;
                    break;
                }
                if(block[spinPosY[i] , spinPosX[i]].IsCollision == true) {
                    //回転位置にブロックがあったら
                    isCanSpin = false;
                    break;
                }
            }


            if(isCanSpin == false) {

                int shiftUp, shiftDown, shiftLeft, shiftRight;
                bool isCanShiftUp, isCanShiftDown, isCanShiftLeft, isCanShiftRight;


                //壁のみを考慮（ブロックをしらん）したシフト

                //シフト数をきめる
                for(shiftRight = 0 ; shiftRight < maxBlockX ; shiftRight++) {
                    isCanShiftRight = true;
                    for(int i = 0 ; i < 4 ; i++) {
                        //シフト位置に壁がある、もしくはブロックがあるとだめ
                        if(spinPosX[i] + shiftRight < 0 || spinPosX[i] + shiftRight >= maxBlockX) {
                            //だめだった
                            isCanShiftRight = false;
                            break;
                        }
                    }

                    //できた
                    if(isCanShiftRight == true) {
                        break;
                    }

                }

                for(shiftLeft = 0 ; shiftLeft < maxBlockX ; shiftLeft++) {
                    isCanShiftLeft = true;
                    for(int i = 0 ; i < 4 ; i++) {
                        //シフト位置に壁がある、もしくはブロックがあるとだめ
                        if(spinPosX[i] - shiftLeft < 0 || spinPosX[i] - shiftLeft >= maxBlockX) {
                            //だめだった
                            isCanShiftLeft = false;
                            break;
                        }
                    }

                    //できた
                    if(isCanShiftLeft == true) {
                        break;
                    }

                }

                for(shiftUp = 0 ; shiftUp < maxBlockY ; shiftUp++) {
                    isCanShiftUp = true;
                    for(int i = 0 ; i < 4 ; i++) {
                        //シフト位置に壁がある、もしくはブロックがあるとだめ
                        if(spinPosY[i] - shiftUp < 0 || spinPosY[i] - shiftUp >= maxBlockY) {
                            //だめだった
                            isCanShiftUp = false;
                            break;
                        }
                    }

                    //できた
                    if(isCanShiftUp == true) {
                        break;
                    }

                }

                for(shiftDown = 0 ; shiftDown < maxBlockY ; shiftDown++) {
                    isCanShiftDown = true;
                    for(int i = 0 ; i < 4 ; i++) {
                        //シフト位置に壁がある、もしくはブロックがあるとだめ
                        if(spinPosY[i] + shiftDown < 0 || spinPosY[i] + shiftDown >= maxBlockY) {
                            //だめだった
                            isCanShiftDown = false;
                            break;
                        }
                    }

                    //できた
                    if(isCanShiftDown == true) {
                        break;
                    }

                }


                //左右のどっちにシフト？
                //小さい方にシフト
                if(shiftRight < shiftLeft) {
                    //右に？
                    if(shiftRight < 3) {
                        for(int i = 0 ; i < 4 ; i++) {
                            spinPosX[i] += shiftRight;
                        }
                        isCanSpin = true;
                    }

                } else {
                    //左に？
                    if(shiftLeft < 3) {
                        for(int i = 0 ; i < 4 ; i++) {
                            spinPosX[i] -= shiftLeft;
                        }
                        isCanSpin = true;
                    }
                }
                

            //上下のどっちにシフト？
            //小さい方にシフト
                if(shiftUp < shiftDown) {
                    //上に？
                    if(shiftUp < 3) {
                        for(int i = 0 ; i < 4 ; i++) {
                            spinPosY[i] -= shiftUp;
                        }
                        isCanSpin = true;
                    }

                } else {
                    //下に？
                    if(shiftDown < 3) {
                        for(int i = 0 ; i < 4 ; i++) {
                            spinPosY[i] += shiftDown;
                        }
                        isCanSpin = true;
                    }
                }
                

                if(isCanSpin == true) {
                    //ブロックを考慮したシフトが必要？
                    bool isMoreShift = false;
                    for(int i = 0 ; i < 4 ; i++) {
                        //回転後の位置が画面内に収まっているか？
                        if(spinPosX[i] >= 0 && spinPosX[i] < maxBlockX && spinPosY[i] >= 0 && spinPosY[i] < maxBlockY) {
                            //回転後の位置にブロックがないか
                            if(block[spinPosY[i] , spinPosX[i]].IsCollision == true) {
                                isMoreShift = true;
                                isCanSpin = false;
                            }
                        }
                    }


                    if(isMoreShift == true) {


                        //シフト数をきめる
                        for(shiftRight = 0 ; shiftRight < maxBlockX ; shiftRight++) {
                            isCanShiftRight = true;
                            for(int i = 0 ; i < 4 ; i++) {
                                //シフト位置に壁がある、もしくはブロックがあるとだめ
                                if(spinPosX[i] + shiftRight < 0 || spinPosX[i] + shiftRight >= maxBlockX || block[spinPosY[i] , spinPosX[i] + shiftRight].IsCollision == true) {
                                    //だめだった
                                    isCanShiftRight = false;
                                    break;
                                }
                            }

                            //できた
                            if(isCanShiftRight == true) {
                                break;
                            }

                        }

                        for(shiftLeft = 0 ; shiftLeft < maxBlockX ; shiftLeft++) {
                            isCanShiftLeft = true;
                            for(int i = 0 ; i < 4 ; i++) {
                                //シフト位置に壁がある、もしくはブロックがあるとだめ
                                if(spinPosX[i] - shiftLeft < 0 || spinPosX[i] - shiftLeft >= maxBlockX || block[spinPosY[i] , spinPosX[i] - shiftLeft].IsCollision == true) {
                                    //だめだった
                                    isCanShiftLeft = false;
                                    break;
                                }
                            }

                            //できた
                            if(isCanShiftLeft == true) {
                                break;
                            }

                        }

                        for(shiftUp = 0 ; shiftUp < maxBlockY ; shiftUp++) {
                            isCanShiftUp = true;
                            for(int i = 0 ; i < 4 ; i++) {
                                //シフト位置に壁がある、もしくはブロックがあるとだめ
                                if(spinPosY[i] - shiftUp < 0 || spinPosY[i] - shiftUp >= maxBlockY || block[spinPosY[i] - shiftUp , spinPosX[i]].IsCollision == true) {
                                    //だめだった
                                    isCanShiftUp = false;
                                    break;
                                }
                            }

                            //できた
                            if(isCanShiftUp == true) {
                                break;
                            }

                        }

                        for(shiftDown = 0 ; shiftDown < maxBlockY ; shiftDown++) {
                            isCanShiftDown = true;
                            for(int i = 0 ; i < 4 ; i++) {
                                //シフト位置に壁がある、もしくはブロックがあるとだめ
                                if(spinPosY[i] + shiftDown < 0 || spinPosY[i] + shiftDown >= maxBlockY || block[spinPosY[i] + shiftDown , spinPosX[i]].IsCollision == true) {
                                    //だめだった
                                    isCanShiftDown = false;
                                    break;
                                }
                            }

                            //できた
                            if(isCanShiftDown == true) {
                                break;
                            }

                        }


                        //左右のどっちにシフト？
                        //小さい方にシフト
                        if(!(shiftRight == 0 && shiftLeft == 0)) {
                            if(shiftRight < shiftLeft) {
                                //右に？
                                if(shiftRight < 3) {
                                    for(int i = 0 ; i < 4 ; i++) {
                                        spinPosX[i] += shiftRight;
                                    }
                                    isCanSpin = true;
                                }

                            } else {
                                //左に？
                                if(shiftLeft < 3) {
                                    for(int i = 0 ; i < 4 ; i++) {
                                        spinPosX[i] -= shiftLeft;
                                    }
                                    isCanSpin = true;
                                }
                            }
                        }

                        //上下のどっちにシフト？
                        //小さい方にシフト
                        if(!(shiftUp == 0 && shiftDown == 0)) {
                            if(shiftUp < shiftDown) {
                                //上に？
                                if(shiftUp < 3) {
                                    for(int i = 0 ; i < 4 ; i++) {
                                        spinPosY[i] -= shiftUp;
                                    }
                                    isCanSpin = true;
                                }

                            } else {
                                //下に？
                                if(shiftDown < 3) {
                                    for(int i = 0 ; i < 4 ; i++) {
                                        spinPosY[i] += shiftDown;
                                    }
                                    isCanSpin = true;
                                }
                            }
                        }


                    }
                }



            }


            if(isCanSpin == true) {
                //実際の座標に変換
                for(int i = 0 ; i < 4 ; i++) {
                    mino[i].CellPosX = spinPosX[i];
                    mino[i].CellPosY = spinPosY[i];
                }

                NumMinoSpin++;
            }



        }


        /// <summary>
        /// 新しいミノの生成
        /// </summary>
        /// <param name="type">生成するミノの形</param>
        /// <returns>true:ミノの生成に成功,false:ミノの生成に失敗</returns>
        private bool minoGeneration(int type) {

            int nextMinoType = type;


            nowType = (Tetrimino.Type)type;
            numMinoSpin = 0;

            for(int i = 0 ; i < mino.Length ; i++) {
                mino[i].Init();
            }

            switch(nextMinoType) {
                case (int)Tetrimino.Type.I:
                    mino[0].CellPosX = 4;
                    mino[0].CellPosY = 0;
                    mino[1].CellPosX = 3;
                    mino[1].CellPosY = 0;
                    mino[2].CellPosX = 5;
                    mino[2].CellPosY = 0;
                    mino[3].CellPosX = 6;
                    mino[3].CellPosY = 0;
                    minoColor.R = 175;
                    minoColor.G = 223;
                    minoColor.B = 228;
                    break;
                case (int)Tetrimino.Type.O:
                    mino[0].CellPosX = 5;
                    mino[0].CellPosY = 0;
                    mino[1].CellPosX = 6;
                    mino[1].CellPosY = 0;
                    mino[2].CellPosX = 5;
                    mino[2].CellPosY = 1;
                    mino[3].CellPosX = 6;
                    mino[3].CellPosY = 1;
                    minoColor.R = 255;
                    minoColor.G = 212;
                    minoColor.B = 0;
                    break;
                case (int)Tetrimino.Type.S:
                    mino[0].CellPosX = 4;
                    mino[0].CellPosY = 1;
                    mino[1].CellPosX = 4;
                    mino[1].CellPosY = 0;
                    mino[2].CellPosX = 5;
                    mino[2].CellPosY = 0;
                    mino[3].CellPosX = 3;
                    mino[3].CellPosY = 1;
                    minoColor.R = 185;
                    minoColor.G = 196;
                    minoColor.B = 47;
                    break;
                case (int)Tetrimino.Type.Z:
                    mino[0].CellPosX = 4;
                    mino[0].CellPosY = 1;
                    mino[1].CellPosX = 4;
                    mino[1].CellPosY = 0;
                    mino[2].CellPosX = 3;
                    mino[2].CellPosY = 0;
                    mino[3].CellPosX = 5;
                    mino[3].CellPosY = 1;
                    minoColor.R = 237;
                    minoColor.G = 26;
                    minoColor.B = 61;
                    break;
                case (int)Tetrimino.Type.J:
                    mino[0].CellPosX = 4;
                    mino[0].CellPosY = 1;
                    mino[1].CellPosX = 5;
                    mino[1].CellPosY = 1;
                    mino[2].CellPosX = 6;
                    mino[2].CellPosY = 1;
                    mino[3].CellPosX = 4;
                    mino[3].CellPosY = 0;
                    minoColor.R = 0;
                    minoColor.G = 103;
                    minoColor.B = 191;
                    break;
                case (int)Tetrimino.Type.L:
                    mino[0].CellPosX = 4;
                    mino[0].CellPosY = 1;
                    mino[1].CellPosX = 3;
                    mino[1].CellPosY = 1;
                    mino[2].CellPosX = 2;
                    mino[2].CellPosY = 1;
                    mino[3].CellPosX = 4;
                    mino[3].CellPosY = 0;
                    minoColor.R = 243;
                    minoColor.G = 152;
                    minoColor.B = 0;
                    break;
                case (int)Tetrimino.Type.T:
                    mino[0].CellPosX = 4;
                    mino[0].CellPosY = 1;
                    mino[1].CellPosX = 3;
                    mino[1].CellPosY = 1;
                    mino[2].CellPosX = 5;
                    mino[2].CellPosY = 1;
                    mino[3].CellPosX = 4;
                    mino[3].CellPosY = 0;
                    minoColor.R = 167;
                    minoColor.G = 87;
                    minoColor.B = 168;
                    break;
            }

            for(int i = 0 ; i < mino.Length ; i++) {
                if(block[mino[i].CellPosY,mino[i].CellPosX].IsCollision == true) {
                    return false;
                }
            }

            return true;

        }


        /// <summary>
        /// 新しいミノの生成
        /// </summary>
        /// <param name="type">生成するミノの形</param>
        /// <returns>true:ミノの生成に成功,false:ミノの生成に失敗</returns>
        private bool minoGeneration(Tetrimino.Type type) {
            int nextMinoType = (int)type;

            return minoGeneration(nextMinoType);
        }


        /// <summary>
        /// 新しいミノの生成
        /// </summary>
        /// <returns>true:ミノの生成に成功,false:ミノの生成に失敗</returns>
        private bool minoGeneration() {
            int nextMinoType = rand.Next(7);


            return minoGeneration(nextMinoType);        
        }

        /// <summary>
        /// ミノの難易度（落下速度）の調整
        /// 設定された難易度レベル（Difficulty.DifficultyLevel）に基づいて調整が行われる
        /// </summary>
        private void SetMinoDiffeculty() {
            for(int i = 0 ; i < mino.Length ; i++) {
                mino[i].DropWaitTime = Difficulty.MinoDropWaitTime;
                mino[i].PlayWaitTime = Difficulty.MinoPlayWaitTime;
            }          
        }

        /// <summary>
        /// ボールの難易度（移動速度）の調整
        /// 設定された難易度レベル（Difficulty.DifficultyLevel）に基づいて調整が行われる
        /// </summary>
        private void SetBallDiffeculty() {
            ball.Power = Difficulty.BallPower;
        }

    }
}
