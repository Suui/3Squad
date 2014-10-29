using UnityEngine;
using System.Collections;

using Medusa;

// Controla zoom y rotacion
public class CameraControl : MonoBehaviour
{

    #region Private Properties

    private Camera cam;
    private float minFOV = 2;
    private float maxFOV = 8;
    private float distance = 50;
    private float zoomSensitivity = 4;
    private float zoomInertia = 4;
    private Vector3 mousePosition;
    private bool isRotating = false;
    private float rotation = 45;
    private float rotSensitivity = 0.1f;
    private float rotInertia = 2;
    private float rotDelta;
    
    private float keyDeltaX;
    private float keySensitivity = 2;

    #endregion

    #region Public Flags

    public bool doRotate;
    public bool doZoom;

    #endregion

    #region Awake

    void Awake()
    {
        cam = Camera.main;
        
        distance = cam.orthographicSize;
        rotation = transform.eulerAngles.y;

        GameMaster.Instance.OnNewBoard += ResetTransform;

    }

    #endregion

    #region Board Resize

    private void ResetTransform(Board board)
    {
        transform.position = new Vector3(((float)board.Columns) / 2 - 0.5f
                                         , -0.75f
                                         , ((float)board.Rows) / 2 - 0.5f);
    }

    #endregion

    #region Update

    void Update()
    {
        if (doZoom)
            ZoomCamera();
        if (doRotate)
            RotateScreen();
    }

    #endregion

    #region Camera Zoom
    
    void ZoomCamera()
    {
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        distance = Mathf.Clamp(distance, minFOV, maxFOV);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, distance, Time.deltaTime * zoomInertia);
    }

    #endregion

    #region Screen Rotation
    
    void RotateScreen()
    {

        #region Mouse Input

        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            mousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        #endregion

        #region Calculate Delta row

        float dx;
        
        if (isRotating)
        {
            dx = (Input.mousePosition.x - mousePosition.x) * rotSensitivity;
            mousePosition = Input.mousePosition;
        } else
        {
            dx = - rotDelta;
        }

        #endregion

        #region Key Interaction

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            keyDeltaX = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            keyDeltaX = 1;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            keyDeltaX = 0;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            keyDeltaX = 0;
        }

        #endregion

        #region Apply Rotation

        dx += keyDeltaX * keySensitivity;
        
        
        rotDelta = Mathf.Lerp(rotDelta, rotDelta + dx, Time.deltaTime * rotInertia);
        
        rotation += rotDelta;
        transform.eulerAngles = new Vector3(30, rotation, 0);

        #endregion

    }

    #endregion
    
}