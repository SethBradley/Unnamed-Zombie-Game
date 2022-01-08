using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
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
    public float elapsedPhoneCallTime;

    private void Awake() 
    {
        unit = GetComponent<Unit>();
       // emotionPosition = transform.Find("Emoticon Slot").transform.localPosition;

    }

    public void InstantiateScreamEmoticon()
    {
        if(activeEmoticon != null)
            ExitActiveEmoticon();
        
            
        activeEmoticon = Instantiate(emoticons[0], transform.position, transform.rotation);
        activeEmoticon.transform.parent = gameObject.transform;
        activeEmoticon.transform.localPosition = emotionPosition;
        
    }
    public void InstantiatePhoneCallEmote()
    {
        if(activeEmoticon != null)
            ExitActiveEmoticon();
        
        
        
        activeEmoticon = Instantiate(emoticons[1], transform.position, transform.rotation);
        
        activeEmoticon.transform.parent = gameObject.transform;
        activeEmoticon.transform.localPosition = new Vector2(0f,1.6f );
        StartCoroutine(PlayPhoneCallSounds());
        StartCoroutine(IncreaseCallFill()); 
    }

    public void ExitActiveEmoticon()
    {
        Animator emoteAnim = activeEmoticon.GetComponent<Animator>();

        emoteAnim.SetBool("Exit", true);
        Destroy(activeEmoticon, emoteAnim.GetCurrentAnimatorStateInfo(0).length);
        activeEmoticon = null;
    }

    public IEnumerator PlayPhoneCallSounds()
    {
        unit.audioSource.clip = DatabaseMaster.instance.GetSoundFXByName("PhoneDialToneSpeedUp");
        unit.audioSource.Play();
        
        yield return new WaitForSeconds(2f);

        unit.audioSource.clip = DatabaseMaster.instance.GetSoundFXByName("PhoneRingOutTone");
        unit.audioSource.Play();

        yield return new WaitForSeconds(3f);
        
        activeEmoticon.GetComponent<Animator>().SetBool("Exit", true);
        unit.audioSource.clip = DatabaseMaster.instance.GetSoundFXByName("PhoneGibberish");
        unit.audioSource.Play();

    }

    public IEnumerator IncreaseCallFill()
    {
        Slider slider = activeEmoticon.transform.Find("bars outline").GetComponent<Slider>();
        elapsedPhoneCallTime = 0;
        float duration = 6.2f;
        while(elapsedPhoneCallTime <= duration )
        {
            elapsedPhoneCallTime += Time.deltaTime; 
            slider.value = elapsedPhoneCallTime;
            yield return null;
        }
    }
}
