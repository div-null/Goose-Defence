using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	[SerializeField]
	private Image _redProgress;

	[SerializeField]
	private Image _progress;

	[SerializeField]
	private Text _hint;

	[SerializeField]
	private float _currentHp;

	[SerializeField]
	private float _maxHp;

	string _postfix;

	public float Hp
	{
		get => _currentHp;
		set
		{
			_currentHp = value;
			_progress.fillAmount = _currentHp / _maxHp;
			_hint.text = _formatNumber(_currentHp) + _postfix;
		}
	}

	/// <summary>
	/// Инициализатор полоски здоровья
	/// </summary>
	/// <param name="maxHp"></param>
	public void Initialize(float maxHp)
	{
		_maxHp = maxHp;
		_postfix = $"/{_formatNumber(_maxHp)}";
		Hp = maxHp;
	}

	public void Destroy()
	{
		this.enabled = false;
		Destroy(gameObject);
	}

	void Reset()
	{
		_redProgress = transform.Find("ProgressRed").GetComponent<Image>();
		_progress = _redProgress.transform.Find("ProgressGreen").GetComponent<Image>();
		_hint = _redProgress.transform.Find("Hint").GetComponent<Text>();
		Initialize(100);
		Hp = 80;
	}

	string _formatNumber(float hp)
	{
		float number = Mathf.Round(hp);
		if (number > 1000000)
		{
			number = Mathf.Round(hp / 1000000 * 10) / 10;
			return $"{number}M";
		}
		else
			if (number > 1000)
		{
			number = Mathf.Round(hp / 1000 * 10) / 10;
			return $"{number}K";
		}
		return $"{number}";
	}
}
