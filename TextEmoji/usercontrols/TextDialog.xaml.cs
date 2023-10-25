using System;
using System.Collections.Generic;
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

namespace TextEmoji.usercontrols
{
    /// <summary>
    /// Logica di interazione per TextDialog.xaml
    /// </summary>
    public partial class TextDialog : UserControl
    {
        private Action<string> copyAction;
        private Action<string> searAction;
        private Action<object> seleAction;
        private Action<string> openAction;

        public string SelectedText = "";
        public string Link = "";
        
        

        public TextDialog(Action<string> copyAction, Action<string> searAction, Action<object> seleAction, string selectedText)
        {
            this.searAction     = searAction;
            this.copyAction     = copyAction;
            this.seleAction     = seleAction;
            this.SelectedText   = selectedText;

            DataContext = this;
            InitializeComponent();

            link.Visibility = Visibility.Collapsed;
            text.Visibility = Visibility.Visible;
        }

        public TextDialog(Action<string> copyAction, Action<string> openAction, string link)
        {
            this.openAction = openAction;
            this.copyAction = copyAction;
            this.Link       = link;

            DataContext = this;
            InitializeComponent();

            this.link.Visibility = Visibility.Visible;
            text.Visibility = Visibility.Collapsed;
        }

        public TextDialog(Action<string> copyAction, Action<string> searAction, Action<object> seleAction, Action<string> openAction, string selectedText, string link)
        {
            this.searAction = searAction;
            this.copyAction = copyAction;
            this.seleAction = seleAction;
            this.openAction = openAction;

            this.SelectedText   = selectedText;
            this.Link           = link;

            DataContext = this;
            InitializeComponent();

            this.link.Visibility = Visibility.Visible;
            text.Visibility = Visibility.Visible;
        }

        public double opacitySelection
        {
            get
            {
                return string.IsNullOrEmpty(SelectedText) ? 0.4 : 1;
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            var brush = Utility.GetBrushColor("#f2f2f2");
            ((Border)sender).Background = brush;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = System.Windows.Media.Brushes.White;
        }

        private void Border_SearchText(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedText)) return;

            searAction?.Invoke(SelectedText);
            Utility.ClosePopup();
        }

        private void Border_CopyText(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedText)) return;

            copyAction?.Invoke(SelectedText);
            Utility.ClosePopup();
        }

        private void Border_SelectAllText(object sender, MouseButtonEventArgs e)
        {
            seleAction?.Invoke(null);
            Utility.ClosePopup();
        }

        private void Border_OpenLink(object sender, MouseButtonEventArgs e)
        {
            openAction?.Invoke(Link);
            Utility.ClosePopup();
        }

        private void Border_CopyLink(object sender, MouseButtonEventArgs e)
        {
            copyAction?.Invoke(Link);
            Utility.ClosePopup();
        }
    }
}
