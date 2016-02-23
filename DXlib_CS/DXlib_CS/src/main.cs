using DxLibDLL;
using System;

namespace DXlib_CS.src {
    class test{

        [STAThread]
        static void Main(){
            Frame frame = new Frame(1024,768);
            frame.Run();

            return;

        }

    }
}