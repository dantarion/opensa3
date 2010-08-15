using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;
namespace OpenSALib3.PSA
{
    class CommandList : DatElement
    {
        buint _offset;
        public unsafe CommandList(DatElement parent, uint offset, Dictionary<int, List<Command>> subroutines)
            : base(parent, offset)
        {
            _offset = *(buint*)(Address);
            if(_offset != 0)
                _children = Command.ReadCommands(this, _offset, subroutines);
            Color = System.Drawing.Color.CadetBlue;
        }
    }
}
