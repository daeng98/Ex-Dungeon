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
        // ���� �ð� üũ
        startTime = Time.time;
    }

    private void Update()
    {
        // �ΰ��ӿ� ���� �ð� ����
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
        // ���� �ð� ��ȯ
        return Time.time - startTime;
    }

    public void ShowResult()
    {
        // ���â �ؽ�Ʈ �÷��̾� ����ü�¿� ���� ����
        float playerCurHealth = condition.curHealth();

        if (playerCurHealth <= 0f)
        {
            ResultText.text = "Die";
        }
        else
        {
            ResultText.text = "Clear";
        }

        // Ÿ�ӽ����� 0 ���ְ� ���콺 ���̰��� ���âui �����
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
        // �ٽ��ϱ� �������� �� �ٽ� �ҷ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        //������ ��ư �������� ������ ����
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}