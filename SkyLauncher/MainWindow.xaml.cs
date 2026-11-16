<<<<<<< HEAD
﻿using LLama;
using LLama.Abstractions;
using LLama.Native;
using LLama.Common;
=======
﻿using OllamaSharp;
>>>>>>> refs/remotes/origin/main
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
<<<<<<< HEAD
using Microsoft.Win32;
=======
>>>>>>> refs/remotes/origin/main

namespace SkyLauncher
{
    public partial class MainWindow : Window
    {
<<<<<<< HEAD
        private LLamaWeights _model;
        private LLamaContext _context;
        private ChatSession _chatSession;
        private OpenFileDialog openFileDialog;
        public string _currentmodel;
        public bool _isGeneration = false;
        private string _filename;
        private string modelPath;
        public MainWindow()
        {
            modelPath = @"G:\SkyLauncher\SkyLauncher\SkyLauncher\models\Qwen3.5-4B-Q4_K_M.gguf";
            InitializeComponent();
            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 2048,
                GpuLayerCount = 3
            };
            _model = LLamaWeights.LoadFromFile(parameters);
            _context = _model.CreateContext(parameters);
            var executor = new InteractiveExecutor(_context);
            _chatSession = new ChatSession(executor);
        }

        private async void InputComboBox(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".gguf";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result.Value)
            {
                _filename = openFileDialog.FileName;
            }
            modelPath = _filename;
            _model?.Dispose();
=======
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

>>>>>>> refs/remotes/origin/main
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
<<<<<<< HEAD
            TextPanel.Text += text + Environment.NewLine;
            var text123 = TextPanel.Text;
            InputTextBox.Clear();
            InferenceParams inferenceParams = new InferenceParams()
            {
                MaxTokens = 32000,
                AntiPrompts = new List<string> { "User:" }
            };
            await foreach (var token in _chatSession.ChatAsync(new ChatHistory.Message(AuthorRole.User, text), inferenceParams))
            {
                TextPanel.Text += token;
=======
            TextPanel.Text += "You: ";
            TextPanel.Text += text + Environment.NewLine;
            var text123 = TextPanel.Text;
            InputTextBox.Clear();
            TextPanel.Text += "AI: ";
            await foreach (var answerToken in _chat.SendAsync(text))
            {
                TextPanel.Text += answerToken;
>>>>>>> refs/remotes/origin/main
            }
            TextPanel.Text += Environment.NewLine;
        }
    }

}
