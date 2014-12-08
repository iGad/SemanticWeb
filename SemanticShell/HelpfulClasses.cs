using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticShell
{
    public class ToolClass
    {
        public string ToolName { get; private set; }
        public int ToolIndex { get; private set; }
        public bool IsSelected { get; private set; }

        public ToolClass(string name, int index)
        {
            ToolName = name;
            IsSelected = false;
            ToolIndex = index;
        }
    }
}
