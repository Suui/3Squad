using UnityEngine;


namespace Medusa
{

    public class GameMaster : MonoBehaviour
    {

        void OnEnable()
        {
            SelectionSM.OnChangingTurn += ChangeTurn;
        }


        void OnDisable()
        {
            SelectionSM.OnChangingTurn -= ChangeTurn;
        }


        public GameObject masterOne;
        public GameObject masterTwo;

        public int startingActionPoints = 5;
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


        void Awake()
        {
            boardGenerator = new BoardGenerator(boardCellPrefab, boardRows, boardColumns);
            boardGenerator.CreateEmptyBoard(boardYSize);
            boardGenerator.SpawnObstacles(obstaclePrefabs, obstaclesLimit, seed);

            Player playerOne = new Player("Player One!", startingActionPoints);
            Player playerTwo = new Player("Player Two!", startingActionPoints);
            players = new[] { playerOne, playerTwo};

            SetUpMasters();

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


        private void SetUpMasters()
        {
            GameObject master1 = Instantiate(masterOne) as GameObject;
            master1.name = "Master 01";
            master1.transform.position = MasterOnePos;
            CurrentBoard["tokens"][MasterOnePos] = master1;


            GameObject master2 = Instantiate(masterOne) as GameObject;
            master2.name = "Master 02";
            master2.transform.position = MasterOnePos;
            CurrentBoard["tokens"][MasterOnePos] = master2;
        }


        public void ChangeTurn(TurnEvents turnEvents)
        {
            
        }


        #region Getters and Setters

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


        public Position MasterOnePos
        {
            get { return new Position(CurrentBoard.Rows / 2, - 2); }
        }


        public Position MasterTwoPos
        {
            get { return new Position(CurrentBoard.Rows / 2, CurrentBoard.Columns + 1); }
        }

        #endregion

    }
}