using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsOutputsController : MonoBehaviour
{
    public Spawner spawner;
    public float speed;
    float initialSpeed;
    [SerializeField]
    Brain brain;
    public float distanceToFrontWall = 0;
    public float distanceToRigthWall = 0;
    public float distanceToLeftWall = 0;
    public float fit = 0;
    public float torque = -1;
    Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        brain = GetComponent<Brain>();
        lastPosition = transform.position;
        initialSpeed = speed;
    }
    // Update is called once per frame
    void Update()
    {
        fit += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        transform.position += transform.right * speed * Time.deltaTime;
        if(speed!=0)
            transform.Rotate(new Vector3(0,0,torque));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right,1);
        Debug.DrawRay(transform.position, transform.right);
        if (hit.collider != null)
        {
            distanceToFrontWall = Vector2.Distance(transform.position, hit.point);
        }
        else
        {
            distanceToFrontWall = 2;
        }
        if(distanceToFrontWall>=0)
            brain.neuralNetwork.inputs[0].value = distanceToFrontWall;

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, (transform.right + transform.up).normalized, 1);
        Debug.DrawRay(transform.position, (transform.right + transform.up).normalized, Color.blue);
        if (hitRight.collider != null)
        {
            distanceToRigthWall = Vector2.Distance(transform.position, hitRight.point);
        }
        else
        {
            distanceToRigthWall = 2;
        }
        if (distanceToRigthWall >= 0)
            brain.neuralNetwork.inputs[1].value = distanceToRigthWall;

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, (transform.right - transform.up).normalized, 1);
        Debug.DrawRay(transform.position, (transform.right - transform.up).normalized,Color.green);
        if (hitLeft.collider != null)
        {
            distanceToLeftWall = Vector2.Distance(transform.position, hitLeft.point);
        }
        else
        {
            distanceToLeftWall = 2;
        }
        if (distanceToLeftWall >= 0)
            brain.neuralNetwork.inputs[2].value = distanceToLeftWall;


        RaycastHit2D coll = Physics2D.Raycast(transform.position, Vector2.zero);
        //If something was hit, the RaycastHit2D.collider will not be null.
        if (coll.collider != null)
        {
            KillNeuron();
        }
    }
    public void KillNeuron()
    {
        speed = 0;
        spawner.CheckEntitiesLife();
    }
    public void ResetEntity()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        speed = initialSpeed;
        fit = 0;
    }
}
