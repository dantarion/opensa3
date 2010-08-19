using System;
using System.Collections;
using System.Drawing;
using System.IO;

namespace Be.Windows.Forms {
    /// <summary>
    ///   Byte provider for (big) files.
    /// </summary>
    public class FileByteProvider : IByteProvider, IDisposable {
        #region WriteCollection class
        /// <summary>
        ///   Represents the write buffer class
        /// </summary>
        private class WriteCollection : DictionaryBase {
            /// <summary>
            ///   Gets or sets a byte in the collection
            /// </summary>
            public byte this[long index] {
                get { return (byte)Dictionary[index]; }
                set { Dictionary[index] = value; }
            }

            /// <summary>
            ///   Adds a byte into the collection
            /// </summary>
            /// <param name = "index">the index of the byte</param>
            /// <param name = "value">the value of the byte</param>
            public void Add(long index, byte value) {
                Dictionary.Add(index, value);
            }

            /// <summary>
            ///   Determines if a byte with the given index exists.
            /// </summary>
            /// <param name = "index">the index of the byte</param>
            /// <returns>true, if the is in the collection</returns>
            public bool Contains(long index) {
                return Dictionary.Contains(index);
            }
        }
        #endregion

        /// <summary>
        ///   Occurs, when the write buffer contains new changes.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        ///   Contains all changes
        /// </summary>
        private readonly WriteCollection _writes;

        /// <summary>
        ///   Contains the file stream.
        /// </summary>
        private FileStream _fileStream;

        /// <summary>
        ///   Read-only access.
        /// </summary>
        private readonly bool _readOnly;

        /// <summary>
        ///   Initializes a new instance of the FileByteProvider class.
        /// </summary>
        /// <param name = "fileName"></param>
        public FileByteProvider(string fileName) {
            _writes = new WriteCollection();
            FileName = fileName;
            try {
                // try to open in write mode
                _fileStream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            } catch {
                // write mode failed, try to open in read-only and fileshare friendly mode.
                _fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                _readOnly = true;
            }
        }

        /// <summary>
        ///   Terminates the instance of the FileByteProvider class.
        /// </summary>
        ~FileByteProvider() {
            Dispose();
        }

        /// <summary>
        ///   Raises the Changed event.
        /// </summary>
        /// <remarks>
        ///   Never used.
        /// </remarks>
        private void OnChanged(EventArgs e) {
            if (Changed != null)
                Changed(this, e);
        }

        /// <summary>
        ///   Gets the name of the file the byte provider is using.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        ///   Returns a value if there are some changes.
        /// </summary>
        /// <returns>true, if there are some changes</returns>
        public bool HasChanges() {
            return (_writes.Count > 0);
        }

        /// <summary>
        ///   Updates the file with all changes the write buffer contains.
        /// </summary>
        public void ApplyChanges() {
            if (_readOnly)
                throw new Exception("File is in read-only mode.");
            if (!HasChanges())
                return;
            var en = _writes.GetEnumerator();
            while (en.MoveNext()) {
                var index = (long)en.Key;
                var value = (byte)en.Value;
                _fileStream.Position = index;
                _fileStream.Write(new[] { value }, 0, 1);
            }
            _writes.Clear();
        }

        /// <summary>
        ///   Clears the write buffer and reject all changes made.
        /// </summary>
        public void RejectChanges() {
            _writes.Clear();
        }

#pragma warning disable 067 //The event '____' is never used
        #region IByteProvider Members
        /// <summary>
        ///   Never used.
        /// </summary>
        public event EventHandler LengthChanged;
#pragma warning restore 067
        /// <summary>
        ///   Reads a byte from the file.
        /// </summary>
        /// <param name = "index">the index of the byte to read</param>
        /// <returns>the byte</returns>
        public byte ReadByte(long index) {
            if (_writes.Contains(index))
                return _writes[index];
            _fileStream.Position = index;
            return (byte)_fileStream.ReadByte();
        }

        /// <summary>
        ///   Gets the length of the file.
        /// </summary>
        public long Length {
            get { return _fileStream.Length; }
        }

        /// <summary>
        ///   Writes a byte into write buffer
        /// </summary>
        public void WriteByte(long index, byte value) {
            if (_writes.Contains(index))
                _writes[index] = value;
            else
                _writes.Add(index, value);
            OnChanged(EventArgs.Empty);
        }

        /// <summary>
        ///   Not supported
        /// </summary>
        public void DeleteBytes(long index, long length) {
            throw new NotSupportedException("FileByteProvider.DeleteBytes");
        }

        /// <summary>
        ///   Not supported
        /// </summary>
        public void InsertBytes(long index, byte[] bs) {
            throw new NotSupportedException("FileByteProvider.InsertBytes");
        }

        /// <summary>
        ///   Returns true
        /// </summary>
        public bool SupportsWriteByte() {
            return !_readOnly;
        }

        /// <summary>
        ///   Returns false
        /// </summary>
        public bool SupportsInsertBytes() {
            return false;
        }

        /// <summary>
        ///   Returns false
        /// </summary>
        public bool SupportsDeleteBytes() {
            return false;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        ///   Releases the file handle used by the FileByteProvider.
        /// </summary>
        public void Dispose() {
            if (_fileStream != null) {
                FileName = null;
                _fileStream.Close();
                _fileStream = null;
            }
            GC.SuppressFinalize(this);
        }
        #endregion

        public Color GetByteColor(long index) {
            throw new NotImplementedException();
        }
    }
}