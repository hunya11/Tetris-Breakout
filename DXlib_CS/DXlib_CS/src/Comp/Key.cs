using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace DXlib_CS.src.Comp {
    sealed class Key : Component{


        bool[] pushedNow;
        bool[] pushedPrev;

        byte[] state;


        public Key() {
            this.pushedNow = new bool[256];
            this.pushedPrev = new bool[256];
            this.state = new byte[256];            
        }

        public override void Init() {
            for(int i = 0 ; i < 256 ; i++) {
                this.pushedNow[i] = false;
                this.pushedPrev[i] = false;
            }
        }

        public override void UpData() {

            DX.GetHitKeyStateAll(out this.state[0]);

        }

        /// <summary>
        /// キーを押した瞬間を検知
        /// </summary>
        /// <param name="keyCode">DXライブラリ準拠のキーコード</param>
        /// <returns>true:押した,false:押していない</returns>
        public bool Pressed(int keyCode) {
            this.pushedPrev[keyCode] = this.pushedNow[keyCode];
            this.pushedNow[keyCode] = (this.state[keyCode] == 1)? true:false;
            if(this.pushedNow[keyCode] && !this.pushedPrev[keyCode]) return true;
            else return false;
        }

        /// <summary>
        /// キーを押している間は検知
        /// </summary>
        /// <param name="keyCode">DXライブラリ準拠のキーコード</param>
        /// <returns>true:押した,false:押していない</returns>
        public bool Pressing(int keyCode){
            return (this.state[keyCode] == 1)? true:false;
        }


    }
}
