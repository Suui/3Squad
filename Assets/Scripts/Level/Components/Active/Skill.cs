using UnityEngine;

using Medusa.Core;

namespace Medusa.Level.Components
{

  public enum SkillState
  {
    Unavailable,
    Available,
    Selected,
    ToConfirm,
    Used
  }

  public abstract class Skill : MonoBehaviour
  {

    public SkillState State
    {
      get;
      set;
    }

    public int cost;
    public string description;

    public abstract void Setup(Board board);

    public abstract void CleanUp();

    public abstract bool HandleClick(Position pos);

    public abstract void Apply();

  }

}