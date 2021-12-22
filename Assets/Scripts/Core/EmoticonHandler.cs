using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum EmoticonType
{
    Scream
}
public class EmoticonHandler : MonoBehaviour
{
    public Unit unit;
    public Vector2 emotionPosition;
    public GameObject activeEmoticon;
    public GameObject[] emoticons;

    private void Awake() 
    {
        unit = GetComponent<Unit>();
       // emotionPosition = transform.Find("Emoticon Slot").transform.localPosition;

    }

    public void InstantiateScreamEmoticon()
    {
        if(activeEmoticon != null)
        {
            Destroy(activeEmoticon);
            activeEmoticon = null;
        }
            
        activeEmoticon = Instantiate(emoticons[0], transform.position, transform.rotation);
        activeEmoticon.transform.parent = gameObject.transform;
        activeEmoticon.transform.localPosition = emotionPosition;
        
    }

    public void EndActiveEmoticon()
    {
        activeEmoticon.GetComponent<Animator>().SetBool("Exit", true);
        Destroy(activeEmoticon, 2f);
        activeEmoticon = null;
    }

}
