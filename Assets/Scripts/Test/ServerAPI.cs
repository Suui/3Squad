using UnityEngine;
using System.Collections;


namespace Medusa
{

	public class ServerAPI : MonoBehaviour
	{

		private string PlayerID;
		private string MatchID;


		void Start()
		{
			RequestUserID();
		}


		void Update()
		{

		}


		private void RequestUserID()
		{
			var form = new WWWForm();

			form.AddField("name", "TestPlayer");
			form.AddField("numberOfPlayers", 2);

			var userIDRequest = new WWW("178.62.230.225:80/api/ticket", form);
			StartCoroutine(RequestUserID(userIDRequest));
		}


		private void RequestMatch()
		{
			var form = new WWWForm();

			form.AddField("playerId", PlayerID);

			var matchIDRequest = new WWW("178.62.230.225:80/api/match", form);
			StartCoroutine(RquestMatchID(matchIDRequest));
		}


		IEnumerator RequestUserID(WWW userIDRequest)
		{
			Debug.Log("Waiting for PlayerID");
			yield return userIDRequest;

			if (userIDRequest.error == null)
			{
				Debug.Log("PlayerID OK: " + userIDRequest.text);
				PlayerID = userIDRequest.text;
				RequestMatch();
			}
			else
				Debug.Log("PlayerID Error: " + userIDRequest.error);
		}


		IEnumerator RquestMatchID(WWW matchIDRequest)
		{
			Debug.Log("Waiting for MatchID");
			yield return matchIDRequest;

			if (matchIDRequest.error == null)
			{
				Debug.Log("MatchID OK: " + matchIDRequest.text);
				MatchID = matchIDRequest.text;
			}
			else
				Debug.Log("MatchID Error: " + matchIDRequest.error);
		}

	}

}