#if (COMMONFRAMEWORK_BITMASKACTION_TEST)

using UnityEngine;

namespace USGFramework.Runtime.BitmaskActions.Test
{
    public class SuspendPrintPeriodically : BitmaskAction
    {
        protected override long ActionMask
        {
            get
            {
                return TestActionBehaviour.TestTask.SuspendPrintPeriodically;
            }
        }

        protected override long SuspendingActions
        {
            get
            {
                return TestActionBehaviour.TestTask.PrintPeriodically;
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

        public override void BeginAction()
        {
            Debug.Log("Begin Suspend Action");
        }

        public override void EndAction()
        {
            Debug.Log("End Suspend Action");
        }
    }
}

#endif