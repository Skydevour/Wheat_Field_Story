namespace CommonFramework.Runtime.CommandLineHandling
{
    /// <summary>
    /// An option passed by a Command Line application.
    /// </summary>
    public class CommandLineOption
    {
        /// <summary>
        /// The Name of the Option.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Value associated with this Option.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The Type of this Option.
        /// </summary>
        public OptionTypeEnum OptionType { get; set; }
    }
}