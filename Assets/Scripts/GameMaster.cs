using UnityEngine;


namespace Medusa
{

    public delegate void BoardOnNew(Board board);


    public class GameMaster : MonoBehaviour
    {

        public event BoardOnNew OnNewBoard;

        public GameObject masterOne;
        public GameObject masterTwo;

        public GameObject masterCellPrefab;
        public GameObject boardCellPrefab;
        public GameObject[] obstaclePrefabs;
        public int obstaclesLimit;
        public int boardRows;
        public int boardColumns;
        public float boardYSize;
        public int seed;

        private Player[] players;

        private BoardGenerator boardGenerator;
        private TurnManagement turnManagement;


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

            Player playerOne = new Player("Player One!", 5);
            Player playerTwo = new Player("Player Two!", 5);
            players = new[] { playerOne, playerTwo};
            
            turnManagement = new TurnManagement(players[0], players[1], seed);


            // TODO: Remove testing block when over
            // Test GUI
            GameObject go = Instantiate(Resources.Load("Prefabs/Superr_Character")) as GameObject;
            go.name = "Superrrr Characterr";
            go.transform.position = new Position(0, 0);
            CurrentBoard["tokens"][new Position(0, 0)] = go;

            go.AddComponent<PlayerComponent>();
            go.GetComponent<PlayerComponent>().Player = players[0];
        }


        public Board CurrentBoard
        {
            get { return boardGenerator.Board; }
        }


        public TurnManagement TurnManagement
        {
            get { return turnManagement; }
        }


        public Player GetPlayerOne
        {
            get { return players[0]; }
        }


        public Player GetPlayerTwo
        {
            get { return players[1]; }
        }
    }
}