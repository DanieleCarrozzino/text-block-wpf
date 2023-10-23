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

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            /*var l = new List<String>();
            l.Add("");
            l.Add("dani🥗dani🤎dani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw ");
            l.Add("dani🥗dani🤎dani🎶dani");
            l.Add("kwhfjeb wehbg kjb 🎶evfkjewb vkj");
            l.Add("dani🥗dani🤎dani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw dani\U0001f957dani\U0001f90edani🎶dani 2k hwe hliw helkj hwklej vkljew vlkjw ");
            l.Add("dani🥗dani🤎dani🎶dani");
            l.Add("kwhfjeb wehbg kjb 🎶evfkjewb vkj");
            l.Add("dani🥗dani🤎dani🎶dani");
            l.Add("kwhfjeb wehbg kjb 🎶evfkjewb vkj");

            list.ItemsSource = l;*/
        }

        public string text
        {
            get
            {
                return "Test dlweh clwe fvlcw dfjn \n òlwkjs vlkdnjwvkldjwnkl";
            }
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
            /*Point p = e.GetPosition(this);
            var userControl = new LinkDialog();
            userControl.Margin = new Thickness(p.X, p.Y, 0, 0);

            mainGrid.Children.Add(userControl);*/
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var w = e.NewSize.Width / 3;
            first.SizeContainer = new Size(w, e.NewSize.Height);
            Size = new Size(w, e.NewSize.Height);
        }

        public void OnPropertyChangedName(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private Size _size = new Size(300, 0);

        public event PropertyChangedEventHandler? PropertyChanged;

        public Size Size
        {
            get
            {
                return _size;
            }
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
