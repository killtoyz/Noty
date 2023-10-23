﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Noty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainMenuItems MainMenu;
        public MainWindow()
        {
            InitializeComponent();
            MainMenu = new MainMenuItems();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e) => MainMenu.OpenFile(textArea);
        private void NewMenuItem_Click(object sender, RoutedEventArgs e) => MainMenu.NewFile(textArea);
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e) => MainMenu.SaveFile(textArea);

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var hm = e.GetPosition(this);
            textArea.Document.FontSize += 1;
        }
    }
}
