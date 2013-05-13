using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
namespace SchoolApp.Extensions
{
    public static class Helpers
    {
        //Role constants
        public const string ADMIN_ROLE = "Administrator";
        public const string TEACHER_ROLE = "Teacher";
        public const string STUDENT_ROLE = "Student";
        public const string REGISTERED_USER_ROLE = "RegisteredUser";

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
        /// <summary>
        /// Get enum description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}