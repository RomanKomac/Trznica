using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour {

    private bool enabling = false;
    private bool GPSenabled = false;
    public TextMesh tm;

    private void printLocation(float latitude, float longitude, float altitude)
    {
        if (tm)
        {
            tm.text = "" + latitude + "\r\n" + longitude + "\r\n" + altitude + "\r\n";
        }
    }

    void Start()
    {
        if (tm)
        {
            tm.text = "GPS enabled: " + Input.location.isEnabledByUser;
        }
    }

    private IEnumerator StartTracking()
    {
        if (!Input.location.isEnabledByUser)
            yield break;
        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Timed out in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            
        }

    }

    void toggleGPS()
    {
        LocationServiceStatus st = Input.location.status;
        if( !Input.location.isEnabledByUser)
        {
            Input.location.Stop();
        }
        else if(Input.location.isEnabledByUser && 
                (st == LocationServiceStatus.Stopped || st == LocationServiceStatus.Failed))
        {
            Input.location.Start();
        }
    }

	void Update () {
        if (GPSenabled != Input.location.isEnabledByUser)
        {
            toggleGPS();
            GPSenabled = Input.location.isEnabledByUser;
            if (tm)
            {
                tm.text = "GPS enabled: " + Input.location.isEnabledByUser;
            }
        }

        if (Input.location.isEnabledByUser && enabling)
        {
            enabling = false;
            StartCoroutine(StartTracking());
        }

        if(Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running)
        {
            printLocation(Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.altitude);
        }
        
	}
}
