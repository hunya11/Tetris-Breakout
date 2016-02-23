using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace DXlib_CS.src.Comp.DrawComp.Image {
    /// <summary>
    /// Dxライブラリを用いた簡易描画
    /// 四角
    /// </summary>
    class ImageBox : DxImage{




        /// <summary>
        /// 四角形の画像作成
        /// </summary>
        /// <param name="sizeX">大きさX</param>
        /// <param name="sizeY">大きさY</param>
        /// <param name="inside">内側の色</param>
        /// <param name="outside">外側（枠）の色</param>
        /// <param name="frameSize">枠の大きさ</param>
        public ImageBox(double sizeX , double sizeY , Color inside , Color outside , double frameSize)
            : this(0.0d , 0.0d , sizeX , sizeY , inside , outside , frameSize) {            
        }

        /// <summary>
        /// 四角形の画像作成
        /// </summary>
        /// <param name="posX">作成するX座標</param>
        /// <param name="posY">作成するY座標</param>
        /// <param name="sizeX">大きさX</param>
        /// <param name="sizeY">大きさY</param>
        /// <param name="inside">内側の色</param>
        /// <param name="outside">外側（枠）の色</param>
        /// <param name="frameSize">枠の大きさ</param>
        public ImageBox(double posX , double posY , double sizeX , double sizeY , Color inside , Color outside , double frameSize) {
            this.Init();
            this.posX = posX;
            this.posY = posY;
            this.sizeX = sizeX;
            this.sizeY = sizeY;

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
            this.SizeX = 0.0;
            this.SizeY = 0.0;
        }

        public override void UpData() {
        }

        public override void Draw() {
            double drawX = posX - (sizeX/2);
            double drawY = posY - (sizeY/2);

            if(isOutsideEnable == true) {                
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA , (int)(outsideColor.Alpha + 0.5));
                DX.DrawBox((int)(drawX + 0.5) , (int)(drawY + 0.5) , (int)(drawX + sizeX + 0.5) , (int)(drawY + sizeY + 0.5) , this.outsideColor.GetDxColor() , (isOutsideFill) ? 1 : 0);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND , (int)(outsideColor.Alpha + 0.5));
            }

            if(isInsideEnable == true) {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA , (int)(InsideColor.Alpha + 0.5));
                DX.DrawBox((int)(drawX + frameSize + 0.5) , (int)(drawY + frameSize + 0.5) , (int)(drawX + sizeX - frameSize + 0.5) , (int)(drawY + sizeY - frameSize + 0.5) , this.InsideColor.GetDxColor() , (isInsideFill) ? 1 : 0);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND , (int)(InsideColor.Alpha + 0.5));
            }
        }


    }
}
