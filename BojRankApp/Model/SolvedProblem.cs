using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BojRankApp.Model
{
    class SolvedProblem
    {

        private int pid;
        public int Pid { get; set; }

        private string? name;
        public string? Name { get; set; }

        private int difficulty;
        public int Difficulty { get; set; }

        private string? tag;
        public string? Tag { get; set; }
    }
}
