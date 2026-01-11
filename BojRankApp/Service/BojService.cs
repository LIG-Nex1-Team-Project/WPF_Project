using BojRankApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Documents;

namespace BojRankApp.Service
{
    internal class BojService
    {

        private readonly HttpClient _client = new HttpClient();
        private const string SolvedProblemApiPrefix = "https://solved.ac/api/v3/search/problem";
        private const string UserApiPrefix = "https://solved.ac/api/v3/user/show";
        public async Task<List<SolvedProblem>> LoadSolvedProblem(string userId)
        {
            var builder = new UriBuilder(SolvedProblemApiPrefix);

            var query = HttpUtility.ParseQueryString(builder.Query);
            string encodedQuery = $"s@{userId}";

            query["query"] = encodedQuery;
            query["sort"] = "level";
            query["direction"] = "desc";

            builder.Query = query.ToString();
            Uri uri = builder.Uri;

            var json = await _client.GetStringAsync(uri);
            var root = JsonNode.Parse(json);

            var problems = new List<SolvedProblem>();

            foreach (var item in root["items"]!.AsArray())
            {
                int problemId = item!["problemId"]!.GetValue<int>();
                string titleKo = item["titleKo"]!.GetValue<string>();
                int level = item["level"]!.GetValue<int>();

                var tags = new List<string>();
        
                foreach (var tag in item["tags"]!.AsArray())
                {
                    var displayNames = tag!["displayNames"]!.AsArray();

                    var koName = displayNames
                        .FirstOrDefault(d =>
                            d!["language"]!.GetValue<string>() == "ko"
                        )?["name"]?.GetValue<string>();

                    if (koName != null)
                    {
                        tags.Add(koName);
                    }
                }

                problems.Add(new SolvedProblem(
                    pid: problemId,
                    name: titleKo,
                    difficulty: level,
                    tags: tags
                ));
            }

            return problems;
        }
    }
}
