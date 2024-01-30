using System;

namespace CommonFramework.Runtime.CommandLineHandling.CommandLineOptions
{
    public abstract class CommandLineStringValueBase : CommandLineOptionBase<string>
    {
        public CommandLineStringValueBase(Action<string> onOptionGet) : base(onOptionGet)
        {
        }

        protected override bool OnParsingValues(string optionInString, out string value)
        {
            value = string.Empty;

            if (!AllowEmptyValues
                && string.IsNullOrEmpty(optionInString))
            {
                return false;
            }

            value = optionInString;
            return true;
        }
    }
}
