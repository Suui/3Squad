using UnityEngine;
using System.Collections;

namespace Medusa
{
    public delegate void BoardChange(Board board);

    public class Director : MonoBehaviour
    {

        public int boardRows;
        public int boardColumns;

        public GameObject terrainPrefab;

        public event BoardChange OnBoardChange;

        private static Director instance;

        public static Director Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<Director>();
                return instance;
            }
        }

        public Board CurrentBoard
        {
            get;
            private set;
        }

        void Start()
        {
            RecreateBoard();
        }

        void Update()
        {
            if (boardRows != CurrentBoard.Rows || boardColumns != CurrentBoard.Columns)
                RecreateBoard();
        }

        private void RecreateBoard()
        {
            if (CurrentBoard != null)
            {
                GameObject.Destroy(CurrentBoard.SceneNode);
            }

            CurrentBoard = new Board(boardRows, boardColumns, "Terrain", "Tokens", "Effects");
        
            foreach (Position pos in Position.Range(CurrentBoard))
            {
                GameObject go = (GameObject)Instantiate(terrainPrefab);
                go.name = "Cell @ " + pos;
                CurrentBoard ["Terrain"] [pos] = go;
                CurrentBoard.ValidatePosition(go);
            }

            if (OnBoardChange != null)
            {
                OnBoardChange(CurrentBoard);
            }

        }
	
    }
}