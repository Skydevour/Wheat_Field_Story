using UnityEngine;

namespace Events.PlayerAnimtionEvents
{
    public class PlayerAnimationBeforeExecuteEvent
    {
        public Vector3 MouseWorldPos;
        public Data.ItemDetails ItemDetails;

        public PlayerAnimationBeforeExecuteEvent(Vector3 mouseWorldPos, Data.ItemDetails itemDetails)
        {
            MouseWorldPos = mouseWorldPos;
            ItemDetails = itemDetails;
        }
    }
}
