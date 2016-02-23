using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXlib_CS.src.Comp.DrawComp.Image;
using DxLibDLL;

namespace DXlib_CS.src.Comp.DrawComp.Object.Effect {
    class EffectBreakBlock : GameObject{

        public override double PosX {
            get { return posX; }
            set { posX = value; }
        }

        public override double PosY {
            get { return posY; }
            set { posY = value; }
        }

        private bool isEnable;
        public bool IsEnable {
            get {
                return isEnable;
            }

            set {
                isEnable = value;
            }
        }

        /// <summary>
        /// 現在のエフェクトの半径
        /// </summary>
        private double radiusSize;

        /// <summary>
        /// エフェクトが1フレームごとにどれだけ大きくなるか
        /// </summary>
        private double radiusSizePerFrame;

        /// <summary>
        /// 画像の濃さ。255が最大で最も濃い
        /// </summary>
        private double alphaPal;

        /// <summary>
        /// 画像の濃さが1フレームごとにどれだけ小さくなるか
        /// </summary>
        private double alphaPalPerFrame;


        public EffectBreakBlock(double posX , double posY,Color color)
            : base(posX , posY) {
            this.Init();


            IsEnable = true;

            this.image = new ImageCircle(radiusSize , color , color , 1);
            image.IsInsideFill = false;
            image.IsOutsideFill = false;

            
            radiusSizePerFrame = 5;

            alphaPalPerFrame = 5;
        }

        public override void Init() {
            radiusSize = 0;
            alphaPal = 255;
        }

        public override void UpData() {
            base.UpData();

            if(IsEnable == true) {
                radiusSize += radiusSizePerFrame;
                alphaPal -= alphaPalPerFrame;

                this.image.Radius = radiusSize;

                //完全に透明になったら
                if(alphaPal < 0) {
                    this.Init();
                    this.isEnable = false;
                }
            }

        }

        public override void Draw() {

            if(IsEnable == true) {
                this.image.InsideColor.Alpha = alphaPal;
                this.image.OutsideColor.Alpha = alphaPal;
                base.Draw();
            }

        }
    }
}
