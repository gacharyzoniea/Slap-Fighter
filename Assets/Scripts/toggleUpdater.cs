using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class toggleUpdater : MonoBehaviour
{
    public GameObject toggle;
    public GameObject lava;
    public GameObject jungle;
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == toggle || EventSystem.current.currentSelectedGameObject == lava)
        {
            toggle.SetActive(true);
        }
        else
        {
            toggle.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(jungle);
        }
    }
}
