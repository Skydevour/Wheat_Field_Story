using System.Collections.Generic;

public class UpdatePlayerBagEvent
{
    public readonly Enums.BagLocation Location;
    public readonly List<Data.PlayerBagItemDetails> List;

    public UpdatePlayerBagEvent(Enums.BagLocation location, List<Data.PlayerBagItemDetails> list)
    {
        Location = location;
        List = list;
    }
}