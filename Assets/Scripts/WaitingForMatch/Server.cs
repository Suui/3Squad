using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using System.Collections;


namespace Medusa
{

	public class Server : MonoBehaviour
	{

		private string playerID;
		private string matchID;
		private const string SERVER_URL = "178.62.230.225:80/api/";


		void Awake()
		{
			DontDestroyOnLoad(this);

			EnemyCharacters = new List<string>();
		    MyTurn = false;

			RequestUserID();
		}


		private void RequestUserID()
		{
            SelectedCharacters characters = GameObject.Find("SelectedCharacters").GetComponent<SelectedCharacters>();
            JSONNode charactersJSON = new JSONClass();
			int c = 0;

		    foreach (var characterName in characters.selectedCharacters)
		    {
				charactersJSON["characters"][c] = characterName;
			    c++;
		    }

			Debug.Log(charactersJSON.ToString());

			var form = new WWWForm();

			form.AddField("name", "TestPlayer");
			form.AddField("numberOfPlayers", 2);
			form.AddField("setup", charactersJSON.ToString());

			var request = new WWW(SERVER_URL + "ticket", form);
			StartCoroutine(WaitForUserID(request));
		}


		private void RequestMatch()
		{
			var form = new WWWForm();

			form.AddField("playerId", playerID);

			var request = new WWW(SERVER_URL + "match", form);
			StartCoroutine(WaitForMatchID(request));
		}


		private void RequestCancel()
		{
			var form = new WWWForm();

			form.AddField("playerId", playerID);

			var request = new WWW(SERVER_URL + "cancel", form);
			StartCoroutine(WaitForCancel(request));
		}


		public void RequestWait()
		{
			var form = new WWWForm();

			form.AddField("matchId", matchID);
			form.AddField("playerId", playerID);
			Debug.Log(playerID + "is waiting for its turn");

			var request = new WWW(SERVER_URL + "wait", form);
			StartCoroutine(WaitForWait(request));
		}


		public void RequestSubmit(JSONNode turnJSON)
		{
			var form = new WWWForm();

			form.AddField("matchId", matchID);
			form.AddField("playerId", playerID);

			if (turnJSON != null)
			{
				form.AddField("turn", turnJSON.ToString());
				Debug.Log("Form JSON: " + turnJSON);
				Debug.Log("Form JSON ToString(): " + turnJSON.ToString());
			}

			var request = new WWW(SERVER_URL + "submit", form);
			StartCoroutine(WaitForSubmit(request));
		}


		private void RequestPlayers()
		{
			var form = new WWWForm();

			form.AddField("matchId", matchID);
			form.AddField("playerId", playerID);

			var request = new WWW(SERVER_URL + "players", form);
			StartCoroutine(WaitForPlayers(request));
		}


		IEnumerator WaitForUserID(WWW request)
		{
			Debug.Log("Waiting for playerID");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("playerID OK: " + request.text);
				playerID = request.text;

				playerID = playerID.Substring(1, playerID.Length - 2);

				RequestMatch();
			}
			else
				Debug.Log("playerID Error: " + request.error);
		}


		IEnumerator WaitForMatchID(WWW request)
		{
			Debug.Log("Waiting for matchID");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("matchID OK: " + request.text);
				matchID = request.text;

				matchID = matchID.Substring(1, matchID.Length - 2);

				RequestPlayers();
			}
			else
			{
				Debug.Log("matchID Error: " + request.error);
				yield return new WaitForSeconds(0.5f);
				RequestMatch();
			}
		}


		IEnumerator WaitForCancel(WWW request)
		{
			Debug.Log("Waiting for Cancel");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("Cancel OK: " + request.text);
				// TODO: Exit game, delete playerId from DB;
			}
			else
				Debug.Log("Cancel Error: " + request.error);
		}


		IEnumerator WaitForWait(WWW request)
		{
			Debug.Log("Waiting for Wait");
			yield return request;

			if (request.error == null)
                ParseWaitJSON(request.text);
			else
				Debug.Log("Wait Error: " + request.error);
		}


	    IEnumerator WaitForSubmit(WWW request)
		{
			Debug.Log("Waiting for Submit");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("Submit OK: " + request.text);
				MyTurn = false;
				StartCoroutine(GameObject.Find("ChangeTurnManager").GetComponent<ChangeTurnManager>().WaitForTurn());
			}
			else
				Debug.Log("Submit Error: " + request.error);
		}


		IEnumerator WaitForPlayers(WWW request)
		{
			Debug.Log("Waiting for Players");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("Players OK: " + request.text);
				ProcessPlayerNumber(request.text);
			}
			else
				Debug.Log("Players Error: " + request.error);
		}


	    private void ProcessPlayerNumber(string playersInfo)
		{
			JSONNode players = JSON.Parse(playersInfo);

		    for (int i = 0; i < players.AsArray.Count; i++)
		    {
			    if (players[i]["enemy"].AsBool == false)
				    PlayerNumber = i + 1;
			    else
			    {
				    JSONNode setup = JSON.Parse(players[i]["setup"]);
				    for (int j = 0; j < setup["characters"].AsArray.Count; j++)
					    EnemyCharacters.Add(setup["characters"][j]);
			    }
		    }

			Application.LoadLevel("Scene_00");
		}


        private void ParseWaitJSON(string text)
        {
            JSONNode json = JSON.Parse(text);

            if (json["next"].AsBool == true)
                MyTurn = true;
            else
                return;

	        Debug.Log("Wait OK, text: " + text);
            Debug.Log("Wait OK, turns JSON: " + json.ToString());
	        PerformReceivedActions(json);
        }


		private void PerformReceivedActions(JSONNode json)
		{
			if (json["turns"].AsArray.Count == 0)
			{
				GameObject.Find("GameMaster").GetComponent<GameMaster>().TurnManagement.ChangeTurn();
				return;
			}

			JSONNode actionsJSON = JSON.Parse(json["turns"][0]);
			Debug.Log("Actions JSON: " + actionsJSON.ToString());

			TurnActions actions = new TurnActions(actionsJSON);
			GameObject.Find("ChangeTurnManager").GetComponent<ChangeTurnManager>().PerformTurnChangeActions(actions);
		}


		public int PlayerNumber { get; private set; }

		public List<string> EnemyCharacters { get; private set; }

	    public bool MyTurn { get; set; }

	}

}