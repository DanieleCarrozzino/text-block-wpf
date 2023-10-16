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
        }

        public string text
        {
            get
            {
                return "me kjqbw kxcb wsdkcghvkwb cdkgh edcbhw ecdmbhn vwsdhjc v";
            }
        }

        private void TextEmoji_LinkClicked(string link)
        {
            System.Console.WriteLine("Clicked" + link);
        }

        private void TextEmoji_SizeChildrenChanged(Size obj)
        {

        }
    }
}
