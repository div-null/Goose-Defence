using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    //"Папки с текстом либо с картинкой" main menu
    public GameObject background, history, pressKeyToStart;
    //"Папки с текстом либо с картинкой" game
    public GameObject goldStat, gooseStat, difficultStat;
    public GameObject infoPanel;
    //Если игра не запущена, значит мы в меню, если нет, то UI надо изменить
    bool isGameStarted = false, canSkip = false;
    string historyStr = "Весь мир был поражён вирусом, из-за которого гуси стали неимоверно большыми, голодными, и практически поработили человечество с помощью звона колокольчиков. Единственное, что осталось у людей - это большой колокол и фермы, на которых они выращивают корм чтобы кормить гусей. Это последняя надежда человечества. С минуты на минуту гуси начнут своё последнее наступление, защитите колокол!";

    void UI_TurnOnMenu()
    {
        infoPanel.SetActive(false);
        background.SetActive(true);
        history.SetActive(true);
        pressKeyToStart.SetActive(true);
        goldStat.SetActive(false);
        gooseStat.SetActive(false);
        difficultStat.SetActive(false);
        StartCoroutine("ReadHistory");

    }

    void UI_TurnOnGame()
    {
        background.SetActive(false);
        history.SetActive(false);
        pressKeyToStart.SetActive(false);
        goldStat.SetActive(true);
        gooseStat.SetActive(true);
        difficultStat.SetActive(true);
    }

    void UI_SetAmountOfGold(int amount)
    {
        goldStat.GetComponent<Text>().text = amount.ToString();
    }

    void UI_SetAmountOfGoose(int dead, int total)
    {
        gooseStat.GetComponent<Text>().text = dead.ToString() + "/" + total.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        UI_TurnOnMenu();
        pressKeyToStart.GetComponent<Text>().text = "Нажмите на любую клавишу, чтобы пропустить историю";
    }

    void ShowHistory()
    {
        history.GetComponentInChildren<Text>().text = "";
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGameStarted == false && canSkip == true && Input.anyKey == true)
        {
            isGameStarted = true;
            UI_TurnOnGame();
        }
    }

    IEnumerator ReadHistory()
    {
        float textSpeed = 0.05f;
        for (int i = 0; i < historyStr.Length; i++)
        {
            if (canSkip == false && Input.anyKey == true)
            {
                textSpeed /= 10f;
            }
            history.GetComponentInChildren<Text>().text += historyStr[i];
            yield return new WaitForSeconds(textSpeed);
        }
        canSkip = true;
        pressKeyToStart.GetComponent<Text>().text = "Нажмите на любую клавишу, чтобы начать игру";
    }
}
