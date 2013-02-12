using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace DefaultConnection.Extensions
{
    public static class Helpers
    {
        /// ---- SerializeAnObject -----------------------------
        /// <summary>
        /// Serializes an object to an XML string
        /// </summary>
        /// <param name="AnObject">The Object to serialize</param>
        /// <returns>XML string</returns>
        public static string SerializeAnObject(object AnObject)
        {

            XmlSerializer Xml_Serializer = new XmlSerializer(AnObject.GetType());
            StringWriter Writer = new StringWriter();

            Xml_Serializer.Serialize(Writer, AnObject);
            return Writer.ToString();
        }


        /// ---- DeSerializeAnObject ------------------------------
        /// <summary>
        /// DeSerialize an object
        /// </summary>
        /// <param name="XmlOfAnObject">The XML string</param>
        /// <param name="ObjectType">The type of object</param>
        /// <returns>A deserialized object...must be cast to correct type</returns>
        public static Object DeSerializeAnObject(string XmlOfAnObject, Type ObjectType)
        {
            StringReader StrReader = new StringReader(XmlOfAnObject);
            XmlSerializer Xml_Serializer = new XmlSerializer(ObjectType);
            XmlTextReader XmlReader = new XmlTextReader(StrReader);
            try
            {
                Object AnObject = Xml_Serializer.Deserialize(XmlReader);
                return AnObject;
            }
            finally
            {
                XmlReader.Close();
                StrReader.Close();
            }
        }
    }
}