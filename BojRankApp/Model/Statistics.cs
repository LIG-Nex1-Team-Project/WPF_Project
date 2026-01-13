using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BojRankApp.Model
{
    public class TagItem
    {
        public string? TagName { get; set; }
        public int TagCount { get; set; }
        public double TagPercent { get; set; }

        public TagItem(string? tagName, int tagCount, double tagPercent)
        {
            TagName = tagName;
            TagCount = tagCount;
            TagPercent = tagPercent;
        }
    }

    public class TierItem
    {
        public string? TierName { get; set; }
        public int TierCount { get; set; }
        public double TierPercent { get; set; }

        public TierItem(string? tierName, int tierCount, double tierPercent)
        {
            TierName = tierName;
            TierCount = tierCount;
            TierPercent = tierPercent;
        }
    }

    public class SUser
    {
        public string Id { get; set; }
        public List<TagItem> StatisticsTags { get; set; }
        public List<TierItem> StatisticsTiers { get; set; }

        public int TagSum { get; set; }

        public SUser() { }

        public SUser(User user)
        {
            Id = user.Id;
            StatisticsTags = LoadTagList(user);
            StatisticsTiers = LoadTierList(user);
        }


        List<TagItem> LoadTagList(User user)
        {
            int tagcnt = 0;
            Dictionary<string, int> Tag = new Dictionary<string, int>();
            foreach (SolvedProblem problem in user.SolvedProblems)
            {
                foreach (var tag in problem.Tags)
                {
                    Tag[tag] = 0;
                }
            }
            foreach (SolvedProblem problem in user.SolvedProblems)
            {
                foreach (var tag in problem.Tags)
                {
                    Tag[tag]++;
                    tagcnt++;
                }
            }

            List<TagItem> TagItems = new List<TagItem>();
            foreach (var item in Tag)
            {
                TagItem temp = new TagItem(item.Key, item.Value, ((double)item.Value / tagcnt * 100));
                TagItems.Add(temp);
            }
            return TagItems.OrderByDescending(x => x.TagPercent).ToList();
        }

        List<TierItem> LoadTierList(User user)
        {
            Dictionary<string, int> Tier = new Dictionary<string, int>();
            foreach (SolvedProblem problem in user.SolvedProblems)
            {
                Tier[int2string(problem.Difficulty)] = 0;
            }
            foreach (SolvedProblem problem in user.SolvedProblems)
            {
                Tier[int2string(problem.Difficulty)]++;
            }

            List<TierItem> TierItems = new List<TierItem>();
            foreach (var item in Tier)
            {
                TierItem temp = new TierItem(item.Key, item.Value, ((double)item.Value / user.SolvedCount * 100));
                TierItems.Add(temp);
            }
            return TierItems.OrderByDescending(x => x.TierPercent).ToList();
        }

        string int2string(int tierI)
        {
            if (tierI == 0)
            {
                return "Unreated";
            }
            else if (tierI >= 1 && tierI <= 5)
            {
                return "Bronze";
            }
            else if (tierI >= 6 && tierI <= 10)
            {
                return "Silver";
            }
            else if (tierI >= 11 && tierI <= 15)
            {
                return "Gold";
            }
            else if (tierI >= 16 && tierI <= 20)
            {
                return "Platinum";
            }
            else if (tierI >= 21 && tierI <= 25)
            {
                return "Diamond";
            }
            else if (tierI >= 26 && tierI <= 30)
            {
                return "Ruby";
            }
            else if (tierI == 31)
            {
                return "Master";
            }
            return "오류발생:int2string()";
        }

    }

    /*
    class TagElement
    {
        public int Idx { get; set; }
        public string? Tag { get; set; }
        public int TagCnt { get; set; }

        public TagEle(int idx, string? tag, int tagcnt)
        {
            Idx = idx;
            Tag = tag;
            TagCnt = tagcnt;
        }

    }

    class TierEle
    {
        public string Id { get; set; }
        public string? Tier { get; set; }
        public int TierCnt { get; set; }

        public TierEle(string id, int tier, int tiercnt)
        {

            Id = id;
            Tier = int2string(tier);
            //Tier = tier;
            TierCnt = tiercnt;
        }

        

    }
    */
}
