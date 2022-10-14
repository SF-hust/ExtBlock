using System;
using System.Collections.Generic;
using System.Text;

namespace ExtBlock.Utility.Logger
{
    public class LoggerWarp : ILogger
    {
        private ILogger? _logger;

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        public LoggerWarp(ILogger? logger)
        {
            _logger = logger;
        }

        public void Trace(string message)
        {
            _logger?.Trace(message);
        }

        public void Info(string message)
        {
            _logger?.Info(message);
        }

        public void Debug(string message)
        {
            _logger?.Debug(message);
        }

        public void Warn(string message)
        {
            _logger?.Warn(message);
        }

        public void Error(string message)
        {
            _logger?.Error(message);
        }
    }
}
