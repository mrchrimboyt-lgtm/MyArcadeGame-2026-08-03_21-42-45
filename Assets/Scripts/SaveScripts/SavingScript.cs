using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingScript
{ //The save file scripts are from previous games I have made although I will like to thank brackeys Tutorial for teaching me the sytax of load and save systems(https://www.youtube.com/watch?v=XOjd_qU2Ido) 
    //does all the technical work of saving and loading
    public static void SaveTheData(SaveManager savemanager)
    {
        
        BinaryFormatter formatter = new BinaryFormatter();      //saves it as a binary file   
        string path = Application.persistentDataPath + "/SaveFile.GONK";      
        FileStream stream2 = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(savemanager);
        formatter.Serialize(stream2, data);
        stream2.Close();

    }
    public static SaveData LoadTheData()
    {
        string path = Application.persistentDataPath + "/SaveFile.GONK";
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        catch
        {
            Debug.Log("Cant find File");
            return (null);
        }



    }
}
