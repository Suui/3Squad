using UnityEngine;
using System.Collections;
using System;

namespace Medusa
{

  public class CellItem : MonoBehaviour
  {

    private Position position;

    public Position Position
    {
      get { return position; }
      set
      { 
        position = value;
        transform.position = new Vector3(position.x, 0, position.y);
      }
    }

    public Layer Layer
    {
      get;
      set;
    }

  }

}