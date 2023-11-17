using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TextEmoji.objects;
using TextEmoji.usercontrols;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChangedName(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        public string text
        {
            get => "Hello there! I'm a simple text lorem ipsum 🩷💻💀 and also a link http://www.google.it a link, yeah";
        }

        private void TextEmoji_LinkClicked(string link)
        {
            System.Console.WriteLine("Clicked " + link);
        }

        private void TextEmoji_SizeChildrenChanged(Size size)
        {
            System.Console.WriteLine("Size changed " + "w :" + size.Width + " h :" + size.Height);
        }

        private void TextEmoji_SelectedChanged(string selectedText)
        {
            System.Console.WriteLine(selectedText);
        }

        private void TextEmoji_RightLinkClicked(string obj, MouseButtonEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var w = e.NewSize.Width / 3;
            first.SizeContainer = new Size(w, e.NewSize.Height);
            Size = new Size(w, e.NewSize.Height);
        }

        

        private Size _size = new Size(300, 0);
        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChangedName("Size");
            }
        }

        private void TextEmoji_CopyTextAction(string obj)
        {

        }

        private void first_CopyTextAction(string obj)
        {

        }
    }
}
