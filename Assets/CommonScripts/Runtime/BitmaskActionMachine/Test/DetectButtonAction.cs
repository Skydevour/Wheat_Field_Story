#if (COMMONFRAMEWORK_BITMASKACTION_TEST)

using UnityEngine;

namespace USGFramework.Runtime.BitmaskActions.Test
{
    public class DetectButtonAction : BitmaskAction
    {
        protected override long ActionMask
        {
            get
            {
                return TestActionBehaviour.TestTask.DetectButton;
            }
        }

        protected override long SuspendingActions
        {
            get
            {
                return 0;
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
            Debug.Log("Begin Detect Button Action");
        }

        public override void UpdateAction()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_ActionManager.TryActivateAction(TestActionBehaviour.TestTask.PrintPeriodically);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                m_ActionManager.TryActivateAction(TestActionBehaviour.TestTask.PrintAnother);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                m_ActionManager.TryActivateAction(TestActionBehaviour.TestTask.SuspendPrintPeriodically);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                m_ActionManager.DeactivateAction(TestActionBehaviour.TestTask.PrintPeriodically);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                m_ActionManager.DeactivateAction(TestActionBehaviour.TestTask.PrintAnother);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                m_ActionManager.DeactivateAction(TestActionBehaviour.TestTask.SuspendPrintPeriodically);
            }
        }

        public override void EndAction()
        {
            Debug.Log("End Detect Button Action");
        }
    }
}

#endif