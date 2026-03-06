using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parametr;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private float multiplier;

    public void SliderVolume(float _volume) => audioMixer.SetFloat(parametr, Mathf.Log10(_volume) * multiplier);


    public void LoadSlider(float _volume)
    {
        if (_volume >= 0.001f)
        {
            slider.value = _volume;
        }
    }
}
