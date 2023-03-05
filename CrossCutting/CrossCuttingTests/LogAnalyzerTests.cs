using CrossCutting;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCuttingTests
{
    [TestFixture]
    public class SystemMonitorTests
    {
        private Mock<FileExtensionManager> mockFileManager;
        private Mock<CrashLoggingService> mockCrashLogger;
        private Mock<EmailService> mockEmailLogger;
        private Mock<CorruptFileLoggingService> mockCorruptFileLogger;

        private SystemMonitor systemMonitor;

        [SetUp]
        public void SetUp()
        {
            mockFileManager = new Mock<FileExtensionManager>();
            mockCrashLogger = new Mock<CrashLoggingService>();
            mockEmailLogger = new Mock<EmailService>();
            mockCorruptFileLogger = new Mock<CorruptFileLoggingService>();

            systemMonitor = new SystemMonitor(
                mockFileManager.Object,
                mockCrashLogger.Object,
                mockEmailLogger.Object,
                mockCorruptFileLogger.Object
            );
        }

        [Test]
        public void ProcessDump_WithValidFile_LogsDetailsWithCrashLogger()
        {
            // Arrange
            string validDumpFile = "validDumpFile.ext";
            mockFileManager.Setup(fm => fm.IsValid(validDumpFile)).Returns(true);

            // Act
            systemMonitor.ProcessDump(validDumpFile);

            // Assert
            mockCrashLogger.Verify(cl => cl.LogError("Dump file is valid" + validDumpFile), Times.Once);
            mockCorruptFileLogger.Verify(cfl => cfl.LogCorruptionDetails(It.IsAny<string>()), Times.Never);
            mockEmailLogger.Verify(el => el.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ProcessDump_WithInvalidFile_LogsDetailsWithCorruptFileLogger()
        {
            // Arrange
            string invalidDumpFile = "invalidDumpFile.invalidExt";
            mockFileManager.Setup(fm => fm.IsValid(invalidDumpFile)).Returns(false);

            // Act
            systemMonitor.ProcessDump(invalidDumpFile);

            // Assert
            mockCorruptFileLogger.Verify(cfl => cfl.LogCorruptionDetails($"Dump file is corrupt: {DateTime.Now}{invalidDumpFile}"), Times.Once);
            mockCrashLogger.Verify(cl => cl.LogError(It.IsAny<string>()), Times.Never);
            mockEmailLogger.Verify(el => el.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ProcessDump_WithValidFileAndCrashLoggerThrowsException_LogsDetailsWithEmailLogger()
        {
            // Arrange
            string validDumpFile = "validDumpFile.ext";
            mockFileManager.Setup(fm => fm.IsValid(validDumpFile)).Returns(true);
            mockCrashLogger.Setup(cl => cl.LogError(It.IsAny<string>())).Throws<Exception>();

            // Act
            systemMonitor.ProcessDump(validDumpFile);

            // Assert
            mockCrashLogger.Verify(cl => cl.LogError("Dump file is valid" + validDumpFile), Times.Once);
            mockEmailLogger.Verify(el => el.SendEmail("HelpDesk@lit.ie", "crashLoggingService Web service threw exception", It.IsAny<string>()), Times.Once);
            mockCorruptFileLogger.Verify(cfl => cfl.LogCorruptionDetails(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ProcessDump_WithInvalidFileAndCorruptFileLoggerThrowsException_LogsDetailsWithEmailLogger()
        {
            // Arrange
            string invalidDumpFile = "invalidDumpFile.invalidExt";
            mockFileManager.Setup(fm => fm.IsValid(invalidDumpFile)).Returns(false);
            mockCorruptFileLogger.Setup(cfl => cfl.LogCorruptionDetails(It.IsAny<string>())).Throws<Exception>();

            // Act
            systemMonitor.ProcessDump(invalidDumpFile);

            // Assert
            mockCorruptFileLogger.Verify(cfl => cfl.LogCorruptionDetails("Dump file is corrupt: " + DateTime.Now + invalidDumpFile), Times.Once);
            mockEmailLogger.Verify(el => el.SendEmail("HelpDesk@lit.ie", "corruptFileLoggingService Web service threw exception", It.IsAny<string>()), Times.Once);
        }
    }
}
