﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    //"Папки с текстом либо с картинкой" main menu
    public GameObject background, history, pressKeyToStart;
    //"Папки с текстом либо с картинкой" game
    public GameObject moneyStat, dangerStat;
    public GameObject infoPanel;
    public GameObject buyPanel;
    public GameObject UIInMenu, UIInGame;
    public GameObject battlefield;
    //Если игра не запущена, значит мы в меню, если нет, то UI надо изменить
    bool isGameStarted = false, canSkip = false;
    string historyStr = "Весь мир был поражён вирусом, из-за которого гуси стали неимоверно большыми, голодными, и практически поработили человечество с помощью звона колокольчиков. Единственное, что осталось у людей - это большой колокол и фермы, на которых они выращивают корм чтобы кормить гусей. Это последняя надежда человечества. С минуты на минуту гуси начнут своё последнее наступление, защитите колокол!";
    [Header("InfoAboutTower")]
    public Sprite[] towerPictures = new Sprite[9];
    public Image displayTower;
    public Button Accept;
    public Text infoAboutTower, damage, radius, speed, reload, cost, health;
    public Text upgradeDamage, upgradeRadius, upgradeSpeed, upgradeReload, upgradeHealth;
    public Animator transitor;

    float price;

    public void UI_TurnOnMenu()
    {
        StartCoroutine(WaitForTransitionToMenu());
    }

    void UI_TurnOnGame()
    {
        StartCoroutine(WaitForTransitionToGame());
    }

    void UI_SetAmountOfGold(int amount)
    {
        moneyStat.GetComponent<Text>().text = amount.ToString();
    }

    void setAmountOfMoney(int gold)
    {
        moneyStat.GetComponentInChildren<Text>().text = gold.ToString();
        setStatus(gold);
    }

    void setStatus(int gold)
    {
        if (Accept.GetComponentInChildren<Text>().text == "")
            Accept.interactable = false;
        else if (price <= gold)
            Accept.interactable = true;
        else Accept.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.WinGame += PrintScore;
        Game.Instance.LooseGame += PrintScore;
        Game.Instance.UpdateGold += setAmountOfMoney;
        UIInMenu.SetActive(true);
        StartCoroutine("ReadHistory");
        UIInGame.SetActive(false);
        history.GetComponentInChildren<Text>().text = "";
        canSkip = false;
        isGameStarted = false;
        pressKeyToStart.GetComponent<Text>().text = "Нажмите на любую клавишу, чтобы пропустить историю";
    }

    public void CloseWindow()
    {
        Application.Quit();
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    public void CloseBuyPanel()
    {
        buyPanel.SetActive(false);
    }

    public void SelectFirstTypeOfTower()
    {
        WindowBuyTower(0);
    }

    public void SelectSecondTypeOfTower()
    {
        WindowBuyTower(3);
    }

    public void SelectThirdTypeOfTower()
    {
        WindowBuyTower(6);
    }

    public void WindowBuyTower(int id)
    {
        infoPanel.SetActive(true);
        //Прячем статы улучшения
        upgradeDamage.gameObject.SetActive(false);
        upgradeRadius.gameObject.SetActive(false);
        upgradeSpeed.gameObject.SetActive(false);
        upgradeReload.gameObject.SetActive(false);
        ShowMainStats(TowerStatsList.GetStatsByPrefabId(id));
        Accept.GetComponentInChildren<Text>().text = "Купить";
        setStatus(Game.Instance.Money);
        buyPanel.SetActive(false);
    }

    void WindowUpgradeTower(Tower tower)
    {
        if (tower.info.PrefabId % 3 == 2)
        {
            //Прячем статы улучшения
            upgradeDamage.gameObject.SetActive(false);
            upgradeRadius.gameObject.SetActive(false);
            upgradeSpeed.gameObject.SetActive(false);
            upgradeReload.gameObject.SetActive(false);
            ShowMainStats(tower.info);
            infoAboutTower.text = "Башня максимального уровня";
            Accept.GetComponentInChildren<Text>().text = "";
            setStatus(Game.Instance.Money);
        }
        else
        {
            ShowMainStats(tower.info);
            ShowUpgradeStats(tower.info);
            price = TowerStatsList.GetStatsByPrefabId(tower.info.PrefabId + 1).Cost;
            cost.text = "Стоимость: " + price;
            Accept.GetComponentInChildren<Text>().text = "Улучшить";
            setStatus(Game.Instance.Money);
        }
    }

    void ShowMainStats(TowerStatsList tower)
    {
        infoAboutTower.text = tower.Name + "\n" + tower.Discription;
        displayTower.sprite = towerPictures[tower.PrefabId];
        damage.text = "Урон: " + tower.Projectile.Damage;
        radius.text = "Радиус атаки: " + tower.Projectile.ExplosionRange;
        speed.text = "Скорость снаряда: " + tower.Projectile.Velocity;
        reload.text = "Скорость перезарядки: " + tower.AttackDelay;
        price = tower.Cost;
        cost.text = "Стоимость: " + price;
    }

    void ShowUpgradeStats(TowerStatsList tower)
    {
        upgradeDamage.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.Damage - tower.Projectile.Damage);
        upgradeRadius.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.ExplosionRange - tower.Projectile.ExplosionRange);
        upgradeSpeed.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.Velocity - tower.Projectile.Velocity);
        upgradeReload.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).AttackDelay - tower.AttackDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted == false && canSkip == true && Input.anyKey == true)
        {
            isGameStarted = true;
            StartCoroutine(WaitForTransitionToGame());
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            foreach (var hit in Physics2D.RaycastAll(rayPos, Vector2.zero))
            {
                if (hit.transform.tag == "UI_tower" || hit.transform.tag == "Tower" || hit.transform.tag == "Place")
                {
                    if (hit.transform.tag == "UI_tower")
                    {
                        var parent = hit.transform.parent;
                        Tower tower = parent.transform.gameObject.GetComponent<Tower>();
                        WindowUpgradeTower(tower);
                        infoPanel.SetActive(true);
                    }
                    if (hit.transform.tag == "Tower")
                    {
                        Tower tower = hit.transform.gameObject.GetComponent<Tower>();
                        WindowUpgradeTower(tower);
                        infoPanel.SetActive(true);
                    }

                    if (hit.transform.tag == "Place")
                        buyPanel.SetActive(true);
                    break;
                }
            }
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
    IEnumerator WaitForTransitionToMenu()
    {
        transitor.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        UIInMenu.SetActive(true);
        history.GetComponentInChildren<Text>().text = "";
        StartCoroutine(ReadHistory());
        UIInGame.SetActive(false);
        canSkip = false;
        isGameStarted = false;
        transitor.SetTrigger("Start");
    }
    IEnumerator WaitForTransitionToGame()
    {
        transitor.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        UIInMenu.SetActive(false);
        infoPanel.SetActive(false);
        buyPanel.SetActive(false);
        UIInGame.SetActive(true);
        transitor.SetTrigger("Start");
        //Начало игры
        TowerFabric.Instance.placeTower(0, new TowerStatsList.TowerTomatoT1());
        TowerFabric.Instance.placeTower(1, new TowerStatsList.TowerTomatoT2());
        TowerFabric.Instance.placeTower(2, new TowerStatsList.TowerTomatoT3());
        TowerFabric.Instance.placeTower(3, new TowerStatsList.TowerCabbageT1());
        TowerFabric.Instance.placeTower(4, new TowerStatsList.TowerPeasT1());

        Game.Instance.startGame();
    }

    public GameObject loseScreen, winScreen;
    public GameObject transitToEnd;

    public void PrintScore(bool result, int score)
    {
        if (result)
            StartCoroutine(Win(score));
    }
    
    IEnumerator Lose()
    {
        transitToEnd.SetActive(true);
        yield return new WaitForSeconds(1f);
        loseScreen.SetActive(true);
    }
    
    IEnumerator Win(int score)
    {
        transitToEnd.SetActive(true);
        yield return new WaitForSeconds(1f);
        winScreen.SetActive(true);
    }
}