using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset {
    public class MovesetSection : DatSection {
        public struct MovesetHeader { }
        public MovesetSection(VoidPtr ptr, VoidPtr stringPtr, DatElement parent)
            : base(ptr, stringPtr, parent) {

        }
    }
}
