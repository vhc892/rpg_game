using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private GameObject setting;

    private bool isSettingsVisible = false;

    private void Start()
    {
        SetMusicVolume();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isSettingsVisible = !isSettingsVisible;
            setting.SetActive(isSettingsVisible);
        }
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume)*20);
    }
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }
}
