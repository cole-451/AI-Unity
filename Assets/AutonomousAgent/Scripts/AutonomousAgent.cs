
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class AutonomousAgent : AIAgent
{
    [SerializeField] Movement movement;
    [SerializeField] Perception seekperception;
    [SerializeField] Perception fleeperception;
    [SerializeField] Perception flockPerception;

    [Header("Wander")]

    [SerializeField] float wanderRadius = 1;

    [SerializeField] float wanderDistance = 1;

    [SerializeField] float wanderDisplacement = 1;

    [Header("Flocking")]




    float wanderAngle = 0.0f;

    void Start()
    {
        wanderAngle = Random.Range(0.00f, 100.00f);
    }

    private UnityEngine.Vector3 Wander()

    {

        // randomly adjust the wander angle within (+/-) displacement range 

        wanderAngle += Random.Range(-wanderDisplacement, wanderDisplacement);

        // calculate a point on the wander circle using the wander angle 

        UnityEngine.Quaternion rotation = UnityEngine.Quaternion.AngleAxis(wanderAngle, UnityEngine.Vector3.up);

        UnityEngine.Vector3 pointOnCircle = rotation * (Vector3.forward * wanderRadius);

        // project the wander circle in front of the agent 

        UnityEngine.Vector3 circleCenter =  movement.Direction * wanderDistance;

        // steer toward the target point (circle center + point on circle) 

        UnityEngine.Vector3 force = GetSteeringForce(circleCenter + pointOnCircle );



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


        transform.position = Utilities.Wrap(transform.position, new Vector3(-15, 0, -15), new Vector3 (15,0,15));
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

    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        
        // accumulate the position vectors of the neighbors
        foreach (var n in neighbors)
	    {
        // add neighbor position to positions
        n.transform.position = positions;

        }

        // average the positions to get the center of the neighbors
        Vector3 center = positions / neighbors.Length;
        // create direction vector to point towards the center of the neighbors from agent position
        Vector3 direction = center - transform.position;

        // steer towards the center point
        Vector3 force = GetSteeringForce(direction);


    return force;
    }



    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        Vector3 separation = Vector3.zero;
        // accumulate the separation vectors of the neighbors
        foreach (var n in neighbors)
	{
            // get direction vector away from neighbor
            Vector3 direction = -n.transform.position ;//< direction vector pointing away from neighbor ??? >;
            float distance = direction.magnitude;
        // check if within separation radius
        if (distance > 0 && distance < radius )//< distance greater than 0 and less than radius>)
		{
                // scale separation vector inversely proportional to the direction distance
                // closer the distance the stronger the separation
                separation += direction * (1 / distance);
            }
        }

        // steer towards the separation point
        Vector3 force = (separation.magnitude > 0/*< separation length is greater than 0 >*/) ? GetSteeringForce(separation) : Vector3.zero;

        return force;
    }
    // NEED HELP!!!
 //   private Vector3 Alignment(GameObject[] neighbors)
 //   {
 //       Vector3 velocities = Vector3.zero;
 //       // accumulate the velocity vectors of the neighbors
 //       foreach (GameObject n in neighbors)
	//{
 //           // get the velocity from the agent movement
 //           if (TryGetComponent<AutonomousAgent>(out GameObject n)/*< get AutonomousAgent component from GameObject neighbor, use TryGetComponent>*/)
	//	{
 //               // add agent movement velocity to velocities
 //               velocities += movement.Velocity;

 //       }
 //       }
 //       // get the average velocity of the neighbors
 //       Vector3 averageVelocity = neighbors.Average(n => movement.Velocity);

 //       // steer towards the average velocity
 //       Vector3 force = < steer towards average velocity >


 //   return force;
 //   }


}
