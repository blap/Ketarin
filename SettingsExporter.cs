using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Ketarin.Database;

namespace Ketarin
{
    /// <summary>
    /// Exports and imports Ketarin settings to files.
    /// </summary>
    internal static class SettingsExporter
    {
        public static void ExportToFile(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings {Indent = true};

            XmlWriter writer = XmlWriter.Create(filename, settings);

            writer.WriteStartElement("Ketarin");

            writer.WriteStartElement("Settings");
            // Export normal settings 
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
            // For JSON database, we need to convert settings to the expected format
            Dictionary<string, string> settingsDict = new Dictionary<string, string>();
            serializer.Serialize(writer, settingsDict);
            writer.WriteEndElement();

            // Export global variables (stored in "variables" table)
            writer.WriteStartElement("GlobalVariables");
            foreach (UrlVariable var in UrlVariable.GlobalVariables.Values)
            {
                writer.WriteStartElement("Variable");
                writer.WriteAttributeString("Name", var.Name);
                writer.WriteAttributeString("Content", var.CachedContent);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // Export code snippets (stored in "snippets" table)
            writer.WriteStartElement("CodeSnippets");
            foreach (Snippet snippet in DbManager.GetSnippets())
            {
                writer.WriteStartElement("Snippet");
                writer.WriteAttributeString("Guid", snippet.Guid.ToString());
                writer.WriteAttributeString("Name", snippet.Name);
                writer.WriteAttributeString("Type", ((int)snippet.Type).ToString());
                writer.WriteString(snippet.Text);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("SetupLists");
            // Save setup lists
            foreach (ApplicationList list in DbManager.GetSetupLists())
            {
                if (!list.IsPredefined)
                {
                    writer.WriteStartElement("List");
                    writer.WriteAttributeString("Name", list.Name);
                    writer.WriteAttributeString("Guid", Database.JsonDbManager.FormatGuid(list.Guid));

                    writer.WriteStartElement("Applications");
                    foreach (ApplicationJob app in list.Applications)
                    {
                        writer.WriteStartElement("Application");
                        writer.WriteAttributeString("Guid", app.Guid.ToString());
                        writer.WriteAttributeString("Name", app.Name);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();

            writer.Close();
        }

        public static void ImportFromFile(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            // Import settings from file as dictionary
            XmlElement? settingsElem = doc.SelectSingleNode("//Settings/dictionary") as XmlElement ??
                                      doc.SelectSingleNode("//dictionary") as XmlElement;

            if (settingsElem != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));

                using (StringReader textReader = new StringReader(settingsElem.OuterXml))
                {
                    var deserialized = serializer.Deserialize(textReader) as Dictionary<string, string>;
                    DbManager.SetSettings(deserialized ?? new Dictionary<string, string>());
                }
            }
            
            // Import global variables
            object? varNodesObj = doc.SelectSingleNode("//GlobalVariables");
            XmlElement? varNodes = varNodesObj as XmlElement;
            if (varNodes != null)
            {
                UrlVariable.GlobalVariables.Clear();

                object? variableNodesObj = doc.SelectNodes("//GlobalVariables/Variable");
                XmlNodeList? variableNodes = variableNodesObj as XmlNodeList;
                if (variableNodes != null)
                {
                    foreach (object varElemObj in variableNodes)
                    {
                        XmlElement? varElem = varElemObj as XmlElement;
                        if (varElem != null)
                        {
                            UrlVariable newVar = new UrlVariable
                            {
                                Name = varElem.GetAttribute("Name"),
                                CachedContent = varElem.GetAttribute("Content")
                            };
                            UrlVariable.GlobalVariables[newVar.Name] = newVar;
                        }
                    }
                }

                UrlVariable.GlobalVariables.Save();
            }

            // Import code snippets
            object? snippetNodesObj = doc.SelectSingleNode("//CodeSnippets");
            XmlElement? snippetNodes = snippetNodesObj as XmlElement;
            if (snippetNodes != null)
            {
                // Clear all snippets from JSON database
                JsonSnippet[] snippets = JsonDbManager.GetSnippets();
                foreach (JsonSnippet snippet in snippets)
                {
                    if (!string.IsNullOrEmpty(snippet.SnippetGuid))
                    {
                        JsonDbManager.DeleteSnippet(snippet.SnippetGuid);
                    }
                }

                object? snippetElementsObj = doc.SelectNodes("//CodeSnippets/Snippet");
                XmlNodeList? snippetElements = snippetElementsObj as XmlNodeList;
                if (snippetElements != null)
                {
                    foreach (object snippetElemObj in snippetElements)
                    {
                        XmlElement? snippetElem = snippetElemObj as XmlElement;
                        if (snippetElem != null)
                        {
                            Snippet snippet = new Snippet
                            {
                                Guid = new Guid(snippetElem.GetAttribute("Guid")),
                                Name = snippetElem.GetAttribute("Name"),
                                Type = (ScriptType) Convert.ToInt32(snippetElem.GetAttribute("Type")),
                                Text = snippetElem.InnerText
                            };
                            snippet.Save();
                        }
                    }
                }
            }

            object? setupNodesObj = doc.SelectSingleNode("//SetupLists");
            XmlElement? setupNodes = setupNodesObj as XmlElement;
            if (setupNodes != null)
            {
                // For JSON database, we don't need to clear setup lists as they're handled differently
                // The import process will overwrite existing lists

                object? listElementsObj = doc.SelectNodes("//SetupLists/List");
                XmlNodeList? listElements = listElementsObj as XmlNodeList;
                if (listElements != null)
                {
                    foreach (object listElemObj in listElements)
                    {
                        XmlElement? listElem = listElemObj as XmlElement;
                        if (listElem != null)
                        {
                            ApplicationList list = new ApplicationList
                            {
                                Name = listElem.GetAttribute("Name"),
                                Guid = new Guid(listElem.GetAttribute("Guid"))
                            };

                            object? appListElementsObj = listElem.SelectNodes("Applications/Application");
                            XmlNodeList? appListElements = appListElementsObj as XmlNodeList;
                            if (appListElements != null)
                            {
                                foreach (object appListElemObj in appListElements)
                                {
                                    XmlElement? appListElem = appListElemObj as XmlElement;
                                    if (appListElem != null)
                                    {
                                        Guid guid = new Guid(appListElem.GetAttribute("Guid"));

                                        ApplicationJob job = DbManager.GetJob(guid);
                                        if (job != null)
                                        {
                                            list.Applications.Add(job);
                                        }
                                    }
                                }
                            }

                            list.Save();
                        }
                    }
                }
            }            
        }
    }
}
