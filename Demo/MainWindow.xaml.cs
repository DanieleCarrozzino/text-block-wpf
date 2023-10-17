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

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            List<string> stringList = new List<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4"
            };

            list.ItemsSource = stringList;
        }

        public string text
        {
            get
            {
                return "me test di un m d,sh vljwdh fvkljw dfvsdò😍 vkhsldjh vljsdhb voljh sdlkvb poswdhfb vpkòhjwb dflbvk sldbnfpwijbnrfpbòjnwpdfkjbnpkldnjwb";
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

        private void TextEmoji_RightLinkClicked(string obj)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var w = e.NewSize.Width / 3;
            first.Size = new Size(w, e.NewSize.Height);
        }
    }
}
