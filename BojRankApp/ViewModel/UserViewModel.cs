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

        [ObservableProperty]
        private string userId = string.Empty;


        [ObservableProperty]
        private User selectedUser;



        public UserViewModel()
        {
            bojService = new BojService();
            Users = new ObservableCollection<User>();
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
        
    }
}
