using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;


namespace Axiom.Runtime.PluginHost
{
    public class AMExtensions
    {
        // Predicates
        private ArrayList _predicates;

        public AMExtensions()
        {
            _predicates = new ArrayList();
        }

        public ArrayList Load()
        {
            /* Disabled for demo release 
            XmlTextReader xmlReader = new XmlTextReader("AMExtensions.xml");
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "ExternalPredicate")
                {
                    ExternalPredicate externalPredicate = new ExternalPredicate();

                    xmlReader.MoveToAttribute("name");
                    externalPredicate.Name = xmlReader.Value;

                    xmlReader.MoveToAttribute("path");
                    externalPredicate.Path = xmlReader.Value;

                    _predicates.Add(externalPredicate);         
                }
            }

            return _predicates;
             */
            return null;
        }
    }
}
