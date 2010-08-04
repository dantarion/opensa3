﻿using System;
using System.Windows.Forms;

namespace Tabuu.UI
{
    public sealed partial class HexViewWindow : Form
    {
        public HexViewWindow(Be.Windows.Forms.IByteProvider provider, String name, long offset = 0, long selectionLength = 0)
        {
            InitializeComponent();
            hexBox1.ByteProvider = provider;
            Text = Text != null ? Text + name : name;
            hexBox1.ScrollByteIntoView(offset);
            hexBox1.Select(offset, selectionLength);

        }

        private void hexBox1_SelectionStartChanged(object sender, EventArgs e)
        {
            var offset = hexBox1.SelectionStart-hexBox1.SelectionStart % 4;
            var bytes = new byte[4];
            for (int i = 0; i < 4; i++)
                bytes[i] = hexBox1.ByteProvider.ReadByte(offset + i);
            Array.Reverse(bytes);
            textBox1.Text = BitConverter.ToInt32(bytes, 0).ToString();
            textBox2.Text = BitConverter.ToSingle(bytes, 0).ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            var bytesperLine = 0;
            Int32.TryParse(textBox3.Text, out bytesperLine);
            if (bytesperLine != 0)
                hexBox1.BytesPerLine = bytesperLine;
        }
    }
}
