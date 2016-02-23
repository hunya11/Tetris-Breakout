using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXlib_CS.src.Comp.DrawComp.Image;
using DXlib_CS.src.Comp.DrawComp.Object.Effect;

namespace DXlib_CS.src.Comp.DrawComp.Object {
    class Bar :GameObject{

        public override double PosX {
            get { return posX; }
            set {
                if(value - image.SizeX/2 < moveMinX) {
                    return;
                }
                if(value + image.SizeX/2 > moveMaxX) {
                    return;
                }

                posX = value;
            }
        }

        public override double PosY {
            get { return posY; }
            set { posY = value; }
        }

        private double moveMinX;
        private double moveMaxX;

        public Bar(double posX , double posY , DxImage image , double moveMinX,double moveMaxX)
            : base(posX , posY , image) {
            this.Init();

            this.moveMinX = moveMinX;
            this.moveMaxX = moveMaxX;
        }

        public override void Init() {
            base.Init();
        }

        public override void UpData() {
            base.UpData();
        }

        public override void Draw() {
            base.Draw();
        }

    }
}
