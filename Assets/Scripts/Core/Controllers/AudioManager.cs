using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
   public Sound[] sounds;

   void Awake() {
      foreach (Sound s in sounds)
      {
         s.source = gameObject.AddComponent<AudioSource>();
         s.source.clip = s.clip;

         s.source.volume = s.volume;
         s.source.pitch = s.pitch;
      }
   }

   public void Play (AudioClip sound, Vector3 unitPos) {
      {
        
        AudioSource.PlayClipAtPoint(sound, unitPos);
      }
   }

}
