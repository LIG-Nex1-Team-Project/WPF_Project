using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BojRankApp.Model
{
    class User
    {
        private string id;
        public string Id{ get; set; }

        private int tier;
        public int Tier { get; set; }

        private int rating;
        public int Rating{ get; set; }

        private int solvedCount;
        public int SolvedCount { get; set; }

        public List<SolvedProblem>? solvedProblem;
        private List<SolvedProblem>? SolvedProblem { get; set; }
    }
}
