using UnityEngine;
using System;
using System.Collections;

using Medusa.Core;
using Medusa.Level.Components;

using Medusa.View;

namespace Medusa.Level
{
  public enum State
  {
    Null,
    Token,
    Skill,
    Confirm }
  ;

  public class StateManager : MonoBehaviour
  {

    public State currentState = State.Null;
    public Skill selectedSkill;
    public Token selectedToken;
    private Layer<Token> tokens;
    public GUIControl gui;
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
      if (currentState != State.Null)
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
        case State.Null:
          if (pos != null)
          {
            if (!tokens.Empty(pos))
            {
              selectedToken = tokens [pos];
              currentState = State.Token;
            }
          }
          break;
        case State.Token:
          if (pos == null || tokens.Empty(pos))
          {
            currentState = State.Null;
            selectedToken = null;
          } else
          {
            selectedToken = tokens [pos];
          }
          break;
        case State.Skill:
          if (selectedSkill.HandleClick(pos))
          {
            currentState = State.Confirm;
          } else
          {
            currentState = State.Token;
            selectedSkill.CleanUp();
            selectedSkill = null;
          }
          break;
        case State.Confirm:
          if (selectedSkill.HandleClick(pos))
          {
            currentState = State.Token;
            selectedSkill.CleanUp();
            selectedSkill = null;
          } else
          {
            currentState = State.Token;
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
        case State.Token:
          selectedSkill = skill;
          currentState = State.Skill;
          skill.Setup(board);
          break;
        case State.Skill:
          if (skill != selectedSkill)
          {
            selectedSkill.CleanUp();
            skill.Setup(board);
            selectedSkill = skill;
          } 
          break;
        case State.Confirm:
          if (skill == selectedSkill)
          {
            skill.Apply();
            selectedSkill = null;
            currentState = State.Token;
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