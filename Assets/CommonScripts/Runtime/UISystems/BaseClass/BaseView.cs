using UnityEngine;

namespace CommonFramework.Runtime.UISystems
{
    /// <summary>
    /// Base class for UI view
    /// </summary>
    public class BaseView : MonoBehaviour
    {
        public virtual void OnEnter(BaseViewContext context)
        {

        }

        public virtual void OnExit(BaseViewContext context)
        {

        }

        public virtual void OnPause(BaseViewContext context)
        {

        }

        public virtual void OnResume(BaseViewContext context)
        {

        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
