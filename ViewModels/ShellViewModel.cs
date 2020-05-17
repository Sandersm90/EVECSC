// EVECSC - Michael
// ShellViewModel.cs
// Last Cleanup: 17/05/2020 17:39
// Created: 16/05/2020 13:21

#region Directives
using System;
using System.IO;
using System.Linq;
using System.Windows;
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
        private Character _selectedCharacter;

        public ShellViewModel()
        {
            if (!Directory.Exists(_backupPath))
                Directory.CreateDirectory(_backupPath);

            Folders = GetFolders();
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

        public Character SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                NotifyOfPropertyChange(() => SelectedCharacter);
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

        public void DoBackup()
        {
            if (SelectedCharacter == null)
            {
                MessageBox.Show("No character selected!", "EVECSC - ERROR");
                return;
            }

            var sourceFile = Path.Combine(SelectedFolder.FilePath, $"core_char_{SelectedCharacter.ID}.dat");
            var dest = Path.Combine(_backupPath, $"{SelectedFolder.Name}");
            var destFile = Path.Combine(_backupPath, $"{SelectedFolder.Name}\\core_char_{SelectedCharacter.ID}.dat");

            if (!Directory.Exists(dest)) Directory.CreateDirectory(dest);

            File.Copy(sourceFile, destFile, true);
            MessageBox.Show($"Backup made for {SelectedCharacter.Name}", "EVECSC - SUCCESS");
        }

        public void DoRestore()
        {
            if (SelectedCharacter == null)
            {
                MessageBox.Show("No character selected!", "EVECSC - ERROR");
                return;
            }

            var destFile = Path.Combine(SelectedFolder.FilePath, $"core_char_{SelectedCharacter.ID}.dat");
            var sourceFile = Path.Combine(_backupPath, $"{SelectedFolder.Name}\\core_char_{SelectedCharacter.ID}.dat");

            if (!File.Exists(sourceFile))
            {
                MessageBox.Show("Backup doesn't exist!", "EVECSC - ERROR");
                return;
            }

            File.Copy(sourceFile, destFile, true);
            MessageBox.Show($"Restored Character Settings for {SelectedCharacter.Name}", "EVECSC - SUCCESS");
        }

        public void DoCopy()
        {
            if (SelectedCharacter == null)
            {
                MessageBox.Show("No character selected!", "EVECSC - ERROR");
                return;
            }

            var copyToCharacters = SelectedFolder.Characters.Where(c => c.ID != SelectedCharacter.ID).ToList();
            var sourceFile = Path.Combine(SelectedFolder.FilePath, $"core_char_{SelectedCharacter.ID}.dat");

            foreach (var character in copyToCharacters)
            {
                var destFile = Path.Combine(SelectedFolder.FilePath, $"core_char_{character.ID}.dat");

                File.Copy(sourceFile, destFile, true);
            }

            MessageBox.Show($"Copied {SelectedCharacter.Name}'s Settings to other Characters", "EVECSC - SUCCESS");
        }
    }
}