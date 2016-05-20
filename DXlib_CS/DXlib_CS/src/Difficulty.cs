using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXlib_CS.src {
    static class Difficulty {

        private static int difficultyLevel;
        public static int DifficultyLevel {
            get {
                return Difficulty.difficultyLevel;
            }
            set {
                if(value >= 0 && value < minoDropWaitTime.Length) {
                    Difficulty.difficultyLevel = value;
                }
            }
        }


        /// <summary>
        /// ミノの落下時間
        /// </summary>
        private static double[] minoDropWaitTime;
        public static double MinoDropWaitTime {
            get {
                return Difficulty.minoDropWaitTime[DifficultyLevel];
            }
        }

        /// <summary>
        /// ミノが固定されるまでの遊び時間
        /// </summary>
        private static double[] minoPlayWaitTime;
        public static double MinoPlayWaitTime {
            get {
                return Difficulty.minoPlayWaitTime[DifficultyLevel];
            }
        }

        /// <summary>
        /// ボールのスピード
        /// </summary>
        private static double[] ballPower;
        public static double BallPower {
            get {
                return Difficulty.ballPower[DifficultyLevel];
            }
        }

        static Difficulty() {

            int numLevel = IniFileHandler.GetSectionCount(@"./Difficulty.ini");
            minoDropWaitTime = new double[numLevel];
            minoPlayWaitTime = new double[numLevel];
            ballPower = new double[numLevel];

            for(int i = 0 ; i < numLevel ; i++) {
                minoDropWaitTime[i] = double.Parse(IniFileHandler.GetIniValue("level" + i , "minoDropWaitTime" , @"./Difficulty.ini"));
                minoPlayWaitTime[i] = double.Parse(IniFileHandler.GetIniValue("level" + i , "minoPlayWaitTime" , @"./Difficulty.ini"));
                ballPower[i] = double.Parse(IniFileHandler.GetIniValue("level" + i , "ballPower" , @"./Difficulty.ini"));
            }

            Init();
        }

        public static void Init() {
            difficultyLevel = 0;

        }
    }
}
