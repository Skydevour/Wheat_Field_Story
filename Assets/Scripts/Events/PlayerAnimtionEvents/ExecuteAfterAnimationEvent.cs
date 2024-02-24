using UnityEngine;

namespace Events.PlayerAnimtionEvents
{
    public class ExecuteAfterAnimationEvent
    {
        public Vector3 MouseWorldPos;
        public Data.ItemDetails ItemDetails;

        public ExecuteAfterAnimationEvent(Vector3 mouseWorldPos,Data.ItemDetails itemDetails)
        {
            MouseWorldPos = mouseWorldPos;
            ItemDetails = itemDetails;
        }
    }
}
