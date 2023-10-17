﻿using System;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            List<string> stringList = new List<string>
            {
                "Item 1 swkdj vljwhb vljhewb dlkvcn welvn dfwb vlkjewvbw vjdnkw vf",
                "Item 2lqjw dckj hewcjh e",
                "Item 3òsdj vòkwje vòkjw evj wlfje voi2hubevhjb we hklfvlk3erhvlhrlewf vhnlrjewh pv hewrlvjhnlrj lk re",
                "Item 4wl dfkvòlkw jòflv òewrl òew rò ròlj vòlrjwfvkjndewfkljvn dgfbve ndfòlb eròlb nòl rneòbkl newròlkjb òl rjwòlbg jrwòl bgkjòerlj"
            };

            list.ItemsSource = stringList;
        }

        public string text
        {
            get
            {
                return "Item 4wl dfkvòlkw jòflv òewrl òew rò ròlj vòlrjwfvkjndewfkljvn dgfbve ndfòlb eròlb nòl rneòbkl newròlkjb òl rjwòlbg jrwòl bgkjòerljItem 4wl dfkvòlkw jòflv òewrl òew rò ròlj vòlrjwfvkjndewfkljvn dgfbve ndfòlb eròlb nòl rneòbkl newròlkjb òl rjwòlbg jrwòl bgkjòerlj";
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
