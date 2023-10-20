using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
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
    /// Logica di interazione per LinkDialog.xaml
    /// </summary>
    public partial class LinkDialog : UserControl
    {
        private Action<string> copyAction;
        private Action<string> openAction;
        private string link = "";

        public LinkDialog(Action<string> copyAction, Action<string> openAction, string link)
        {
            this.openAction = openAction;
            this.copyAction = copyAction;
            this.link       = link;

            DataContext = this;
            InitializeComponent();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            var brush = Utility.GetSelectionBrushColor("#f2f2f2");
            ((Border)sender).Background = brush;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = System.Windows.Media.Brushes.White;
        }

        private void Border_OpenLink(object sender, MouseButtonEventArgs e)
        {
            openAction?.Invoke(link);
            Utility.ClosePopup();
        }

        private void Border_CopyLink(object sender, MouseButtonEventArgs e)
        {
            copyAction?.Invoke(link);
            Utility.ClosePopup();
        }
    }
}
