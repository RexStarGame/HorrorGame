using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITimer : MonoBehaviour
{
    public float TimeSurvived;
    public bool playing;
    public TMP_Text timerText;
    public SaveData saveData;

    // Update is called once per frame
    private void Start()
    {
        saveData.GetComponent<SaveData>();
        
           playing = true;
    }
    void Update()
    {
        if (playing == true) // er i live
        {
            TimeSurvived += Time.deltaTime;
            int hours = Mathf.FloorToInt(TimeSurvived / 60f);
            int minutes = Mathf.FloorToInt(TimeSurvived / 60f);
            int seconds = Mathf.FloorToInt(TimeSurvived % 60f);
            int milliseconds = Mathf.FloorToInt((TimeSurvived * 100f) % 100f);
            timerText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");

        }
    }

    private void OnApplicationQuit()
    {
        saveData.dataSaver();
    }

}

