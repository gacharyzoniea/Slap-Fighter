using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnableMenuScript : MonoBehaviour
{
    AudioSource m_AudioSource;
    public AudioClip m_Clip;
    public GameObject menu;
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(menu);
        if(m_AudioSource != null && m_Clip != null)
        {
            m_AudioSource.PlayOneShot(m_Clip, .7f);
        }
    }
}
