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

namespace BojRankApp.ViewModel
{
    public partial class UserViewModel: ObservableObject
    {

        private BojService bojService;

        public ObservableCollection<User> Users { get; }

        [ObservableProperty]
        private string userId = string.Empty;


        public UserViewModel()
        {
            bojService = new BojService();
            Users = new ObservableCollection<User>();
        }

        [RelayCommand]
        public async Task AddUserCommand(string userId)
        {
            if (string.IsNullOrWhiteSpace(UserId))
                return;

            User user = await bojService.LoadUser(userId);
            Users.Add(user);
        }

    }
}
