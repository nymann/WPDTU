using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UMLaut.Model.Implementation;

namespace UMLaut.Serialization
{
    public class Serializer
    {

        public void SerializeToFile(Diagram diagram)
        {
            using (FileStream stream = File.Create(diagram.FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                serializer.Serialize(stream, diagram);
            }
        }

    }
}
