using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DatabaseMaster : MonoBehaviour
{
    public static DatabaseMaster instance;
    public List<AudioClip> allAudioClips;

    private void Awake() 
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else 
            instance = this;    
    }


    public AudioClip GetSoundFXByName(string audioClipName)
    {
        return allAudioClips.FirstOrDefault(x => x.name == audioClipName);
    }

}
