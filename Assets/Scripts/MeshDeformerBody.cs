using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerBody : MonoBehaviour
{
    public Rigidbody rigidbody;
    private Renderer rend;
    bool colliding = false;
    Collision lastCollision;
    public float gravity = Physics.gravity.magnitude;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding)
        {
            Ray inputRay = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
                if (deformer)
                {
                    Vector3 point = hit.point;
                    point += hit.normal;
                    deformer.AddDeformingForce(point, rigidbody.mass * gravity);
                }
            }
            if (transform.localScale.x >= 2.0f)
            {
                Vector3 xScale = new Vector3(transform.localScale.x-1f, 0);
                inputRay = new Ray(transform.position+xScale, Vector3.down);
                if (Physics.Raycast(inputRay, out hit))
                {
                    MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
                    if (deformer)
                    {
                        Vector3 point = hit.point;
                        point += hit.normal;
                        deformer.AddDeformingForce(point, rigidbody.mass * gravity);
                    }
                }
                inputRay = new Ray(transform.position - xScale, Vector3.down);
                if (Physics.Raycast(inputRay, out hit))
                {
                    MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
                    if (deformer)
                    {
                        Vector3 point = hit.point;
                        point += hit.normal;
                        deformer.AddDeformingForce(point, rigidbody.mass * gravity);
                    }
                }
            }            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;
        lastCollision = collision;
        var deformer = collision.gameObject.GetComponent<MeshDeformer>();
        if (deformer)
        {
            var contactPoint = collision.GetContact(0);
            float appliedForce = rigidbody.mass * Vector3.Dot(contactPoint.normal, collision.relativeVelocity);
            deformer.AddDeformingForce(contactPoint.point+contactPoint.normal, Mathf.Abs(appliedForce));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //colliding = false;
    }

}
