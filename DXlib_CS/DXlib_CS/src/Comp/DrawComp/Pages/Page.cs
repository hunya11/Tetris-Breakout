using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.Diagnostics;

namespace DXlib_CS.src.Comp.DrawComp.Pages {
    abstract class Page : DrawableComponent{

        protected static Key keys;

        protected static int pageState;
        public static int PageState {
            get { return Page.pageState; }
        }

        protected static Stopwatch globalTimer;
        public static Stopwatch GlobalTimer {
            get {
                return globalTimer;
            }
        }

        protected Stopwatch localTimer;
        public Stopwatch LocalTimer {
            get {
                return localTimer;
            }
        }


        public enum State:int {
            END = -1, TITLE, GAME,AI

        };
        
        static Page() {
            Page.keys = new Key();
            globalTimer = new Stopwatch();
            globalTimer.Start();
        }

        public Page() {
            localTimer = new Stopwatch();
            localTimer.Start();
        }

        ~Page(){
        }

        public override void Init() {
            DX.InitGraph();
            DX.InitFontToHandle();
            localTimer.Reset();
            localTimer.Start();
        }

        public override void UpData() {
            keys.UpData();
        }

        public abstract void LoadResource();




    }
}
