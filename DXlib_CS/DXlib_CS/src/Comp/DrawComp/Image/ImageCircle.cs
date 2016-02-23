using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace DXlib_CS.src.Comp.DrawComp.Image {
    class ImageCircle : DxImage {


        public ImageCircle(double radius , Color inside , Color outside , double frameSize)
            : this(0.0d , 0.0d , radius , inside , outside , frameSize) {
        }

        public ImageCircle(double posX , double posY , double radius , Color inside , Color outside , double frameSize) {

            this.Init();
            this.posX = posX;
            this.posY = posY;
            this.Radius = radius;

            this.InsideColor = inside;
            this.outsideColor = outside;

            this.isInsideFill = true;
            this.isOutsideFill = true;

            this.isInsideEnable = true;
            this.isOutsideEnable = true;

            this.frameSize = frameSize;
        }

        public override void Init() {
            this.PosX = 0.0;
            this.PosY = 0.0;
            this.Radius = 1.0;
        }

        public override void UpData() {
        }

        public override void Draw() {
            if(isOutsideEnable == true) {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA , (int)(outsideColor.Alpha + 0.5));
                DX.DrawCircle((int)(posX + 0.5) , (int)(posY + 0.5) , (int)(Radius + 0.5) , this.outsideColor.GetDxColor() , (isOutsideFill) ? 1 : 0);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND , (int)(outsideColor.Alpha + 0.5));
            }
            if(isInsideEnable == true) {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA , (int)(InsideColor.Alpha + 0.5));
                DX.DrawCircle((int)(posX + 0.5) , (int)(posY + 0.5) , (int)(Radius - frameSize + 0.5) , this.InsideColor.GetDxColor() , (isInsideFill) ? 1 : 0);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND , (int)(InsideColor.Alpha + 0.5));
            }
        }

    }
}
