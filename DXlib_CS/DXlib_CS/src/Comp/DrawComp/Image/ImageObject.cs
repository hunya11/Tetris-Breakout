using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXlib_CS.src.Comp.DrawComp.Image {
    abstract class ImageObject :DrawableComponent{

        //中心のX座標
        protected double posX = -1.0;
        public double PosX {
            get { return posX; }
            set { posX = value; }
        }

        //中心のY座標
        protected double posY = -1.0;
        public double PosY {
            get { return posY; }
            set { posY = value; }
        }
        

    }
}
