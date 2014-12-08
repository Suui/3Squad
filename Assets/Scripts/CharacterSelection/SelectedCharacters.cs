using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Medusa
{

	public class SelectedCharacters : MonoBehaviour
	{

		public List<string> selectedCharacters;


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

			selectedCharacters = new List<string>();
		}


		private void AddCharacter(string name)
		{
			if (name == "Empty") return;

			if (selectedCharacters.Contains(name) == false)
				selectedCharacters.Add(name);

			if (selectedCharacters.Count == 3)
				Application.LoadLevel("Scene_00");
		}


		private void RemoveCharacter(string name)
		{
			if (name == "Empty") return;

			if (selectedCharacters.Contains(name))
				selectedCharacters.Remove(name);
		}

	}

}
