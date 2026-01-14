using UnityEngine;

public class AutonomousAgent : AIAgent
{
    [SerializeField] Movement movement;
    [SerializeField] Perception perception;

    void Start()
    {

    }

    void Update()
    {
        movement.ApplyForce(Vector3.forward);
    }

}
