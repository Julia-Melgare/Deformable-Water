using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    Rigidbody rigidbody;
    public float speed = 10.0f;
    public float gravity = Physics.gravity.magnitude;
    public float maxVelocityChange = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        Vector3 velocity = rigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));

        Ray inputRay = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer)
            {
                Vector3 point = hit.point;
                point += hit.normal;
                deformer.AddDeformingForce(point, rigidbody.mass*gravity);
            }
        }

    }
}
