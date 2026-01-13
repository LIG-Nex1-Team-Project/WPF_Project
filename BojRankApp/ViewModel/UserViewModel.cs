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
            Problems = new ObservableCollection<SolvedProblem>();
            Users = bojService.LoadFile();
        }

        [RelayCommand]
        public async Task AddUser(string userId)
        {

            var target = Users.FirstOrDefault(u => u.Id == userId);
            if (target != null)
            {
                MessageBox.Show("이미 존재하는 사용자입니다.");
                return;
            }

            User user = await bojService.LoadUser(userId);
            if (user == null)
            {
                MessageBox.Show("사용자를 찾을 수 없습니다.");
                return;
            }

            Users.Add(user);
            SelectedUser = user;
            bojService.SaveFile(Users);
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
            bojService.SaveFile(Users);
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
            bojService.SaveFile(Users);
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
         
    }

}
