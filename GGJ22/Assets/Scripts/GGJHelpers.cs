using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ22.Helpers
{
    class Helpers
    {

        public Shader lineShader;

        // Override helper for 2d coords
        public void DrawLine(Vector2 start, Vector2 end, Color color, float duration = 0.2f)
        {
            DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(end.x, end.y, 0.0f), color, duration);
        }
        // https://answers.unity.com/questions/8338/how-to-draw-a-line-using-script.html
        public void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
        {
            GameObject myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.material = new Material(Shader.Find("Legacy Shaders/Diffuse"));
            //lr.material = new Material(lineShader);
            lr.SetColors(color, color);
            lr.SetWidth(0.1f, 0.1f);
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            GameObject.Destroy(myLine, duration);
        }
    }
}