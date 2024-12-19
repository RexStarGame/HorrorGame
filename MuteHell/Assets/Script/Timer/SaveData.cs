using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    [SerializeField] private UITimer _UITimer = new UITimer();
    private List<TimerData> times = new List<TimerData>();

    public void SaveTimer(TimerData timer)
    {
        // Add the current UITimer instance to the list
        times.Add(timer);

        // Serialize the list to JSON
        string json = JsonUtility.ToJson(new TimerListWrapper { TimerList = times });

        // Save the JSON string to a file
        File.WriteAllText(Application.persistentDataPath + "/UITimer.json", json);

        Debug.Log($"Data saved to: {Application.persistentDataPath}/UITimer.json");
        
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/UITimer.json";

        if (File.Exists(filePath))
        {
            // Reads the file content
            string json = File.ReadAllText(filePath);

            // deserializes the json to a list
            TimerListWrapper wrapper = JsonUtility.FromJson<TimerListWrapper>(json);
            times = wrapper.TimerList;

            Debug.Log("Data loaded successfully");
        }
        else
        {
            Debug.LogWarning("No save file found");
        }
    }

    [System.Serializable]
    private class TimerListWrapper
    {
        public List<TimerData> TimerList;
    }

}