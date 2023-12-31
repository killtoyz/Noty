﻿using Noty.Commands;
using Noty.Models;
using Noty.Services;
using Noty.Shared.FileOperations;
using Noty.Views.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Noty.ViewModels
{
    public class FileTabViewModel : BaseViewModel
    {
        #region private Fields
        private readonly IFileServiceCreator _fileServiceCreator;
        #endregion

        #region Properties
        public DocumentModel? Document { get; private set; }
        public ObservableCollection<TabViewModel> TabItems { get; set; } =
            new ObservableCollection<TabViewModel>();
        public TabViewModel? CurrentTab { get; set; }
        #endregion
       
        #region Commands

        #region Tabs
        public ICommand PinTabCommand { get; }
        private bool CanPinTabCommandExecute(object parameter) => true;
        private void OnPinTabCommandExecuted(object parameter)
        {
            var tab = (TabViewModel)parameter;
            var tabIndex = TabItems.IndexOf(tab);
            var lastPinnedIndex = TabItems.IndexOf(TabItems.LastOrDefault(x => x.IsPinned));

            if (tabIndex == lastPinnedIndex + 1 || tabIndex == lastPinnedIndex)
            {
                tab.IsPinned = tab.IsPinned ? false : true;
                return;
            }

            if (!tab.IsPinned)
            {
                TabItems.Move(tabIndex, lastPinnedIndex + 1);           
                tab.IsPinned = true;
            }
            else
            {
                TabItems.Move(tabIndex, lastPinnedIndex);           
                tab.IsPinned = false;
            }
        }
        public ICommand CloseTabCommand { get; }
        private bool CanCloseTabCommandExecute(object parameter) => true;
        private void OnCloseTabCommandExecuted(object parameter)
        {
            var tab = (TabViewModel)parameter;
            TabItems.Remove(tab);
            if (tab == CurrentTab) CurrentTab = TabItems.LastOrDefault();
        }
        public ICommand AddTabItemCommand { get; }
        private bool CanAddTabItemCommandExecute(object parameter) => true;
        private void OnAddTabItemCommandExecuted(object parameter)
        {
            var filePath = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\";
            var defaultFileName = "Untitled.txt";
            var untitledCount = 0;

            while (File.Exists(filePath + defaultFileName))
            {
                untitledCount++;
                defaultFileName = $"Untitled ({untitledCount}).txt";
            }

            Document = new DocumentModel
            {
                FileName = defaultFileName,
                FilePath = filePath + defaultFileName,
                TextContent = string.Empty
            };

            _fileServiceCreator.CreateService(Document.FilePath, Document.FileExtension).NewFile();
            TabItems.Add(CurrentTab = new TabViewModel(Document));
        }

        #endregion

        #region TopMenu (main)
        public ICommand OpenSettingsCommand { get; }
        private bool CanOpenSettingsCommandExecute(object parameter) => true;
        private void OnOpenSettingCommandExecuted(object parameter)
        {
            var settingsWindow = new MainSettingsWindow();

            if (CurrentTab != null)
            {
                var editor = CurrentTab.Editor;
                var dataContextToSettingsWindow = editor;
                settingsWindow.DataContext = dataContextToSettingsWindow;
            }

            settingsWindow.ShowDialog();
        }

        public ICommand SaveFileCommand { get; }
        private bool CanSaveFileCommandExecute(object parameter) => CurrentTab != null;
        private void OnSaveFileCommandExecuted(object parameter) => 
            _fileServiceCreator.CreateService(Document.FilePath, Document.FileExtension).Save(Document.TextContent);

        public ICommand SaveAsFileCommand { get; }
        private bool CanSaveAsFileCommandExecute(object parameter) => CurrentTab != null;
        private void OnSaveAsFileCommandExecuted(object parameter)
        {
            SaveFileCommand.Execute(Document.FilePath);

            var dialog = new DefaultDialogService();
            if (dialog.SaveFileDialog())
                _fileServiceCreator.CreateService(dialog.FilePath, Document.FileExtension).SaveAs(Document.TextContent);
        }

        public ICommand NewFileCommand { get; }
        private bool CanNewFileCommandExecute(object parameter) => true;
        private void OnNewFileCommandExecuted(object parameter)
        {
            var dialog = new DefaultDialogService();
            
            if (!dialog.SaveFileDialog()) return;
            
            Document = new DocumentModel
            {
                FilePath = dialog.FilePath,
                FileName = dialog.FileName,
                FileExtension = dialog.FileExtension,
                TextContent = string.Empty
            };

            TabItems.Add(new TabViewModel(Document));

            _fileServiceCreator.CreateService(dialog.FilePath, Document.FileExtension).NewFile();
        }

        public ICommand OpenFileCommand { get; }
        private bool CanOpenFileCommandExecute(object parameter) => true;
        private void OnOpenFileCommandExecuted(object parameter)
        {
            var dialog = new DefaultDialogService();
            if (!dialog.OpenFileDialog()) return;
            
            Document = new DocumentModel
            {
                FilePath = dialog.FilePath,
                FileName = dialog.FileName,
                FileExtension = dialog.FileExtension
            };

            Document.TextContent = _fileServiceCreator.CreateService(Document.FilePath, Document.FileExtension).Open();

            TabItems.Add(CurrentTab = new TabViewModel(Document));
        }
        #endregion

        #endregion

        #region Constructors
        public FileTabViewModel()
        {
            _fileServiceCreator = new FileServiceCreator<TxtFileService>();

            #region Commands init
            SaveFileCommand = new DelegateCommand(OnSaveFileCommandExecuted, CanSaveFileCommandExecute);
            SaveAsFileCommand = new DelegateCommand(OnSaveAsFileCommandExecuted, CanSaveAsFileCommandExecute);
            NewFileCommand = new DelegateCommand(OnNewFileCommandExecuted, CanNewFileCommandExecute);
            OpenFileCommand = new DelegateCommand(OnOpenFileCommandExecuted, CanOpenFileCommandExecute);
            OpenSettingsCommand = new DelegateCommand(OnOpenSettingCommandExecuted, CanOpenSettingsCommandExecute);


            AddTabItemCommand = new DelegateCommand(OnAddTabItemCommandExecuted, CanAddTabItemCommandExecute);
            CloseTabCommand = new DelegateCommand(OnCloseTabCommandExecuted, CanCloseTabCommandExecute);
            PinTabCommand = new DelegateCommand(OnPinTabCommandExecuted, CanPinTabCommandExecute);
            #endregion
        }
        #endregion
    }
}
