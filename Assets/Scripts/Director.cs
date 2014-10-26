using UnityEngine;


namespace Medusa
{

    public delegate void BoardOnNew(Board board);


    public class Director : MonoBehaviour
    {

        public event BoardOnNew OnNewBoard;

        public int boardRows;
        public int boardColumns;

        public GameObject terrainPrefab;


        #region Singleton

        private static Director instance;

        public static Director Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Director>();
                return instance;
            }
        }

        #endregion


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

            // Destroy old board
            if (CurrentBoard != null)
                Destroy(CurrentBoard.SceneNode);

            // Create Scene Node
            CurrentBoard = new Board(boardRows, boardColumns, "Terrain", "Tokens", "Effects", "Overlay");

            // Set Terrain
            foreach (Position pos in CurrentBoard.AllBoardPositions())
            {
                GameObject go = (GameObject)Instantiate(terrainPrefab);
                go.name = "Cell @ " + pos;
                CurrentBoard ["Terrain"] [pos] = go;
                go.transform.position = pos;
            }

            // Call Event
            if (OnNewBoard != null)
                OnNewBoard(CurrentBoard);

        }


        public Board CurrentBoard
        {
            get;
            private set;
        }

    }
}