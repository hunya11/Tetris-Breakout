using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXlib_CS.src.Comp.DrawComp.Image {
    /// <summary>
    /// Dxライブラリを用いた簡易的な表示機能
    /// </summary>
    abstract class DxImage : ImageObject {

        protected Color insideColor;
        public Color InsideColor {
            get { return insideColor; }
            set { insideColor = value; }
        }

        protected Color outsideColor;
        public Color OutsideColor {
            get { return outsideColor; }
            set { outsideColor = value; }
        }

        protected bool isOutsideFill;
        public bool IsOutsideFill {
            get {
                return isOutsideFill;
            }

            set {
                isOutsideFill = value;
            }
        }

        protected bool isInsideFill;
        public bool IsInsideFill {
            get {
                return isInsideFill;
            }

            set {
                isInsideFill = value;
            }
        }

        protected bool isInsideEnable;
        public bool IsInsideEnable {
            get {
                return isInsideEnable;
            }

            set {
                isInsideEnable = value;
            }
        }

        protected bool isOutsideEnable;
        public bool IsOutsideEnable {
            get {
                return isOutsideEnable;
            }

            set {
                isOutsideEnable = value;
            }
        }


        protected double frameSize;


        //X方向の大きさ
        protected double sizeX = -1.0;
        public double SizeX {
            get { return sizeX; }
            set { sizeX = value; }
        }

        //Y方向の大きさ
        protected double sizeY = -1.0;
        public double SizeY {
            get { return sizeY; }
            set { sizeY = value; }
        }



        protected double radius;
        public double Radius {
            get {
                return radius;
            }

            set {
                radius = value;
            }
        }




    }
}
