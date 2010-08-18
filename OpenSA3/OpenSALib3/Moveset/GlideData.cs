using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.Moveset
{

    class GlideData : DatElement
    {
        public unsafe GlideData(DatElement parent, int offset)
            : base(parent, offset)
        {
            Name = "GlideData";
            Length = 0;
            for (int i = 0; i < 20; i++)
            {
                this[i] = new GenericElement<float>(this, (int)(FileOffset + i * 4), "GlideFloat");
            }
            this["GlideInt1"] = new GenericElement<int>(this, (int)(FileOffset + 20 * 4), "GlideInt1");
            this["GlideInt2"] = new GenericElement<int>(this, (int)(FileOffset + 21 * 4), "GlideInt2");
        }
    }
}
