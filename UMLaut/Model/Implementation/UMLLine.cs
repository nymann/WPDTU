using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLaut.Model.Enum;
using UMLaut.Model.Interface;

namespace UMLaut.Model.Implementation
{
    class UMLLine : ILine
    {
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public string Label { get; set; }
        public ELine Type { get; }


        public UMLLine(Guid FromId, Guid ToId)
        {
            this.FromId = FromId;
            this.ToId = ToId;
        }

    }
}
