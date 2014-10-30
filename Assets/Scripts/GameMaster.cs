using UnityEngine;


namespace Medusa
{

    public delegate void BoardOnNew(Board board);


    public class GameMaster : MonoBehaviour
    {

        public event BoardOnNew OnNewBoard;

        public GameObject boardCellPrefab;
        public GameObject[] obstaclePrefabs;
        public int obstaclesLimit;
        public int boardRows;
        public int boardColumns;
        public float boardYSize;
        public int seed;

        private BoardGenerator boardGenerator;


        // TODO: Not used?! Calvin watcha doin'!
        #region Singleton

        private static GameMaster instance;

        public static GameMaster Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<GameMaster>();
                return instance;
            }
        }

        #endregion


        void Awake()
        {
            boardGenerator = new BoardGenerator(boardCellPrefab, boardRows, boardColumns);
            boardGenerator.CreateEmptyBoard(boardYSize);
            boardGenerator.SpawnObstacles(obstaclePrefabs, obstaclesLimit, seed);


            // TODO: Remove testing block when over
            // Test GUI
            GameObject go = Instantiate(Resources.Load("Prefabs/Superr_Character")) as GameObject;
            go.name = "Superrrr Characterr";
            go.transform.position = new Position(0, 0);
            CurrentBoard["tokens"][new Position(0, 0)] = go;
        }


        public Board CurrentBoard
        {
            get { return boardGenerator.Board; }
        }

    }
}