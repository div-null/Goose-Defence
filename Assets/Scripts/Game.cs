using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EndGame (bool result, int score);
public delegate void StatUpdate (int score);

public class Game : Singleton<Game>
{

	// События победы и поражения
	public EndGame LooseGame;
	public EndGame WinGame;

	public event StatUpdate UpdateGold;
	public event StatUpdate UpdateScore;

	/// <summary>
	/// Игра идёт
	/// </summary>
	public bool IsGameStarted { get; protected set; } = false;

	public int Score { get => _score; protected set { _score = value; UpdateScore?.Invoke(_score); } }

	public int Money { get => _money; protected set { _money = value; UpdateGold?.Invoke(_money); } }

	[SerializeField]
	private int _baseMoney = 1000;

	[SerializeField]
	private int _money;

	[SerializeField]
	private int _score;

	/// <summary>
	/// Ожидание между получением денег
	/// </summary>
	[SerializeField]
	private float _moneyBackDelay = 1f;

	/// <summary>
	/// Количество денег получаемое за ожидание
	/// </summary>
	private int _moneyPerDelay = 50;

	private Coroutine _earnMoneyRoutine;

	public void Clear ()
	{
		Score = 0;
		Money = _baseMoney;
	}

	public void IncreaseScore (int ammount)
	{
		Score += ammount;
	}

	public void IncreaseMoney (int ammount)
	{
		Money += ammount;
	}

	public void DecreaseMoney (int ammount)
	{
		Money -= ammount;
	}

	public void StartGame ()
	{
		Score = 0;
		Money = _baseMoney;
		StartCoroutine(_beginGame());
		GooseFabric.Instance.StartSpawning();
	}

	public void FinishGame (bool result, int score)
	{
		StopAllCoroutines();
		StartCoroutine(_endGame());
		GooseFabric.Instance.Stopspawning();
	}

	void Awake ()
	{
		LooseGame += FinishGame;
		WinGame += FinishGame;
	}

	/// <summary>
	/// Регулярное получение денег
	/// </summary>
	/// <returns></returns>
	IEnumerator _earnMoney ()
	{
		while ( true )
		{
			yield return new WaitForSeconds(_moneyBackDelay);

			Money += (int)( _moneyPerDelay * ( GooseFabric.Instance.GooseLvl / 8f + 1 ) );
		}
	}

	IEnumerator _beginGame ()
	{
		TowerFabric.Instance.SpawnLocation();
		yield return new WaitForSeconds(3f);
		IsGameStarted = true;
		_earnMoneyRoutine = StartCoroutine(_earnMoney());
	}

	IEnumerator _endGame ()
	{
		IsGameStarted = false;
		this.StopRoutine(_earnMoneyRoutine);
		yield return new WaitForSeconds(0);
	}

}
