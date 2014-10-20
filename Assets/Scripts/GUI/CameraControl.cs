using UnityEngine;
using System.Collections;

namespace Medusa
{

// Controla zoom y rotacion
  public class CameraControl : MonoBehaviour
  {
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

    public bool doRotate;
    public bool doZoom;

    void Start()
    {
      Board board = GameObject.Find("BoardNode").GetComponent<Board>();
      transform.position = new Vector3((float)board.width / 2
                                    , -0.75f
                                    , (float)board.height / 2);
      cam = Camera.main;

      distance = cam.orthographicSize;
      rotation = transform.eulerAngles.y;

    }
  
    void Update()
    {
      if (doZoom)
        ZoomCamera();
      if (doRotate)
        RotateScreen();
    }

    void ZoomCamera()
    {
      distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
      distance = Mathf.Clamp(distance, minFOV, maxFOV);
      cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, distance, Time.deltaTime * zoomInertia);
    }

    void RotateScreen()
    {

      if (Input.GetMouseButtonDown(1))
      {
        isRotating = true;
        mousePosition = Input.mousePosition;
      }
      if (Input.GetMouseButtonUp(1))
      {
        isRotating = false;
      }

      float dx;

      if (isRotating)
      {
        dx = (Input.mousePosition.x - mousePosition.x) * rotSensitivity;
        mousePosition = Input.mousePosition;
      } else
      {
        dx = - rotDelta;
      }

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

      dx += keyDeltaX * keySensitivity;


      rotDelta = Mathf.Lerp(rotDelta, rotDelta + dx, Time.deltaTime * rotInertia);

      rotation += rotDelta;
      transform.eulerAngles = new Vector3(30, rotation, 0);

    }

  }


}