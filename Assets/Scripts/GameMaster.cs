using UnityEngine;


namespace Medusa
{

    public class GameMaster : MonoBehaviour
    {

		public delegate void ResetSM();
		public static event ResetSM OnChangingTurn;

        public delegate void PreviousTurnEvents(ClickInfo clickInfo);
        public static event PreviousTurnEvents OnPreviousEvents;


        void OnEnable()
        {
			ExitEndTurnState.OnChangingTurn += ChangeTurn;
		}


        void OnDisable()
		{
			ExitEndTurnState.OnChangingTurn -= ChangeTurn;
        }


	    #region Variables

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

	    #endregion


        void Awake()
        {
	        BoardSetup();
	        
			PlayersSetup();

			ButtonsSetup();

			SpawnSelfMaster();

	        SpawnSelfCharacters();
        }


	    #region Setup Functions

	    private void BoardSetup()
	    {
		    boardGenerator = new BoardGenerator(boardCellPrefab, boardRows, boardColumns);
		    boardGenerator.CreateEmptyBoard(boardYSize);
		    boardGenerator.SpawnObstacles(obstaclePrefabs, obstaclesLimit, seed);
	    }


	    private void PlayersSetup()
	    {
		    Player playerOne = new Player("Player 01", startingActionPoints);
		    Player playerTwo = new Player("Player 02", startingActionPoints);
		    players = new[] {playerOne, playerTwo};

		    turnManagement = new TurnManagement(players[0], players[1], seed);

		    GameObject player = new GameObject(turnManagement.CurrentPlayer.name) {tag = turnManagement.CurrentPlayer.name};

		    transform.parent = player.transform;
		    GameObject.Find("BoardNode").transform.parent = player.transform;
	    }


	    private void ButtonsSetup()
	    {
		    GameObject parentObject = new GameObject("Buttons");

		    // Exit and EndTurn buttons
		    CreateButton("Textures/Buttons/ExitButton", parentObject, "ExitButton", "Exit");
		    CreateButton("Textures/Buttons/EndTurnButton", parentObject, "EndTurnButton", "EndTurn");

		    // Confirm and Cancel skillName buttons
		    CreateButton("Textures/Buttons/ConfirmButton", parentObject, "ConfirmCancel", "Confirm");
		    CreateButton("Textures/Buttons/CancelButton", parentObject, "ConfirmCancel", "Cancel");

		    // Info button
		    CreateButton("Textures/Buttons/InfoButton", parentObject, "InfoButton", "Info");

		    // Hide the Confirm and Cancel skillName and the Info buttons at first
		    foreach (var go in GameObject.FindGameObjectsWithTag("ConfirmCancel"))
			    go.GetComponent<GUITexture>().enabled = false;

		    GameObject.FindGameObjectWithTag("InfoButton").GetComponent<GUITexture>().enabled = false;
	    }


	    private void SpawnSelfMaster()
	    {
		    GameObject master1 = Instantiate(masterOne) as GameObject;
		    master1.name = "Master 01";
		    master1.transform.position = MasterOnePos;
		    master1.GetComponent<PlayerComponent>().Player = players[0];

		    CurrentBoard["tokens"][MasterOnePos] = master1;
	    }


	    private void SpawnSelfCharacters()
	    {
			SelectedCharacters characters = GameObject.Find("SelectedCharacters").GetComponent<SelectedCharacters>();
			int pos = 5;

			foreach (var name in characters.selectedCharacters)
			{
				GameObject go = Instantiate(Resources.Load("Prefabs/" + name)) as GameObject;
				go.name = name;
				go.transform.position = new Position(pos, 0);
				go.GetComponent<PlayerComponent>().Player = GetPlayerOne;

				CurrentBoard["tokens"][new Position(pos, 0)] = go;

				pos -= 2;
			}
	    }

	    #endregion


	    #region CreateButtons Function

	    public static void CreateButton(string texturePath, GameObject parentObject, string tag, string id)
	    {
		    GameObject button = Instantiate(Resources.Load("Prefabs/Button_Template")) as GameObject;
		    button.GetComponent<ClickableButton>().Id = id;
		    button.tag = tag;
		    button.name = id + " Button";
		    button.transform.parent = parentObject.transform;

		    button.transform.position = new Vector3(0.0f, 0.0f, 0.1f);

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
			    gui.pixelInset = new Rect(Screen.width / 2 - width / 2, Screen.height - height, width, height);
	    }

	    #endregion


		public void ChangeTurn(TurnActions turnActions)
		{
			GameObject.Find("ChangeTurnManager").GetComponent<ChangeTurnManager>().PerformTurnChangeActions(turnManagement.EnemyPlayerThisTurn, turnActions);

			if (OnChangingTurn != null)
				OnChangingTurn();
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