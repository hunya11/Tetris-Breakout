using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXlib_CS.src.Comp.DrawComp.Image;
using DXlib_CS.src.Comp.DrawComp.Object.Effect;
using System.Windows;

namespace DXlib_CS.src.Comp.DrawComp.Object {
    class Ball : GameObject{

        public override double PosX {
            get { return posX; }
            set { posX = value; }
        }

        public override double PosY {
            get { return posY; }
            set { posY = value; }
        }

        public double defaultAngle;
        public double angle;
        public double Angle {
            get {
                return angle;
            }

            set {
                angle = value;
            }
        }

        private double minX;
        public double MinX {
            get {
                return minX;
            }

            set {
                minX = value;
            }
        }

        private double maxX;
        public double MaxX {
            get {
                return maxX;
            }

            set {
                maxX = value;
            }
        }

        private double maxY;
        public double MaxY {
            get {
                return maxY;
            }

            set {
                maxY = value;
            }
        }

        private double power;
        public double Power {
            get {
                return power;
            }
            set {
                power = value;
            }
        }



        public Ball(double posX , double posY , DxImage image,double angle,double power)
            : base(posX , posY , image) {
            this.Init();

            this.defaultAngle = angle;
            this.Angle = angle - 90;
            this.power = power;

        }

        public override void Init() {
            base.Init();
        }

        public override void UpData() {
            

            //斜辺（power）と角度（angle）使ってxとy方向にどれだけ進むか
            //ラジアンに変換
            double radians = Angle * (Math.PI / 180);

            double powX = power * Math.Cos(radians);
            double powY = power * Math.Sin(radians);


            posX += powX;
            posY += powY;



            base.UpData();

        }

        public override void Draw() {
            base.Draw();
        }

        /// <summary>
        /// ぼうと玉の反射処理
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>true:あたった,false:はずれた</returns>
        public bool Reflection(GameObject bar) {

            
            if(Hit.CheckBoxToCircle(bar.Image , this.image) == true) {

                //底辺
                double a = image.PosX - bar.PosX;

                //高さ
                double b = image.PosY - bar.PosY;

                //絶対値
                double absA = Math.Abs(a);
                double absB = Math.Abs(b);

                //斜辺
                double c = Math.Sqrt(absA * absA + absB * absB);

                //棒とぶつかったとき中心に近いほど垂直に打ち上げるように
                double powMin = bar.Image.SizeY / 2 + this.image.Radius;
                double pow = powMin / c;


                //str = "下側";
                if(defaultAngle >= 0 && defaultAngle < 90) {
                    double tempAngle = defaultAngle - 0;
                    defaultAngle = (90) * (1 - pow) + (180 * pow);
                    //defaultAngle = (180 - tempAngle) * (1 - pow) + (180 * pow);
                    angle = defaultAngle - 90;
                } else if(defaultAngle >= 270 && defaultAngle < 360) {
                    double tempAngle = 360 - defaultAngle;
                    defaultAngle = (270) * (1 - pow) + (180 * pow);
                    //defaultAngle = (180 + tempAngle) * (1 - pow) + (180 * pow);
                    angle = defaultAngle - 90;
                }


                //AI
                //中心からどれだけ離れているか
                //離れているほどストレス値追加
                double angleAbs = System.Math.Abs(angle - 90);
                if(angleAbs > 15) {
                    DifficultyAI.BallStress += 4;
                } else {
                    DifficultyAI.BallStress -= 1;
                }



                return true;
                
            }

            return false;
        }

        /// <summary>
        /// 棒とブロックと壁の反射処理
        /// </summary>
        /// <param name="block"></param>
        /// <returns>true:あたった,false:はずれた</returns>
        public bool Reflection(GameObject[,] block) {

            bool isHitSometing = false;

            /*かべ*/
            if(this.PosX - image.Radius < minX) {
                if(this.defaultAngle >= 180 && defaultAngle < 270) {
                    double tempAngle = defaultAngle - 180;
                    defaultAngle = 180 - tempAngle;
                    angle = defaultAngle - 90;
                    isHitSometing = true;
                } else if(defaultAngle >= 270 && defaultAngle <360) {
                    double tempAngle = 360 - defaultAngle;
                    defaultAngle = 0 + tempAngle;
                    angle = defaultAngle - 90;
                    isHitSometing = true;
                }
            }
            if(this.PosX + image.Radius > maxX) {
                if(this.defaultAngle >= 90 && defaultAngle < 180) {
                    double tempAngle = 180 - defaultAngle;
                    defaultAngle = 180 + tempAngle;
                    angle = defaultAngle - 90;
                    isHitSometing = true;
                } else if(defaultAngle >= 0 && defaultAngle < 90) {
                    double tempAngle = defaultAngle - 0;
                    defaultAngle = 360 - tempAngle;
                    angle = defaultAngle - 90;
                    isHitSometing = true;
                }
            }
            if(this.PosY + image.Radius > maxY) {
                if(this.defaultAngle >= 90 && defaultAngle < 180) {
                    double tempAngle = defaultAngle - 90;
                    defaultAngle = 90 - tempAngle;
                    angle = defaultAngle - 90;
                    isHitSometing = true;
                } else if(defaultAngle >= 180 && defaultAngle < 270) {
                    double tempAngle = 270 - defaultAngle;
                    defaultAngle = 270 + tempAngle;
                    angle = defaultAngle - 90;
                    isHitSometing = true;
                }
            }


            /*ブロック*/
            for(int y = 0 ; y < block.GetLength(0) ; y++) {
                for(int x = 0 ; x < block.GetLength(1) ; x++) {
                    if(block[y,x].IsCollision == true && Hit.CheckBoxToCircle(block[y , x].Image , this.image) == true) {

                        /*角度を計算してブロックのどの面に当たったかを判定*/
                        double a = image.PosX - block[y , x].PosX;
                        double b = image.PosY - block[y , x].PosY;

                        double absA = Math.Abs(a);
                        double absB = Math.Abs(b);

                        double num = absB / absA;

                        double atan = Math.Atan(num) * (180 / Math.PI);
                        double atan2 = atan;

                        bool isX;
                        bool isY;
                        if(a < 0) {
                            isX = false;
                        } else {
                            isX = true;
                        }
                        if(b < 0) {
                            isY = false;
                        } else {
                            isY = true;
                        }

                        if(isX == true && isY == false) {
                            atan2 = 90 - atan;
                        }
                        if(isX == true && isY == true) {
                            atan2 = 90 + atan;
                        }
                        if(isX == false && isY == true) {
                            atan2 = 180 + 90 - atan;
                        }
                        if(isX == false && isY == false) {
                            atan2 = 270 + atan;
                        }

                        if(atan2 >= 45 && atan2 <= 135) {
                            //str = "右側";
                            if(this.defaultAngle >= 180 && defaultAngle < 270) {
                                double tempAngle = defaultAngle - 180;
                                defaultAngle = 180 - tempAngle;
                                angle = defaultAngle - 90;
                            } else if(defaultAngle >= 270 && defaultAngle < 360) {
                                double tempAngle = 360 - defaultAngle;
                                defaultAngle = 0 + tempAngle;
                                angle = defaultAngle - 90;
                            }
                        }else if(atan2 >= 135 && atan2 <= 225) {
                            //str = "下側";
                            if(defaultAngle >= 0 && defaultAngle < 90) {
                                double tempAngle = defaultAngle - 0;
                                defaultAngle = 180 - tempAngle;
                                angle = defaultAngle - 90;
                            } else if(defaultAngle >= 270 && defaultAngle < 360) {
                                double tempAngle = 360 - defaultAngle;
                                defaultAngle = 180 + tempAngle;
                                angle = defaultAngle - 90;
                            }
                        }else if(atan2 >= 225 && atan2 <= 315) {
                            //str = "左側";
                            if(this.defaultAngle >= 90 && defaultAngle < 180) {
                                double tempAngle = 180 - defaultAngle;
                                defaultAngle = 180 + tempAngle;
                                angle = defaultAngle - 90;
                            } else if(defaultAngle >= 0 && defaultAngle < 90) {
                                double tempAngle = defaultAngle - 0;
                                defaultAngle = 360 - tempAngle;
                                angle = defaultAngle - 90;
                            }
                        }else if(atan2 >= 315 || atan2 <= 45) {
                            //str = "上側";
                            if(this.defaultAngle >= 90 && defaultAngle < 180) {
                                double tempAngle = defaultAngle - 90;
                                defaultAngle = 90 - tempAngle;
                                angle = defaultAngle - 90;
                            } else if(defaultAngle >= 180 && defaultAngle < 270) {
                                double tempAngle = 270 - defaultAngle;
                                defaultAngle = 270 + tempAngle;
                                angle = defaultAngle - 90;
                            }
                        }

                        isHitSometing = true;

                    }                        

                }
            }

            return isHitSometing;

        }
        
    }
}
