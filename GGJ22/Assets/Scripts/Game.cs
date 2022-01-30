using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MapPoint;
using GGJ22.Helpers;

public class Game : MonoBehaviour
{
    // If the Game is in debug mode, print additional debugging GUI output etc
    public bool debugEnabled = true;
    // Debug queue
    private string myLog;
    private List<string> myLogQueue = new List<string>();
    // Maximum number of debug messages stored
    public int maxDebug = 20;

    // Map precision parameters;
    // interpretation as presented in https://docs.microsoft.com/en-us/bingmaps/articles/bing-maps-tile-system
    //public int lod = 20; // Level of Depth, 20 
    public int lod = 10; 
    public int tileSize = 256; // Customizable tile sizes; how many pixels will construct a playable tile

    // Last observed latitude and longitude
    public double lat = 0.0;
    public double lon = 0.0;
    // Last observed pixel coordinates {x,y}
    private double x = 0.0;
    private double x2 = 0.0; // Old x; for drawing walk pattern
    private double y = 0.0;
    private double y2 = 0.0; // Old y; for drawing walk pattern
    // Game status polling interval
    public static float interval = 5.0f;
    private float currentInterval = interval;
    // Class for assisting with drawing in Unity etc
    Helpers helper = new Helpers();
    // Debug movement amount for moving around
    public double debugChange = 0.1;

    // Update is called once per frame
    void Update()
    {
        currentInterval -= Time.deltaTime;
        if (currentInterval < 0)
        {
            currentInterval = interval;
            //Debug.Log("Polling game state update");
            if (debugEnabled & Input.location.status != LocationServiceStatus.Failed)
            {
#if UNITY_EDITOR
                Debug.Log("Reading artificial editor-imputed LOG/LAT...");
#elif PLATFORM_ANDROID
                // Extract GPS coordinates
                this.lat = Input.location.lastData.latitude;
                this.lon = Input.location.lastData.longitude;
                //Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                Debug.Log("Location Android LAT: " + Input.location.lastData.latitude + " LON: " + Input.location.lastData.longitude + " (" + Input.location.lastData.timestamp + " time)");
#endif
                // Update {x,y} with tiling system
                // Store previous coordinates
                x2 = x;
                y2 = y;
                TileSystem.LatLongToPixelXY(lat, lon, lod, out x, out y, tileSize);
                Debug.Log("Character position set to {x=" + x + ",y=" + y + ")");
                helper.DrawLine(new Vector2((float) x2, (float) y2), new Vector2((float) x, (float) y), Color.green, 1.0f);
            }
        }
    }

    // GPS location service start and run cycle
    IEnumerator Start()
    {
        Debug.Log("Starting Game...");

        // Load game state
        loadGameState();

#if UNITY_EDITOR
        Debug.Log("Using Editor-based artificial location input");
        yield break;
#elif PLATFORM_ANDROID    
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User does not have location services enabled.");
            yield break;
        }

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            Debug.Log("Waiting for location services to be initialized...");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Location services timed out (20s)");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stops the location service if there is no need to query location updates continuously.
        //Input.location.Stop();
        Debug.Log("Ending Start");
#endif
    }

    // Game instance is loaded at the beginning; it will persist through whole session
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Load initial game state
    void loadGameState()
    {

    }

    // Update game state per every polling interval
    void updateGameState()
    {

    }

    /* 
     * Getters for Game status
     */

    public double getLatitude()
    {
        return this.lat;
    }
    public double getLongitude()
    {
        return this.lon;
    }
    public int getLOD()
    {
        return this.lod;
    }
    public int getTileSize()
    {
        return this.tileSize;
    }
    public bool getDebug() {
        return this.debugEnabled;
    }
    public double getX()
    {
        return this.x;
    }
    public double getY()
    {
        return this.y;
    }

    // If game is in debug mode, update information
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        //myLogQueue.Enqueue(newString);
        myLogQueue.Add(newString);
        // Trim debug log from the start
        if (myLogQueue.Count > maxDebug) myLogQueue.RemoveAt(0);
        // Make the string output
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            //myLogQueue.Enqueue(newString);

            myLogQueue.Add(newString);
        }
        myLog = string.Empty;
        // Reverse queue so latest info is always displayed on top when printing to GUI
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }

    // Draw GUI
    void OnGUI()
    {
        if (debugEnabled)
        {
            GUILayout.Label(myLog);

            //GUILayout.BeginArea(new Rect(Screen.width - 150, 100, 140, 200));
            if (GUI.Button(new Rect(Screen.width - 50, 0, 50, 50), "Lat-"))
            {
                lat -= debugChange;
                //Debug.Log("W pressed");
            }
            if (GUI.Button(new Rect(Screen.width - 50, 50, 50, 50), "Lat+"))
            {
                lat -= debugChange;
                //Debug.Log("E pressed");
            }
            if (GUI.Button(new Rect(Screen.width - 50, 100, 50, 50), "Lon-"))
            {
                lon -= debugChange;
                //Debug.Log("E pressed");
            }
            if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 50), "Lon+"))
            {
                lon += debugChange;
                //Debug.Log("E pressed");
            }
            if (GUI.Button(new Rect(Screen.width - 50, 200, 50, 50), "Lon+/Lat-"))
            {
                lon += debugChange;
                lat -= debugChange;
                //Debug.Log("E pressed");
            }
            if (GUI.Button(new Rect(Screen.width - 50, 250, 50, 50), "Lon+/Lat+"))
            {
                lon += debugChange;
                lat += debugChange;
                //Debug.Log("E pressed");
            }
            if (GUI.Button(new Rect(Screen.width - 50, 300, 50, 50), "Lon-/Lat+"))
            {
                lon -= debugChange;
                lat += debugChange;
                //Debug.Log("E pressed");
            }
            if (GUI.Button(new Rect(Screen.width - 50, 350, 50, 50), "Lon-/Lat-"))
            {
                lon -= debugChange;
                lat -= debugChange;
                //Debug.Log("E pressed");
            }
            if (GUI.Button(new Rect(Screen.width - 50, 400, 50, 50), "2x"))
            {
                debugChange = debugChange * 2;
                Debug.Log("debugChange changed to: " + debugChange);
            }
            if (GUI.Button(new Rect(Screen.width - 50, 450, 50, 50), "1/2x"))
            {
                debugChange = debugChange / 2;
                Debug.Log("debugChange changed to: " + debugChange);
            }
            if (GUI.Button(new Rect(Screen.width - 50, 500, 50, 50), "Int++"))
            {
                interval++;
                Debug.Log("Interval changed to: " + interval);
            }
            if (GUI.Button(new Rect(Screen.width - 50, 550, 50, 50), "Int--"))
            {
                interval--;
                Debug.Log("Interval changed to: " + interval);
            }
            //GUILayout.EndArea();
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

}
