// EVECSC - Michael
// ShellViewModel.cs
// Last Cleanup: 17/05/2020 08:42
// Created: 16/05/2020 13:21

#region Directives
using System;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using EVECSC.Models;
#endregion

namespace EVECSC.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP", "EVE");
        private readonly string _backupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EVECSC", "Backups");
        private Folder _selectedFolder;

        public ShellViewModel()
        {
            if (!Directory.Exists(_backupPath))
                Directory.CreateDirectory(_backupPath);

            Folders = GetFolders();

            // foreach (var folder in Folders)
            // {
            //     folder.Characters = folder.GetCharacters();
            // }
        }

        public string ApplicationTitle { get; } = "EVE Character Settings Copier";

        public BindableCollection<Folder> Folders { get; set; }

        public Folder SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                _selectedFolder = value;
                NotifyOfPropertyChange(() => SelectedFolder);
            }
        }

        private BindableCollection<Folder> GetFolders()
        {
            var directories = Directory.EnumerateDirectories(_path, "*sharedcache*")
                .SelectMany(p => Directory.EnumerateDirectories(p, "settings*"))
                .Select(directory => new Folder {Name = new DirectoryInfo(directory).Name, FilePath = directory})
                .ToList();
            return new BindableCollection<Folder>(directories);
        }
    }
}