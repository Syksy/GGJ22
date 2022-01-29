using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    //variables
    public float moveSpeed = 120;
    public GameObject character;

    private Rigidbody2D rb;
    private float ScreenWidth;


    // Use this for initialization
    void Start()
    {
        ScreenWidth = Screen.width;
        rb = character.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        //loop over every touch found
        while (i < Input.touchCount)
        {
            if (Input.GetTouch(i).position.x > ScreenWidth / 2)
            {
                //move right
                RunCharacter(100f);
            }
            if (Input.GetTouch(i).position.x < ScreenWidth / 2)
            {
                //move left
                RunCharacter(-100f);
            }
            ++i;
        }
    }
    void FixedUpdate()
    {
    #if UNITY_EDITOR
    RunCharacter(Input.GetAxis("Horizontal"));
    #endif
    }

    private void RunCharacter(float horizontalInput)
    {
        //move player
        rb.AddForce(new Vector2(horizontalInput * moveSpeed * Time.deltaTime, 0));

    }
}