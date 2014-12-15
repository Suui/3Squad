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
            GameObject go = Instantiate(Resources.Load("Prefabs/Eagle")) as GameObject;
            go.name = "Eagle";
            go.transform.position = new Position(0, 0);
            CurrentBoard["tokens"][new Position(0, 0)] = go;

			GameObject go2 = Instantiate(Resources.Load("Prefabs/Turtle")) as GameObject;
			go2.name = "Turtle";
			go2.transform.position = new Position(3, 4);
			CurrentBoard["tokens"][new Position(3, 4)] = go2;

			GameObject go3 = Instantiate(Resources.Load("Prefabs/Frog")) as GameObject;
			go3.name = "Frog";
			go3.transform.position = new Position(1, 13);
			CurrentBoard["tokens"][new Position(1, 13)] = go3;

        }


        public Board CurrentBoard
        {
            get { return boardGenerator.Board; }
        }

    }
}