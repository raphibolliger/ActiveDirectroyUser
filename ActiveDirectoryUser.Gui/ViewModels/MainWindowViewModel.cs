using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ActiveDirectoryUser.Core;
using ActiveDirectoryUser.Gui.ViewModels.Common;
using ActiveDirectoryUser.Model;
using Microsoft.Win32;

namespace ActiveDirectoryUser.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private string _jsonFilePath;

        public ObservableCollection<User> Users { get; private set; }

        public string JsonFilePath
        {
            get { return _jsonFilePath; }
            set
            {
                _jsonFilePath = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenFileCommand { get; private set; }

        public MainWindowViewModel()
        {
            Users = new ObservableCollection<User>();
            OpenFileCommand = new RelayCommand<Window>(this.OpenFile);
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
    }
}
