using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace DBModel
{
    public class XmlManager
    {
       private XmlDocument xmlDoc;
       private string XmlPath;

        /// <summary>
        /// This class need the Xml File name that going to be load. 
        /// </summary>
        /// <param name="fileName"></param>
        public XmlManager(string fileName){
            xmlDoc = new XmlDocument();
            this.XmlPath = System.Configuration.ConfigurationManager.AppSettings["XmlPath"].ToString();
            this.xmlDoc.Load(XmlPath+"/"+fileName);
        }

        public string getContentById(string id) { 
                string value= "" ;
                XmlNodeList list = ((XmlElement)xmlDoc.DocumentElement).ChildNodes;
                if (list.Count > 0) {
                    try
                    {
                        return list[0].Value.ToString();
                    }
                    catch (Exception ex) { 
                    
                    }
                }
                foreach (XmlElement v in list) {                    
                    if (v. GetAttribute("id").ToString().Equals(id)) {
                        value = v.InnerText;
                        return value;
                    }
                }
            return value;
        }
        
    }
}
