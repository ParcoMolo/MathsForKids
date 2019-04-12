using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG512_ExamProject_Marco_Sahd_3417.Classes
{
    public class Modifier
    {
        public string Description { get; set; }
        public List<int> Operators { get; set; }
    }

    /// <summary>
    /// This class is created in order to allow the user to select a difficulty (i.e. choose between grade 1-4 difficulty)
    /// First I created the modifier class in order to get and set the description for the difficulty and also the list of operators used for the questions
    /// (The difference in difficulty between classes is the amount of operators included (Grade 1 only includes "+", Grade 2 includes "+","-", etc)
    /// </summary>
    public enum EGrades
    {
        One,
        Two,
        Three,
        Four
    }

    public class GradeDifficulty
    {

    }

    public class LevelOne : GradeDifficulty
    {
        public Modifier Modifier { get; set; } = new Modifier()
        {
            Description = "Grade 1",
            Operators = new List<int> { 1 }
        };
    }

    public class LevelTwo : GradeDifficulty
    {
        public Modifier Modifier { get; } = new Modifier()
        {
            Description = "Grade 2",
            Operators = new List<int> { 1, 2 }
        };
    }

    public class LevelThree : GradeDifficulty
    {
        public Modifier Modifier { get; } = new Modifier()
        {
            Description = "Grade 3",
            Operators = new List<int> { 1, 2, 3 }
        };
    }
    public class LevelFour : GradeDifficulty
    {
        public Modifier Modifier { get; } = new Modifier()
        {
            Description = "Grade 4",
            Operators = new List<int> { 1, 2, 3, 4 }
        };
    }
}
