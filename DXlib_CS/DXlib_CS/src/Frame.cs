using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using DXlib_CS.src.Comp.DrawComp.Pages;

namespace DXlib_CS.src {
    sealed class Frame {

        static int windowSizeX;
        public static int WindowSizeX {
            get { return windowSizeX; }
            set { windowSizeX = value; }
        }

        static int windowSizeY;
        public static int WindowSizeY {
            get { return windowSizeY; }
            set { windowSizeY = value; }
        }

        static string windowText;
        public static string WindowText {
            get { return windowText; }
            set { windowText = value; }
        }

        Page page;



        public Frame(int x=800,int y=600,string text=""){

            WindowSizeX = x;
            WindowSizeY = y;
            WindowText = text;

            DX.ChangeWindowMode(DX.TRUE);
            DX.SetGraphMode(WindowSizeX , WindowSizeY , 32);
            
            DX.SetOutApplicationLogValidFlag(DX.FALSE);
            if(DX.DxLib_Init() == -1) {
            }

            DX.SetMainWindowText(WindowText);
            
            DX.SetWindowSizeChangeEnableFlag(DX.FALSE);
            DX.SetMouseDispFlag(DX.TRUE);
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            this.page = new PageTitle();
            this.page.Init();

        }

        ~Frame() {
            DX.DxLib_End(); 
        }



        public void Run() {

            bool isEnd = false;
            int beforPage = Page.PageState;
            OXGame.FPSTimer fps = new OXGame.FPSTimer();
            while(DX.ProcessMessage() == 0 && isEnd == false) {

                fps.WaitNextFrame();

                this.page.UpData();

                if(beforPage != Page.PageState) {
                    switch(Page.PageState) {
                        case (int)Page.State.TITLE:
                            this.page = new PageTitle();
                            this.page.Init();
                            break;
                        case (int)Page.State.GAME:
                            this.page = new PageGame();
                            this.page.Init();
                            break;
                        case (int)Page.State.END:
                            isEnd = true;
                            break;
                    }
                }


                if(fps.IsDraw() == true) {

                    // 画面に描かれているものを一回全部消す
                    DX.ClsDrawScreen();

                    //背景
                    DX.DrawBox(0,0,WindowSizeX,WindowSizeY,new Color(100,100,255).GetDxColor(),1);

                    this.page.Draw();

                    // 裏画面の内容を面画面に反映される
                    DX.ScreenFlip();
                    fps.CalcFps();
                }

                beforPage = Page.PageState;

            }

            return;

        }

    }
}
