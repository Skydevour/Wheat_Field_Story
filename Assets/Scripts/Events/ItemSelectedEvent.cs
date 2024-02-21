public class ItemSelectedEvent
{
    public readonly bool IsSelected;
    public readonly Data.ItemDetails ItemDetails;

    public ItemSelectedEvent(Data.ItemDetails itemDetails, bool isSelected)
    {
        IsSelected = isSelected;
        ItemDetails = itemDetails;
    }
}