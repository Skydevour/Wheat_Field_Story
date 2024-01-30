#if (COMMONFRAMEWORK_BITMASKACTION_TEST)

using System.Collections.Generic;
using UnityEngine;

namespace USGFramework.Runtime.BitmaskActions.Test
{
    public class TestActionBehaviour : MonoBehaviour
    {
        public static class TestTask
        {
            public const long DetectButton = 1 << 0;
            public const long PrintPeriodically = 1 << 3;
            public const long PrintAnother = 1 << 40;
            public const long SuspendPrintPeriodically = 1 << 45;
        }

        private BitmaskActionMachine actionMachine;
        private BitmaskAction[] bitmaskActions;

        void Start()
        {
            bitmaskActions = new BitmaskAction[] { new DetectButtonAction(),
                                                    new PrintPeriodicallyAction(),
                                                    new PrintAnotherAction(),
                                                    new SuspendPrintPeriodically()};

            actionMachine = new BitmaskActionMachine(bitmaskActions);
            actionMachine.TryActivateAction(TestTask.DetectButton);
        }

        void Update()
        {
            actionMachine.OnMachineUpdate();
        }
    }
}

#endif
