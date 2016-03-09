using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    public int size;
    public int points;
    
    public void setSize(int platformSize)
    {
        size = platformSize;
    }

    public float getSize()
    {
        return size;
    }

    public void setPoints(int p)
    {
        points = p;
    }

    public int getPoints()
    {
        return points;
    }
}
