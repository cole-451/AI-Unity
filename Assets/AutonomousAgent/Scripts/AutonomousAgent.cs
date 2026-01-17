

using Unity.VisualScripting;
using UnityEngine;

public class AutonomousAgent : AIAgent
{
    [SerializeField] Movement movement;
    [SerializeField] Perception seekperception;
    [SerializeField] Perception fleeperception;

    [Header("Wander")]

    [SerializeField] float wanderRadius = 1;

    [SerializeField] float wanderDistance = 1;

    [SerializeField] float wanderDisplacement = 1;



    float wanderAngle = 0.0f;

    void Start()
    {
        wanderAngle = Random.Range(0.00f, 100.00f);
    }

    private UnityEngine.Vector3 Wander()

    {

        // randomly adjust the wander angle within (+/-) displacement range 

        wanderAngle += wanderAngle - wanderDisplacement - wanderDisplacement;

        // calculate a point on the wander circle using the wander angle 

        UnityEngine.Quaternion rotation = UnityEngine.Quaternion.AngleAxis(wanderAngle, UnityEngine.Vector3.up);

        UnityEngine.Vector3 pointOnCircle = rotation * (Vector3.forward *wanderRadius);

        // project the wander circle in front of the agent 

        UnityEngine.Vector3 circleCenter =  movement.Direction * wanderDistance;

        // steer toward the target point (circle center + point on circle) 

        UnityEngine.Vector3 force = GetSteeringForce(circleCenter+ pointOnCircle );



        return force;

    }

    void Update()
    {
        // store if target found, used for wander if no target 

        bool hasTarget = false;


        if (seekperception != null)
        {
        var gameObjects = seekperception.GetGameObjects();
        if (gameObjects.Length > 0)
        {
                hasTarget = true;
            Vector3 force = Seek(gameObjects[0]);
            movement.ApplyForce(force);
        }

        }
        if (fleeperception != null) { 
       var gameObjects = fleeperception.GetGameObjects();
        if (gameObjects.Length > 0)
        {
                hasTarget |= true;
            Vector3 force = Flee(gameObjects[0]);
            movement.ApplyForce(force);
        }
        }
        // if no target then wander 

        if (!hasTarget)

        {

            Vector3 force = Wander();

            movement.ApplyForce(force);
        
        }


        transform.position = Utilities.Wrap(transform.position, new Vector3(-15, -15, -15), new Vector3 (15,15,15));
        if (movement.Velocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Velocity, Vector3.up);
        }
    }

    Vector3 Seek(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    Vector3 Flee(GameObject target)
    {
        Vector3 direction =transform.position - target.transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    public Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;

        Vector3 steeringForce = desired - movement.Velocity;

        Vector3 force = Vector3.ClampMagnitude(steeringForce, movement.maxForce);

        return force;

    }
}
