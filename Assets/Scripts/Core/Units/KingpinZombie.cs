using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingpinZombie : LeaderZombie, ISelectable, IMovable
{
    [Header("Radius")]
    public float minionWanderRadius;
    public CircleCollider2D circleRadius;
    public List<NormalZombie> zombiesInCommandList = new List<NormalZombie>();
    private PlayerSkills playerSkills; // Teddy change 
     


    private void Awake() 
    {
        base.Awake();
        
        playerSkills = new PlayerSkills();// Teddy change 
        //stateMachine = new StateMachine(this);
        
    }
    public bool CanTurnHumans(){ // Teddy change 
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.TurnHumans);
    }    

    public PlayerSkills GetPlayerSkills()// Teddy Change
    {
        return playerSkills;
    }
    

    private void Start() 
    {
        
    }
}
