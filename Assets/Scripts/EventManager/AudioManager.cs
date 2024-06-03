using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager:MonoBehaviour,ISystem
{
    [FormerlySerializedAs("BackGroundMusicSouse")] [FormerlySerializedAs("mainBackGroundMusic")] public AudioSource BackGroundMusicSourse;
    [FormerlySerializedAs("mainMainMusic")] public AudioSource mainMainMusicSourse;
    [FormerlySerializedAs("mainBackmusic")] public AudioClip mainBackGroundmusic;
    public AudioClip FrezeClip;
    public AudioClip ScoreUPClip;
    public AudioClip HighScoreClip;
    public AudioClip ButtonClip;
    public AudioClip GameOverClip;
    public float Backvol;
    public float Mainvol;

    private void Start()
    {
        BackGroundMusicSourse.clip = mainBackGroundmusic;
        BackGroundMusicSourse = GetComponent<AudioSource>();
        AudioData audioData = AudioManager.Load();
        Backvol = audioData.Backvol;
        Mainvol=audioData.Mainvol;
        BackGroundMusicSourse.volume = audioData.Backvol;
        mainMainMusicSourse.volume = audioData.Mainvol;
        BackGroundMusicSourse.Play();
    }

    public void ChangeBackGround(float data)
    {
        Backvol = data;
        BackGroundMusicSourse.volume= Backvol;
        AudioManager.Save(Mainvol,Backvol);
    }
    
    public void ChangeMain(float data)
    {
        Mainvol = data;
        mainMainMusicSourse.volume= Mainvol;
        AudioManager.Save(Mainvol,Backvol);
    }

    public void Play(AudioClip audioClip)
    {
        mainMainMusicSourse.PlayOneShot(audioClip);
    }

    public void UIon()
    {
        BackGroundMusicSourse.spatialBlend = 0.5f;
    }

    public void UIoff()
    {
        BackGroundMusicSourse.spatialBlend = 0f;
    }

    public void Continue()
    {
        return;
    }

    public void GameOver()
    {
        BackGroundMusicSourse.PlayOneShot(GameOverClip);
    }

    public void Restart()
    {
        return;
    }
    
    public static void Save(float main,float back)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/audio.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        AudioData data = new AudioData(main,back);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Function to load the player's score
    public static AudioData Load()
    {
        string path = Application.persistentDataPath + "/audio.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AudioData data = formatter.Deserialize(stream) as AudioData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return new AudioData(1,1);
        }
    }
}


[System.Serializable]
public class AudioData
{
    public float Backvol;
    public float Mainvol;

    public AudioData( float Backvol,float Mainvol)
    {
        this.Backvol = Backvol;
        this.Mainvol = Mainvol;
    }
}