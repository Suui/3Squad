using UnityEngine;

namespace Medusa
{

  public class Test : Skill
  {

    private Board board;
    private Position position;

    public Material highlightMaterial;
    public Material selectMaterial;
    public Material successMaterial;

    void Awake()
    {
      position = Position.FromString(description);
    }

    public override void Apply()
    {
      Debug.Log(position + " was clicked");
      board.GetLayer<Floor>() [position].Select(successMaterial);
    }

    public override void CleanUp()
    {
      Debug.Log("Cleaning");
      board.GetLayer<Floor>() [position].Unselect();
    }

    public override bool HandleClick(Position pos)
    {
      if (pos == position)
      {
        board.GetLayer<Floor>() [position].Select(selectMaterial);
        return true;
      } else 
        return false;
    }

    public override void Setup(Board board)
    {
      this.board = board;
      board.GetLayer<Floor>() [position].Select(highlightMaterial);
    }

  }

}