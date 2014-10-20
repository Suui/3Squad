using UnityEngine;
using System.Collections;

namespace Medusa
{

  public class Floor : CellItem
  {
      
    private Material baseMaterial;

    void Awake()
    {
      baseMaterial = transform.GetChild(0).renderer.material;
    }

    public void Select(Material mat)
    {
      transform.GetChild(0).renderer.material = mat;
    }

    public void Unselect()
    {
      Select(baseMaterial);
    }
  }

}