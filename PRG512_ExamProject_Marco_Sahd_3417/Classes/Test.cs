using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG512_ExamProject_Marco_Sahd_3417.Classes
{
    public class Test
    {
        public Person Person {get;set;}
        public EGrades Grade { get; set; }
        public EDifficulty Difficulty { get; set; }
        public List<string> Questions { get; set; }
        public List<string> Answers { get; set; }
        public bool IsPracticeMode { get; set; }
    }
}
