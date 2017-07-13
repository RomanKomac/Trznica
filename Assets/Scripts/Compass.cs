using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Compass : MonoBehaviour {

    public Transform mapa;
    public Transform trznica;
    public bool rotateTrznica = false;
    Vector3 rot = new Vector3();
    Vector3[] rots = new Vector3[400];
    Vector3[] secondaryRots = new Vector3[50];
    private float currentHeading = 0;
    public bool compassEnabled = false;
    int cycle = 3;
    int cycler = 0;
    int nofver = 0;
    // Use this for initialization
    void Start () {
        Input.compass.enabled = true;
        Input.location.Start();
        for (int i = 0; i < rots.Length; i++) {
            rots[i] = new Vector3();
            rots[i].z = Input.compass.trueHeading;
            currentHeading = Input.compass.trueHeading;
        }
	}

    public void toggleTrznica() {
        rotateTrznica = !rotateTrznica;
        if (rotateTrznica)
        {
            mapa.gameObject.GetComponent<Image>().color = new Vector4(1, 0, 0, 1);
        }
        else {
            mapa.gameObject.GetComponent<Image>().color = new Vector4(0.5f, 0.5f, 0.5f, 1);
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
        //float mult = 0.5f;
        //float reduce = mult / rots.Length;
        for (int i = 0; i < rots.Length; i++) {
            rot += rots[i];
        }
        rot /= rots.Length;

        for (int i = 0; i < secondaryRots.Length - 1; i++)
        {
            secondaryRots[i + 1] = secondaryRots[i];
        }
        secondaryRots[0].z = rot.z;

        rot.z = 0;
        for (int i = 0; i < secondaryRots.Length; i++)
        {
            rot += secondaryRots[i];
        }
        rot /= secondaryRots.Length;

        mapa.eulerAngles = rot;
        float heading = 0;
        if (rotateTrznica)
        {
            heading = -rot.z;
        }
        else {
            heading = 0;
        }
        trznica.eulerAngles = new Vector3(0, Mathf.LerpAngle(trznica.eulerAngles.y, heading, 0.03f), 0);

    }
}
