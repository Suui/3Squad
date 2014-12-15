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

			var request = new WWW("178.62.230.225:80/api/ticket", form);
			StartCoroutine(RequestUserID(request));
		}


		private void RequestMatch()
		{
			var form = new WWWForm();

			form.AddField("superFail", PlayerID);

			var request = new WWW("178.62.230.225:80/api/match", form);
			StartCoroutine(RquestMatchID(request));
		}


		IEnumerator RequestUserID(WWW request)
		{
			Debug.Log("Waiting for PlayerID");
			yield return request;

			if (request.error == null)
			{
				Debug.Log("PlayerID OK: " + request.text);
				PlayerID = request.text;
				RequestMatch();
			}
			else
				Debug.Log("PlayerID Error: " + request.error);
		}


		IEnumerator RquestMatchID(WWW request)
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

	}

}