using BojRankApp.Model;
using BojRankApp.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Text.Json;


namespace BojRankApp.ViewModel
{
    public partial class UserViewModel: ObservableObject
    {

        private BojService bojService;

        public ObservableCollection<User> Users { get; } 
        public ObservableCollection<SolvedProblem> Problems { get; }

        [ObservableProperty]
        private string userId = string.Empty;

        [ObservableProperty]
        private SUser? selectedSUser; // 통계

        [ObservableProperty]
        private User selectedUser;

        public UserViewModel()
        {
            bojService = new BojService();
            Users = new ObservableCollection<User>();
            Problems = new ObservableCollection<SolvedProblem>();
            LoadFile();
        }
        [RelayCommand]
        public async Task AddUser(string userId)
        {
            User user = await bojService.LoadUser(userId);
            var target = Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                MessageBox.Show("사용자를 찾을 수 없습니다.");
                return;
            }
            else if(target != null)
            {
                MessageBox.Show("이미 있는 사용자입니다.");
                return;
            }
                Users.Add(user);
            SelectedUser = user;
            SaveFile();
        }
        [RelayCommand]
        public void DelUser()
        {
            //User user = await bojService.LoadUser(userId);
            //User target = Users.FirstOrDefault(u => u.Id == user.Id);

            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
            }
            SaveFile();
        }
        [RelayCommand]
        public async Task ResetUser()
        {
            var currentList = Users.ToList();
            Users.Clear();

            foreach (var user in currentList)
            {
                var updatedUser = await bojService.LoadUser(user.Id);
                Users.Add(updatedUser);          
            }

            var sorted = Users.OrderByDescending(u => u.Rating).ToList();
            Users.Clear();

            foreach (var u in sorted)
            {
                Users.Add(u);
            }

            SaveFile();
        }
        partial void OnSelectedUserChanged(User value)
        {
            Problems.Clear();

            if (value == null) return;

            foreach (var problem in value.SolvedProblems)
            {
                Problems.Add(problem);
            }

            if (value != null)
            {
                // 선택된 사용자를 기반으로 SUser(통계 정보) 생성
                
                SelectedSUser = new SUser(value);
                //Debug.WriteLine(SelectedSUser.StatisticsTags);
            }
            else
            {
                SelectedSUser = null;
            }
        }
        // 저장 
        public void SaveFile()
        {
            string path = "users.txt";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true // 보기 좋게
            };

            string json = JsonSerializer.Serialize(Users, options);
            File.WriteAllText(path, json);
        }

        public void LoadFile()
        {
            string path = "users.txt";

            if (!File.Exists(path))
                return;

            string json = File.ReadAllText(path);
            var users = JsonSerializer.Deserialize<List<User>>(json);

            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
    }

}
