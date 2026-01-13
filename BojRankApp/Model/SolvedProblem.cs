using BojRankApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;



namespace BojRankApp.Model
{
    public class Problem
    {
        public const string TierImageUrlPrefix = "https://images.weserv.nl/?url=static.solved.ac/tier_small";
        public const string TierImageUrlPostfix = ".svg&output=png";
        public int Pid { get; set; }
        public string? Name { get; set; }
        public int Difficulty { get; set; }
        public string TierImageUrl { get { return $"{TierImageUrlPrefix}/{Difficulty}{TierImageUrlPostfix}"; } }

        public List<string>? Tags { get; set; }
    }
}

public class SolvedProblem : Problem
{
    public SolvedProblem(int pid, string? name, int difficulty, List<string>? tags)
    {
        Pid = pid;
        Name = name;
        Difficulty = difficulty;
        Tags = tags;
    }
}
public class UnSolvedProblem : Problem
{
    public UnSolvedProblem(int pid, string? name, int difficulty, List<string>? tags)
    {
        Pid = pid;
        Name = name;
        Difficulty = difficulty;
        Tags = tags;
    }
}

