using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BojRankApp.Model;
using BojRankApp.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

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
            // Debug.WriteLine(userId);
            if (user == null)
            {
                MessageBox.Show("사용자를 찾을 수 없습니다.");
                return;
            }

            Users.Add(user);
            
        }




        [RelayCommand]
        public void DelUser()
        {
            
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
            }
        }


    }
}
