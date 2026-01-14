using UnityEngine;

public abstract class Perception : MonoBehaviour
{

    [SerializeField] string info;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] protected string tagName;
    [SerializeField] protected float maxDistance;
    [SerializeField, Range(0, 180)]protected float maxAngle;
   

    public abstract GameObject[] GetGameObjects();
   
}
