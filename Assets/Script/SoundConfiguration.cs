using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundConfiguration : MonoBehaviour
{
    [SerializeField] protected Slider masterSlider;
    //[SerializeField] protected Slider effectsSlider;
    [SerializeField] protected Text musicText;
    //[SerializeField] protected Text effectsText;

    private void Start()
    {
        LoadVolume();
    }

    protected void VolumeValue(float volume)
    {
        musicText.text = (volume*100).ToString();
    }

    public void SaveVolume()
    {
        float volume = masterSlider.value;
        PlayerPrefs.SetFloat("masterVolume", volume);
        AudioListener.volume = volume;
        //Debug.Log(volume);
        //VolumeValue(volume);
    }

    public void UpdateGUI()
    {
        float volume = masterSlider.value;
        VolumeValue(volume);
    }

    protected void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat("masterVolume");
        masterSlider.value = volume;
        VolumeValue(volume);
        AudioListener.volume = volume;
    }

}
