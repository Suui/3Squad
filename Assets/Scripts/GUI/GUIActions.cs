using System;
using UnityEngine;


public class GUIRelatedActions : MonoBehaviour
{

    private bool displayingCharacterMenu = false;
    private bool displayingMovementMenu = false;
    private bool displayingAttackMenu = false;

    private Selection selection;
    private ColoringCells coloringCells;


    void Start()
    {
        selection = GetComponent<Selection>();
        coloringCells = GetComponent<ColoringCells>();
    }


    void OnGUI()
    {
        ResetBooleans();

        if (displayingCharacterMenu)
            DisplayCharacterMenu(selection.currentSelection["token"].GetComponent<Character>());

        if (displayingMovementMenu)
            DisplayMovementMenu(selection.currentSelection["token"].GetComponent<Character>());

        if (displayingAttackMenu)
            DisplayAttackMenu(selection.currentSelection["token"].GetComponent<Character>());
    }


    private void ResetBooleans()
    {
        if (selection.currentSelection["token"] != null && selection.currentSelection["token"].GetComponent<Character>())
            displayingCharacterMenu = true;
        else
            displayingCharacterMenu = false;

        if (GameObject.FindGameObjectWithTag("movingToCell"))
        {
            displayingMovementMenu = true;
            displayingCharacterMenu = false;
        }
        else
            displayingMovementMenu = false;

        if (GameObject.FindGameObjectWithTag("objectiveCell"))
        {
            displayingAttackMenu = true;
            displayingCharacterMenu = false;
        }
        else
            displayingAttackMenu = false;
    }


    public void DisplayCharacterMenu(Character character)
    {
        // Basic Attack
        int i = 0;
        if (GUI.Button(new Rect(20 + 105 * i, Screen.height - 35, 100, 30), character.CharacterInfo[i + 1]))
        {
            coloringCells.DeColorAttackCells();
            coloringCells.ColorBasicAttackCells(character);
        }
        i++;

        // Movement
        if (GUI.Button(new Rect(20 + 105 * i, Screen.height - 35, 100, 30), character.CharacterInfo[i + 1] + " " + character.Moves))
        {
            coloringCells.DeColorAllMovementCells();
            coloringCells.ColorMovementCells(character);
        }
        i++;

        // Special Attack One
        if (GUI.Button(new Rect(20 + 105 * i, Screen.height - 35, 100, 30), character.CharacterInfo[i + 1]))
        {
            character.SpecialAttackOne();
            coloringCells.DeColorAttackCells();
            coloringCells.ColorSpecialAttackOneCells(character);
        }
        i++;

        // Special Attack Two
        if (GUI.Button(new Rect(20 + 105 * i, Screen.height - 35, 100, 30), character.CharacterInfo[i + 1]))
        {
            character.SpecialAttakTwo();
            coloringCells.DeColorAttackCells();
            //TODO: coloringCells.ColorSpecialAttackTwoCells();
        }
    }


    public void DisplayMovementMenu(Character character)
    {
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 35, 120, 30), "Moves available: " + character.Moves);

        if (GUI.Button(new Rect(150, Screen.height - 35, 120, 30), "Cancel Move"))
        {
            character.Moves = character.Moves + selection.movementSteps.Count;
            selection.movementSteps.Clear();

            coloringCells.DeColorAllMovementCells();
            coloringCells.ColorBasicAttackCells(character);
            coloringCells.ColorMovementCells(character);
        }

        if (GUI.Button(new Rect(Screen.width - 270, Screen.height - 35, 120, 30), "Apply Move"))
        {
            character.Movement();
            coloringCells.DeColorAllMovementCells();

            selection.MoveSelectedCell();
            selection.movementSteps.Clear();
        }
    }


    private void DisplayAttackMenu(Character character)
    {
        if (GUI.Button(new Rect(150, Screen.height - 35, 120, 30), "Cancel Attack"))
        {
            selection.objectiveList.Clear();

            coloringCells.DeColorAttackCells();
            coloringCells.ColorBasicAttackCells(character);
            coloringCells.ColorMovementCells(character);
        }

        if (GUI.Button(new Rect(Screen.width - 270, Screen.height - 35, 120, 30), "Confirm Attack"))
        {
            character.BasicAttack();
            selection.objectiveList.Clear();

            coloringCells.DeColorAttackCells();
            coloringCells.ColorBasicAttackCells(character);
            coloringCells.ColorMovementCells(character);
        }
    }


    public void DisplayMasterMenu(Master master)
    {

    }

}
