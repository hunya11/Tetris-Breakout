using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DXlib_CS.src.Comp {
    class Tetrimino : Component{

        public enum Type : int {
            I = 0,O,S,Z,J,L,T

        };

        private int cellPosX;
        private int cellPosY;

        /// <summary>
        /// ミノが落下できるか
        /// </summary>
        private bool isCanDrop;
        private double previousDropTime;
        private double currentDropTime;
        double dropWaitTime;

        /// <summary>
        /// ミノが固定されるまでの遊びができるか
        /// </summary>
        private bool isCanPlay;
        private double previousPlayTime;
        private double currentPlayTime;
        double playWaitTime;
        

        private bool isQuickDrop;
        private double previousQuickDropTime;
        private double currentQuickDropTime;
        double QuickDropWaitTime;


        private Stopwatch minoTimer;



        public int CellPosX {
            get {
                return cellPosX;
            }

            set {
                cellPosX = value;
            }
        }

        public int CellPosY {
            get {
                return cellPosY;
            }

            set {
                cellPosY = value;
            }
        }

        public bool IsCanDrop {
            get {
                return isCanDrop;
            }

            set {
                isCanDrop = value;
            }
        }

        public double DropWaitTime {
            get {
                return dropWaitTime;
            }

            set {
                dropWaitTime = value;
            }
        }

        public bool IsCanPlay {
            get {
                return isCanPlay;
            }

        }

        public bool IsQuickDrop {
            get {
                return isQuickDrop;
            }

            set {
                isQuickDrop = value;
            }
        }

        public Tetrimino() {
            minoTimer = new Stopwatch();

            this.Init();
        }

        public override void Init() {
            this.cellPosX = 0;
            this.cellPosY = 0;
            this.isCanDrop = true;
            this.isCanPlay = true;
            IsQuickDrop = false;
            minoTimer.Reset();
            minoTimer.Start();
            previousDropTime = minoTimer.Elapsed.TotalSeconds;
            currentDropTime = minoTimer.Elapsed.TotalSeconds;
            previousPlayTime = minoTimer.Elapsed.TotalSeconds;
            currentPlayTime = minoTimer.Elapsed.TotalSeconds;
            previousQuickDropTime = minoTimer.Elapsed.TotalSeconds;
            currentQuickDropTime = minoTimer.Elapsed.TotalSeconds;
            dropWaitTime = 0.25;
            playWaitTime = dropWaitTime + 0.2;
            QuickDropWaitTime = 0.01;
        }


        public override void UpData() {

            if(isCanDrop == true) {
                //落ちているときの処理

                if(IsQuickDrop == true) {
                    //doubleの引き算だから有効桁数下がるかも
                    if(currentQuickDropTime - previousQuickDropTime > QuickDropWaitTime) {
                        cellPosY++;
                        previousQuickDropTime = currentQuickDropTime;
                    }
                    currentQuickDropTime = minoTimer.Elapsed.TotalSeconds;
                } else {
                    //doubleの引き算だから有効桁数下がるかも
                    if(currentDropTime - previousDropTime > dropWaitTime) {
                        cellPosY++;
                        previousDropTime = currentDropTime;
                    }
                    currentDropTime = minoTimer.Elapsed.TotalSeconds;
                }


                //ブロックが落ちているときは常に遊び時間更新
                //テトリミノが着地したあと、1段以上落下すれば、遊び時間がリセットされる仕様
                isCanPlay = true;
                previousPlayTime = minoTimer.Elapsed.TotalSeconds;
                currentPlayTime = minoTimer.Elapsed.TotalSeconds;

            } else if(IsCanPlay == true) {
                //固定されるまでの遊び処理

                //クイック時は遊び時間なし
                if(isQuickDrop == true) {
                    isCanPlay = false;
                } else {
                    if(currentPlayTime - previousPlayTime > playWaitTime) {
                        isCanPlay = false;
                    }
                }

                currentPlayTime = minoTimer.Elapsed.TotalSeconds;
            }

        }

    }
}
