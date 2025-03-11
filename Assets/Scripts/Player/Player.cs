using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public UIItem uiItem;

    public ItemData itemData;
    public Action addItem;

    private float startTime;
    public GameObject uiResult;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI inGameTimerText;
    public TextMeshProUGUI ResultText;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        uiItem = GameObject.Find("UIItem")?.GetComponent<UIItem>();
    }

    private void Start()
    {
        // 시작 시간 체크
        startTime = Time.time;
    }

    private void Update()
    {
        // 인게임에 진행 시간 설정
        if (inGameTimerText != null)
        {
            float elapsedTime = CheckTime();
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            inGameTimerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public float CheckTime()
    {
        // 진행 시간 반환
        return Time.time - startTime;
    }

    public void ShowResult()
    {
        // 결과창 텍스트 플레이어 현재체력에 따라 변경
        float playerCurHealth = condition.curHealth();

        if (playerCurHealth <= 0f)
        {
            ResultText.text = "Die";
        }
        else
        {
            ResultText.text = "Clear";
        }

        // 타임스케일 0 해주고 마우스 보이게함 결과창ui 띄어줌
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        uiResult.SetActive(true);

        float resultTime = CheckTime();
        int minutes = Mathf.FloorToInt(resultTime / 60);
        int seconds = Mathf.FloorToInt(resultTime % 60);
        timeText.text = $"{minutes:00} : {seconds:00}";
    }

    public void RestartGame()
    {
        // 다시하기 눌렀을때 씬 다시 불러옴
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        //나가기 버튼 눌렀을때 꺼지게 설정
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}