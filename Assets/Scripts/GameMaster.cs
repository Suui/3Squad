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
            SetUpButtons();

            turnManagement = new TurnManagement(players[0], players[1], seed);


            // TODO: Remove testing block when over
            // Test GUI
            GameObject go = Instantiate(Resources.Load("Prefabs/Fox")) as GameObject;
            go.name = "Fox";
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
            master2.transform.position = MasterTwoPos;
            CurrentBoard["tokens"][MasterTwoPos] = master2;
        }


        private void SetUpButtons()
        {
            // Exit and EndTurn buttons
			CreateButton("Textures/Buttons/ExitButton", "ExitEndTurn", "Exit");
			CreateButton("Textures/Buttons/EndTurnButton","ExitEndTurn", "EndTurn");

            // Confirm and Cancel skill buttons
			CreateButton("Textures/Buttons/ConfirmButton", "ConfirmCancel", "Confirm");
			CreateButton("Textures/Buttons/CancelButton", "ConfirmCancel", "Cancel");

			// Info button
			CreateButton("Textures/Buttons/InfoButton", "Info", "Info");

            // Hide the Confirm and Cancel skill buttons at first
			foreach (var go in GameObject.FindGameObjectsWithTag("ConfirmCancel"))
				go.GetComponent<GUITexture>().enabled = false;
        }


        public void ChangeTurn(TurnEvents turnEvents)
        {
            
        }


        public static void CreateButton(string texturePath, string tag, string id)
        {
            GameObject button = Instantiate(Resources.Load("Prefabs/Button_Template")) as GameObject;
			button.GetComponent<ClickableButton>().Id = id;
			button.tag = tag;
			button.name = id;

			GUITexture gui = button.GetComponent<GUITexture>();

			gui.texture = Resources.Load(texturePath) as Texture2D;
			int width = gui.texture.width;
			int height = gui.texture.height;

			if (id == "Exit")
				gui.pixelInset = new Rect(Screen.width - width, Screen.height - height, width, height);

			if (id == "EndTurn")
				gui.pixelInset = new Rect(Screen.width - width, 0, width, height);

			if (id == "Confirm")
				gui.pixelInset = new Rect((Screen.width / 12) * 5 - width, Screen.height / 24, width, height);

			if (id == "Cancel")
				gui.pixelInset = new Rect((Screen.width / 12) * 7, Screen.height / 24, width, height);

			if (id == "Info")
				gui.pixelInset = new Rect(Screen.width / 2 - width, Screen.height - height, width, height);

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