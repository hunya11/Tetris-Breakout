using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXlib_CS.src.Comp.DrawComp {
    abstract class DrawableComponent : Component{

        /// <summary>
        /// 描画処理
        /// </summary>
        public abstract void Draw();
    
    }
}
