using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCine : MonoBehaviour
{
public float Speed = 10f;
public float lookatspeed = 5f;


    void Update () {
 
        if (Input.GetKey ("w")) {
            transform.position += transform.TransformDirection (Vector3.forward) * Time.deltaTime * Speed;
        }   else if (Input.GetKey ("s")) {
            transform.position -= transform.TransformDirection (Vector3.forward) * Time.deltaTime * Speed;
        }
 
        if (Input.GetKey ("a") && !Input.GetKey ("d")) {
                transform.position += transform.TransformDirection (Vector3.left) * Time.deltaTime * Speed;
            } else if (Input.GetKey ("d") && !Input.GetKey ("a")) {
                transform.position -= transform.TransformDirection (Vector3.left) * Time.deltaTime * Speed;
            }

        //mouse look at
        float horizontal = Input.GetAxis("Mouse X") * lookatspeed;
		float vertical = Input.GetAxis("Mouse Y") * lookatspeed;

		transform.Rotate(0f, horizontal, 0f, Space.World);
		//transform.Rotate(-vertical, 0f, 0f, Space.Self);
        }
}