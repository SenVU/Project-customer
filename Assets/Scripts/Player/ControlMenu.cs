using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    [SerializeField] private RawImage  controlImage;
    private bool isMenuVisible = false;

    private void Start()
    {
        controlImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleControlPanelMenu();
        }
    }

    public void ToggleControlPanelMenu()
    {
        isMenuVisible = !isMenuVisible;
        controlImage.gameObject.SetActive(isMenuVisible);
    }
}
