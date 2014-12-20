using System.Collections.Generic;
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

			SpawnMasters();

	        SpawnCharacters();

            // We set the initial turn to the enemy after initialization because the initial player is
            // determined by the server. We are asking for the actual player in the WaitForTurn() Coroutine.
            SetTurnToEnemy();
            StartCoroutine(GameObject.Find("ChangeTurnManager").GetComponent<ChangeTurnManager>().WaitForTurn());
        }


        private void SetTurnToEnemy()
        {
            TurnManagement.ChangeTurn();
        }


        public void ChangeTurn(TurnActions turnActions)
        {
            GameObject.Find("ChangeTurnManager").GetComponent<ChangeTurnManager>().PerformTurnChangeActions(turnActions);

            if (OnChangingTurn != null)
                OnChangingTurn();
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

		    turnManagement = new TurnManagement(players[0], players[1]);

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


	    private void SpawnMasters()
	    {
			Server server = GameObject.Find("Server").GetComponent<Server>();

		    if (server.PlayerNumber == 1)
		    {
			    SpawnMaster(masterOne, players[0], MasterOnePos, "Master 01");	// Local master
			    SpawnMaster(masterTwo, players[1], MasterTwoPos, "Master 02");	// Remote master
		    }
		    else
		    {
				SpawnMaster(masterOne, players[0], MasterTwoPos, "Master 01");	// Local master
			    SpawnMaster(masterTwo, players[1], MasterOnePos, "Master 02");	// Remote master
		    }
	    }


	    private void SpawnMaster(GameObject masterPrefab, Player player, Position masterPos, string masterName)
	    {
			GameObject master = Instantiate(masterPrefab) as GameObject;
			master.name = masterName;
			master.GetComponent<PlayerComponent>().Player = player;

			if (masterPos == MasterTwoPos)
				master.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

			master.transform.position = masterPos;
			CurrentBoard["tokens"][masterPos] = master;
	    }


	    private void SpawnCharacters()
	    {
		    Server server = GameObject.Find("Server").GetComponent<Server>();
			SelectedCharacters selChar = GameObject.Find("SelectedCharacters").GetComponent<SelectedCharacters>();

			int row = 5, selfCol = 0, enemyCol = 0;

		    if (server.PlayerNumber == 1)
				enemyCol = CurrentBoard.Columns - 1;
		    else
			    selfCol = CurrentBoard.Columns - 1;

			SpawnCharacters(selChar.selectedCharacters, GetPlayerOne, row, selfCol);	// Local characters
			SpawnCharacters(server.EnemyCharacters, GetPlayerTwo, row, enemyCol);		// Remote characters
	    }


	    private void SpawnCharacters(List<string> characters, Player player, int row, int col)
	    {
			foreach (var name in characters)
			{
				GameObject go = Instantiate(Resources.Load("Prefabs/" + name)) as GameObject;
				go.name = name;
				go.transform.position = new Position(row, col);
				go.GetComponent<PlayerComponent>().Player = player;

				if (col != 0)
					go.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

				Position goPos = new Position(row, col);
				CurrentBoard["tokens"][goPos] = go;
				row -= 2;
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