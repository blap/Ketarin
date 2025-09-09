using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Ketarin.Database;

namespace Ketarin
{
    /// <summary>
    /// Represents a piece of code that can be re-used for any commands.
    /// </summary>
    public class Snippet
    {
        /// <summary>
        /// Gets or sets the GUID of the snippet.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name of the snippet.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the language/type of the snippet.
        /// </summary>
        public ScriptType Type { get; set; }

        /// <summary>
        /// Gets or sets the content of the snippet.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Saves the snippet to the database.
        /// </summary>
        public void Save()
        {
            if (this.Guid == Guid.Empty)
            {
                this.Guid = Guid.NewGuid();

                // Overwrite existing names - check if snippet already exists
                JsonSnippet[] existingSnippets = JsonDbManager.GetSnippets();
                foreach (JsonSnippet snippet in existingSnippets)
                {
                    if (snippet.Name == Name && snippet.Type == Type.ToString())
                    {
                        if (!string.IsNullOrEmpty(snippet.SnippetGuid))
                        {
                            this.Guid = Guid.Parse(snippet.SnippetGuid);
                        }
                        break;
                    }
                }
            }

            // Save snippet to JSON database
            JsonSnippet jsonSnippet = new JsonSnippet
            {
                SnippetGuid = this.Guid.ToString(),
                Name = Name,
                Type = Type.ToString(),
                Text = Text
            };
            JsonDbManager.SaveSnippet(jsonSnippet);
        }

        internal void Hydrate(IDataReader reader)
        {
            this.Name = reader["Name"] as string ?? string.Empty;
            this.Type = Command.ConvertToScriptType(reader["Type"] as string ?? string.Empty);
            this.Text = reader["Text"] as string ?? string.Empty;
            this.Guid = new Guid(reader["SnippetGuid"] as string ?? string.Empty);
        }

        /// <summary>
        /// Removes a snippet from the database.
        /// </summary>
        internal void Delete()
        {
            // Delete snippet from JSON database
            JsonDbManager.DeleteSnippet(this.Guid.ToString());
        }
    }
}
