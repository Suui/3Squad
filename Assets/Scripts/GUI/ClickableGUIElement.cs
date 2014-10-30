using UnityEngine;


public class ClickableGUIElement : MonoBehaviour
{

    public delegate void ClickAction(GameObject guiElement);
    public static event ClickAction OnClick;


    void OnMouseUp()
    {
        Debug.Log("Pushed!");

        if (OnClick != null)
            OnClick(gameObject);
    }

}
