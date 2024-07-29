using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenuController : MonoBehaviour
{
    public FloatVariable bossHealth;

    public GameObject panel;
    public string mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth != null && bossHealth.Value <= 0)
        {
            panel.SetActive(true);
        }
    }

    public void OnExit()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
