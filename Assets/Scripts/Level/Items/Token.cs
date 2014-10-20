using UnityEngine;
using System.Collections;

namespace Medusa
{

  public class Token : CellItem
  {

    public Info[] Infos
    {
      get
      {
        return transform.GetComponents<Info>();
      }
    }
    public Skill[] Skills
    {
      get
      {
        return transform.GetComponents<Skill>();
      }
    }

  }
}