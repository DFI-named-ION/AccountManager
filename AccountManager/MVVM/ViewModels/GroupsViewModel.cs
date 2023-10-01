using System.Threading.Tasks;
using AccountManager.Core;
using AccountManager.MVVM.Models;
using AccountManager.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;

namespace AccountManager.MVVM.ViewModels
{
    public class GroupsViewModel: ObservableObject
    {
        public RelayCommand BrowseGroupImageCommand { get; set; }
        public RelayCommand AddGroupCommand { get; set; }
        public RelayCommand RemGroupCommand { get; set; }


        static private ProgramViewModel _parent = null;
        static public ProgramViewModel Parent
        { get => _parent; set => _parent = value; }


        public ObservableCollection<Group> Groups
        {
            get => GetGroups();
            set => OnPropertyChanged(nameof(Groups));
        }


        private Group _selectedGroup;
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                DbClient.CommitData();
                OnPropertyChanged(nameof(SelectedGroup));
                OnPropertyChanged(nameof(IsAvailable));
            }
        }


        public bool IsAvailable
        {
            get => _selectedGroup != null;
        }


        public GroupsViewModel()
        {
            BrowseGroupImageCommand = new RelayCommand(async x =>
            {
                var select = new OpenFileDialog();
                select.InitialDirectory = Directory.GetCurrentDirectory();
                select.Title = "Select group icon";
                select.Filter = "Images|*.jpg;*.jpeg;*.png;";
                select.Multiselect = false;
                select.ShowDialog();
                if (select.FileName != string.Empty)
                    SelectedGroup.ImagePath = DataCryptography.AESEncrypt(select.FileName);

                var selected = SelectedGroup;
                SelectedGroup = null;
                SelectedGroup = selected;
                Groups = await Task.Run(() => GetGroups());
            });

            RemGroupCommand = new RelayCommand(async x =>
            {
                await Task.Run(() => DbClient.RemoveGroup(SelectedGroup));
                Groups = await Task.Run(() => GetGroups());
            });

            AddGroupCommand = new RelayCommand(async x =>
            {
                var group = await Task.Run(() => DbClient.CreateGroup(_parent.User, "Title", "Description", "Empty"));
                Groups = await Task.Run(() => GetGroups());
            });
        }


        private ObservableCollection<Group> GetGroups()
        {
            return new ObservableCollection<Group>(DbClient.GetGroups(_parent.User));
        }
    }
}
