using AccountManager.Core;
using AccountManager.MVVM.Models;
using AccountManager.Services;
using System.Windows;

namespace AccountManager.MVVM.ViewModels
{
    public class ProgramViewModel : ObservableObject
    {
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand MinimizeWindowCommand { get; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand GroupsViewCommand { get; set; }
        public RelayCommand AccountsViewCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }


        static private AuthViewModel _auth = null;
        static public AuthViewModel Parent
        { get => _auth; set => _auth = value; }


        public HomeViewModel HomeVM { get; set; }
        public GroupsViewModel GroupsVM { get; set; }
        public AccountsViewModel AccountsVM { get; set; }


        private object _currentView = null;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }


        private User _user = null;
        public User User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        private Visibility _visibleState = Visibility.Collapsed;
        public Visibility VisibleState
        {
            get => _visibleState;
            set
            {
                if (value == Visibility.Hidden)
                {
                    _visibleState = Visibility.Hidden;
                    _auth.VisibleState = Visibility.Visible;
                }
                else
                    _visibleState = value;
                OnPropertyChanged(nameof(VisibleState));
            }
        }


        public ProgramViewModel()
        {
            AuthViewModel.Parent = this;
            GroupsViewModel.Parent = this;
            AccountsViewModel.Parent = this;
            _auth = AuthViewModel.Instance;

            HomeVM = new HomeViewModel();
            GroupsVM = new GroupsViewModel();
            AccountsVM = new AccountsViewModel();
            CurrentView = HomeVM;
            HomeViewCommand = new RelayCommand(x =>
            {
                CurrentView = HomeVM;
            });
            GroupsViewCommand = new RelayCommand(x =>
            {
                CurrentView = GroupsVM;
            });
            AccountsViewCommand = new RelayCommand(x =>
            {
                CurrentView = AccountsVM;
            });
            LogOutCommand = new RelayCommand(x =>
            {
                VisibleState = Visibility.Hidden;
                HomeViewCommand.Execute(null);
            });
            CloseWindowCommand = new RelayCommand(x =>
            {
                DbClient.CommitData();
                Application.Current.Shutdown();
            });
            MinimizeWindowCommand = new RelayCommand(x => 
            {
                SystemCommands.MinimizeWindow(App.Current.Windows[1]);
            });
        }
    }
}