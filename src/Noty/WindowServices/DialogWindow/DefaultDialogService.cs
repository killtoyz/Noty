﻿using Microsoft.Win32;

namespace Noty.Services
{
    public class DefaultDialogService : IDialogService
    {
        private string dialogFilter = "Txt files (*.txt)|*.txt|Rtf files (*rtf)|*.rtf|JSON files (*.json*)|*.json|XAML files (*.xaml)|*.xaml|All files (*.*)|*.*";
        public string FilePath { get; private set; }
        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = dialogFilter;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = dialogFilter;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            //
        }
    }
}