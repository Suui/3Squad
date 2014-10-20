using UnityEngine;
using System;
using System.Collections.Generic;

namespace Medusa.Core
{

  public class Board : MonoBehaviour
  {
    public int width;
    public int height;
    private Dictionary<Type,Layer> layers = new Dictionary<Type,Layer >();

   
    public Layer<T> CreateLayer<T>() where T : CellItem
    {
      Layer<T> lay = new Layer<T>(width, height);
      lay.SceneNode.transform.parent = transform;
      layers [typeof(T)] = lay;
      return lay;
    }

    public Layer<T> GetLayer<T>() where T : CellItem
    {
      return (Layer<T>)layers [typeof(T)];
    }

  }
}