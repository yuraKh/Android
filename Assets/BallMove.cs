using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour
{
	public Transform camobj, camdir;
	Vector3 from = new Vector3 (0.25f, 1f, -1f);
	Vector3 to = new Vector3 (0.25f, 2.354f, -2.772f);
	Rigidbody rb;
	float dis1, newproc, t = 0.5f;
	bool b = false;
	
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		//Camera.main.transform.localPosition = Vector3.Lerp (from, to, t);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.name == "Exit") {
			Application.LoadLevel (1);
		}
		if (other.tag == "Player") {
			GetComponent<MeshRenderer> ().material.color = Color.Lerp (GetComponent<MeshRenderer> ().material.color, new Color (Random.Range (0, 0.255f), Random.Range (0, 0.255f), Random.Range (0, 0.255f)), 1f);
		}
	}
	// Update is called once per frame
	void Update ()
	{
		camdir.position = transform.position;
		Vector3 accel = Input.acceleration; 
		rb.AddForce (camdir.transform.TransformDirection (accel.x * 40, 0, (accel.y + 0.6f) * 60));
		
		
		if (Input.touchCount == 1) {
			if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				Vector2 posds = Input.GetTouch (0).deltaPosition;
				
				if (Mathf.Abs (posds.x) > 0.4) {
					camdir.Rotate (0, posds.x, 0, Space.World);
				}
				
				if (Mathf.Abs (posds.y) > 0.4) {
					camobj.Rotate (posds.y, 0, 0, Space.Self);
				}
			}
			dis1 = 0;
		} else if (Input.touchCount == 2) {
			if (Input.GetTouch (0).phase == TouchPhase.Began || Input.GetTouch (1).phase == TouchPhase.Began) {
				dis1 = Vector2.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);
			}
			if (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (1).phase == TouchPhase.Moved) {
				float dis2 = Vector2.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);
				if (dis1 > dis2) {
					float proc = 1 - dis2 / (dis1 / 100f) * 0.01f;
					newproc = t + proc;
				}
				if (dis1 < dis2) {
					float proc = 1 - dis1 / (dis2 / 100f) * 0.01f;
					newproc = t - proc;
				}
				if (newproc > 1) {
					newproc = 1f;
				} else if (newproc < 0) {
					newproc = 0f;
				}
				b = true;
				
			}
		} else {
			dis1 = 0;
		}
		
		if (b) {
			if (Mathf.Abs (t - newproc) > 0.02f) {
				if (t < newproc) {
					t += 0.02f;
				}
				if (t > newproc) {
					t -= 0.02f;
				}
				Camera.main.transform.localPosition = Vector3.Lerp (from, to, t);
			} else {
				b = false;
			}
		}
		
		
	}
}

