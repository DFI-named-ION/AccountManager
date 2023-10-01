using System.Windows;
using AccountManager.Core;
using AccountManager.MVVM.Models;

namespace AccountManager.MVVM.ViewModels
{
    public class AuthViewModel: ObservableObject
    {
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand MinimizeWindowCommand { get; }
        public RelayCommand LogInCommand { get; set; }
        public RelayCommand SignInCommand { get; set; }


        static private AuthViewModel _instance = new AuthViewModel();
        static public AuthViewModel Instance { get => _instance; }

        static private ProgramViewModel _parent = null;
        static public ProgramViewModel Parent
        { get => _parent; set => _parent = value; }


        public LogInViewModel LogInVM { get; set; }
        public SignInViewModel SignInVM { get; set; }


        private bool _isWorking = false;
        public bool IsWorking
        {
            get => _isWorking;
            set
            {
                if (_isWorking != value)
                {
                    _isWorking = value;
                    OnPropertyChanged(nameof(IsWorking));
                }
            }
        }


        private Visibility _visibleState = Visibility.Visible;
        public Visibility VisibleState
        {
            get => _visibleState;
            set
            {
                _visibleState = value;
                OnPropertyChanged(nameof(VisibleState));
            }
        }


        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }


        public User User 
        {
            set
            {
                if (value != _parent.User) 
                {
                    _parent.User = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }


        public AuthViewModel()
        {
            LogInViewModel.Parent = Instance;
            SignInViewModel.Parent = Instance;
            LogInVM = LogInViewModel.Instance;
            SignInVM = SignInViewModel.Instance;

            CurrentView = LogInVM;

            LogInCommand = new RelayCommand(x =>
            {
                CurrentView = LogInVM;
            });
            SignInCommand = new RelayCommand(x =>
            {
                CurrentView = SignInVM;
            });
            CloseWindowCommand = new RelayCommand(x =>
            {
                Application.Current.Shutdown();
            });
            MinimizeWindowCommand = new RelayCommand(x =>
            {
                SystemCommands.MinimizeWindow(Application.Current.MainWindow);
            });
        }
    }
}
