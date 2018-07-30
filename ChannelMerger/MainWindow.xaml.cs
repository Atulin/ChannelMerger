using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageMagick;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Button = System.Windows.Controls.Button;
using ComboBox = System.Windows.Controls.ComboBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
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
            ImageSource imageSource = null;

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
                
                imageSource = new BitmapImage(new Uri(file.FileName));
            }

            switch (tag.ToLower())
            {
                case "r":
                    _channelPaths[0] = file.FileName;
                    redPath.Text = file.FileName;
                    if (imageSource != null)
                        RedImage.Source = imageSource;
                    break;

                case "g":
                    _channelPaths[1] = file.FileName;
                    greenPath.Text = file.FileName;
                    if (imageSource != null)
                        GreenImage.Source = imageSource;
                    break;

                case "b":
                    _channelPaths[2] = file.FileName;
                    bluePath.Text = file.FileName;
                    if (imageSource != null)
                        BlueImage.Source = imageSource;
                    break;

                case "a":
                    _channelPaths[3] = file.FileName;
                    alphaPath.Text = file.FileName;
                    if (imageSource != null)
                        AlphaImage.Source = imageSource;
                    break;

                default:
                    throw new ArgumentException("How? Just... how?");
            }
        }

        private void MergeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_outputPath)) return;

            string[] colours = {
                RedFill.Text.ToLower(),
                GreenFill.Text.ToLower(),
                BlueFill.Text.ToLower(),
                AlphaFill.Text.ToLower()
            };

            for (int i = 0; i < _channelPaths.Length; i++)
            {
                _channelImages.Add(String.IsNullOrEmpty(_channelPaths[i])
                    ? new MagickImage("xc:"+colours[i], _settings)
                    : new MagickImage(_channelPaths[i]));
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

        private void ResolutionList_OnSelected(object sender, EventArgs eventArgs)
        {
            string[] sDimensions = ResolutionList.Text.Split('x');
            int[] dimensions =
            {
                Int32.Parse(sDimensions[0]),
                Int32.Parse(sDimensions[1])
            };

            _settings.Width  = dimensions[0];
            _settings.Height = dimensions[1];
        }

        private void FillSelection_FocusLost(object sender, RoutedEventArgs e)
        {
            Title = ((ComboBox) sender).Name + ((ComboBox) sender).Text;
        }

        private void Preview_Click(object sender, MouseButtonEventArgs e)
        {
            if (((Image) sender).Source != null)
            {
                PreviewWindow.IsOpen = true;
                PreviewWindowImage.Source = ((Image) sender).Source;
            }
        }
    }
}
