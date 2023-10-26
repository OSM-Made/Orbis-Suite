using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuiteService
{
    internal class OrbisConsoleFormatter : ConsoleFormatter
    {
        public OrbisConsoleFormatter(string name) : base(name) { }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
        {
            string message =
            logEntry.Formatter(
                logEntry.State, logEntry.Exception);

            if (message == null)
            {
                return;
            }

            textWriter.Write($"{DateTime.Now.ToString("HH:mm:ss")}[OrbisSuiteService]");
            textWriter.Write(message);
            textWriter.Write("\n");
        }
    }
}
