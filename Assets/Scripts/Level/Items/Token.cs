using UnityEngine;
using System.Collections;

using Medusa.Core;

using Medusa.Level.Components;

namespace Medusa.Level
{

  public class Token : CellItem
  {

    public Info[] Infos
    {
      get
      {
        return gameObject.GetComponents<Info>();
      }
    }
    public Skill[] Skills
    {
      get
      {
        return gameObject.GetComponents<Skill>();
      }
    }

  }
}