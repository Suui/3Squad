using UnityEngine;
using System.Collections;


namespace Medusa
{

	public class Server : MonoBehaviour
	{

		private string PlayerID;
		private string MatchID;


		void Awake()
		{
			DontDestroyOnLoad(this);
			RequestUserID();
		}


		private void RequestUserID()
		{
			var form = new WWWForm();

			form.AddField("name", "TestPlayer");
			form.AddField("numberOfPlayers", 2);

			var request = new WWW("178.62.230.225:80/api/ticket", form);
			StartCoroutine(RequestUserID(request));
		}


		private void RequestMatch()
		{
			var form = new WWWForm();

			form.AddField("playerId", PlayerID);

			var request = new WWW("178.62.230.225:80/api/match", form);
			StartCoroutine(RequestMatchID(request));
		}


		private void RequestCancel()
		{
			var form = new WWWForm();

			form.AddField("playerId", PlayerID);

			var request = new WWW("178.62.230.225:80/api/cancel", form);
			StartCoroutine(WaitForCancel(request));
		}


		IEnumerator RequestUserID(WWW request)
		{
			Debug.Log("Waiting for PlayerID");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("PlayerID OK: " + request.text);
				PlayerID = request.text;

				PlayerID = PlayerID.Remove(0, 1);
				PlayerID = PlayerID.Remove(PlayerID.Length - 1, 1);

				Debug.Log(PlayerID);

				RequestMatch();
			}
			else
				Debug.Log("PlayerID Error: " + request.error);
		}


		IEnumerator RequestMatchID(WWW request)
		{
			Debug.Log("Waiting for MatchID");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("MatchID OK: " + request.text);
				MatchID = request.text;
			}
			else
				Debug.Log("MatchID Error: " + request.error);
		}


		IEnumerator WaitForCancel(WWW request)
		{
			Debug.Log("Waiting for Cancel");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("Cancel OK: " + request.text);
				// TODO: Exit game;
			}
			else
				Debug.Log("Cancel Error: " + request.error);
		}

	}

}