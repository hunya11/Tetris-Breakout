using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXlib_CS.src {
    static class Difficulty {

        private static int minoDifficultyLevel;
        /// <summary>
        /// テトリス（ミノ）の難易度
        /// </summary>
        public static int MinoDifficultyLevel {
            get {
                return Difficulty.minoDifficultyLevel;
            }
            set {
                if(value >= 0 && value < minoDropWaitTime.Length) {
                    Difficulty.minoDifficultyLevel = value;
                }
            }
        }



        private static int ballDifficultyLevel;
        /// <summary>
        /// ブロック崩し（ボール）の難易度
        /// </summary>
        public static int BallDifficultyLevel {
            get {
                return Difficulty.ballDifficultyLevel;
            }
            set {
                if(value >= 0 && value < ballPower.Length) {
                    Difficulty.ballDifficultyLevel = value;
                }
            }
        }



        private static double[] minoDropWaitTime;
        /// <summary>
        /// ミノの落下時間
        /// </summary>
        public static double MinoDropWaitTime {
            get {
                return Difficulty.minoDropWaitTime[MinoDifficultyLevel];
            }
        }


        private static double[] minoPlayWaitTime;
        /// <summary>
        /// ミノが固定されるまでの遊び時間
        /// </summary>
        public static double MinoPlayWaitTime {
            get {
                return Difficulty.minoPlayWaitTime[MinoDifficultyLevel];
            }
        }

        private static double[] ballPower;
        /// <summary>
        /// ボールのスピード
        /// </summary>
        public static double BallPower {
            get {
                return Difficulty.ballPower[BallDifficultyLevel];
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
            minoDifficultyLevel = 0;
            ballDifficultyLevel = 0;

        }
    }
}
