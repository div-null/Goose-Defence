using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EndGame(bool result, int score);
public delegate void StatUpdate(int score);

public class Game : Singleton<Game>
{

	// События победы и поражения
	public EndGame LooseGame;
	public EndGame WinGame;

	public event StatUpdate UpdateGold;
	public event StatUpdate UpdateScore;

	///<summary>
	/// Задержка перед показом UI для конца игры
	///</summary>
	public static float gameEndTimeOut = 2.5f;

	/// <summary>
	/// Типы снарядов
	/// </summary>
	[SerializeField]
	public ProjectileStats[,] ProjectilesTypes;

	/// <summary>
	/// Игра идёт
	/// </summary>
	public bool isGameStarted { get; protected set; } = false;


	[SerializeField]
	public int baseMoney = 1000;

	/// <summary>
	/// Ожидание между получением денег
	/// </summary>
	[SerializeField]
	float moneyBackDelay = 1f;

	/// <summary>
	/// Количество денег получаемое за ожидание
	/// </summary>
	int moneyPerDelay = 50;

	[SerializeField]
	int money;
	public int Money { get { return money; } protected set { money = value; UpdateGold?.Invoke(money); } }

	[SerializeField]
	int score;
	public int Score { get { return score; } protected set { score = value; UpdateScore?.Invoke(score); } }

	public void Clear()
	{
		Score = 0;
		Money = baseMoney;
	}

	public void increaseScore(int ammount)
	{
		Score += ammount;
	}

	public void increaseMoney(int ammount)
	{
		Money += ammount;
	}

	public void decreaseMoney(int ammount)
	{
		Money -= ammount;
	}

	/// <summary>
	/// КОРУТИНА СПАВНА ГУСЕЙ И УВЕЛИЧЕНИЯ СЛОЖНОСТИ
	/// </summary>
	/// <returns></returns>
	IEnumerator SpawnGooses()
	{
		GooseFabric.Instance.StartSpawning();
		yield return null;
	}

	public void startGame()
	{
		Score = 0;
		Money = baseMoney;
		StartCoroutine(BeginGame());
	}

	public void finishGame(bool result, int score)
	{
		StopAllCoroutines();
		GooseFabric.Instance.Stopspawning();
		StartCoroutine(EndGame());
	}

	/// <summary>
	/// Регулярное получение денег
	/// </summary>
	/// <returns></returns>
	IEnumerator EarnMoney()
	{
		while (true)
		{
			yield return new WaitForSeconds(moneyBackDelay);

			Money += (int)(moneyPerDelay * (GooseFabric.Instance.GooseLvl / 8f + 1));
		}
	}

	IEnumerator BeginGame()
	{
		TowerFabric.Instance.spawnLocation();
		yield return new WaitForSeconds(3f);
		isGameStarted = true;
		StartCoroutine(EarnMoney());
		StartCoroutine(SpawnGooses());
	}

	IEnumerator EndGame()
	{
		isGameStarted = false;
		StopCoroutine("EarnMoney");
		StopCoroutine("SpawnGooses");
		yield return new WaitForSeconds(0);
	}

	void Awake()
	{
		LooseGame += finishGame;
		WinGame += finishGame;

	}
}
