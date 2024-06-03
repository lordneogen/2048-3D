
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

enum Types
{
    HPup = 5000,
    UnFreezeOne = 1000,
    UnFreezeAll = 2500,
    Chance = 20,
}
//
[System.Serializable]
public class BoosterStore
{
    public int BuyTocken = 100;
    public int ScoreStart=100;
    public int Inc=2;
    //
    public int Max=20;
    public int Cur=1;
    public int Min=1;
    //
    public string Type;

    public BoosterStore(int scoreStart,int inc,int cur,int max,int min,string type,int buyTocken)
    {
        ScoreStart = scoreStart;
        Inc = inc;
        Cur = cur;
        Max = max;
        Min = min;
        Type = type;
        BuyTocken = buyTocken;
    }
    
    //
    
    public void Save()
    {
        SaveData(this);
    }
    
    //

    public BoosterStore Load()
    {
        BoosterStore boosterStore=LoadData(Type);
        if (boosterStore == null) return new BoosterStore(Types.HPup.ToString()., 2, 1, 20, 1, "Empty",100);
        else return boosterStore;
    }

    public bool NewLV()
    {
        if (Cur > Max) return false;
        Cur++;
        ScoreStart = ScoreStart - (int)Mathf.Max(1, ScoreStart /10);
        return true;
    }
    
    public static void SaveData(BoosterStore boosterStore)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = $"{Application.persistentDataPath}/{boosterStore.Type}.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        global::BoosterStore data=boosterStore;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Function to load the player's score
    public static BoosterStore LoadData(string Type)
    {
        string path = $"{Application.persistentDataPath}/{Type}.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            BoosterStore data = formatter.Deserialize(stream) as BoosterStore;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }
}