using System.Collections.Generic;
using UnityEngine;


namespace Medusa
{

	public class Display : MonoBehaviour
	{

		public int charactersDisplayedPerPage = 16;
		public List<GameObject> selectedCharacters;

		private int sizeRatio;
		private List<string> availableCharacters;


		void Awake()
		{
			CalculateRatio();
			SetUpAvailableCharacters();

			DisplayUI();
		}


		private void SetUpAvailableCharacters()
		{
			availableCharacters = new List<string> {"Fox", "Frog", "Ram", "Eagle", "Turtle"};
		}


		private void CalculateRatio()
		{
			sizeRatio = 1920 / Screen.width;
		}


		private void DisplayUI()
		{
			int textureSize = Screen.width / 8;
			float horizontalPos = (Screen.width % 8) / 2;
			float verticalPos = textureSize;

			foreach (var name in availableCharacters)
			{
				GameObject frame = Instantiate(Resources.Load("Prefabs/SelectionSceneFrame")) as GameObject;
				frame.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
				frame.name = name;

				GUITexture gui = frame.GetComponent<GUITexture>();
				gui.texture = Resources.Load("Textures/SelectionScene/" + name + "Sel") as Texture2D;

				gui.pixelInset = new Rect(horizontalPos, verticalPos, textureSize, textureSize);

				horizontalPos += textureSize;
			}

			for (int i = availableCharacters.Count; i < charactersDisplayedPerPage; i++)
			{
				if (i == charactersDisplayedPerPage / 2)
				{
					horizontalPos = (Screen.width % 8) / 2;
					verticalPos = 0.0f;
				}

				GameObject frame = Instantiate(Resources.Load("Prefabs/SelectionSceneFrame")) as GameObject;
				frame.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
				frame.name = "Empty";

				GUITexture gui = frame.GetComponent<GUITexture>();
				gui.texture = Resources.Load("Textures/SelectionScene/" + "Empty" + "Sel") as Texture2D;

				gui.pixelInset = new Rect(horizontalPos, verticalPos, textureSize, textureSize);

				horizontalPos += textureSize;
			}

		}
	}

}
