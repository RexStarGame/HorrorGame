using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SaveData : MonoBehaviour
{
    [SerializeField] private UITimer _UITimer = new UITimer();
    string[] spillerenstid;
 
    public void dataSaver()
    {
        spillerenstid = new string[0];
        string time = JsonUtility.ToJson(_UITimer);
        
        System.IO.File.WriteAllText(Application.persistentDataPath + "/UITimer.json", time);
        Debug.Log(Application.persistentDataPath);

        for (int i = 0; i < spillerenstid.Length; i++) 
        {
            
        }
    }

}