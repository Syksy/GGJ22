using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Game game;
    private double lat; // Latitude in GPS coordinate system
    private double lon; // Longitude in GPS coordinate system
    private int x; // Pixelated coordinate in world system
    private int y; // Pixelated coordinate in world system
    private int lod; // Level of depth
    private int tileSize; // Tile size
    
    // Start is called before the first frame update
    void Start()
    {
        // Find and store the global game object
        game = GameObject.Find("Game").GetComponent<Game>();
        // Fetch values from the Game-script
        lod = game.getLOD();
        tileSize = game.getTileSize();
    }

    // Update is called once per frame
    void Update()
    {
        //lat = game.getLatitude();
        //lon = game.getLongitude();
        // Calculate new {x,y} plane coordinates and set these
        //TileSystem.LatLongToPixelXY(lat, lon, lod, out x, out y, tileSize);
        // Set new character location
        this.gameObject.transform.position = new Vector3((float) game.getX(), (float) game.getY(), 0);
        //if (game.getDebug()) Debug.Log("Character position set to {x=" + x + ",y=" + y + ")");
    }
}
