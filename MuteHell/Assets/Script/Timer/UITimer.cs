using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UITimer : MonoBehaviour
{
    public TimerData timerData = new TimerData();
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
            timerData.TimeSurvived += Time.deltaTime;
            int hours = Mathf.FloorToInt(timerData.TimeSurvived / 3600f);
            int minutes = Mathf.FloorToInt((timerData.TimeSurvived % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(timerData.TimeSurvived % 60f);
            int milliseconds = Mathf.FloorToInt((timerData.TimeSurvived * 100f) % 100f);
            timerText.text = $"{hours:00}:{minutes:00}:{seconds:00}:{milliseconds:00}";

        }
    }

    private void OnApplicationQuit()
    {
        saveData.SaveTimer(timerData);
    }
    
}

