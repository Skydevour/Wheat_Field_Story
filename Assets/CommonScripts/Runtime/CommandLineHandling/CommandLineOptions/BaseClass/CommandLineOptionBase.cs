using System;

namespace CommonFramework.Runtime.CommandLineHandling.CommandLineOptions
{
    public abstract class CommandLineOptionBase<T> : IHandlingOptions
    {
        protected abstract char ShortOptionName { get; }
        protected abstract string LongOptionName { get; }

        protected abstract bool AllowEmptyValues { get; }

        private Action<T> onOptionGetCallback;

        public CommandLineOptionBase(Action<T> onOptionGet)
        {
            onOptionGetCallback = onOptionGet;
        }

        public bool OnHandlingOptions(CommandLineOption option)
        {
            //Don't handle Symbols
            if (option.OptionType == OptionTypeEnum.Symbol)
            {
                return false;
            }

            //Shortname checks
            if (option.OptionType == OptionTypeEnum.ShortName
                && option.Name.Length != 1)
            {
                return false;
            }

            if (option.OptionType == OptionTypeEnum.ShortName
                && option.Name.Length == 1
                && option.Name[0] != ShortOptionName)
            {
                return false;
            }

            //Long name checks
            if (option.OptionType == OptionTypeEnum.LongName
                && !option.Name.Equals(LongOptionName))
            {
                return false;
            }

            //Parsing value checks
            if (!OnParsingValues(option.Value, out T value))
            {
                return false;
            }

            onOptionGetCallback?.Invoke(value);
            return true;
        }

        protected abstract bool OnParsingValues(string optionInString, out T value);
    }
}
