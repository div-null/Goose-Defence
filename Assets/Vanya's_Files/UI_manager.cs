using System.Collections;
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
    //Если игра не запущена, значит мы в меню, если нет, то UI надо изменить
    bool isGameStarted = false, canSkip = false;
    string historyStr = "Весь мир был поражён вирусом, из-за которого гуси стали неимоверно большыми, голодными, и практически поработили человечество с помощью звона колокольчиков. Единственное, что осталось у людей - это большой колокол и фермы, на которых они выращивают корм чтобы кормить гусей. Это последняя надежда человечества. С минуты на минуту гуси начнут своё последнее наступление, защитите колокол!";
    [Header("InfoAboutTower")]
    public Button Accept;
    public Text infoAboutTower, damage, radius, speed, reload, cost;
    public Text upgradeDamage, upgradeRadius, upgradeSpeed, upgradeReload;
    public Animator transitor;

    public void UI_TurnOnMenu()
    {
        transitor.SetBool("Status", false);
        UIInMenu.SetActive(true);
        StartCoroutine("ReadHistory");
        UIInGame.SetActive(false);
        history.GetComponentInChildren<Text>().text = "";
        canSkip = false;
        isGameStarted = false;
        transitor.SetBool("Status", true);
    }

    void UI_TurnOnGame()
    {
        StartCoroutine("WairForTransition");
        UIInMenu.SetActive(false);
        infoPanel.SetActive(false);
        buyPanel.SetActive(false);
        UIInGame.SetActive(true);
    }

    void UI_SetAmountOfGold(int amount)
    {
        moneyStat.GetComponent<Text>().text = amount.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        UI_TurnOnMenu();
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
        BuyTower(0);
    }

    public void SelectSecondTypeOfTower()
    {
        BuyTower(3);
    }

    public void SelectThirdTypeOfTower()
    {
        BuyTower(6);
    }

    public void BuyTower(int id)
    {
        infoPanel.SetActive(true);
        //Прячем статы улучшения
        upgradeDamage.gameObject.SetActive(false);
        upgradeRadius.gameObject.SetActive(false);
        upgradeSpeed.gameObject.SetActive(false);
        upgradeReload.gameObject.SetActive(false);
        ShowMainStats(TowerStatsList.GetStatsByPrefabId(id));
        Accept.GetComponentInChildren<Text>().name = "Купить";
        if (TowerStatsList.GetStatsByPrefabId(id).Cost < int.Parse(moneyStat.GetComponentInChildren<Text>().text))
            Accept.enabled = false;
        else Accept.enabled = true;
        buyPanel.SetActive(false);
    }

    void UpgradeTower(Tower tower)
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
            Accept.enabled = false;
            Accept.GetComponent<Text>().name = "";
        }
        else
        {
            ShowMainStats(tower.info);
            ShowUpgradeStats(tower.info);
            Accept.enabled = true;
            //Т.к стоимость за покупку улучшения в след уровне тавера (или нет?)
            cost.text = "Стоимость: " + TowerStatsList.GetStatsByPrefabId(tower.info.PrefabId + 1).Cost;
            Accept.GetComponent<Text>().name = "Улучшить";
            if (TowerStatsList.GetStatsByPrefabId(tower.info.PrefabId + 1).Cost < int.Parse(moneyStat.GetComponentInChildren<Text>().text))
                Accept.enabled = false;
            else Accept.enabled = true;
        }
    }

    void ShowMainStats(TowerStatsList tower)
    {
        infoAboutTower.text = tower.Name + "\n" + tower.Discription;
        damage.text = "Урон: " + tower.Projectile.Damage;
        radius.text = "Радиус атаки: " + tower.Projectile.ExplosionRange;
        speed.text = "Скорость снаряда: " + tower.Projectile.Velocity;
        reload.text = "Скорость перезарядки: " + tower.AttackDelay;
        cost.text = "Стоимость: " + tower.Cost;
    }

    void ShowUpgradeStats(TowerStatsList tower)
    {
        upgradeDamage.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.Damage - tower.Projectile.Damage);
        upgradeRadius.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.ExplosionRange - tower.Projectile.ExplosionRange);
        upgradeSpeed.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.Velocity - tower.Projectile.Velocity);
        upgradeReload.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).AttackDelay - tower.AttackDelay);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGameStarted == false && canSkip == true && Input.anyKey == true)
        {
            isGameStarted = true;
            StartCoroutine(WaitForTransitionToGame());
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            if (hit)
            {
                if (hit.transform.tag == "Tower")
                {
                    Tower tower = hit.transform.gameObject.GetComponent<Tower>();
                    infoPanel.SetActive(true);
                }
                else if (hit.transform.tag == "Place")
                    buyPanel.SetActive(true);
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
        transitor.SetBool("Status", false);
        yield return new WaitForSeconds(3f);
        transitor.SetBool("Status", true);
    }
}
