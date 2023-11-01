using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace YPPGUtilities.Core
{

    /// <summary>
    /// ! Warning: The Logger class is not thread-safe
    /// The Logger class is used to log messages, whilst also providing a way to toggle the messages
    /// rendered in Unity's console on and off. To use, create the Logger once (!) and then
    /// access it from your scripts using: Logger.Instance
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// The global instance making use of the singleton design pattern
        /// </summary>
        public static Logger Instance { private set; get; }

        /// <summary>
        /// If set to true, all info logs are also printed to the Unity console
        /// </summary>
        public bool ShouldPrintInfoLogs { private get; set; }

        /// <summary>
        /// If set to true, all warning logs are also printed to the Unity console
        /// </summary>
        public bool ShouldPrintWarningLogs { private get; set; }

        /// <summary>
        /// If set to true, all error logs are also printed to the Unity console
        /// </summary>
        public bool ShouldPrintErrorLogs { private get; set; }

        /// <summary>
        /// If set to true, all success logs are also printed to the Unity console
        /// </summary>
        public bool ShouldPrintSuccessLogs { private get; set; }

        /// <summary>
        /// Stores all logs
        /// </summary>
        private LinkedList<Log> _log;

        /// <summary>
        /// Creates the logger instance
        /// </summary>
        public Logger()
        {
            if (Instance != null)
                throw new TypeAccessException("Should not try to create a new logger instance " +
                    "if there is already an existing instance.");

            Instance = this;
            _log = new();
        }

        /// <summary>
        /// Logs the specified error and logs it in the Unity console if ShouldPrintInfoLogs
        /// is set to true
        /// </summary>
        /// <param name="info">the info message as a string</param>
        public void LogInfo(string info)
        {
            _log.AddFirst(new Log(info, Log.LogType.INFO));

            if (ShouldPrintInfoLogs)
            {
                Debug.Log(info);
            }
        }

        public void LogInfo(string info, LoggingOptions options)
        {
            if (options.EnableLogging)
            {
                LogInfo(info);
            }
        }

        /// <summary>
        /// Logs the specified error and logs it in the Unity console if ShouldPrintSuccessLogs
        /// is set to true
        /// </summary>
        /// <param name="successMessage">the success message as a string</param>
        public void LogSuccess(string successMessage)
        {
            _log.AddFirst(new Log(successMessage, Log.LogType.SUCCESS));

            if (ShouldPrintSuccessLogs)
            {
                Debug.Log($"<color=green>{successMessage}</color>");
            }
        }

        public void LogSuccess(string successMessage, LoggingOptions options)
        {
            if (options.EnableLogging)
            {
                LogSuccess(successMessage);
            }
        }

        /// <summary>
        /// Logs the specified error and logs it in the Unity console if ShouldPrintErrorLogs
        /// is set to true
        /// </summary>
        /// <param name="info">the error message as a string</param>
        public void LogError(string error)
        {
            _log.AddFirst(new Log(error, Log.LogType.ERROR));

            if (ShouldPrintErrorLogs)
            {
                Debug.LogError(error);
            }
        }

        public void LogError(string error, LoggingOptions options)
        {
            if (options.EnableLogging)
            {
                LogError(error);
            }
        }

        /// <summary>
        /// Logs the specified warning and logs it in the Unity console if ShouldPrintWarningLogs
        /// is set to true
        /// </summary>
        /// <param name="warning">the warning message as a string</param>
        public void LogWarning(string warning)
        {
            _log.AddFirst(new Log(warning, Log.LogType.WARNING));

            if (ShouldPrintWarningLogs)
            {
                Debug.LogWarning(warning);
            }
        }

        public void LogWarning(string warning, LoggingOptions options)
        {
            if (options.EnableLogging)
            {
                LogWarning(warning);
            }
        }

        /// <summary>
        /// Represents a single log, which consists of a message and a log type
        /// </summary>
        public class Log
        {
            private readonly string _message;
            private readonly LogType _logType;

            /// <summary>
            /// Creates a new log with the message and log type as the required arguments
            /// </summary>
            /// <param name="message">the message describing the log</param>
            /// <param name="logType">the type of the log</param>
            /// </summary>
            public Log(string message, LogType logType)
            {
                _message = message;
                _logType = logType;
            }

            /// <summary>
            /// INFO: Logs that inform the user about an event
            /// ERROR: Logs that inform the user about an error
            /// WARNING: Logs that warn the user about an event
            /// SUCCESS: Logs that inform the user about a successful event 
            /// </summary>
            public enum LogType
            {
                INFO,
                ERROR,
                WARNING,
                SUCCESS
            }
        }
    }

}
