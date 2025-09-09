using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Ketarin.Database;

namespace Ketarin
{
    [Serializable(), XmlInclude(typeof(CustomSetupInstruction)), XmlInclude(typeof(StartProcessInstruction)), XmlInclude(typeof(CopyFileInstruction)), XmlInclude(typeof(CloseProcessInstruction))]
    public class SetupInstruction : ICloneable
    {
        /// <summary>
        /// Application this instruction belongs to.
        /// </summary>
        [XmlIgnore()]
        public ApplicationJob Application { get; set; } = null!;

        /// <summary>
        /// Name of the instruction (type).
        /// </summary>
        public virtual string Name
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public virtual void Execute()
        {
        }

        /// <summary>
        /// Saves the setup instructions to the database.
        /// </summary>
        public void Save(IDbTransaction transaction, int position)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            XmlWriterSettings settings = new XmlWriterSettings {Indent = true};

            StringBuilder output = new StringBuilder();

            using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
            {
                serializer.Serialize(xmlWriter, this);
            }

            // Save setup instruction to JSON database
            JsonSetupInstruction jsonInstruction = new JsonSetupInstruction
            {
                JobGuid = this.Application.Guid.ToString(),
                Position = position,
                Data = output.ToString()
            };
            JsonDbManager.SaveSetupInstruction(jsonInstruction);
        }

        #region ICloneable Member

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
