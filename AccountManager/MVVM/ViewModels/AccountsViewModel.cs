using AccountManager.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AccountManager.MVVM.Models;
using AccountManager.Core;

namespace AccountManager.MVVM.ViewModels
{
    public class AccountsViewModel: ObservableObject
    {
        public RelayCommand AddAccountCommand { get; }
        public RelayCommand RemAccountCommand { get; }


        static private ProgramViewModel _parent = null;
        static public ProgramViewModel Parent
        { get => _parent; set => _parent = value; }


        public bool IsAvailableByGroup
        {
            get => _selectedGroup != null;
        }


        public bool IsAvailableByAccount
        {
            get => _selectedAccount != null;
        }


        public ObservableCollection<Group> Groups
        {
            get => GetGroups();
            set
            {
                OnPropertyChanged(nameof(Groups));
            }
        }


        private Group _selectedGroup;
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                Accounts = GetAccounts();
                OnPropertyChanged(nameof(SelectedGroup));
                OnPropertyChanged(nameof(IsAvailableByGroup));
            }
        }


        public ObservableCollection<Account> Accounts
        {
            get => GetAccounts();
            set
            {
                OnPropertyChanged(nameof(Accounts));
            }
        }


        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                DbClient.CommitData();
                OnPropertyChanged(nameof(SelectedAccount));
                OnPropertyChanged(nameof(IsAvailableByAccount));
            }
        }


        public AccountsViewModel()
        {
            AddAccountCommand = new RelayCommand(async x =>
            {
                var account = await Task.Run(() => DbClient.CreateAccount(_parent.User, SelectedGroup, "Title", "Login", "Password", "Email"));
                Accounts = await Task.Run(() => GetAccounts());
            });
            RemAccountCommand = new RelayCommand(async x =>
            {
                await Task.Run(() => DbClient.RemoveAccount(SelectedAccount));
                Accounts = await Task.Run(() => GetAccounts());
            });
        }


        private ObservableCollection<Account> GetAccounts()
        {
            return new ObservableCollection<Account>(DbClient.GetAccounts(_parent.User, SelectedGroup));
        }

        private ObservableCollection<Group> GetGroups()
        {
            return new ObservableCollection<Group>(DbClient.GetGroups(_parent.User));
        }
    }
}
