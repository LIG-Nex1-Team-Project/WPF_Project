using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BojRankApp.Model
{
    public class SolvedProblem
    {
        public int Pid { get; set; }
        public string? Name { get; set; }
        public int Difficulty { get; set; }
        public List<string>? Tags { get; set; }

        public SolvedProblem(int pid, string? name, int difficulty, List<string>? tags)
        {
            Pid = pid;
            Name = name;
            Difficulty = difficulty;
            Tags = tags;
        }
    }
}
