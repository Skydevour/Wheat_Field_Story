namespace CommonFramework.Runtime.BitmaskActions
{
    /// <summary>
    /// Base class of BitmaskAction state
    /// </summary>
    public abstract class BitmaskAction
    {
        #region Protected Fields

        protected BitmaskActionMachine actionManager;

        //following actions, masks, flags, need to be overwritten by the inhereted class
        //they need to be defined with the application implementation

        /// <summary>
        /// Defined mask of this action
        /// </summary>
        protected abstract long ActionMask
        {
            get;
        }

        /// <summary>
        /// Other actions which will suspend this action from updating
        /// </summary>
        protected abstract long SuspendingActions
        {
            get;
        }

        /// <summary>
        /// Other actions which will block this actions from starting if it's not running
        /// </summary>
        protected abstract long BlockingActions
        {
            get;
        }

        /// <summary>
        /// Other actions which was required by this action, in order for this action to be executed
        /// Note that this action will be deactivate if required actions were killed
        /// </summary>
        protected abstract long RequiredActions
        {
            get;
        }

        /// <summary>
        /// Other actions which will cause this action to be terminted
        /// </summary>
        protected abstract long TerminatedActions
        {
            get;
        }

        /// <summary>
        /// This flags indicate that if this action can be restarted if it's already running
        /// </summary>
        protected abstract bool Retriggerable
        {
            get;
        }

        #endregion

        #region Public Virtual Methods

        /// <summary>
        /// Called when begin to execute actions
        /// </summary>
        public virtual void BeginAction() { }

        /// <summary>
        /// Called when action is ending
        /// </summary>
        public virtual void EndAction() { }

        /// <summary>
        /// Action to be updated in FixedUpdate
        /// </summary>
        public virtual void FixedUpdateAction() { }

        /// <summary>
        /// Action to be update
        /// </summary>
        public virtual void UpdateAction() { }

        /// <summary>
        /// Action to be updated in LateUpdate
        /// </summary>
        public virtual void LateUpdateAction() { }

        /// <summary>
        /// Check if this action can be activate or not
        /// </summary>
        /// <returns>Return true if can be activated</returns>
        public virtual bool CanBeActivated() { return true; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialization of Action
        /// </summary>
        /// <param name="actionManager">Owner BitmaskMachine to be assigned</param>
        public void Init(BitmaskActionMachine actionManager)
        {
            this.actionManager = actionManager;
        }

        /// <summary>
        /// Gets the action mask, one unique bit mask
        /// </summary>
        /// <returns>The action mask.</returns>
        public long GetActionMask()
        {
            return ActionMask;
        }

        /// <summary>
        /// Gets the actions suspend this, these actions will suspend the actions from update calls
        /// Hence, action will be suspend by the suspendActions
        /// </summary>
        /// <returns>The actions suspend this.</returns>
        public long GetActionsSuspendingThis()
        {
            return SuspendingActions;
        }

        /// <summary>
        /// Gets the actions blocking this. this will prevent actions from being activated
        /// </summary>
        /// <returns>The actions blocking this.</returns>
        public long GetActionsBlockingThis()
        {
            return BlockingActions;
        }

        /// <summary>
        /// Actions that required other actions to activate
        /// </summary>
        /// <returns>The required actions.</returns>
        public long GetRequiredActions()
        {
            return RequiredActions;
        }

        /// <summary>
        /// Actions that will be forced to terminated if the dedicate action will need to activate
        /// </summary>
        /// <returns>The terminated actions.</returns>
        public long GetTerminatedActions()
        {
            return TerminatedActions;
        }

        /// <summary>
        /// Determines whether this action is retriggerable.
        /// </summary>
        /// <returns><c>true</c> if this instance is retriggerable; otherwise, <c>false</c>.</returns>
        public bool IsRetriggerable()
        {
            return Retriggerable;
        }

        /// <summary>
        /// Ask actionManager to activate action
        /// </summary>
        public void TryActivateAction()
        {
            actionManager.TryActivateAction(this);
        }

        /// <summary>
        /// Ask actionManaget to deactivate action
        /// </summary>
        public void DeactivateAction()
        {
            actionManager.DeactivateAction(this);
        }

        #endregion
    }
}
