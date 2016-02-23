using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXlib_CS.src.Comp.DrawComp.Image;

namespace DXlib_CS.src.Comp.DrawComp.Object {
    abstract class GameObject : DrawableComponent{


        protected double posX;
        public abstract double PosX {
            get;
            set;
        }

        protected double posY;
        public abstract double PosY {
            get;
            set;
        }


        protected DxImage image;
        public DxImage Image {
            get {
                return image;
            }

            set {
                image = value;
            }
        }


        private bool isCollision;
        public bool IsCollision {
            get { return isCollision; }
            set { isCollision = value; }
        }

        public GameObject(double posX , double posY) {
            this.posX = posX;
            this.posY = posY;

            this.Init();
        }

        public GameObject(double posX , double posY , DxImage image) {
            this.posX = posX;
            this.posY = posY;

            this.Image = image;
            Image.PosX = this.posX;
            Image.PosY = this.posY;

            this.Init();
        }

        public override void Init() {
            this.isCollision = false;
        }

        public override void UpData() {
            Image.PosX = this.posX;
            Image.PosY = this.posY;
            Image.UpData();
        }

        public override void Draw() {
            Image.Draw();
        }

    }
}
