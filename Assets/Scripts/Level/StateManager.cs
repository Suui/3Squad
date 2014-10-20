using UnityEngine;
using System;
using System.Collections;

namespace Medusa
{
  public enum State
  {
    NothingSelected,
    TokenSelected,
    SkillSelected,
    ConfirmSkill }
  ;

  public class StateManager : MonoBehaviour
  {

    public State currentState = State.NothingSelected;
    public Skill selectedSkill;
    public Token selectedToken;
    private Layer<Token> tokens;
    private GUIControl gui;
    private Board board;

    void Awake()
    {
      board = GameObject.Find("BoardNode").GetComponent<Board>();
      gui = GetComponent<GUIControl>();
    }

    void Start()
    {
      tokens = board.GetLayer<Token>();
      
      gui.OnClick += DebugClick;
      gui.OnClick += OnClick;
      gui.OnSkill += OnSkill;
    }

    void OnGUI()
    {
      if (currentState != State.NothingSelected)
      {
        gui.RenderInfo(selectedToken.Infos);
        gui.GetInteraction(selectedToken.Skills);
      }
    }

    void DebugClick(Position pos)
    {
      Debug.Log("Click @ " + pos);
    }

    void OnClick(Position pos)
    {

      switch (currentState)
      {
        case State.NothingSelected:
          if (pos != null)
          {
            if (!tokens.Empty(pos))
            {
              selectedToken = tokens [pos];
              currentState = State.TokenSelected;
            }
          }
          break;
        case State.TokenSelected:
          if (pos == null || tokens.Empty(pos))
          {
            currentState = State.NothingSelected;
            selectedToken = null;
          } else
          {
            selectedToken = tokens [pos];
          }
          break;
        case State.SkillSelected:
          if (selectedSkill.HandleClick(pos))
          {
            currentState = State.ConfirmSkill;
          } else
          {
            currentState = State.TokenSelected;
            selectedSkill.CleanUp();
            selectedSkill = null;
          }
          break;
        case State.ConfirmSkill:
          if (!selectedSkill.HandleClick(pos))
          {
            currentState = State.TokenSelected;
            selectedSkill.CleanUp();
            selectedSkill = null;
          }
          break;
      }
    }

    void OnSkill(Skill skill)
    {
      switch (currentState)
      {
        case State.TokenSelected:
          selectedSkill = skill;
          currentState = State.SkillSelected;
          skill.Setup(board);
          break;
        case State.SkillSelected:
          if (skill != selectedSkill)
          {
            selectedSkill.CleanUp();
            skill.Setup(board);
            selectedSkill = skill;
          } else
          {
            selectedSkill.CleanUp();
            selectedSkill = null;
            currentState = State.TokenSelected;
          }
          break;
        case State.ConfirmSkill:
          if (skill == selectedSkill)
          {
            skill.Apply();
            selectedSkill = null;
            currentState = State.TokenSelected;
          } else
          {
            selectedSkill.CleanUp();
            skill.Setup(board);
            selectedSkill = skill;
          }
          break;
      }

    }


  }

}