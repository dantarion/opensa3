using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenSALib3;
namespace Tabuu
{
    /// <summary>
    /// Interaction logic for ScriptWindow.xaml
    /// </summary>
    public partial class ScriptWindow : Window
    {
        public ScriptWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Text += ">>\n" + RunScript.FromString(ScriptTextBox.Text, null);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Text = "";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            OutputTextBox.Text += ">>" + CommandTextBox.Text + "\n" + RunScript.FromString(CommandTextBox.Text, null);
        }
    }
}
