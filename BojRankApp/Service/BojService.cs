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
        
    }
}
