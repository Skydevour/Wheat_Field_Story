using System;

namespace CommonFramework.Runtime.CommandLineHandling.CommandLineOptions
{
    public abstract class CommandLineNoValueOptionBase : IHandlingOptions
    {
        protected abstract char ShortOptionName { get; }
        protected abstract string LongOptionName { get; }

        private Action onOptionGetCallback;

        public CommandLineNoValueOptionBase(Action onOptionGet)
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

            onOptionGetCallback?.Invoke();
            return true;
        }
    }
}
