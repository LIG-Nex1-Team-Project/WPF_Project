using BojRankApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Documents;

namespace BojRankApp.Service
{
    internal class BojService
    {
        public ObservableCollection<User> Users { get; }
        private readonly HttpClient _client = new HttpClient();
        private const string ProblemApiPrefix = "https://solved.ac/api/v3/search/problem";
        private const string UserApiPrefix = "https://solved.ac/api/v3/user/show";

        private const int PageElementNum = 50;
        private const int PageElementNum2 = 51;
        private const int PageElementNum3 = 53;

        public BojService()
        {
            Users = new ObservableCollection<User>();
        }
        public async Task<User> LoadUser(string userId)
        {
            // JSON 형식으로 변환
            var builder = new UriBuilder(UserApiPrefix);

            var query = HttpUtility.ParseQueryString(builder.Query);
            query["handle"] = userId;

            builder.Query = query.ToString();
            Uri uri = builder.Uri;

            // Debug.WriteLine(uri.ToString());
            try
            {
                var json = await _client.GetStringAsync(uri); // bad request in Debug
                var node = JsonNode.Parse(json);

                int tier = node!["tier"]!.GetValue<int>();
                int rating = node!["rating"]!.GetValue<int>();

                int solvedCount = node["solvedCount"]!.GetValue<int>();
                //int unsolvedCount = node["unsolvedCount"]!.GetValue<int>();

                int page = (solvedCount / PageElementNum) + 1;

                List<SolvedProblem> solvedProblems = await LoadSolvedProblem(userId, page);
                List<UnSolvedProblem> unsolvedProblems = await LoadUnSolvedProblem(userId);
                int unsolvedCount = unsolvedProblems.Count;

                return new User(
                   id: userId,
                   tier: tier,
                   rating: rating,
                   solvedCount: solvedCount,
                   solvedProblems: solvedProblems,
                   unsolvedCount: unsolvedCount,
                   unsolvedProblems: unsolvedProblems
                   );
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        } // getURL_User

        public async Task<List<SolvedProblem>> LoadSolvedProblem(string userId, int page)
        {
            var builder = new UriBuilder(ProblemApiPrefix);

            var query = HttpUtility.ParseQueryString(builder.Query);
            string encodedQuery = $"s@{userId}";

            query["query"] = encodedQuery;
            query["sort"] = "level";
            query["direction"] = "desc";

            var problems = new List<SolvedProblem>();

            for (int i = 1; i <= page; i++)
            {
                query["page"] = i.ToString();

                builder.Query = query.ToString();
                Uri uri = builder.Uri;

                var json = await _client.GetStringAsync(uri);
                var root = JsonNode.Parse(json);


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
            }

            return problems;
        } // getURL_Problem

        public async Task<List<UnSolvedProblem>> LoadUnSolvedProblem(string userId)
        {
            var builder = new UriBuilder(ProblemApiPrefix);

            var query = HttpUtility.ParseQueryString(builder.Query);
            string encodedQuery = $"t@{userId} !s@{userId}";

            query["query"] = encodedQuery;
            query["sort"] = "level";
            query["direction"] = "desc";

            builder.Query = query.ToString();
            Uri uri = builder.Uri;

            var json = await _client.GetStringAsync(uri);
            var root = JsonNode.Parse(json);

            var problems = new List<UnSolvedProblem>();

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

                problems.Add(new UnSolvedProblem(
                    pid: problemId,
                    name: titleKo,
                    difficulty: level,
                    tags: tags
                ));
            }

            return problems;
        } // getURL_Problem

        public void SaveFile(ObservableCollection<User> users)
        {
            string path = "users.txt";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true // 보기 좋게
            };

            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(path, json);
        }

        public ObservableCollection<User> LoadFile()
        {
            string path = "users.txt";

            if (!File.Exists(path))
                return new ObservableCollection<User>();

            string json = File.ReadAllText(path);
            var users = JsonSerializer.Deserialize<List<User>>(json);


            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
            return Users;
        }
    }
}
