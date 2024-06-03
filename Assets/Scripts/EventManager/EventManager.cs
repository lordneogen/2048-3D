using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
// using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EventManager : MonoBehaviour
{
    //
    public static EventManager Instance = null;
    public EffectSystem EffectSystem;
    public AudioManager audioManager;
    public bool doubleClick;
    public Action<bool> doubleClickCheck;
    //
    public GameObject BlockPref;
    public GameObject BlockEmptyPref;
    public int Size;
    public float SizeBox;
    public Action<int> ScoreEnc;
    public int SpawnRate;
    public int StopRateBase;
    public List<ISystem> Systems;
    //
#if UNITY_EDITOR
    public int SizeSystem;
    #endif
    //
    public int HP = 3; 
    [HideInInspector]
    public int HPCur=3;
    public int Score = 0;
    public int BonusesCount = 0;
    public int HighScore;
    public bool new_record=false;
    public float Speed;
    //
    public float EffectRate = 1f;
    //
    [SerializeField]
    private GameObject PauseUI;
    [SerializeField]
    private GameObject SettingUI;
    //
    [HideInInspector]
    public RotateObjectWithMouse RotateObjectWithMouse;
    private float rotate;

    private void Start()
    {
        StopAllUI();
    }
    
    

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        HPCur = HP;
        try
        {
            doubleClick = LoadScore().doubleClick;
            HighScore = LoadScore().highscore;
            rotate = LoadScore().touch_sensitivity;
            RotateObjectWithMouse.rotationSpeedMobile = LoadScore().touch_sensitivity;
        }
        catch
        {
            
        }

        Systems = new List<ISystem>();
        Size += 2;
        //
        if (Instance == null)
        {
            // ScoreEnc?.Invoke(0);
            Instance = this;
        } else if(Instance == this){
            Destroy(gameObject);
        }
        // DontDestroyOnLoad(gameObject);
        //
    }

    private void Update()
    {
#if UNITY_EDITOR
        SizeSystem = Systems.Count;
        #endif
    }

    public void ContinueAll()
    {
        if (HPCur > 0)
        {
            Debug.Log("Continue");
            foreach (var _system in Systems)
            {
                _system.Continue();
            }
        }
    }
    
    
    public void RestartAll()
    {
        Debug.Log("Restart");
        foreach (var _system in Systems)
        {
            _system.Restart();;
        }
    }

    public void ChangeRotateObjectWithMouse(float data)
    {
        rotate = data;
        // Debug.Log(data);
        try
        {
            RotateObjectWithMouse.rotationSpeedMobile = data;
        }
        catch
        {

        }

        SaveScore(HighScore,data,doubleClick);
    }

    public void GameOverAll()
    {
        Debug.Log("GameOver");
        //
        SaveScore(HighScore,RotateObjectWithMouse.rotationSpeedMobile,doubleClick);
        audioManager.GameOver();
        //
        foreach (var _system in Systems)
        {
            _system.GameOver();
        }
    }
    
    //
    
    // Function to save the player's score
    
#if UNITY_EDITOR

    [ContextMenu("Save")]
    public void Save()
    {
        SaveScore(HighScore,RotateObjectWithMouse.rotationSpeedMobile,doubleClick);
    }
    
    [ContextMenu("Load")]

    public void Load()
    {
        HighScore = LoadScore().highscore;
        RotateObjectWithMouse.rotationSpeedMobile = LoadScore().touch_sensitivity;
        doubleClick = LoadScore().doubleClick;
    }
    
    #endif
    public static void SaveScore(int score,float RotateObjectWithMouse,bool doubleClick)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player1.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(score,RotateObjectWithMouse,doubleClick);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Function to load the player's score
    public static PlayerData LoadScore()
    {
        string path = Application.persistentDataPath + "/player1.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return new PlayerData(0,1f,false);
        }
    }

    public void StopAllUI()
    { 
        PauseUI.SetActive(false);
        SettingUI.SetActive(false);
        try
        {
            SaveScore(HighScore,RotateObjectWithMouse.rotationSpeedMobile,doubleClick);
            audioManager.UIoff();
        }
        catch
        {
            
        }
        Time.timeScale = 1f;
    }
    public void Pause()
    {
        StopAllUI();
        audioManager.UIon();
        Time.timeScale = 0f;
        PauseUI.SetActive(true);
    }

    public void Setting()
    {
        StopAllUI();
        audioManager.UIon();
        Time.timeScale = 0f;
        SettingUI.SetActive(true);
    }

    public void SetDoubleClick(bool data)
    {
        doubleClick = data;
        doubleClickCheck?.Invoke(data);
        SaveScore(HighScore,rotate,doubleClick);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Rotate()
    {
        RotateObjectWithMouse.Rotate();
    }
    //
    
}

[System.Serializable]
public class PlayerData
{
    public int highscore;
    public float touch_sensitivity;
    public bool doubleClick;
    public PlayerData(int highscore,float touch_sensitivity,bool doubleClick)
    {
        this.doubleClick = doubleClick;
        this.highscore = highscore;
        this.touch_sensitivity = touch_sensitivity;
    }
}