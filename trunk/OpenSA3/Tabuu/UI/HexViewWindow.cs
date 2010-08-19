using System;
using System.Windows.Forms;

namespace Tabuu.UI {
    public sealed partial class HexViewWindow : Form {
        public HexViewWindow(Be.Windows.Forms.IByteProvider provider, String name, long offset = 0,
                             long selectionLength = 0) {
            InitializeComponent();
            hexBox1.ByteProvider = provider;
            Text = Text != null ? Text + name : name;
            hexBox1.ScrollByteIntoView(offset);
            hexBox1.Select(offset, selectionLength);
        }

        private void HexBox1SelectionStartChanged(object sender, EventArgs e) {
            var offset = hexBox1.SelectionStart - hexBox1.SelectionStart%4;
            var bytes = new byte[4];
            for (var i = 0; i < 4; i++)
                bytes[i] = hexBox1.ByteProvider.ReadByte(offset + i);
            Array.Reverse(bytes);
            textBox1.Text = BitConverter.ToInt32(bytes, 0).ToString();
            textBox2.Text = BitConverter.ToSingle(bytes, 0).ToString();
        }

        private void TextBox3TextChanged(object sender, EventArgs e) {
            int bytesperLine;
            Int32.TryParse(textBox3.Text, out bytesperLine);
            if (bytesperLine != 0)
                hexBox1.BytesPerLine = bytesperLine;
        }

        private void Button1Click(object sender, EventArgs e) {
            int jumpto;
            var success = Int32.TryParse(textBox4.Text, System.Globalization.NumberStyles.HexNumber,
                           new System.Globalization.CultureInfo("en-US"), out jumpto);
            if (!success || jumpto >= hexBox1.ByteProvider.Length) return;
            hexBox1.ScrollByteIntoView(jumpto);
            hexBox1.Select(jumpto, 4);
        }

        private void textBox2_TextChanged2(object sender, EventArgs e)
        {
            var offset = hexBox1.SelectionStart - hexBox1.SelectionStart % 4;
            float value;
            if (!float.TryParse(textBox2.Text, out value))
                return;
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            for(int i = 0; i < 4; i++)
                hexBox1.ByteProvider.WriteByte(offset + i,bytes[i]);
            hexBox1.Refresh();
                
        }

        private void textBox1_TextChanged2(object sender, EventArgs e)
        {
            var offset = hexBox1.SelectionStart - hexBox1.SelectionStart % 4;
            int value;
            if (!int.TryParse(textBox1.Text, out value))
                return;
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            for (int i = 0; i < 4; i++)
                hexBox1.ByteProvider.WriteByte(offset + i, bytes[i]);
            hexBox1.Refresh();
        }
    }
}