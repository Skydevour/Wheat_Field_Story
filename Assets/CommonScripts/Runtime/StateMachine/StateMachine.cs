namespace CommonFramework.Runtime.FiniteStateMachine
{
    /// <summary>
    /// StateMachine handler class
    /// </summary>
    public class StateMachine
    {
        #region Private Fields

        IState currentState;

        #endregion

        #region Public Methods

        /// <summary>
        /// Change state to the desired state
        /// </summary>
        /// <param name="newState">state to be changed</param>
        public void ChangeState(IState newState)
        {
            if (currentState != null)
                currentState.OnStateExit();

            currentState = newState;
            currentState.OnStateEnter();
        }

        /// <summary>
        /// Update state machine
        /// </summary>
        public void OnMachineUpdate()
        {
            if (currentState != null)
                currentState.OnStateUpdate();
        }

        #endregion
    }
}
