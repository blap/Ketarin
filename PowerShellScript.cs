using System;
using System.Text;
using Ketarin.Forms;

namespace Ketarin
{
    internal class PowerShellScript
    {
        private readonly string scriptText;

        public PowerShellScript(string scriptText)
        {
            this.scriptText = scriptText;
        }

        public string LastOutput { get; private set; } = string.Empty;

        internal void Execute(ApplicationJob application, ApplicationJobError? errorInfo = null)
        {
            // Use our new ScriptExecutor instead of PowerShell
            Database.ScriptExecutor executor = new Database.ScriptExecutor(this.scriptText, Database.ScriptType.PowerShell);
            executor.Execute(application, errorInfo);
            this.LastOutput = executor.LastOutput;
        }
    }
}