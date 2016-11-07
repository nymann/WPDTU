using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLaut.Model.Enum;
using UMLaut.Model.Interface;

namespace UMLaut.Model.Implementation
{
    public class UMLLine : ILine
    {
        /// <summary>
        /// Id of the starting shape
        /// </summary>
        public Guid FromId { get; set; }
        /// <summary>
        /// Id of the end shape
        /// </summary>
        public Guid ToId { get; set; }
        /// <summary>
        /// Label of the line
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Type of the line
        /// </summary>
        public ELine Type { get; }


        /// <summary>
        /// Default constructor for the line. 
        /// </summary>
        /// <param name="FromId"></param>
        /// <param name="ToId"></param>
        public UMLLine(Guid FromId, Guid ToId)
        {
            Type = ELine.Solid;
            this.FromId = FromId;
            this.ToId = ToId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FromId"></param>
        /// <param name="ToId"></param>
        /// <param name="Type"></param>
        public UMLLine(Guid FromId, Guid ToId, ELine Type)
        {
            this.Type = Type;
            this.FromId = FromId;
            this.ToId = ToId;
        }
    }
}
