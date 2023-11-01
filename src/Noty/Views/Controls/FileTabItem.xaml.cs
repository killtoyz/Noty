﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Noty
{
    public partial class FileTabItem : INotifyPropertyChanged
    {
        public FileTabItem()
        {
            InitializeComponent();
        }

        private string currentLnNumber = "1";
        public string CurrentLnNumber
        { 
            get => currentLnNumber;
            set 
            {   
                currentLnNumber = value;
                OnPropertyChanged(); 
            }        
        }

        private string currentChNumber = "1";
        public string CurrentChNumber
        {
            get => currentChNumber;
            set { currentChNumber = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string property = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private void TextArea_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var caret = TextArea.CaretIndex;
            var line = TextArea.GetLineIndexFromCharacterIndex(caret);
            var ch = TextArea.GetCharacterIndexFromLineIndex(line);

            if (line == 0) ch = caret;
            else ch = caret - TextArea.GetCharacterIndexFromLineIndex(line);

            CurrentLnNumber = (line + 1).ToString();
            CurrentChNumber = (ch + 1).ToString();
        }
    }
}