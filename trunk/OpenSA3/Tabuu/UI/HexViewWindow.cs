using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
namespace Tabuu
{
    namespace UI
    {
        public partial class HexViewWindow : Form
        {
            public HexViewWindow(Be.Windows.Forms.IByteProvider provider, long offset, String name, long selectionLength = 4)
            {
                InitializeComponent();
                hexBox1.ByteProvider = provider;
                Text += name;
                hexBox1.ScrollByteIntoView(offset);
                hexBox1.Select(offset, selectionLength);

            }
        }
    }
}
