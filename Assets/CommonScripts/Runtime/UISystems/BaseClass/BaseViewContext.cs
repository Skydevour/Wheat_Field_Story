namespace CommonFramework.Runtime.UISystems
{
    /// <summary>
    /// Base class of storing info about current view context
    /// </summary>
    public class BaseViewContext
    {
        public UIType ViewType { get; private set; }

        public BaseViewContext(UIType viewType)
        {
            ViewType = viewType;
        }
    }
}
