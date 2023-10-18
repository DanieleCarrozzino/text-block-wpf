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

            List<string> stringList = new List<string>
            {
                "Item 1 swkdj vljwhb www.google.it vljhewb dlkvcn welvn dfwb vlkjewvbw 🥗🧻vjdnkw vf",
                "Item 2lqjw dc👍kj hewcjh e",
                "Item 3òsdj vòkwje vòkjw evj wlfje🥲👩🏻‍🎨 voi2hubevhjb we hklfvlk3erhvlhrlewf vhnlrjewh pv hewrlvjhnlrj lk re",
                "Item 4wl dfkvòlkw🥰 jòflv òewrl òew rò ròlj vòlrjwfvkjndewfkljvn dgfbve ndfòlb eròlb nòl rneòbkl newròlkjb òl rjwòlbg jrwòl bgkjòerlj"
            };

            list.ItemsSource = stringList;
        }

        public string text
        {
            get
            {
                return "Item 4wl dfkvòlkw jòflv òvòkwje vòkjw evj wlfje\U0001f972vòkwje vòkjw evj wlfje\U0001f972vòkwje vòk www.google.it jw evj wlfje\U0001f972 ewrl òew rò ròlj www.google.it  vòlrjwfvkjndewfkljvn dgfbve ndfòlb eròlb nòl rneòbkl newròlkjb òl r www.google.it jwòlbg jrwòl bgkjòerljItem 4wl dfkvòlkw jòflv òewrl òew rò ròlj vòlrjwfvkjndewfkljvn dgfbve ndfòlb eròlb nòl rneòbkl new www.google.it ròlkjb òl rjwòlbg jrwòl bgkjòerlj";
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

        private Size _size = new Size(200, 0);

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
    }
}
