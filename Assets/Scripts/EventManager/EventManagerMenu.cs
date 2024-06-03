using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManagerMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GoToGame()
    {
        SceneManager.LoadScene("Main Game");
    }
    
    public void QuitGame()
    {
        Debug.Log("Выход из игры");
        Application.Quit(); // Завершает приложение
    }
}
