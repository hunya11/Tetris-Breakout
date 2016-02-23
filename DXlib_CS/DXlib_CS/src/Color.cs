using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace DXlib_CS.src {
    class Color {

        int r;
        public int R {
            get { return r; }
            set { r = value; }
        }

        int g;
        public int G {
            get { return g; }
            set { g = value; }
        }

        int b;
        public int B {
            get { return b; }
            set { b = value; }
        }

        double alpha;
        public double Alpha {
            get {
                return alpha;
            }

            set {
                if(value > 255) {
                    value = 255;
                }
                alpha = value;
            }
        }


        public Color(int r , int g , int b,double alpha) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.alpha = alpha;
        }

        public Color(int r , int g , int b) : this(r , g , b , 255) {
        }

        public uint GetDxColor() {
            return DX.GetColor(r , g , b);
        }

        public Color Clone() {
            Color temp = new Color(0,0,0);
            temp.r = this.r;
            temp.g = this.g;
            temp.b = this.b;
            return temp;
        }

    }
}
