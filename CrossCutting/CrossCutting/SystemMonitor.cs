using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting
{
    public class SystemMonitor
    {
        private FileExtensionManager fileManager;
        private CrashLoggingService crashLogger;
        private EmailService emailLogger;
        private CorruptFileLoggingService corruptFileLogger;
        public SystemMonitor(FileExtensionManager fileExtensionManager, CrashLoggingService crashLoggingService, EmailService emailService, CorruptFileLoggingService corruptFileLoggingService)
        {
            fileManager = fileExtensionManager;
            crashLogger = crashLoggingService;
            emailLogger = emailService;
            corruptFileLogger = corruptFileLoggingService;
        }
        public void ProcessDump(string DumpFile)
        {
            if (fileManager.IsValid(DumpFile))
            {
                /* Dump file valid so log details with the crashLoggingService Web service. */
                ValidFileLogDump(DumpFile);
            }
            else
            {
                /* Dump file is invalid so log details with the corruptFileLoggingService Web service. */
                InvalidFileLogDump(DumpFile);
            }
        }

        private void ValidFileLogDump(string DumpFile)
        {
            try
            {
                crashLogger.LogError("Dump file is valid" + DumpFile);
                // called crashLoggingService
            }
            catch (Exception e)
            {
                emailLogger.SendEmail("HelpDesk@lit.ie", "crashLoggingService Web service threw exception", e.Message);
                /* i.e. called emailService which logs the exception with the email service */
            }
        }

        private void InvalidFileLogDump(string DumpFile)
        {
            try
            {
                corruptFileLogger.LogCorruptionDetails("Dump file is corrupt: " + DateTime.Now + DumpFile);  // called crashLoggingService
            }
            catch (Exception e)
            {
                emailLogger.SendEmail("HelpDesk@lit.ie", "corruptFileLoggingService Web service threw exception", e.Message);
                /* i.e. called emailService which logs the exception with the email service */
            }
        }
    }

}
