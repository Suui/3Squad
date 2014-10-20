using UnityEngine;
using System.Collections;
using System;

namespace Medusa
{

  public delegate void OnClickHandler(Position clicked);
  public delegate void OnSkillHandler(Skill clicked);

  public class GUIControl : MonoBehaviour
  {

    public int leftOffset = 10;
    public int topOffset = 10;
    public int buttonWidth = 50;
    public int buttonHeight = 50;
    public int labelWidth = 200;
    public int labelHeight = 20;

    public event OnClickHandler OnClick;

    public event OnSkillHandler OnSkill;

    private bool launchClick = true;

    private Token renderToken;

    public void RenderInfo(Info[] infos)
    {
      for (int i = 0; i < infos.Length; i++)
      {
        GUI.Label(new Rect(leftOffset, topOffset + (labelHeight + topOffset) * i, labelWidth, labelHeight), infos [i].Content);
      }

    }

    public void GetInteraction(Skill[] skills)
    {
      launchClick = false;
      if (skills != null)
      {
        for (int i = 0; i < skills.Length; i++)
        {
          if (GUI.Button(new Rect(leftOffset + ((buttonWidth + leftOffset) * i), Screen.height - topOffset - buttonHeight, buttonWidth, buttonHeight), skills [i].description))
          {
            SkillEvent(skills [i]);
            Event.current.Use();
            launchClick = true;
            return;
          }
        }
      }
      launchClick = true;
      return;
    }

    // Click detection
    void Update()
    {

      if (launchClick && GUIUtility.hotControl == 0)
      {
        if (Input.GetMouseButtonDown(0))
        {
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit hitInfo;
          if (Physics.Raycast(ray, out hitInfo))
          {
            Floor terrain = hitInfo.transform.GetComponent<Floor>();
            if (terrain == null)
            {
              throw new InvalidOperationException("Selected Object is no Terrain");
            } else
            {
              ClickEvent(terrain.Position);
            }
          } else
          {
            ClickEvent(null);
          } 
          
        }
      }
    }

    // Lanzar el evento de forma segura
    private void ClickEvent(Position pos)
    {
      if (OnClick != null)
      {
        OnClick(pos);
      }
    }

    private void SkillEvent(Skill skill)
    {
      if (OnSkill != null)
      {
        OnSkill(skill);
      }
    }

  }
}