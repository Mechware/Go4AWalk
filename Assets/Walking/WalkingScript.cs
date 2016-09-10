using UnityEngine;
using System.Collections;
using System;

public enum LocationState
{
    Disabled,
    TimedOut,
    Failed,
    Enabled
}

public class WalkingScript : MonoBehaviour
{


    public int GPSUpdatesBeforeAverage = 5;
    public float desiredAccuracyInMeters = 1f;
    public float updateDistanceInMeters = 0f;

    [HideInInspector]
    // Approximate radius of the earth (in kilometers)
    const float EARTH_RADIUS = 6371;
    public LocationState state;
    // Position on earth (in degrees)
    public float latitude;
    public float longitude;
    // Distance walked (in meters) since last update 
    public float deltaDistance;
    // Amount of times GPS has updated
    public int gpsUpdates = 1;
    // Timestamp of last data
    public double timestamp;
    // Total lat and long for averaging
    float totalLat = 0;
    float totalLong = 0;
    // Previous average lat and long
    public float prevLatitude, prevLongitude;

    // Use this for initialization
    IEnumerator Start()
    {
        // Only load this script if walking screen
        if(!Player.walking) {
            state = LocationState.Disabled;
            yield break;
        }

        state = LocationState.Disabled;
        latitude = 0f;
        longitude = 0f;
        
        if (Input.location.isEnabledByUser)
        {
            Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
            int waitTime = 15;

            while (Input.location.status == LocationServiceStatus.Initializing && waitTime > 0) {
                yield return new WaitForSeconds(1);
                waitTime--;
            }

            if (waitTime == 0) {
                state = LocationState.TimedOut;
            }
            else if (Input.location.status == LocationServiceStatus.Failed) {
                state = LocationState.Failed;
            }
            else {
                state = LocationState.Enabled;
                timestamp = Input.location.lastData.timestamp;
                prevLatitude = Input.location.lastData.latitude;
                prevLongitude = Input.location.lastData.longitude;
                totalLong = prevLongitude;
                totalLat = prevLatitude;
            }
        }
    }

    void OnApplicationPause(bool pauseState)
    {
        if (pauseState)
        {
            Input.location.Stop();
            state = LocationState.Disabled;
        }
        else
        {
            StartCoroutine(initializeGPS());
        }
    }

    IEnumerator initializeGPS() {

        Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
        
        int waitTime = 15;

        while (Input.location.status == LocationServiceStatus.Initializing && waitTime > 0) {
            yield return new WaitForSeconds(1);
            waitTime--;
        }

        if (waitTime == 0) {
            state = LocationState.TimedOut;
        } else if (Input.location.status == LocationServiceStatus.Failed) {
            state = LocationState.Failed;
        } else {
            state = LocationState.Enabled;
        }
    }

    // The Haversine formula
    // Veness, C. (2014). Calculate distance, bearing and more between
    //  Latitude/Longitude points. Movable Type Scripts. Retrieved from
    //  http://www.movable-type.co.uk/scripts/latlong.html
    float Haversine(float lastLongitude, float lastLatitude, float currLongitude, float currLatitude)
    {
        float deltaLatitude = (currLatitude - lastLatitude) * Mathf.Deg2Rad;
        float deltaLongitude = (currLongitude - lastLongitude) * Mathf.Deg2Rad;
        float a = Mathf.Pow(Mathf.Sin(deltaLatitude / 2), 2) +
            Mathf.Cos(lastLatitude * Mathf.Deg2Rad) * Mathf.Cos(currLatitude * Mathf.Deg2Rad) *
            Mathf.Pow(Mathf.Sin(deltaLongitude / 2), 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return EARTH_RADIUS * c;
    }

    private int timeOfLastDistanceUpdate;

    // Update is called once per frame
    void Update()
    {
        if (state == LocationState.Enabled)
        {
            
            if (timestamp != Input.location.lastData.timestamp) {
                

                totalLat += Input.location.lastData.latitude;
                totalLong += Input.location.lastData.longitude;
                timestamp = Input.location.lastData.timestamp;
                gpsUpdates++;

                if (gpsUpdates == GPSUpdatesBeforeAverage) {
                    longitude = totalLong / GPSUpdatesBeforeAverage;
                    latitude = totalLat / GPSUpdatesBeforeAverage;
                    deltaDistance = Haversine(prevLongitude, prevLatitude, longitude, latitude) * 1000f;

                    prevLongitude = longitude;
                    prevLatitude = latitude;

                    if (deltaDistance > 0f) {
                        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0,
                                                                System.DateTimeKind.Utc);

                        double curTime = (DateTime.UtcNow - epochStart).TotalSeconds;
                        float timeChanged = (float) (curTime - timeOfLastDistanceUpdate);
                        int timeChange = Mathf.CeilToInt(timeChanged);

                        Player.updateDistance(deltaDistance, timeChange);
                        timeOfLastDistanceUpdate = (int) curTime;
                    }
                    
                    gpsUpdates = 1;
                    totalLong = longitude;
                    totalLat = latitude;

                } else {
                    latitude = Input.location.lastData.latitude;
                    longitude = Input.location.lastData.longitude;
                } // if averaging updates
            } // If timestamp has been updated 
        } // if state is enabled
    } // update

    public string getGPSData() {
        string text;
        switch (state) {
            case LocationState.Enabled:
                DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0,
                                                                System.DateTimeKind.Utc);

                double curTime = (DateTime.UtcNow - epochStart).TotalSeconds;
                float timeChanged = (float) (curTime - timestamp);
                int timeChange = Mathf.CeilToInt(timeChanged);


                text = "Time since last update: " + timeChange + "\n" +
                        "Previous Latitude: " + prevLatitude + "\n" +
                        "Previous Longitude: " + prevLongitude + "\n" +
                        "Current Longitude: " + longitude + "\n" +
                        "Current Latitude: " + latitude + "\n" +
                        "Distance: " + Player.totalDistance + "\n" +
                        "Delta Distance: " + deltaDistance + "\n" +
                        "GPS updates: " + gpsUpdates;

                break;
            case LocationState.Disabled:
                text = "GPS disabled";
                break;
            case LocationState.Failed:
                text = "GPS failed";
                break;
            case LocationState.TimedOut:
                text = "GPS timed out";
                break;
            default:
                text = "GPS error occurred";
                break;
        } // Switch stmt

        return text;

    } //getGPSData

} // class