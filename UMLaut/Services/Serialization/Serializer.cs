﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UMLaut.Model.Implementation;

namespace UMLaut.Services
{
    public class Serializer
    {
        public async void AsyncSerializeToFile(Diagram diagram)
        {
            await Task.Run(() => SerializeToFile(diagram));
        }

        private void SerializeToFile(Diagram diagram)
        {
            using (FileStream stream = File.Create(diagram.FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                serializer.Serialize(stream, diagram);
            }
        }

    }
}
