using System.Collections.Generic;
using UnityEngine;

namespace CommonFramework.Runtime.BitmaskActions
{
    /// <summary>
    /// Bitmask action machine handler class
    /// Note that this class should be executed with other methods such as MonoBehaviour
    /// See TestActionBehaviour.cs for example
    /// </summary>
    public class BitmaskActionMachine
    {
        #region Private Fields

        private BitmaskAction[] actions = null;
        private long currentAction;
        private Dictionary<long, BitmaskAction> registeredActions;

        #endregion

        #region Constrcutors

        /// <summary>
        /// Constructor of BitmaskActionMachine
        /// This initialize all bitmask actions
        /// Note that the order of bitmask action input arrays indicates the execution order
        /// </summary>
        /// <param name="actionInputs">All bitmask action in this machine</param>
        public BitmaskActionMachine(BitmaskAction[] actionInputs)
        {
            actions = actionInputs;

            registeredActions = new Dictionary<long, BitmaskAction>();
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i] != null)
                {
                    actions[i].Init(this);
                    registeredActions.Add(actions[i].GetActionMask(), actions[i]);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Activate action by the given bit mask
        /// </summary>
        /// <param name="actionKey">bit mask action key</param>
        public void TryActivateAction(long actionKey)
        {
            if (registeredActions.ContainsKey(actionKey) == false)
            {
                Debug.LogWarning("[BitmaskActionMachine :: TryActivateAction]: action not registered !!! key: " + actionKey);
                return;
            }

            BitmaskAction actionValue;
            registeredActions.TryGetValue(actionKey, out actionValue);
            TryActivateAction(actionValue);
        }

        /// <summary>
        /// Deactivate action by the given mask
        /// </summary>
        /// <param name="actionKey">bitmask action key</param>
        public void DeactivateAction(long actionKey)
        {
            if (registeredActions.ContainsKey(actionKey) == false)
            {
                Debug.LogWarning("[BitmaskActionMachine :: DeactivateAction]: action not registered !!! key: " + actionKey);
                return;
            }

            BitmaskAction actionValue;
            registeredActions.TryGetValue(actionKey, out actionValue);
            DeactivateAction(actionValue);
        }

        /// <summary>
        /// Deactivate multiple actions by the given mask
        /// </summary>
        /// <param name="actionKeys">Mask of the keys</param>
        public void DeactivateMultipleActions(long actionKeys)
        {
            DeactivateActions(actionKeys);
        }

        /// <summary>
        /// Try to activate certain BitmaskAction
        /// </summary>
        /// <param name="action">Action to be activated</param>
        public void TryActivateAction(BitmaskAction action)
        {
            if (action == null)
            {
                Debug.LogError("[BitmaskActionMachine :: TryActivateAction]: action null !!!: ");
                return;
            }

            if (action.GetActionMask() == 0)
            {
                Debug.LogError("[BitmaskActionMachine :: TryActivateAction]: action mask is 0 !!!: ");
                return;
            }

            if (CanBeActivated(action))
            {
                if (action.IsRetriggerable() && IsActive(action) == true)
                {
                    DeactivateAction(action);
                    ActivateAction(action);
                }
                else if (IsActive(action) == false)
                {
                    ActivateAction(action);
                }
                else
                {
                    Debug.LogError("[BitmaskActionMachine :: TryActivateAction]: fail to activate action: " + action.GetActionMask());
                }
            }
        }

        /// <summary>
        /// Deactivate the given BitmaskAction
        /// </summary>
        /// <param name="action">Action to be deactivate</param>
        public void DeactivateAction(BitmaskAction action)
        {
            if (action == null)
            {
                Debug.Log("[BitmaskActionMachine :: DeactivateAction]: action null !!!: ");
                return;
            }

            if (action.GetActionMask() == 0)
            {
                Debug.Log("[BitmaskActionMachine :: DeactivateAction]: action mask is 0 !!!: ");
                return;
            }

            if (IsActive(action))
            {
                currentAction &= (~action.GetActionMask());
                action.EndAction();
                DeactivateActions(GetActionsRequiring(action)); // deactivate dependent actions
            }
        }

        /// <summary>
        /// Get running actions
        /// </summary>
        /// <returns>A mask of all running actions</returns>
        public long GetCurrentMasks()
        {
            return currentAction;
        }

        /// <summary>
        /// Called for update actions in MonoBehaviour updates
        /// </summary>
        public void OnMachineUpdate()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                UpdateAction(actions[i]);
            }
        }

        /// <summary>
        /// Called for update actions in MonoBehaviour Fixed updates
        /// </summary>
        public void OnMachineFixedUpdate()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                FixedUpdateAction(actions[i]);
            }
        }

        /// <summary>
        /// Called for update actions in MonoBehaviour Late update
        /// </summary>
        public void OnMachineLateUpdate()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                LateUpdateAction(actions[i]);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check if given action can be activate through all conditions
        /// </summary>
        /// <param name="action">Bitmask action to be checked</param>
        /// <returns>Return true if can be activated</returns>
        private bool CanBeActivated(BitmaskAction action)
        {
            return
                action.CanBeActivated()
                && (!IsActive(action) || action.IsRetriggerable())
                && !IsBlocked(action)
                && HasRequiredActions(action);
        }

        /// <summary>
        /// Check if the given action was running
        /// </summary>
        /// <param name="action">Action to be cehcked</param>
        /// <returns>Return true if running action mask was raised</returns>
        private bool IsActive(BitmaskAction action)
        {
            if ((action.GetActionMask() & currentAction) != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the given action should be suspended from updates
        /// </summary>
        /// <param name="action">Action to be checked</param>
        /// <returns>Return true if there are other actions running which was marked as suspended by the given action</returns>
        private bool IsSuspended(BitmaskAction action)
        {
            if ((action.GetActionsSuspendingThis() & currentAction) != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the given action was blocked by the other actions that were already running
        /// </summary>
        /// <param name="action">Action to be checked</param>
        /// <returns>Return true if other blocking action were already running</returns>
        private bool IsBlocked(BitmaskAction action)
        {
            if ((action.GetActionsBlockingThis() & currentAction) != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the dependent actions were running
        /// </summary>
        /// <param name="action">Given action to be checked</param>
        /// <returns>Return true if there is no need for other dependent action, or all required actions were all up and running</returns>
        private bool HasRequiredActions(BitmaskAction action)
        {
            if (action.GetRequiredActions() == 0) //no need to check this if we dont need to depend on any required actions
                return true;

            if ((action.GetRequiredActions() & currentAction) != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Activate the given actions
        /// Note that this would terminted other actions that were marked in terminted masks by the given actions
        /// Also this function updates the running masks of machine
        /// </summary>
        /// <param name="action">Action to be activated</param>
        private void ActivateAction(BitmaskAction action)
        {
            DeactivateActions(action.GetTerminatedActions());
            currentAction |= action.GetActionMask();
            action.BeginAction();
        }

        /// <summary>
        /// Deactivate multiple actions by the given masks
        /// </summary>
        /// <param name="actions">Action masks as long integer</param>
        private void DeactivateActions(long actions)
        {
            if (actions == 0)
            {
                return;
            }

            long actionsToDeactivate = currentAction & actions;

            if (actionsToDeactivate == 0)
            {
                return;
            }

            List<BitmaskAction> bitmaskActionsToDeactivate = new List<BitmaskAction>();
            GetActionsFromRegistered(actionsToDeactivate, ref bitmaskActionsToDeactivate);

            if (bitmaskActionsToDeactivate.Count > 0)
            {
                foreach (BitmaskAction actionToDeactivate in bitmaskActionsToDeactivate)
                {
                    DeactivateAction(actionToDeactivate);
                }
            }
        }

        /// <summary>
        /// Find out BitmaskAction that was registered in this machine
        /// </summary>
        /// <param name="actions">Action masks</param>
        /// <param name="bitmaskActions">List of BitmaskAction that were found registerer in this machine</param>
        private void GetActionsFromRegistered(long actions, ref List<BitmaskAction> bitmaskActions)
        {
            long key = 0;
            for (int bit = 0; bit < 64; bit++)
            {
                key = 1 << bit;

                if ((key & actions) == 0)
                    continue;

                if (registeredActions.ContainsKey(key) == false)
                    continue;

                BitmaskAction bitAction;
                registeredActions.TryGetValue(key, out bitAction);
                bitmaskActions.Add(bitAction);
            }
        }

        /// <summary>
        /// Get action masks that was dependent to the given action
        /// </summary>
        /// <param name="action">Action to be checked</param>
        /// <returns>Returns a mask that was dependent to the given action</returns>
        private long GetActionsRequiring(BitmaskAction action)
        {
            if (registeredActions.ContainsValue(action) == false)
                return 0;

            long key = action.GetActionMask();
            long requiredActions = 0;
            long mask = 0;
            BitmaskAction bitAction;

            for (int bit = 0; bit < 64; bit++)
            {
                mask = 1 << bit;

                if (key == mask)
                    continue;

                if (registeredActions.ContainsKey(mask) == false)
                    continue;

                registeredActions.TryGetValue(mask, out bitAction);

                if ((bitAction.GetRequiredActions() & key) != 0)
                {
                    requiredActions |= mask;
                }
            }

            return requiredActions;
        }

        /// <summary>
        /// Run action on the fixedupdate
        /// </summary>
        /// <param name="action">Action to be ran</param>
        private void FixedUpdateAction(BitmaskAction action)
        {
            if (IsActive(action) == false)
                return;

            if (IsSuspended(action) == true)
                return;

            action.FixedUpdateAction();
        }

        /// <summary>
        /// Run action on update
        /// </summary>
        /// <param name="action">Action to be ran</param>
        private void UpdateAction(BitmaskAction action)
        {
            if (IsActive(action) == false)
                return;

            if (IsSuspended(action) == true)
                return;

            action.UpdateAction();
        }

        /// <summary>
        /// Run action on late update
        /// </summary>
        /// <param name="action">Action to be ran</param>
        private void LateUpdateAction(BitmaskAction action)
        {
            if (IsActive(action) == false)
                return;

            if (IsSuspended(action) == true)
                return;

            action.LateUpdateAction();
        }

        #endregion
    }
}
