using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

    public Transform mapa;
    Vector3 rot = new Vector3();
    Vector3[] rots = new Vector3[10];
    public bool compassEnabled = false;
    int cycle = 4;
    int cycler = 0;
    // Use this for initialization
    void Start () {
        Input.compass.enabled = true;
        Input.location.Start();
        for (int i = 0; i < rots.Length; i++) {
            rots[i] = new Vector3();
            rots[i].z = Input.compass.trueHeading;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (cycler % cycle != 0)
            return;

        for (int i = 0; i < rots.Length - 1; i++) {
            rots[i + 1] = rots[i];
        }
        rots[0].z = Input.compass.trueHeading;
        rot.z = 0;
        float mult = 0.5f;
        //float reduce = mult / rots.Length;
        for (int i = 0; i < rots.Length; i++) {
            rot += rots[i] * mult;
            mult *= 0.5f;
        }
        mapa.eulerAngles = rot;
	}
}
