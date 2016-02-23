using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXlib_CS.src.Comp.DrawComp.Image;
using DXlib_CS.src.Comp.DrawComp.Object.Effect;

namespace DXlib_CS.src.Comp.DrawComp.Object {
    class Block : GameObject{

        public override double PosX {
            get { return posX; }
            set { posX = value; }
        }
        
        public override double PosY {
            get { return posY; }
            set { posY = value; }
        }

        public Block(double posX , double posY , DxImage image)
            : base(posX , posY , image) {
                this.Init();
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
