using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private bool cameraEnabled = true;

    public float panSpeed = 30f;
    public float panBorderThicccness = 10f;
    public float scrollSpeed = 5f;
    public float minY = 10F;
    public float maxY = 80f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Disable || Enable 
        if (Input.GetKeyDown(KeyCode.Escape))
            cameraEnabled = !cameraEnabled;

        if (!cameraEnabled)
            return;

        //Camera Up
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThicccness)
        {
            transform.Translate(Vector3.up * panSpeed * Time.deltaTime, Space.World); //Vector 0f ,0f , 1f * panspeed *timedeltatime, using global Camera
        }
        //Camera right
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThicccness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World); //Vector 0f ,0f , 1f * panspeed *timedeltatime, using global Camera
        }
        //Camera Left
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThicccness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World); //Vector 0f ,0f , 1f * panspeed *timedeltatime, using global Camera
        }
        //Camera Down
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThicccness)
        {
            transform.Translate(Vector3.down * panSpeed * Time.deltaTime, Space.World); //Vector 0f ,0f , 1f * panspeed *timedeltatime, using global Camera
        }


        //  Scroll Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 position = transform.position;
        position.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        position.y = Mathf.Clamp(position.y, minY, maxY); // Restricts zoom betwen minY and MaxY, no zoom through floor || zoom out to space
        transform.position = position;

    }
}
