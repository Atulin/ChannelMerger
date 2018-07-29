using System;
using System.Windows;
using System.Windows.Forms;
using ImageMagick;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Button = System.Windows.Controls.Button;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;


namespace ChannelMerger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly string[] _channelPaths = new string[4];
        private readonly MagickImageCollection _channelImages = new MagickImageCollection();
        private string _outputPath;
        private readonly MagickReadSettings _settings = new MagickReadSettings();

        public MainWindow()
        {
            InitializeComponent();
            
            // Tells the xc: reader the image to create should be 800x600
            _settings.Width = 800;
            _settings.Height = 600;
        }

        private async void ChannelBtn_Click(object sender, RoutedEventArgs e)
        {
            string tag = ((Button) sender).Tag.ToString();

            OpenFileDialog file = new OpenFileDialog
            {
                Filter = Helpers.ImagesFilter()
            };
            file.ShowDialog();

            if (!String.IsNullOrEmpty(file.FileName))
            {
                MagickImage img = new MagickImage(file.FileName);
                if (!img.CheckGrayscale())
                {
                    await this.ShowMessageAsync("Error!", "This image isn't grayscale!");
                    return;
                }

            }

            switch (tag.ToLower())
            {
                case "r":
                    _channelPaths[0] = file.FileName;
                    redPath.Text = file.FileName;
                    break;

                case "g":
                    _channelPaths[1] = file.FileName;
                    greenPath.Text = file.FileName;
                    break;

                case "b":
                    _channelPaths[2] = file.FileName;
                    bluePath.Text = file.FileName;
                    break;

                case "a":
                    _channelPaths[3] = file.FileName;
                    alphaPath.Text = file.FileName;
                    break;

                default:
                    throw new ArgumentException("How? Just... how?");
            }
        }

        private void MergeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_outputPath)) return;

            foreach (string path in _channelPaths)
            {
                _channelImages.Add(String.IsNullOrEmpty(path)
                    ? new MagickImage("xc:white", _settings)
                    : new MagickImage(path));
            }

            _channelImages.MergeChannels(outputPath.Text, fileName.Text);
            _channelImages.Clear();
        }

        private void OutputBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var folder = new FolderBrowserDialog())
            {
                if (!string.IsNullOrEmpty(_outputPath))
                {
                    folder.SelectedPath = _outputPath;
                }
                folder.ShowDialog();
                _outputPath = folder.SelectedPath;
            }
            outputPath.Text = _outputPath;
        }
    }
}
