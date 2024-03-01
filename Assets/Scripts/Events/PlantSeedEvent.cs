public class PlantSeedEvent
{
    public readonly int SeedID;
    public readonly Data.TileDetails TileDetails;

    public PlantSeedEvent(int id, Data.TileDetails tileDetails)
    {
        SeedID = id;
        TileDetails = tileDetails;
    }
}