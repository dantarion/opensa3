using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tabuu.UI
{
    interface TabuuByteProvider : Be.Windows.Forms.IByteProvider
    {
        Color GetByteColor(long index);
    }
}
