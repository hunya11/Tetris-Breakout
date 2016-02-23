using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXlib_CS.src.Comp {
    abstract class Component {

        /// <summary>
        /// 初期化処理
        /// </summary>
        public abstract void Init();


        /// <summary>
        /// 更新処理
        /// </summary>
        public abstract void UpData();


    }
}
