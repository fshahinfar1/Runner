using System;

public class RoadPlacerLogic
{
    private static int cur=0;
    public static RoadType nextRoad()
    {
        int count = Enum.GetNames(typeof(RoadType)).Length;
        RoadType type = (RoadType)cur;
        cur += 1;
        cur %= count;
        return type;
    }

    public static void _Reset() {
        cur = 0;
    }
}
