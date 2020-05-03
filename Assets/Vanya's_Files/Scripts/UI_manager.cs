using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : Singleton<UI_manager>
{
    //"Папки с текстом либо с картинкой" main menu
    public GameObject background, history, pressKeyToStart;
    //"Папки с текстом либо с картинкой" game
    public GameObject moneyStat, dangerStat, scoreStat;
    public GameObject infoPanel;
    public GameObject buyPanel;
    public GameObject UIInMenu, UIInGame;
    public GameObject battlefield;
    //Если игра не запущена, значит мы в меню, если нет, то UI надо изменить
    bool isGameStarted = false, canSkip = false;
    string historyStr = "     Весь мир был поражён вирусом, из-за которого гуси стали неимоверно большими, голодными, и практически поработили человечество с помощью звона колокольчиков. Единственное, что осталось у людей - это большой колокол и фермы, на которых они выращивают корм чтобы кормить гусей ради своего спасения. Это место - последняя надежда человечества. С минуты на минуту гуси начнут своё последнее наступление, защитите колокол!";
    [Header("InfoAboutTower")]
    public Sprite[] towerPictures = new Sprite[9];
    public Image displayTower;
    public Button Accept;
    public Text title, infoAboutTower, damage, radius, speed, reload, cost, health, attackRange;
    public Text upgradeDamage, upgradeRadius, upgradeSpeed, upgradeReload, upgradeHealth, upgradeAttackRange;
    public Animator transitor;
    Tower tower;
    Place place;
    int savedId;
    float price;

    [Header("Audio")]
    public AudioSource ButtonCloseSound;
    public AudioSource ButtonSelect;
    public AudioSource ButtonBuy;
    public AudioSource ButtonBuild;
    public AudioSource Writing;
    //public AudioSource ButtonSelect;

    //public AudioSource ButtonBuy;
    //public AudioSource ButtonUpgrade;
    // public AudioSource Writing;


    [Header("ResultsAtTheEndOfGame")]
    public Text scoreText;
    public Image resultImage;
    public Image signImage;
    public Sprite[] resultSprites = new Sprite[2];
    public Sprite[] resultSigns = new Sprite[2];
    public GameObject resultScreen;
    public GameObject transitToEnd;


    public void UI_TurnOnMenu()
    {
        ButtonCloseSound.Play();
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

    public void setDangerLvl(int lvl)
    {
        dangerStat.GetComponent<Text>().text = lvl + " уровень угрозы";
    }

    void setStatus(int gold)
    {
        if (Accept.GetComponentInChildren<Text>().text == "")
            Accept.interactable = false;
        else if (price <= gold)
            Accept.interactable = true;
        else Accept.interactable = false;
    }

    void WriteScore(int score)
    {
        score = (score == null) ? 0 : score;
        scoreStat.GetComponent<Text>().text = "Счёт: " + score;
    }

    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.UpdateScore += WriteScore;
        Game.Instance.WinGame += PrintScore;
        Game.Instance.LooseGame += PrintScore;
        Game.Instance.UpdateGold += setAmountOfMoney;
		GooseFabric.Instance.UpdateGooseLvl += setDangerLvl;
		StartWriting();
    }



    public void StartWriting()
    {
        isGameStarted = false;
        canSkip = true;
        UIInMenu.SetActive(true);
        StartCoroutine("ReadHistory");
        resultScreen.SetActive(false);
        UIInGame.SetActive(false);
        history.GetComponentInChildren<Text>().text = "";
        canSkip = false;
        isGameStarted = false;
        pressKeyToStart.GetComponent<Text>().text = "Нажмите на любую клавишу, чтобы пропустить историю";
    }


    public void CloseWindow()
    {
        ButtonCloseSound.Play();
        Application.Quit();
    }

    public void CloseInfoPanel()
    {
        ButtonCloseSound.Play();
        infoPanel.SetActive(false);
    }

    public void CloseBuyPanel()
    {
        ButtonCloseSound.Play();
        buyPanel.SetActive(false);
    }

    public void SelectFirstTypeOfTower()
    {
        ButtonSelect.Play();
        WindowBuyTower(0);
    }

    public void SelectSecondTypeOfTower()
    {
        ButtonSelect.Play();
        WindowBuyTower(3);
    }

    public void SelectThirdTypeOfTower()
    {
        ButtonSelect.Play();
        WindowBuyTower(6);
    }

    public void WindowBuyTower(int id)
    {
        savedId = id;
        infoPanel.SetActive(true);
        //Прячем статы улучшения
        upgradeDamage.gameObject.SetActive(false);
        upgradeRadius.gameObject.SetActive(false);
        upgradeSpeed.gameObject.SetActive(false);
        upgradeReload.gameObject.SetActive(false);
        upgradeHealth.gameObject.SetActive(false);
        upgradeAttackRange.gameObject.SetActive(false);
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
            upgradeHealth.gameObject.SetActive(false);
            upgradeAttackRange.gameObject.SetActive(false);
            ShowMainStats(tower.info);
            infoAboutTower.text = "Башня максимального уровня";
            Accept.GetComponentInChildren<Text>().text = "";
            setStatus(Game.Instance.Money);
        }
        else
        {
            ShowMainStats(tower.info);
            upgradeDamage.gameObject.SetActive(true);
            upgradeRadius.gameObject.SetActive(true);
            upgradeSpeed.gameObject.SetActive(true);
            upgradeReload.gameObject.SetActive(true);
            upgradeHealth.gameObject.SetActive(true);
            upgradeAttackRange.gameObject.SetActive(true);
            ShowUpgradeStats(tower.info);
            price = TowerStatsList.GetStatsByPrefabId(tower.info.PrefabId + 1).Cost;
            cost.text = "Стоимость: " + price;
            Accept.GetComponentInChildren<Text>().text = "Улучшить";
            setStatus(Game.Instance.Money);
        }
    }

    void ShowMainStats(TowerStatsList tower)
    {
        title.text = tower.Name;
        infoAboutTower.text = tower.Discription;
        displayTower.sprite = towerPictures[tower.PrefabId];
        damage.text = "Урон: " + tower.Projectile.Damage;
        attackRange.text = "Радиус атаки: " + tower.Range;
        radius.text = "Зона поражения: " + tower.Projectile.ExplosionRange;
        speed.text = "Скорость снаряда: " + tower.Projectile.Velocity;
        reload.text = "Перезарядка: " + tower.AttackDelay;
        health.text = "Здоровье: " + tower.MaxHP;
        cost.text = "Стоимость: " + tower.Cost;
    }

    void ShowUpgradeStats(TowerStatsList tower)
    {
        upgradeDamage.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.Damage - tower.Projectile.Damage);
        upgradeRadius.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.ExplosionRange - tower.Projectile.ExplosionRange);
        upgradeAttackRange.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Range - 11);
        upgradeSpeed.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).Projectile.Velocity - tower.Projectile.Velocity);
        upgradeReload.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).AttackDelay - tower.AttackDelay);
        upgradeHealth.text = "+" + (TowerStatsList.GetStatsByPrefabId(tower.PrefabId + 1).MaxHP - tower.MaxHP);
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
                        tower = parent.transform.gameObject.GetComponent<Tower>();
                        WindowUpgradeTower(tower);
                        infoPanel.SetActive(true);
                    }
                    else if (hit.transform.tag == "Tower")
                    {
                        tower = hit.transform.gameObject.GetComponent<Tower>();
                        WindowUpgradeTower(tower);
                        infoPanel.SetActive(true);
                    }
                    else if (hit.transform.tag == "Place")
                    {
                        place = hit.transform.gameObject.GetComponent<Place>();
                        if (place.isFree)
                            buyPanel.SetActive(true);
                    }
                    break;
                }
            }
        }
    }

    public void ClickAccept()
    {
        if (Accept.GetComponentInChildren<Text>().text == "Улучшить")
        {
            Game.Instance.decreaseMoney((int)price);
            TowerFabric.Instance.upgradeTower(tower.TowerOrder);
            infoPanel.SetActive(false);
            ButtonBuild.Play();
        }
        else if (Accept.GetComponentInChildren<Text>().text == "Купить")
        {
            Game.Instance.decreaseMoney((int)price);
            TowerFabric.Instance.placeTower(place.Order, TowerStatsList.GetStatsByPrefabId(savedId));
            infoPanel.SetActive(false);
            ButtonBuy.Play();
        }
        moneyStat.GetComponentInChildren<Text>().text = Game.Instance.Money.ToString();
    }

    IEnumerator ReadHistory()
    {
        float textSpeed = 0.05f;
        Writing.Play();
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
        Writing.Stop();
    }

    IEnumerator WaitForTransitionToMenu()
    {
        transitor.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        UIInMenu.SetActive(true);
        history.GetComponentInChildren<Text>().text = "";
        StartCoroutine(ReadHistory());
        resultScreen.SetActive(false);
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
        resultScreen.SetActive(false);
        transitor.SetTrigger("Start");
        //Начало игры
        //TowerFabric.Instance.placeTower(0, new TowerStatsList.TowerTomatoT1());
        //TowerFabric.Instance.placeTower(1, new TowerStatsList.TowerTomatoT2());
        //TowerFabric.Instance.placeTower(2, new TowerStatsList.TowerTomatoT3());
        //TowerFabric.Instance.placeTower(3, new TowerStatsList.TowerCabbageT1());
        //TowerFabric.Instance.placeTower(4, new TowerStatsList.TowerPeasT1());

        Game.Instance.startGame();
        //TowerFabric.Instance.placeTower(0, new TowerStatsList.TowerPeasT3());
        //TowerFabric.Instance.placeTower(1, new TowerStatsList.TowerPeasT3());
    }

    public void PrintScore(bool result, int score)
    {
        if (result)
            StartCoroutine(Result(result, score));
    }

    public void PrintScore2()
    {
        StartCoroutine(Result(true, 110));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        ClearLevel.Instance.Clear();
        GooseFabric.Instance.Stopspawning();
        StartWriting();
    }

    IEnumerator Result(bool result, int score)
    {
        transitToEnd.SetActive(true);
        yield return new WaitForSeconds(1f);
        resultScreen.SetActive(true);
        scoreText.text = "Счёт: " + score;
        if (result)
        {
            resultImage.sprite = resultSprites[0];
            signImage.sprite = resultSigns[0];
        }
        else
        {
            resultImage.sprite = resultSprites[1];
            signImage.sprite = resultSigns[1];
        }
    }
}
