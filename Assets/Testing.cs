using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private KingpinZombie player;
    [SerializeField] private UI_SkillTree uI_SkillTree;

    private void Start() 
    {
        uI_SkillTree.SetPlayerSkills(player.GetPlayerSkills());
    }

}
