#if (COMMONFRAMEWORK_BITMASKACTION_TEST)

using UnityEngine;

namespace USGFramework.Runtime.BitmaskActions.Test
{
    public class PrintPeriodicallyAction : BitmaskAction
    {
        protected override long ActionMask
        {
            get
            {
                return TestActionBehaviour.TestTask.PrintPeriodically;
            }
        }

        protected override long SuspendingActions
        {
            get
            {
                return TestActionBehaviour.TestTask.PrintAnother | TestActionBehaviour.TestTask.SuspendPrintPeriodically;
            }
        }

        protected override long BlockingActions
        {
            get
            {
                return 0;
            }
        }

        protected override long RequiredActions
        {
            get
            {
                return 0;
            }
        }

        protected override long TerminatedActions
        {
            get
            {
                return 0;
            }
        }

        protected override bool Retriggerable
        {
            get
            {
                return true;
            }
        }

        private float timeToPrint = 0f;

        public override void BeginAction()
        {
            Debug.LogFormat("Begin Print Periodically Action: {0}", Time.realtimeSinceStartup);
            timeToPrint = Time.realtimeSinceStartup + 5f;
        }

        public override void UpdateAction()
        {
            if (Time.realtimeSinceStartup > timeToPrint)
            {
                Debug.LogFormat("Update Print Periodically Action: {0}", Time.realtimeSinceStartup);
                timeToPrint = Time.realtimeSinceStartup + 5f;
            }
        }

        public override void EndAction()
        {
            Debug.Log("End Print Periodically Action");
        }
    }
}

#endif