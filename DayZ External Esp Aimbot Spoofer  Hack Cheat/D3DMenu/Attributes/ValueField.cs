using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DMenu.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct,
                       AllowMultiple = true)  // multiuse attribute
    ]
    public class ValueField : Attribute
    {
        public string Name { get; set; }

        public ValueField(string name)
        {
            this.Name = name;
        }
    }
}
