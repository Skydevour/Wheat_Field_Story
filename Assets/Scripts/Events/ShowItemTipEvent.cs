public class ShowItemTipEvent
{
    public readonly bool IsShow;
    public readonly Slot SlotBag;

    public ShowItemTipEvent(bool isShow, Slot slotBag)
    {
        IsShow = isShow;
        SlotBag = slotBag;
    }
}