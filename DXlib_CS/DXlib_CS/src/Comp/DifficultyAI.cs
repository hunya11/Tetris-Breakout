using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DXlib_CS.src.Comp {
    static class DifficultyAI{

        /// <summary>
        /// プレイヤーの感じているボールに対するストレス
        /// </summary>
        static double ballStress;
        public static double BallStress {
            get {
                return DifficultyAI.ballStress;
            }
            set {
                DifficultyAI.ballStress = value;
            }
        }

        /// <summary>
        /// プレイヤーの感じているミノに対するストレス
        /// </summary>
        static double minoStress;
        public static double MinoStress {
            get {
                return DifficultyAI.minoStress;
            }
            set {
                DifficultyAI.minoStress = value;
            }
        }

        /// <summary>
        /// ボールの難易度レベル
        /// </summary>
        static double ballDifficulty;

        /// <summary>
        /// ミノの難易度レベル
        /// </summary>
        static double minoDifficulty;
        public static double MinoDifficulty {
            get {
                return DifficultyAI.minoDifficulty;
            }
            set {
                DifficultyAI.minoDifficulty = value;
            }
        }

        /// <summary>
        /// タイマー
        /// </summary>
        private static Stopwatch difficultyTimer;

        //ストレスレベルをチェックする間隔関連
        private static double previousCheckDifficulty;
        private static double currentCheckDifficulty;
        private static double checkDifficultyTime;


        private static Queue<double> q_BallStress;
        private static Queue<double> q_MinoStress;

        static DifficultyAI() {

            difficultyTimer = new Stopwatch();

            q_BallStress = new Queue<double>();
            q_MinoStress = new Queue<double>();

            Difficulty.Init();
            Init();

        }

        public static void Init() {

            DifficultyAI.ballStress = 0;
            DifficultyAI.minoStress = 0;

            ballDifficulty = 0;
            minoDifficulty = 0;

            difficultyTimer.Reset();
            difficultyTimer.Start();
            previousCheckDifficulty = difficultyTimer.Elapsed.TotalSeconds;
            currentCheckDifficulty = difficultyTimer.Elapsed.TotalSeconds;
            checkDifficultyTime = 5.0f;

            q_BallStress.Clear();
            q_MinoStress.Clear();

        }

        public static void UpData() {

            //doubleの引き算だから有効桁数下がるかも
            if(currentCheckDifficulty - previousCheckDifficulty > checkDifficultyTime) {

                q_MinoStress.Enqueue(MinoStress);
                MinoStress = 0;
                previousCheckDifficulty = currentCheckDifficulty;
            }
            currentCheckDifficulty = difficultyTimer.Elapsed.TotalSeconds;

            if(q_MinoStress.Count > 0) {
                minoDifficulty = q_MinoStress.ToArray().Average();                
            }
        }


        public static void setDifficulty() {
            
            

        }


    }
}
