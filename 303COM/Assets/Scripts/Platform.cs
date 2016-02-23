using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    public int size;
    
    public void setSize(int platformSize)
    {
        size = platformSize;
    }

    public float getSize()
    {
        return size;
    }
}
