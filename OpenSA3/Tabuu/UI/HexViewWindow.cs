using System;
using System.Windows.Forms;

namespace Tabuu.UI {
    namespace UI {
        public sealed partial class HexViewWindow : Form {
            public HexViewWindow(Be.Windows.Forms.IByteProvider provider, long offset, String name, long selectionLength = 4) {
                InitializeComponent();
                hexBox1.ByteProvider = provider;
                Text = Text != null ? Text + name : name;
                hexBox1.ScrollByteIntoView(offset);
                hexBox1.Select(offset, selectionLength);

            }
        }
    }
}
