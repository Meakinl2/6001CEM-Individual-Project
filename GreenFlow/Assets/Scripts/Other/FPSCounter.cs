using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class FpsCounter : MonoBehaviour
{
    private string filePath;

    private float rawFps;
    private float smoothedFps1,smoothedFps2,smoothedFps3,smoothedFps4,smoothedFps5;
    
    private void Awake()
    {
        string dateTime = System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        filePath = "Assets/FPS_Logs/" + dateTime + "-fps_log_raw.txt";
    }

    private IEnumerator Start()
    {
        GUI.depth = 2;
        
        while (true)
        {
            rawFps = 1f / Time.unscaledDeltaTime;

            smoothedFps1 = (0.2f * rawFps) + (1f - 0.2f) * smoothedFps1;
            smoothedFps2 = (0.4f * rawFps) + (1f - 0.4f) * smoothedFps2;
            smoothedFps3 = (0.6f * rawFps) + (1f - 0.6f) * smoothedFps3;
            smoothedFps4 = (0.8f * rawFps) + (1f - 0.8f) * smoothedFps4;
            smoothedFps5 = (1.0f * rawFps) + (1f - 1.0f) * smoothedFps5;

            yield return new WaitForSeconds(0.1f);
            
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ", " 
                    + rawFps.ToString("F2") + ", " + smoothedFps1.ToString("F2") + ", " + smoothedFps2.ToString("F2") + ", " 
                    + smoothedFps3.ToString("F2") + ", " + smoothedFps4.ToString("F2") + ", " + smoothedFps5.ToString("F2") + ", ");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error writing to file: " + e.Message);
            }
        } 
    }
    
    private void OnGUI()
    {
        Rect location = new Rect(5, 5, 85, 25);
        string text = $"FPS: {Mathf.Round(rawFps)}";
        Texture black = Texture2D.linearGrayTexture;
        GUI.DrawTexture(location, black, ScaleMode.StretchToFill);
        GUI.color = Color.black;
        GUI.skin.label.fontSize = 18;
        GUI.Label(location, text);
    }

}
