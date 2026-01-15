using UnityEngine;

public class AutonomousAgent : AIAgent
{
    [SerializeField] Movement movement;
    [SerializeField] Perception seekperception;
    [SerializeField] Perception fleeperception;

    void Start()
    {

    }

    void Update()
    {
        if (seekperception != null)
        {
        var gameObjects = seekperception.GetGameObjects();
        if (gameObjects.Length > 0)
        {
            Vector3 force = Seek(gameObjects[0]);
            movement.ApplyForce(force);
        }

        }
        if (fleeperception != null) { 
       var gameObjects = fleeperception.GetGameObjects();
        if (gameObjects.Length > 0)
        {
            Vector3 force = Flee(gameObjects[0]);
            movement.ApplyForce(force);
        }
        }
        //foreach (var go in gameObjects) {
        //    Debug.DrawLine(transform.transform.position, go.transform.position, Color.orange);
        //}

       // movement.ApplyForce(transform.forward);

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
