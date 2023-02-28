using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsStorageScript : MonoBehaviour {
    private byte musicVolume;
    private byte soundEffectsVolume;
    private byte masterVolume;
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(transform.gameObject); // prevents object destruction between scenes
	}
	public void setMusicVolume (float incomingMusicVolume)
    {
        musicVolume = (byte)(incomingMusicVolume);
        Debug.Log(musicVolume);
    }
    public int getMusicVolume()
    {
        return musicVolume;
    }
    public void setEffectVolume(float incomingEffectVolume)
    {
        soundEffectsVolume = (byte)(incomingEffectVolume);
        Debug.Log(soundEffectsVolume);
    }
    public int getEffectVolume()
    {
        return soundEffectsVolume;
    }
    public void setMasterVolume(float incomingMasterVolume)
    {
        masterVolume = (byte)(incomingMasterVolume);
        Debug.Log(masterVolume);
    }
    public int getMasterVolume()
    {
        return masterVolume;
    }
}
