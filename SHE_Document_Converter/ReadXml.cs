using System;
using System.Collections.Generic;
using System.Xml;


namespace SHE_Document_converter
{
    public class ReadXml
    {
        public static Dictionary<string,string> ReadSqlFomrXML(string url)
        {
            Dictionary<string, string> sqls = new Dictionary<string, string>();
            using (XmlReader reader = XmlReader.Create(url))
            {
                string function = null;
                string sql = null;

                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element
                        && reader.Name == "SqlString")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element &&
                                reader.Name == "Function")
                            {
                                function = reader.ReadString();
                                break;
                            }
                        }
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element &&
                                reader.Name == "Sql")
                            {
                                sql = reader.ReadString();
                                break;
                            }
                        }
                        sqls[function] = sql;
                    }
                }
            }
            return sqls;

        }
    }
}
