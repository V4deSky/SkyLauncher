using LLama;
using LLama.Abstractions;
using LLama.Common;
using LLama.Native;
using Microsoft.Win32;
﻿using OllamaSharp;
using OllamaSharp.Models;
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
        private LLamaWeights _model;
        private LLamaContext _context;
        private ChatHistory _chatHistory;
        private OpenFileDialog openFileDialog;
        private ChatSession _session;
        public string _currentmodel;
        public bool _isGeneration = false;
        private string _filename;
        private string modelPath;
        private InteractiveExecutor executor;
        private bool AISelected;
        public MainWindow()
        {
            modelPath = _filename;
            InitializeComponent();
        }

        private void InputComboBox(object sender, RoutedEventArgs e)
        {
            AISelected = true;
            openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".gguf";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result.Value)
            {
                _filename = openFileDialog.FileName;
            }
            modelPath = _filename;
            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 4096,
                GpuLayerCount = 3
            };
            _model = LLamaWeights.LoadFromFile(parameters);
            _context = _model.CreateContext(parameters);
            executor = new InteractiveExecutor(_context);
            _chatHistory = new ChatHistory();
            _chatHistory.AddMessage(AuthorRole.System, "where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.");
            _chatHistory.AddMessage(AuthorRole.User, "Hello, Bob.");
            _chatHistory.AddMessage(AuthorRole.Assistant, "Hello. How may I help you today?");

            _session = new(executor, _chatHistory);
        }

        private async void SendButton_Сlick(object sender, RoutedEventArgs e)
        {

            if (!_isGeneration && AISelected)
            {
                await SendChat();
            }
        }

        private async void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isGeneration && e.Key == Key.Enter && AISelected)
            {
                await SendChat();
            }
        }
        private async Task SendChat()
        {
            string text = InputTextBox.Text;
            TextPanel.Text += text + Environment.NewLine;
            var text123 = TextPanel.Text;
            InputTextBox.Clear();
            InferenceParams inferenceParams = new InferenceParams()
            {
                MaxTokens = 2048,
                AntiPrompts = new List<string> {"User:", "user:","<|end|>","<|im_end|>","<|user|>"}
            };
            await foreach (var token in _session.ChatAsync(new ChatHistory.Message(AuthorRole.User, text), inferenceParams))
            {
                TextPanel.Text += token;
            }
        }

    }
}
