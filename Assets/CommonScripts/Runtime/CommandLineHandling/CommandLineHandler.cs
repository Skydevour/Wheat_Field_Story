using CommonFramework.Runtime.CommandLineHandling.CommandLineOptions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonFramework.Runtime.CommandLineHandling
{
    public static class CommandLineHandler
    {
        private static List<IHandlingOptions> handlers = new List<IHandlingOptions>();

        public static void ParsingCommandLine()
        {
            string[] args = Environment.GetCommandLineArgs();
            var options = CommandLineParser.ParseOptions(args);
            foreach (var option in options)
            {
                Debug.LogFormat("{0}, {1}, {2}", option.Name, option.OptionType, option.Value);
                foreach (var handler in handlers)
                {
                    if (handler.OnHandlingOptions(option))
                        break;
                }
            }
        }

        public static void Register(IHandlingOptions handler)
        {
            handlers.Add(handler);
        }
    }
}