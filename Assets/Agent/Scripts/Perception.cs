using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string tagName;
    public float maxDistance;
    public float maxAngle;

    public abstract GameObject[] GetGameObjects();
   
}
