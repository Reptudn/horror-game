using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenuManager : MonoBehaviour
{

    [Header("Player Settings")]
    public PlayerController playerController;
    public GameObject playerPrefab;
    public Slider slider;
    public TextMeshProUGUI senseValue;


    [Header("Volume Components")]
    public AudioMixer audioMixer;
    public TextMeshProUGUI volumeValue;


    void Start(){

        //playerController = playerPrefab.GetComponent<PlayerController>();

        slider.value = playerController.sensitivity;
        senseValue.SetText(playerController.sensitivity.ToString());

        float value = 0;
        if(audioMixer.GetFloat("GameAudio", out float _value)) value = _value;
        volumeValue.SetText(value.ToString());
    }

    void Update(){
        
    }

    public GameObject[] mainMenuComp;
    public void MainMenuBack(){
        foreach(var o in mainMenuComp) o.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    public void SetVolume(float volume){

        audioMixer.SetFloat("GameAudio", volume);
        volumeValue.SetText(volume.ToString());

    }

    public void SetMouseSense(float sense){
        playerController.sensitivity = sense;
        senseValue.SetText(playerController.sensitivity.ToString());
    }

    public void SetQuality(int index){
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullscreen(bool fullscreen){
        Screen.fullScreen = fullscreen;
    }

    

}   
