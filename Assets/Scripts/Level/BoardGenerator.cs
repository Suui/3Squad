using UnityEngine;
using System.Collections;

namespace Medusa
{
  public class BoardGenerator : MonoBehaviour
  {

    public GameObject terrainPrefab;
    public GameObject tokenPrefab;
    private Board board;

    void Awake()
    {
      board = GetComponent<Board>();
      board.CreateLayer<Floor>();
      board.CreateLayer<Token>();
    }

    void Start()
    {
      Layer<Floor> floorLayer = board.GetLayer<Floor>();
      foreach (Position pos in floorLayer.Positions)
      {
        GameObject go = (GameObject)Instantiate(terrainPrefab);
        go.name = "Cell @ " + pos;
        floorLayer.Put(go, pos);
      }

      Layer<Token> tokenLayer = board.GetLayer<Token>();
      tokenLayer.Put((GameObject)Instantiate(tokenPrefab), new Position(1, 1));

    }

  }

}