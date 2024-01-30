using System.Collections.Generic;

namespace CommonFramework.Runtime.UISystems
{
    /// <summary>
    /// Managers that store stack info about view context
    /// </summary>
    public class ViewContextManager : MonoSingleton<ViewContextManager>
    {
        private Stack<BaseViewContext> contextStack = new Stack<BaseViewContext>();

        /// <summary>
        /// Push a new view context and jump to it
        /// </summary>
        /// <param name="nextContext"></param>
        public void Push(BaseViewContext nextContext)
        {

            if (contextStack.Count != 0)
            {
                BaseViewContext curContext = contextStack.Peek();
                BaseView curView = UIManager.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
                curView.OnPause(curContext);
            }

            contextStack.Push(nextContext);
            BaseView nextView = UIManager.Instance.GetSingleUI(nextContext.ViewType).GetComponent<BaseView>();
            nextView.transform.SetAsLastSibling();
            nextView.OnEnter(nextContext);
        }

        /// <summary>
        /// Pop a view context and return to the previous one
        /// </summary>
        public void Pop()
        {
            if (contextStack.Count != 0)
            {
                BaseViewContext curContext = contextStack.Peek();
                contextStack.Pop();

                BaseView curView = UIManager.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
                curView.transform.SetAsFirstSibling();
                curView.OnExit(curContext);
            }

            if (contextStack.Count != 0)
            {
                BaseViewContext lastContext = contextStack.Peek();
                BaseView curView = UIManager.Instance.GetSingleUI(lastContext.ViewType).GetComponent<BaseView>();
                curView.OnResume(lastContext);
            }
        }

        public BaseViewContext PeekOrNull()
        {
            if (contextStack.Count != 0)
            {
                return contextStack.Peek();
            }
            return null;
        }
    }
}
