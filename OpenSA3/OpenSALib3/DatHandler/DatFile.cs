﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using BrawlLib.SSBB.ResourceNodes;
using OpenSALib3.Utility;
using System.Text;
using OpenSALib3.Moveset;

namespace OpenSALib3.DatHandler
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DatFileHeader
    {
        public bint FileSize;
        public bint DataChunkSize;
        public bint OffsetCount;
        public bint SectionCount;
        public bint ReferenceCount;
        public bint Unknown1;
        public bint Unknown2;
        public bint Unknown3;
    }

    public unsafe class DatFile : DatElement
    {
        #region Static Methods
        public static unsafe bool isDatFile(ResourceNode node)
        {
            return node.WorkingUncompressed.Length == *(bint*)node.WorkingUncompressed.Address;
                
        }
        public static DatFile FromFile(string filename)
        {
            var node = NodeFactory.FromFile(null, filename);
            return FromNode(node);
        }

        public static DatFile FromNode(ResourceNode node)
        {
            var file =
                new DatFile(node)
                {
                    Filename = node.RootNode.OriginalSource.Map.FilePath
                };
            return file;
        }
        #endregion

        private MDL0Node _model = null;
        public MDL0Node Model
        {
            get { return _model; }
        }
        private Dictionary<int, string> _boneNames;
        public void LoadModel(string filename)
        {
            try
            {
                _boneNames = new Dictionary<int, string>();
                var node = NodeFactory.FromFile(null, filename);
                _model = node.Children[0].Children[0].Children[0] as MDL0Node;
                foreach (MDL0BoneNode innernode in _model.FindChildrenByType("", ResourceType.MDL0Bone))
                {
                    //Debug.Assert(_boneNames.ContainsKey(innernode.BoneIndex));
                    _boneNames[innernode.BoneIndex] = innernode.Name;
                }
            }
            catch (Exception)
            {
                _model = null;
                _boneNames = null;
            }
        }
        public string GetBoneName(int boneIndex)
        {
            if (_boneNames == null)
                return "No Model Ref Loaded";
            if (!_boneNames.ContainsKey(boneIndex))
                return "????";
            return _boneNames[boneIndex];
        }

        private DatFileHeader _header;
        private VoidPtr _dataChunk;
        private DataSource _source;
        public ResourceNode Node;

        [Browsable(false)]
        private NamedList<DatSection> _sections = new NamedList<DatSection>("Sections");
        public List<DatSection> Sections { get { return _sections; } }

        private NamedList<DatSection> _references = new NamedList<DatSection>("References");
        [Browsable(false)]
        public List<DatSection> References { get { return _references; } }

        [Category("File")]
        [ReadOnly(true)]
        [Browsable(true)]
        public bool Changed { get; set; }

        [Category("File")]
        [ReadOnly(true)]
        [Browsable(true)]
        public String Filename { get; set; }

        [Browsable(false)]
        public override VoidPtr Address
        {
            get { return Node.WorkingSource.Address + Marshal.SizeOf(_header); }
        }

        [Browsable(false)]
        public override DatFile RootFile
        {
            get { return this; }
        }
        private List<int> external = new List<int>();
        public bool isExternal(int offset)
        {
            return external.Contains(offset);
        }
        protected DatFile(ResourceNode node)
            : base(null, 0)
        {
            _source = node.OriginalSource;
            Node = node;
            _header = *(DatFileHeader*)node.WorkingUncompressed.Address;
            if (_header.FileSize != node.WorkingUncompressed.Length)
                throw new Exception("This is not a valid moveset file");
            Name = node.TreePathAbsolute;
            Length = (int)(_header.FileSize - Marshal.SizeOf(_header));
            Changed = false;
            Color = System.Drawing.Color.Transparent;
            //Start Parse
            var section = _header.DataChunkSize + _header.OffsetCount * 4;
            var stringBase = (int)(section + (_header.SectionCount + _header.ReferenceCount) * 8);
            var section2 = section + _header.SectionCount * 8;
            //Parse References FIRST

            for (var i = 0; i < _header.ReferenceCount; i++)
            {
                var s = DatSection.Factory(this, section2, stringBase);
                References.Add(s);
                var tmp = s.DataOffset;
                do
                {
                    external.Add(tmp);
                    tmp = ReadInt(tmp);
                }
                while (tmp != -1);
                section2 += 8;
            }
            //Parse sections
            for (var i = 0; i < _header.SectionCount; i++)
            {
                var s = DatSection.Factory(this, section, stringBase);
                Sections.Add(s);
                section += 8;
            }
            var offsetchunk = new UnknownElement(this, _header.DataChunkSize, "OffsetChunk", _header.OffsetCount * 4);
            ComputeDataLengths(Sections);
            foreach (DatSection s in Sections)
                s.Parse();
            var stringChunk = new UnknownElement(this, stringBase, "StringChunk", _header.FileSize - stringBase);

            //Setup Tree Structure
            this["Sections"] = Sections;
            this["References"] = References;
            this["OffsetChunk"] = offsetchunk;
            this["StringChunk"] = stringChunk;

        }

        private void ComputeDataLengths(IEnumerable<DatSection> sections)
        {
            var sorted = sections.OrderBy(x => x.DataOffset).ToList();
            for (var i = 0; i < sorted.Count; i++)
                if (i < sorted.Count - 1)
                    sorted[i].DataLength = sorted[i + 1].DataOffset - sorted[i].DataOffset;
                else sorted[i].DataLength = _header.DataChunkSize - sorted[i].DataOffset;
        }
        public string Report(bool onlyholes)
        {
            var usagedata = new List<UsageData>();
            foreach (IEnumerable child in _dictionary.Values)
                collectUsage(child, ref usagedata);
            foreach (UsageData ug in susagedata)
                usagedata.Add(ug);
            usagedata.Sort();
            var usage = 0;
            foreach (UsageData ug in usagedata)
                usage += ug.length;
            var sb = new StringBuilder(0x150000);
            sb.AppendFormat("{0}\n", this.Name);
            sb.AppendFormat("Total Size:{0}\n", Length);
            sb.AppendFormat("Bytes Parsed:{0}\n", usage);
            sb.AppendFormat("% Complete:{0}\n", (float)usage / (Length+0x20));

            int lastdata = 0;
            foreach (UsageData ug in usagedata)
            {
                if (ug.offset < lastdata)
                    sb.AppendFormat("OVERLAP\n");
                else if (ug.offset > lastdata)
                    sb.AppendFormat("{1:X08} HOLE - {0:X08}\n", ug.offset - lastdata, lastdata);
                if(!onlyholes)
                sb.AppendFormat("@0x{0:X08} - len {1:X08} - {2:25}\n", ug.offset, ug.length, ug.ID);
                lastdata = ug.offset + ug.length;

            }
            return sb.ToString();
        }
        public struct UsageData : IComparable<UsageData>
        {
            public int offset;
            public int length;
            public string ID;

            public int CompareTo(UsageData other)
            {
                return offset - other.offset;
            }
        }
        public void collectUsage(IEnumerable element, ref List<UsageData> list)
        {
            foreach (IEnumerable child in element)
                collectUsage(child, ref list);//Get the child usage
            DatElement de = element as DatElement;
            if (de != null)
            {
                UsageData ud;
                ud.offset = de.FileOffset;
                ud.length = de.Length;
                ud.ID = de.Path + " " + de.GetType().Name;
                if (!list.Exists(x => x.offset == de.FileOffset))
                    list.Add(ud);
            }
        }
        List<UsageData> susagedata = new List<UsageData>();
        public string ReadString(int offset)
        {
            string s = new String((sbyte*)(Address + offset));
            UsageData ud;
            ud.offset = offset;
            ud.length = s.Length + 1;
            ud.length += ud.length % 8;
            ud.ID = "String";
            // if (susagedata.Count(x => x.offset == offset) == 0)
            //     susagedata.Add(ud);
            return s;
        }
    }
}