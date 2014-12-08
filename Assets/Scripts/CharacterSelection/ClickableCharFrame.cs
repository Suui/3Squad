using UnityEngine;


namespace Medusa
{
	public class ClickableCharFrame : MonoBehaviour
	{

		public delegate void FrameClick(string name);
		public static event FrameClick OnFrameClick;


		void OnMouseUp()
		{
			if (OnFrameClick != null)
				OnFrameClick(gameObject.name);
		}

	}
}
