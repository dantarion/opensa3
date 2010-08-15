using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.Moveset
{

    class GlideData : DatElement
    {
        public unsafe GlideData(DatElement parent, uint offset)
            : base(parent, offset)
        {
            Name = "GlideData";
            Length = 0;
            for (int i = 0; i < 20; i++)
            {
                _children.Add(new GenericElement<float>(this, (uint)(FileOffset + i * 4), "GlideFloat"));
            }
            _children.Add(new GenericElement<int>(this, (uint)(FileOffset + 20 * 4), "GlideInt1"));
            _children.Add(new GenericElement<int>(this, (uint)(FileOffset + 21 * 4), "GlideInt1"));
        }
    }
}
