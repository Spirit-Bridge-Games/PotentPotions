using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuController : MonoBehaviour
{
    public FloatVariable playerHealth;

    public GameObject panel;
    public string mainMenuScene;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth != null && playerHealth.Value <= 0) 
        {
            panel.SetActive(true);
        }
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExit()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
