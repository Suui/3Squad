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
        private SelectionBehaviour selection;


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

            //selection = new SelectionBehaviour(boardGenerator.Board);
            //selection.Run();
        }


        void Update()
        {

        }


        public Board CurrentBoard
        {
            get { return boardGenerator.Board; }
        }

    }
}