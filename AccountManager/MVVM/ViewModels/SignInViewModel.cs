using AccountManager.Core;
using AccountManager.Services;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AccountManager.MVVM.ViewModels
{
    public class SignInViewModel: ObservableObject
    {
        public RelayCommand SignInCommand { get; }


        static private SignInViewModel _instance = new SignInViewModel();
        static public SignInViewModel Instance { get { return _instance; } }
        static private AuthViewModel _parent = null;
        static public AuthViewModel Parent
        { get => _parent; set => _parent = value; }


        private string _logInErrorMessage = string.Empty;
        public string SignInErrorMessage
        {
            get => _logInErrorMessage;
            set
            {
                if (_logInErrorMessage != value)
                {
                    _logInErrorMessage = value;
                    OnPropertyChanged(nameof(SignInErrorMessage));
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
                    OnPropertyChanged(nameof(Login));
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


        public SignInViewModel()
        {
            SignInCommand = new RelayCommand(async x =>
            {
                _parent.IsWorking = true;
                SignInErrorMessage = FindIssues(Login, @"(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9_\-\.]{6,}$");
                PasswordErrorMessage = FindIssues(Password, @"[a-zA-Z][a-zA-Z0-9_\-\.]{5,15}$");
                if (SignInErrorMessage == "" && PasswordErrorMessage == "")
                {
                    var user = await Task.Run(() => DbClient.SignInUser(Login, Password));
                    if (user != null)
                    {
                        _parent.User = user;
                        SignInErrorMessage = "Success!";
                        PasswordErrorMessage = "Success!";
                    }
                    else
                    {
                        SignInErrorMessage = "Already taken!";
                    }
                }
                _parent.IsWorking = false;
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
