using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboarTexts : MonoBehaviour {
    public TextMesh[] billboards;
    public Transform container;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach(TextMesh tm in billboards)
        {
            tm.transform.rotation = Camera.main.transform.rotation;
        }
        foreach (Transform tf in container) {
            Transform cam = Camera.main.transform;
            if (tf.tag == "Logo")
            {
                tf.rotation = cam.parent.rotation;
            }
            else {
                tf.rotation = cam.parent.rotation*Quaternion.Euler(0, 180, 0);
            }
        }
	}
}
