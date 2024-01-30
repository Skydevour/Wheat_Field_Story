using UnityEngine;

namespace CommonFramework.Runtime.UISystems
{
    public class StaticView : BaseView
    {
        public override void OnEnter(BaseViewContext context)
        {
            this.gameObject.SetActive(true);
        }

        public override void OnExit(BaseViewContext context)
        {
            this.gameObject.SetActive(false);
        }

        public override void OnPause(BaseViewContext context)
        {
            this.gameObject.SetActive(false);
        }

        public override void OnResume(BaseViewContext context)
        {
            this.gameObject.SetActive(true);
        }
    }
}
