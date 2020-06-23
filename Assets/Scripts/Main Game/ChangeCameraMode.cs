using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraMode : MonoBehaviour
{
    [SerializeField]
    float width;
    [SerializeField]
    float height;
    void Start()
    {

       
        
    }

    // Update is called once per frame
    void Update()
    {
        width = Screen.width;
        height = Screen.height;
        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Screen.width > Screen.height)
            {
                GetComponent<Camera>().orthographicSize = 5;
            }
            if (Screen.width < Screen.height)
            {
                GetComponent<Camera>().orthographicSize = 8;
            }
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Screen.width > Screen.height)
            {
                GetComponent<Camera>().orthographicSize = 5;
            }
            if (Screen.width < Screen.height)
            {
                GetComponent<Camera>().orthographicSize = 8;
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Screen.width > Screen.height)
            {
                GetComponent<Camera>().orthographicSize = 5;
            }
            if (Screen.width < Screen.height)
            {
                GetComponent<Camera>().orthographicSize = 8;
            }
            /*
            if (Screen.orientation == ScreenOrientation.Portrait)
            {
                GetComponent<Camera>().orthographicSize = 8;
            }
            if(Screen.orientation == ScreenOrientation.Landscape)
            {

                GetComponent<Camera>().orthographicSize = 5;
            }
            */
        }
    }
}
