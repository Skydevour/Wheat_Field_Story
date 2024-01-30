namespace CommonFramework.Runtime.FiniteStateMachine
{
    /// <summary>
    /// Interface of State
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Calling when entering state
        /// </summary>
        void OnStateEnter();

        /// <summary>
        /// Calling when updating state
        /// </summary>
        void OnStateUpdate();

        /// <summary>
        /// Calling when leaving state
        /// </summary>
        void OnStateExit();
    }
}
