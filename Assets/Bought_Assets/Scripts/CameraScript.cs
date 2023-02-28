using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public static float pixelsToUnits = 100f;
    public static float scale = 1f;

    public Vector2 nativeResolution = new Vector2(1920f, 1080f);

    void Awake()
    {
        var camera = GetComponent<Camera>();

        if (camera.orthographic)
        {
            scale = Screen.height / nativeResolution.y;
            pixelsToUnits += scale;
            Debug.Log(pixelsToUnits);
            camera.orthographicSize = (Screen.height / 2.0f) / pixelsToUnits;
        }
        else
        {
            Debug.Log("none orthographic Camera");
        }
    }
}
