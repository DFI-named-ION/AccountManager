using AccountManager.Core;
using System.Windows;
using AccountManager.Services;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AccountManager.MVVM.ViewModels
{
    public class LogInViewModel: ObservableObject
    {
        public RelayCommand LogInCommand { get; }


        static private LogInViewModel _instance = new LogInViewModel();
        static public LogInViewModel Instance { get => _instance; }
        static private AuthViewModel _parent = null;
        static public AuthViewModel Parent 
        { get => _parent; set => _parent = value; }


        private string _logInErrorMessage = string.Empty;
        public string LogInErrorMessage 
        {
            get => _logInErrorMessage;
            set
            {
                if (_logInErrorMessage != value)
                {
                    _logInErrorMessage = value;
                    OnPropertyChanged(nameof(LogInErrorMessage));
                }
            }
        }


        private string _passwordErrorMessage = string.Empty;
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set
            {
                if (_passwordErrorMessage != value)
                {
                    _passwordErrorMessage = value;
                    OnPropertyChanged(nameof(PasswordErrorMessage));
                }
            }
        }


        public bool IsWorking 
        {
            get => !_parent.IsWorking;
            set
            {
                if (_parent.IsWorking != value)
                {
                    _parent.IsWorking = value;
                    OnPropertyChanged(nameof(IsWorking));
                }
            }
        }


        public Visibility VisibleState
        {
            get => _parent.VisibleState;
            set
            {
                if (_parent.VisibleState != value)
                {
                    _parent.VisibleState = value;
                    OnPropertyChanged(nameof(VisibleState));
                }
            }
        }


        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }


        private string _login = string.Empty;
        public string Login
        {
            get => _login;
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }

        private LogInViewModel()
        {
            LogInCommand = new RelayCommand(async x => 
            {
                IsWorking = true;
                LogInErrorMessage = FindIssues(Login, @"(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9_\-\.]{6,}$");
                PasswordErrorMessage = FindIssues(Password, @"[a-zA-Z][a-zA-Z0-9_\-\.]{5,15}$");
                if (LogInErrorMessage == "" && PasswordErrorMessage == "")
                {
                    var user = await Task.Run(() => DbClient.LogInUser(Login, Password));
                    if (user != null)
                    {
                        _parent.User = user;
                        VisibleState = Visibility.Hidden;
                    }
                    else
                    {
                        LogInErrorMessage = "Not found!";
                        PasswordErrorMessage = "Not found!";
                    }
                }
                IsWorking = false;
            });
        }

        private string FindIssues(string s, string exp)
        {
            Regex r = new Regex(exp);
            if (r.IsMatch(s))
                return "";
            return "Incorrect input!";
        }
    }
}