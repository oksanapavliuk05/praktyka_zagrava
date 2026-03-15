using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData{
    public bool[] isActive;
    public bool[] isFinished;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Load(); 
    }

    void Start()
    {
    }

    public void Save()
    {
        Debug.Log("Save called");

        BinaryFormatter formatter = new BinaryFormatter(); 
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create); 
        SaveData data = new SaveData(); 
        data = saveData; 
        formatter.Serialize(file, data); 
        file.Close(); 
        Debug.Log("Saved");
    }


    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
            saveData = binaryFormatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Loaded!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Save();
    }
}
