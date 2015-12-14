using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using ActiveDirectoryUser.Core;
using ActiveDirectoryUser.Gui.ViewModels.Common;
using ActiveDirectoryUser.Model;
using Microsoft.Win32;

namespace ActiveDirectoryUser.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private string _jsonFilePath;
        private bool _showWarningMessage;

        public ObservableCollection<User> Users { get; private set; }

        public ObservableCollection<string> AdUsers { get; private set; } 

        public string JsonFilePath
        {
            get { return _jsonFilePath; }
            set
            {
                _jsonFilePath = value;
                OnPropertyChanged();
            }
        }

        public bool ShowWarningMessages
        {
            get { return _showWarningMessage; }
            set
            {
                _showWarningMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenFileCommand { get; private set; }
        public ICommand SaveUsersToAdCommand { get; private set; }
        public ICommand LoadAdUsersCommand { get; private set; }
        public ICommand DeleteUsresOnAdCommand { get; private set; }

        public MainWindowViewModel()
        {
            Users = new ObservableCollection<User>();
            AdUsers = new ObservableCollection<string>();
            OpenFileCommand = new RelayCommand<Window>(this.OpenFile);
            SaveUsersToAdCommand = new RelayCommand(this.SaveUsersToAd);
            LoadAdUsersCommand = new RelayCommand(this.LoadAdUsers);
            DeleteUsresOnAdCommand = new RelayCommand(this.DeleteAllUsersOnAd);
            LoadAdUsers();
        }

        private void OpenFile(Window window)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                this.JsonFilePath = openFileDialog.FileName;
            }
            var service = new UserService();
            var userList = service.ReadUserListFromFile(JsonFilePath);
            userList.ForEach(u => Users.Add(u));
        }

        private void LoadAdUsers()
        {
            AdUsers.Clear();
            foreach (var user in AdUserService.LoadAllUsersFromAd())
            {
                AdUsers.Add(user);
            }

        }

        private void SaveUsersToAd()
        {
            foreach (var user in Users)
            {
                try
                {
                    AdUserService.AddUser(user);
                }
                catch (Exception ex)
                {
                    if (!ShowWarningMessages)
                    {
                        MessageBox.Show(ex.Message + "\n" + user.UserName, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteAllUsersOnAd()
        {
            foreach (var adUser in AdUsers)
            {
                AdUserService.DeleteUser(adUser);
            }
            AdUsers.Clear();
            LoadAdUsers();
        }
    }
}
