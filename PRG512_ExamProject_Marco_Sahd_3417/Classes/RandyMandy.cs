using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRG512_ExamProject_Marco_Sahd_3417.Classes
{
    public class RandyMandy
    {
        public List<Tuple<string, string>> _generatedQnA;

        public RandyMandy()
        {

        }

        public async Task<List<Tuple<string, string>>> GenerateThisTest(TestParams testparams)
        {
            _generatedQnA = new List<Tuple<string, string>>();
            var mod = GetTestModifier(testparams);
            var count = 0;
            while (count < testparams.Questions)
            {
                Task<string> taskA = Task.Run(() =>
                   ShuffleMeAnOperationSon(mod, testparams.Difficulty)
                );

                // Execute the continuation when the antecedent finishes.
                Tuple<string, string> taskB = await taskA.ContinueWith(queuedTask =>
                {
                    var operation = taskA.Result;
                    Expression shnarfy = new Expression(operation);
                    return new Tuple<string, string>(operation, $"{shnarfy.calculate()}");
                });

                _generatedQnA.Add(new Tuple<string, string>(taskB.Item1, taskB.Item2));
                count++;
            }
            return _generatedQnA;
        }

        private static Random rndOperation = new Random();
        private static string ShuffleMeAnOperationSon(Modifier modifier, EDifficulty difficulty)
        {            
            var list = modifier.Operators;
            var mychosenNumber = list[rndOperation.Next(0, list.Count)];

            Tuple<int, int> myNumbers = ShuffleMeANumberSon(difficulty);
            switch (mychosenNumber)
            {
                case 1:
                    return $"{myNumbers.Item1}+{myNumbers.Item2}";
                case 2:
                    return $"{myNumbers.Item1}-{myNumbers.Item2}";
                case 3:
                    return $"{myNumbers.Item1}*{myNumbers.Item2}";
                case 4:
                    return $"{myNumbers.Item1}/{myNumbers.Item2}";
                default:
                    return $"{myNumbers.Item1}+{myNumbers.Item2}";
            }
        }

        private static Random randomNum = new Random();
        private static Tuple<int, int> ShuffleMeANumberSon(EDifficulty difficulty)
        {
            switch (difficulty)
            {
                case EDifficulty.Easy:
                    return new Tuple<int, int>(randomNum.Next(0, 20), randomNum.Next(0, 20));
                case EDifficulty.Hard:
                    return new Tuple<int, int>(randomNum.Next(0, 30), randomNum.Next(0, 30));
                case EDifficulty.Harder:
                    return new Tuple<int, int>(randomNum.Next(15, 30), randomNum.Next(15, 30));
                default:
                    return new Tuple<int, int>(randomNum.Next(0, 20), randomNum.Next(0, 20));
            }
        }


        private Modifier GetTestModifier(TestParams testParams)
        {
            switch (testParams.Grade)
            {
                case EGrades.One:
                    return new LevelOne().Modifier;
                case EGrades.Two:
                    return new LevelTwo().Modifier;
                case EGrades.Three:
                    return new LevelThree().Modifier;
                case EGrades.Four:
                    return new LevelFour().Modifier;
                default:
                    return new LevelOne().Modifier;
            }
        }

    }
}



