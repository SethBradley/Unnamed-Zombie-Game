using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UI_SkillTree : MonoBehaviour{
  
  public static bool GameIsPaused = false;
  public GameObject UI_skilltree;
  private PlayerSkills playerSkills; 



 
 private void Update() {
      if(Input.GetKeyDown(KeyCode.N))
      {
          if(GameIsPaused)
          {
                close();
          } else
          {
              Pause();
          }
      }
  }
 public void close ()
  {
    if (UI_skilltree != null)
    UI_skilltree.SetActive(false);
    Time.timeScale = 1f;
    GameIsPaused = false;
    FindObjectOfType<AudioManager>().Play("pauseclose", Camera.main.transform.position);
  }
 public void Pause ()
  {
    UI_skilltree.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;
    FindObjectOfType<AudioManager>().Play("pausesound", Camera.main.transform.position);
      
  }
  public void OpenSkillTree() 
  {
    if(UI_skilltree != null)
    {
      bool isActive = UI_skilltree.activeSelf;

      UI_skilltree.SetActive(!isActive);
    }
  }
  public void UnlockSkill()
  {
    playerSkills.UnlockSkill(PlayerSkills.SkillType.TurnHumans);
   Debug.Log("Click!");
  }

  public void SetPlayerSkills(PlayerSkills playerSkills) {
    this.playerSkills = playerSkills;
  }
}

