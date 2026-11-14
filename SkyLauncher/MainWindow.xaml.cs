using OllamaSharp;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkyLauncher
{
    public partial class MainWindow : Window
    {
        private Chat _chat;
        private string _currentmodel;
        private bool _isGeneration = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task SendChat()
        {
            string text = InputTextBox.Text;
            Console.WriteLine(text);
        }


        private async void SendButton_Сlick(object sender, RoutedEventArgs e)
        {
            if (!_isGeneration)
            {
                SendChat();
            }
        }

        private async void InpteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isGeneration && e.Key == Key.Enter)
            { 
                SendChat(); 
            }
        }

    }
}