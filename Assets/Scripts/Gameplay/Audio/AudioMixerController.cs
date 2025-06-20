
using UnityEngine;
using UnityEngine.Audio;
public class AudioMixerController : MonoBehaviour
{
    public static AudioMixerController instance;

    [SerializeField] AudioMixer audioMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetMasterVolume(float value)
    {
        if (value == 0f)
        {
            audioMixer.SetFloat("masterVolume", -80);
        }
        else
            audioMixer.SetFloat("masterVolume", Mathf.Log10(value) * 20f); //found this function on the internet, works better then just sending the pure value into the mixer
    }
    public void SetSFXVolume(float value)
    {
        if (value == 0f)
        {
            audioMixer.SetFloat("sfxVolume", -80);
        }
        else
            audioMixer.SetFloat("sfxVolume", Mathf.Log10(value) * 20f);

    }
    public void SetMusicVolume(float value)
    {
        if (value == 0f)
        {
            audioMixer.SetFloat("musicVolume", -80);
        }
        else
            audioMixer.SetFloat("musicVolume", Mathf.Log10(value) * 20f);
    }

    //the reason we have 2 of the same function is because we want to use a slider (returns a float value) and a input field (returns a string value) as possible inputs for volume
    public void SetMasterVolume(string value)
    {
        if (float.Parse(value) == 0f)
        {
            audioMixer.SetFloat("masterVolume", -80);
        }
        else
            audioMixer.SetFloat("masterVolume", Mathf.Log10(float.Parse(value)) * 20f);
    }
    public void SetSFXVolume(string value)
    {
        if (float.Parse(value) == 0f)
        {
            audioMixer.SetFloat("sfxVolume", -80);
        }
        else
            audioMixer.SetFloat("sfxVolume", Mathf.Log10(float.Parse(value)) * 20f);
    }
    public void SetMusicVolume(string value)
    {

        if (float.Parse(value) == 0f)
        {
            audioMixer.SetFloat("musicVolume", -80);
        }
        else
            audioMixer.SetFloat("musicVolume", Mathf.Log10(float.Parse(value)) * 20f);
    }

    

    public float GetMasterVolume()
    {
        float output;
        audioMixer.GetFloat("masterVolume", out output);
        return output;
    }
    public float GetSfxVolume()
    {
        float output;
        audioMixer.GetFloat("sfxVolume", out output);
        return output;
    }
    public float GetMusicVolume()
    {
        float output;
        audioMixer.GetFloat("musicVolume", out output);
        return output;
    }
}
