using System.Collections.Generic;
using UnityEngine;


namespace Medusa
{

	public class SelectedCharacters : MonoBehaviour
	{

		private List<GameObject> selectedCharacters;


		void OnEnable()
		{
			ClickableCharFrame.OnFrameClick += AddCharacter;
		}


		void OnDisable()
		{
			ClickableCharFrame.OnFrameClick -= AddCharacter;
		}


		void Awake()
		{
			DontDestroyOnLoad(this);

			selectedCharacters = new List<GameObject>();
		}


		private void AddCharacter(string name)
		{
			GameObject character = Instantiate(Resources.Load("Prefabs/" + name)) as GameObject;
			character.name = name;
		}


		private void RemoveCharacter(string name)
		{
			
		}

	}

}
