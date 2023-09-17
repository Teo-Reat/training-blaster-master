using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBehaviorTeo : MonoBehaviour
{
    public bool isGameActive;
    public int timer;
    public GameObject titleScreen;
    public GameObject restartScreen;
    public GameObject batteriesInfo;
    public Button buttonStart;
    public Button buttonRestart;

    public TextMeshProUGUI accJetValue;
    public TextMeshProUGUI accGunValue;
    public TextMeshProUGUI accDroidValue;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        buttonStart.onClick.AddListener(GameStart);
        buttonRestart.onClick.AddListener(GameRestart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowAccValue(string jet, string gun, string droid)
    {
        accGunValue.text = gun;
        accJetValue.text = jet;
        accDroidValue.text = droid;
    }
    public void GameStart()
    {
        Debug.Log("Game Start Func");
        isGameActive = true;
        StartCoroutine(SetGameOverTimer());
        Time.timeScale = 1;
        titleScreen.SetActive(false);
        batteriesInfo.SetActive(true);
        
    }
    public void GameOver()
    {
        Debug.Log("Game Over Func");
        isGameActive = false;
        Time.timeScale = 0;
        restartScreen.SetActive(true);
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator SetGameOverTimer()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            UpdateTimer();
        }
    }
    private void UpdateTimer()
    {
        timer -= 1;
        if (timer == 0)
        {
            GameOver();
        }
    }
}
