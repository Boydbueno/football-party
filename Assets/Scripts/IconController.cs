using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IconController : MonoBehaviour
{
    public float ShowTime = 1.5f;

    private Image _icon;
	// Use this for initialization
	void Start()
	{
	    _icon = GetComponent<Image>();
	}

    public void Show()
    {
        _icon.enabled = true;
        Invoke("Hide", ShowTime);
    }

    public void Hide()
    {
        _icon.enabled = false;
    }
}
