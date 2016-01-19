using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    int size;
    
    public void setSize(int platformSize)
    {
        size = platformSize;
    }

    public int getSize()
    {
        return size;
    }
}
