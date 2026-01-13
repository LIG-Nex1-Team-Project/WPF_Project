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
        public ObservableCollection<SolvedProblem> Problems { get; }

        public ObservableCollection<UnSolvedProblem> UnSolvedProblems { get; }
        
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
            Problems = new ObservableCollection<SolvedProblem>();  // 1번
            UnSolvedProblems = new ObservableCollection<UnSolvedProblem> { }; 
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

            foreach (var problem in value.SolvedProblems) // 2번
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
