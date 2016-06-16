using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DXlib_CS.src.Comp {
    static class DifficultyAI{




        /*ボール関係*/

        static double ballStress;
        /// <summary>
        /// プレイヤーの感じているボールに対するストレス
        /// </summary>
        public static double BallStress {
            get {
                return DifficultyAI.ballStress;
            }
            set {
                DifficultyAI.ballStress = value;
            }
        }

        static double ballStressAverage;
        /// <summary>
        /// プレイヤーの感じているボールに対するストレス平均
        /// </summary>
        public static double BallStressAverage {
            get {
                return DifficultyAI.ballStressAverage;
            }
        }

        /// <summary>
        /// 計算したストレス値がこの閾値以下ならユーザには余裕がある（難易度を上げる）
        /// </summary>
        private static double minoStressThresholdEasy;

        /// <summary>
        /// 計算したストレス値がこの閾値以上ならユーザには余裕がない（難易度を下げる）
        /// </summary>
        private static double minoStressThresholdHard;

        /// <summary>
        /// ボールのストレス値を蓄積
        /// </summary>
        private static Queue<double> q_BallStress;




        /*ミノ関係*/

        static double minoStress;
        /// <summary>
        /// プレイヤーの感じているミノに対するストレス
        /// </summary>
        public static double MinoStress {
            get {
                return DifficultyAI.minoStress;
            }
            set {
                DifficultyAI.minoStress = value;
            }
        }        

        static double minoStressAverage;
        /// <summary>
        /// プレイヤーの感じているミノに対するストレス平均
        /// </summary>
        public static double MinoStressAverage {
            get {
                return DifficultyAI.minoStressAverage;
            }
        }

        /// <summary>
        /// 計算したストレス値がこの閾値以下ならユーザには余裕がある（難易度を上げる）
        /// </summary>
        private static double ballStressThresholdEasy;

        /// <summary>
        /// 計算したストレス値がこの閾値以上ならユーザには余裕がない（難易度を下げる）
        /// </summary>
        private static double ballStressThresholdHard;

        /// <summary>
        /// ミノのストレス値を蓄積
        /// </summary>
        private static Queue<double> q_MinoStress;




        /*その他*/

        /// <summary>
        /// タイマー
        /// </summary>
        private static Stopwatch difficultyTimer;

        //ストレスレベルをチェックする間隔関連
        private static double previousCheckDifficulty;
        private static double currentCheckDifficulty;
        private static double checkDifficultyTime;

        //キューに何個つめこむか
        private static int maxQueue;
        




        static DifficultyAI() {

            difficultyTimer = new Stopwatch();

            q_BallStress = new Queue<double>();
            q_MinoStress = new Queue<double>();

            Difficulty.Init();
            Init();

        }

        public static void Init() {

            ballStress = 0;
            minoStress = 0;

            ballStressAverage = 0;
            minoStressAverage = 0;

            difficultyTimer.Reset();
            difficultyTimer.Start();
            previousCheckDifficulty = difficultyTimer.Elapsed.TotalSeconds;
            currentCheckDifficulty = difficultyTimer.Elapsed.TotalSeconds;
            checkDifficultyTime = 1.0f;

            q_BallStress.Clear();
            q_MinoStress.Clear();

            maxQueue = 5;

            minoStressThresholdEasy = 2;
            minoStressThresholdHard = 3;

            ballStressThresholdEasy = 2;
            ballStressThresholdHard = 3;

        }

        public static void UpData() {

            //doubleの引き算だから有効桁数下がるかも
            if(currentCheckDifficulty - previousCheckDifficulty > checkDifficultyTime) {

                //ストレス値の更新

                if(q_MinoStress.Count >= maxQueue) q_MinoStress.Dequeue();
                q_MinoStress.Enqueue(MinoStress);
                MinoStress = 0;

                if(q_BallStress.Count >= maxQueue) q_BallStress.Dequeue();
                q_BallStress.Enqueue(ballStress);
                ballStress = 0;


                previousCheckDifficulty = currentCheckDifficulty;
            }
            currentCheckDifficulty = difficultyTimer.Elapsed.TotalSeconds;


            //AI
            //難易度の調整

            if(q_MinoStress.Count >= maxQueue) {
                minoStressAverage = q_MinoStress.ToArray().Average();

                if(minoStressAverage <= minoStressThresholdEasy) {
                    //簡単そうなら難易度を上げて、キューをクリア
                    Difficulty.MinoDifficultyLevel++;
                    q_MinoStress.Clear();
                } else if(minoStressAverage >= minoStressThresholdHard) {
                    //難しそうなら難易度を下げて、キューをクリア
                    Difficulty.MinoDifficultyLevel--;
                    q_MinoStress.Clear();
                }

            }


            if(q_BallStress.Count >= maxQueue) {
                ballStressAverage = q_BallStress.ToArray().Average();

                if(ballStressAverage <= ballStressThresholdEasy) {
                    //簡単そうなら難易度を上げて、キューをクリア
                    Difficulty.BallDifficultyLevel++;
                    q_BallStress.Clear();
                } else if(ballStressAverage >= ballStressThresholdHard) {
                    //難しそうなら難易度を下げて、キューをクリア
                    Difficulty.BallDifficultyLevel--;
                    q_BallStress.Clear();
                }

            }


        }
        


    }
}
