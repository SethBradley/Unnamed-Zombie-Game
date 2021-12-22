using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanicMeter : MonoBehaviour
{
    public Slider slider;
    public float currentSliderValue;
    public float slideSpeed;
    AudioSource audioSource;
    public IEnumerator increasePanic_Co;

    private void Start() 
    {
        slider = GetComponent<Slider>();
        audioSource = GetComponent<AudioSource>();
        increasePanic_Co = null;
        EventBus.StartListening("IncreasePanic", Start_GradualSlideIncrease);
    }

    public void Start_GradualSlideIncrease()
    {
        
        if(increasePanic_Co == null)
        {
            currentSliderValue = slider.value;
            increasePanic_Co = GradualSlideIncrease();
            StartCoroutine(increasePanic_Co);
        }


    }
    public IEnumerator GradualSlideIncrease()
    {
        float time = 0;
        float duration = Mathf.Abs(currentSliderValue - GameplayHandler.instance.totalPanic)/50f;
        audioSource.Play();
        while(time < duration)
        {
            audioSource.pitch = 1f + (slider.value/100);
            slider.value = Mathf.Lerp(currentSliderValue, GameplayHandler.instance.totalPanic,time/duration);
            time += Time.deltaTime; // * slideSpeed;

            yield return null;
        }
        audioSource.Stop();
        increasePanic_Co = null;
    }
}
