using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // If the Game is in debug mode, print additional debugging GUI output etc
    public bool debugEnabled = true;
    // Debug queue
    private string myLog;
    private List<string> myLogQueue = new List<string>();
    // Maximum number of debug messages stored
    private int maxDebug = 10;

    // Game status polling interval
    private static float interval = 5.0f;
    private float currentInterval = interval;

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
                //Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                Debug.Log("Location LAT: " + Input.location.lastData.latitude + " LON: " + Input.location.lastData.longitude + " (" + Input.location.lastData.timestamp + " time)");
            }
        }
    }

    // GPS location service start and run cycle
    IEnumerator Start()
    {
        Debug.Log("Starting Game...");

        // Load game state
        loadGameState();

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
