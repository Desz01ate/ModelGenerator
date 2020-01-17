using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGenerator.Core.Event
{
    public class FileGeneratedEventArgs : EventArgs
    {
        public readonly string FileName;
        public FileGeneratedEventArgs(string fileName)
        {
            this.FileName = fileName;
        }
    }
}
