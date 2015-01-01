using UnityEngine;


public class BaseInfo : MonoBehaviour{

	public static Vector3 position = new Vector3(0.1f, 0.9f, 0.1f);
	public static Vector3 positionText = new Vector3(0.2f, 0.9f, 0.1f);
	public string name;


	public void ShowUpInfo()
	{
		GameObject go = new GameObject();
		go.AddComponent<GUIText>();
		go.tag = "SkillIcon";
		go.transform.position = positionText;

		if (GetComponentInParent<Life>() != null)
			go.guiText.text = GetComponentInParent<Life>().currentLife + " / " + GetComponentInParent<Life>().maximumLife;

		go.guiText.fontSize = 30;
		go.transform.parent = gameObject.transform;

		GameObject skillGUI = Instantiate(Resources.Load("Prefabs/Skill_Template")) as GameObject;
		skillGUI.GetComponent<GUITexture>().texture = Resources.Load("Textures/CharacterFrame/" + name + "Frame") as Texture2D;
		skillGUI.transform.position = position;
		skillGUI.transform.parent = gameObject.transform;
	}
}
