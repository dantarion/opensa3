using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OpenSALib3;
using System.Windows.Controls;
using BrawlLib.SSBB.ResourceNodes;
namespace Tabuu
{
    namespace Utility
    {
        public class DatWrapper : DatFile, IEnumerable, Be.Windows.Forms.IByteProvider
        {
            public static DatWrapper fromFile(string filename)
            {
                var node = NodeFactory.FromFile(null, filename);
                return new DatWrapper(node.Children[0]);
            }
            protected DatWrapper(ResourceNode s)
                : base(s)
            {
            }
            public IEnumerator GetEnumerator()
            {
                return new DatEnemerator(this);
            }
            #region IByteProvider
            public void ApplyChanges()
            {
                throw new NotImplementedException();
            }

            public event EventHandler Changed;

            public void DeleteBytes(long index, long length)
            {
                throw new NotImplementedException();
            }

            public bool HasChanges()
            {
                throw new NotImplementedException();
            }

            public void InsertBytes(long index, byte[] bs)
            {
                throw new NotImplementedException();
            }

            public long Length
            {
                get { return FileSize; }
            }

            public event EventHandler LengthChanged;

            public unsafe byte ReadByte(long index)
            {
                return *(byte*)(Address + (uint)index);
            }

            public bool SupportsDeleteBytes()
            {
                throw new NotImplementedException();
            }

            public bool SupportsInsertBytes()
            {
                throw new NotImplementedException();
            }

            public bool SupportsWriteByte()
            {
                return true;
            }

            public unsafe void WriteByte(long index, byte value)
            {
                *(byte*)(Address + (uint)index) = value;
            }
            #endregion
            #region Scripting Usability Functions
            public unsafe int readInt(int offset)
            {
                return *(bint*)(Address + offset);
            }
            public unsafe short readShort(int offset)
            {
                return *(bshort*)(Address + offset);
            }
            public unsafe float readFloat(int offset)
            {
                return *(bfloat*)(Address + offset);
            }
            #endregion
        }
        class NamedList<T> : IEnumerable
        {
            private List<T> list;
            public String Name { get; set; }
            public NamedList(List<T> l, String n)
            {
                list = l;
                Name = n;
            }
            public IEnumerator GetEnumerator()
            {
                return list.GetEnumerator();
            }
            public override string ToString()
            {
                return Name;
            }
        }
        class DatEnemerator : IEnumerator
        {
            private DatFile f;
            public int i = -1;
            public DatEnemerator(DatFile f)
            {
                this.f = f;
            }

            public object Current
            {

                get
                {
                    switch (i)
                    {
                        case 0:
                            return new NamedList<DatSection>(f.Sections, "Sections");
                        case 1:
                            return new NamedList<DatSection>(f.References, "References");
                    }
                    return null;
                }
            }

            public bool MoveNext()
            {
                i++;
                if (i > 1)
                    return false;
                return true;
            }

            public void Reset()
            {
                i = -1;
            }
        }

    }
}