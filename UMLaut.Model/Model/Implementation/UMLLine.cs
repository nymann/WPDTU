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
        private UMLLine()
        {

        }
        public UMLLine(Guid From, Guid To)
        {
            Type = ELine.Solid;
            this.From = From;
            this.To = To;
        }

        public UMLLine(Guid From, ELine type)
        {
            this.Type = Type;
            this.From = From;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FromId"></param>
        /// <param name="ToId"></param>
        /// <param name="Type"></param>
        public UMLLine(Guid From, Guid To, ELine Type)
        {
            this.Type = Type;
            this.From = From;
            this.To = To;
        }
        /// <summary>
        /// Id of the starting shape
        /// </summary>
        public Guid From { get; set; }
        /// <summary>
        /// Id of the end shape
        /// </summary>
        public Guid To { get; set; }
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

    }
}
