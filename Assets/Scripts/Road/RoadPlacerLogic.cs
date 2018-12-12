public class RoadPlacerLogic
{
    private static int cur=0;
    public static RoadType nextRoad()
    {
        RoadType type = (RoadType)cur;
        cur += 1;
        cur %= 2;
        return type;
    }
}
