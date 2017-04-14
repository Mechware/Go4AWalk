using UnityEngine;
using System.Collections;
using System;
using Gamelogic.Extensions;

public enum LocationState
{
    Disabled,
    TimedOut,
    Failed,
    Enabled,
    Stopped,
    Initializing
}

public class GPS : MonoBehaviour
{
    public static GPS gpsObject;
    
    public int GPSUpdatesBeforeAverage = 5;
    public float desiredAccuracyInMeters = 1f;
    public float updateDistanceInMeters = 0f;
    public float timeBetweenChecks = 1f;

    [HideInInspector]
    // Approximate radius of the earth (in kilometers)
    public ObservedValue<LocationState> state;

    // Position on earth (in degrees)
    public float latitude;
    public float longitude;

    // Distance walked (in meters) since last update 
    public ObservedValue<float> deltaDistance;
    public float deltaTime;

    // Amount of times GPS has updated
    private int gpsUpdates = 1;

    // Timestamp of last data
    private double timestamp;

    // Total lat and long for averaging
    private float totalLat = 0;
    private float totalLong = 0;

    // Previous average lat and long
    private float prevLatitude, prevLongitude;
    public double timeOfLastDistanceUpdate;

    private const float EARTH_RADIUS = 6371;

    // Use this for initialization
    void Awake() {
        gpsObject = this;
        deltaDistance = new ObservedValue<float>(0);
        timeOfLastDistanceUpdate = DateTime.UtcNow.Second;
        state = new ObservedValue<LocationState>(LocationState.Initializing);
        latitude = 0f;
        longitude = 0f;
    }

    IEnumerator Start()
    {

        yield return StartCoroutine(initializeGPS());

        if(state.Value == LocationState.Enabled) {
            initializeVariables();
        }
    }

    void initializeVariables() {
        gpsUpdates = 1;
        timestamp = Input.location.lastData.timestamp;
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        prevLatitude = latitude;
        prevLongitude = longitude;
        totalLong = prevLongitude;
        totalLat = prevLatitude;
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        timeOfLastDistanceUpdate = (int) (DateTime.UtcNow - epochStart).TotalSeconds;
    }

    void OnApplicationPause(bool pauseState)
    {
        if (pauseState)
        {
            Input.location.Stop();
            state.Value = LocationState.Disabled;
        }
        else
        {
            StartCoroutine(initializeGPS());
        }
    }

    IEnumerator initializeGPS() {

        if (!Input.location.isEnabledByUser) {
            state.Value = LocationState.Disabled;
            yield break;
        }

        Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
        
        int waitTime = 15;

        while (Input.location.status == LocationServiceStatus.Initializing && waitTime > 0) {
            yield return new WaitForSeconds(1);
            waitTime--;
        }

        if (waitTime == 0) {
            state.Value = LocationState.TimedOut;
        } else if (Input.location.status == LocationServiceStatus.Failed) {
            state.Value = LocationState.Failed;
        } else {
            state.Value = LocationState.Enabled;
            StartCoroutine(checkForUpdates());
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

    IEnumerator checkForUpdates() {
        while(state.Value == LocationState.Enabled) {
            if (timestamp != Input.location.lastData.timestamp) {

                totalLat += Input.location.lastData.latitude;
                totalLong += Input.location.lastData.longitude;
                timestamp = Input.location.lastData.timestamp;
                gpsUpdates++;

                if (gpsUpdates == GPSUpdatesBeforeAverage) {
                    longitude = totalLong / GPSUpdatesBeforeAverage;
                    latitude = totalLat / GPSUpdatesBeforeAverage;
                    deltaDistance.Value = Haversine(prevLongitude, prevLatitude, longitude, latitude) * 1000f;

                    prevLongitude = longitude;
                    prevLatitude = latitude;

                    if (deltaDistance.Value > 0f) {
                        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0,
                                                                System.DateTimeKind.Utc);

                        double curTime = (DateTime.UtcNow - epochStart).TotalSeconds;
                        deltaTime = (float) (curTime - timeOfLastDistanceUpdate);
                        timeOfLastDistanceUpdate = curTime;
                    }

                    gpsUpdates = 1;
                    totalLong = longitude;
                    totalLat = latitude;

                } else {
                    latitude = Input.location.lastData.latitude;
                    longitude = Input.location.lastData.longitude;
                } // if averaging updates
            } // If timestamp has been updated
            yield return new WaitForSeconds(timeBetweenChecks);
        }
    }

    public string getGPSData() {

        string text;
        switch (state.Value) {
            case LocationState.Enabled:
                DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0,
                                                                DateTimeKind.Utc);

                double curTime = (DateTime.UtcNow - epochStart).TotalSeconds;
                float timeChanged = (float) (curTime - timestamp);
                int timeChange = Mathf.CeilToInt(timeChanged);


                text =  "Time since last update: " + timeChange + "\n" +
                        "Previous Latitude: " + prevLatitude + "\n" +
                        "Previous Longitude: " + prevLongitude + "\n" +
                        "Current Longitude: " + longitude + "\n" +
                        "Current Latitude: " + latitude + "\n" +
                        "Delta Distance: " + deltaDistance.Value + "\n" +
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
            case LocationState.Stopped:
                text = "GPS stopped";
                break;
            case LocationState.Initializing:
                text = "GPS initializing";
                break;
            default:
                text = "GPS error occurred";
                break;
        } // Switch stmt

        return text;

    } //getGPSData

} // class