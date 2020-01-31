using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ModelVisualizer.Interface
{
    public interface IPreviewable
    {
        public string Content { get; }
        public DataTable Structure { get; }
    }
}
