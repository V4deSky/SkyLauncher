using LLama;
using LLama.Abstractions;
using LLama.Native;
using LLama.Common;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace SkyLauncher
{
    public partial class MainWindow : Window
    {
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
            }
            TextPanel.Text += Environment.NewLine;
        }
    }

}
