using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BojRankApp.Model
{
    public class User
    {
        public const string TierImageUrlPrefix = "https://images.weserv.nl/?url=static.solved.ac/tier_small";
        public const string TierImageUrlPostfix = ".svg&output=png";

        public string Id{ get; set; }
        public int Tier { get; set; }
        public string TierImageUrl { get { return $"{TierImageUrlPrefix}/{Tier}{TierImageUrlPostfix}"; } }
        public int Rating{ get; set; }
        public int SolvedCount { get; set; }
        public int UnSolvedCount { get; set; }
        public List<SolvedProblem>? SolvedProblems { get; set; }
        public List<UnSolvedProblem>? UnSolvedProblems { get; set; }

        public User( 
            string id,
            int tier,
            int rating,
            int solvedCount,
            int unsolvedCount,
            List<SolvedProblem> solvedProblems,
            List<UnSolvedProblem> unsolvedProblems
            )
        {
            Id = id;
            Tier = tier;
            Rating = rating;
            SolvedCount = solvedCount;
            UnSolvedCount = unsolvedCount;
            SolvedProblems = solvedProblems;
            UnSolvedProblems = unsolvedProblems;
        }

    }
}
