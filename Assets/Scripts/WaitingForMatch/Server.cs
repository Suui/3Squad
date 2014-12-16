using SimpleJSON;
using UnityEngine;
using System.Collections;


namespace Medusa
{

	public class Server : MonoBehaviour
	{

		private string playerID;
		private string matchID;


		void Awake()
		{
			DontDestroyOnLoad(this);
			WaitForUserID();
		}


		private void WaitForUserID()
		{
			var form = new WWWForm();

			form.AddField("name", "TestPlayer");
			form.AddField("numberOfPlayers", 2);

			var request = new WWW("178.62.230.225:80/api/ticket", form);
			StartCoroutine(WaitForUserID(request));
		}


		private void RequestMatch()
		{
			var form = new WWWForm();

			form.AddField("playerId", playerID);

			var request = new WWW("178.62.230.225:80/api/match", form);
			StartCoroutine(WaitForMatchID(request));
		}


		private void RequestCancel()
		{
			var form = new WWWForm();

			form.AddField("playerId", playerID);

			var request = new WWW("178.62.230.225:80/api/cancel", form);
			StartCoroutine(WaitForCancel(request));
		}


		private void RequestWait()
		{
			var form = new WWWForm();

			form.AddField("matchId", matchID);
			form.AddField("playerId", playerID);

			var request = new WWW("178.62.230.225:80/api/wait", form);
			StartCoroutine(WaitForWait(request));
		}


		private void RequestSubmit(JSONNode turnJSON)	// OR turnString?
		{
			var form = new WWWForm();

			form.AddField("matchId", matchID);
			form.AddField("playerId", playerID);
			form.AddField("turn", turnJSON);

			var request = new WWW("178.62.230.225:80/api/submit", form);
			StartCoroutine(WaitForSubmit(request));
		}


		private void RequestPlayers()
		{
			var form = new WWWForm();

			form.AddField("matchId", matchID);
			form.AddField("playerId", playerID);

			var request = new WWW("178.62.230.225:80/api/players", form);
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
				Debug.Log("playerID = " + playerID);

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
				Debug.Log("matchID = " + matchID);

				RequestPlayers();
			}
			else
				Debug.Log("matchID Error: " + request.error);
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
			{
				Debug.Log("Wait OK: " + request.text);
				// TODO: Returns the turns, next, and activePlayers
			}
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
				// TODO: Set the player to the other one so we can't play this turn
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

			PlayerNumber = players[0]["enemy"].AsBool == false ? 1 : 2;

			RequestWait();

			//Application.LoadLevel("Scene_00");
		}


		public void SubmitCharactersJSON(JSONNode charactersJSON)
		{
			
		}


		public int PlayerNumber { get; private set; }

	}

}