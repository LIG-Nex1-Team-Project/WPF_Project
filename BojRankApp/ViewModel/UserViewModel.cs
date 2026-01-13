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

namespace BojRankApp.ViewModel
{
    public partial class UserViewModel: ObservableObject
    {

        private BojService bojService;

        public ObservableCollection<User> Users { get; }
        //public ObservableCollection<SolvedProblem> Problems { get; }
        public ObservableCollection<UnSolvedProblem> Problems { get; }
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
            Problems = new ObservableCollection<UnSolvedProblem>();  // 1번
        }

        [RelayCommand]
        public async Task AddUser(string userId)
        {
            User user = await bojService.LoadUser(userId);
            if (user == null)
            {
                MessageBox.Show("사용자를 찾을 수 없습니다.");
                return;
            }
            Users.Add(user);
            SelectedUser = user;
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

        }
        partial void OnSelectedUserChanged(User value)
        {
            Problems.Clear();

            if (SelectedUser == null) return;

            Problems.Clear();

            if (value == null) return;

            foreach (var problem in value.UnSolvedProblems) // 2번
            {
                Problems.Add(problem);
            }

            if (value != null)
            {
                // 선택된 사용자를 기반으로 SUser(통계 정보) 생성
                SelectedSUser = new SUser(value);
            }
            else
            {
                SelectedSUser = null;
            }
        }

        
    }
}
