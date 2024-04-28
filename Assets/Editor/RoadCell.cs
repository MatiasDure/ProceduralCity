using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCell
{
    public bool HasRoad;
    public RoadCell RightNeighbor;
    public RoadCell LeftNeighbor;
    public RoadCell BottomNeighbor;
    public RoadCell TopNeighbor;
    public Vector3 Position;
    public byte neighborhoodPattern = 0;

    public RoadCell(Vector3 pPosition)
    {
        Position = pPosition;
    }

    public int CheckPrefab()
    {
        if (RightNeighbor != null && RightNeighbor.HasRoad) neighborhoodPattern |= 1 << 0; 
        if (LeftNeighbor != null && LeftNeighbor.HasRoad) neighborhoodPattern |= 1 << 1;
        if (BottomNeighbor != null && BottomNeighbor.HasRoad) neighborhoodPattern |= 1 << 2;
        if (TopNeighbor != null && TopNeighbor.HasRoad) neighborhoodPattern |= 1 << 3;

        byte pattern = neighborhoodPattern;
        neighborhoodPattern = 0;

        return pattern;
    }

    public override string ToString()
    {
        string rightNeighborInfo = RightNeighbor != null ? RightNeighbor.Position.ToString() : "null";
        string leftNeighborInfo = LeftNeighbor != null ? LeftNeighbor.Position.ToString() : "null";
        string topNeighborInfo = TopNeighbor != null ? TopNeighbor.Position.ToString() : "null";
        string bottomNeighborInfo = BottomNeighbor != null ? BottomNeighbor.Position.ToString() : "null";

        return string.Format("Road Cell:\nPosition: {0}\nIs selected: {1}\nNeighbors:\n\tRight: {2}\n\tLeft: {3}\n\tTop: {4}\n\tBottom: {5}", Position, HasRoad, rightNeighborInfo, leftNeighborInfo, topNeighborInfo, bottomNeighborInfo);
    }
}
