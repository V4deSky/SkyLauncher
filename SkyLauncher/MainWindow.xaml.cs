using OllamaSharp;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkyLauncher
{
    public partial class MainWindow : Window
    {
        public Chat _chat;
        private OllamaApiClient _ollama;
        public string _currentmodel;
        public bool _isGeneration = false;
        public MainWindow()
        {
            InitializeComponent();
            var uri = new Uri("http://localhost:11434");
            _ollama = new OllamaApiClient(uri);
            _currentmodel = "qwen3:4b";
            _ollama.SelectedModel = _currentmodel;
            _chat = new Chat(_ollama);
        }

        private async void InputComboBox(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.AddedItems[0] as ComboBoxItem;
            if (selectedItem != null)
            {
                _currentmodel = selectedItem.Content.ToString();
            }

        }

        private async void SendButton_Сlick(object sender, RoutedEventArgs e)
        {
            
            if (!_isGeneration)
            {
                await SendChat();
            }
        }

        private async void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isGeneration && e.Key == Key.Enter)
            {
                await SendChat();
            }
        }
        private async Task SendChat()
        {
            string text = InputTextBox.Text;
            TextPanel.Text += "You: ";
            TextPanel.Text += text + Environment.NewLine;
            var text123 = TextPanel.Text;
            InputTextBox.Clear();
            TextPanel.Text += "AI: ";
            await foreach (var answerToken in _chat.SendAsync(text))
            {
                TextPanel.Text += answerToken;
            }
            TextPanel.Text += Environment.NewLine;
        }
    }

}
