using System;
using System.IO;

namespace LoggerNamespace
{
    public class Logger {
        private StreamWriter _logStream;
        private readonly string _logFilePath;

        public Logger(string logDirectory = "logs") {
            try {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(logDirectory);

                // Forming a file name with a timestamp
                string timestamp = GetLoggerDateTime();
                _logFilePath = Path.Combine(logDirectory, $"{timestamp}.log");

                // Open a file for recording (add to an existing one)
                _logStream = new StreamWriter(_logFilePath, append: true) { AutoFlush = true };
            }
            catch (Exception ex) {
                Console.Error.WriteLine($"Error: Could not open log file! {ex.Message}");
                LogError("Error: Could not open log file!");
            }
        }

        public string GetLoggerDateTime() {
            return DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        public void LogError(string message) {
            if (_logStream != null && _logStream.BaseStream.CanWrite) {
                string timestamp = GetLoggerDateTime();
                _logStream.WriteLine($"[{timestamp}] ERROR: {message}");
            }
        }

        public void Close() {
            if (_logStream != null) {
                _logStream.Close();
                _logStream.Dispose();
            }
        }
    }
}