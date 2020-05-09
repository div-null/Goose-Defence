using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    Image redProgress;

    [SerializeField]
    Image progress;

    [SerializeField]
    Text hint;

    [SerializeField]
    float currentHp;

    [SerializeField]
    float maxHp;

    string postfix;

    public float Hp { get { return currentHp; } set { 
            currentHp = value;
            progress.fillAmount = currentHp / maxHp;
            hint.text = formatNumber(currentHp) + postfix;
        } }


    string formatNumber(float hp)
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

    void Reset()
    {
        redProgress = transform.Find("ProgressRed").GetComponent<Image>();
        progress = redProgress.transform.Find("ProgressGreen").GetComponent<Image>();
        hint = redProgress.transform.Find("Hint").GetComponent<Text>();
        Initialize(100);
        Hp = 80;
    }

    /// <summary>
    /// Инициализатор полоски здоровья
    /// </summary>
    /// <param name="_maxHp"></param>
    public void Initialize(float _maxHp)
    {
        maxHp = _maxHp;
        postfix = $"/{formatNumber(maxHp)}";
        Hp = _maxHp;
    }

    public void Destroy()
    {
        this.enabled = false;
        Destroy(gameObject);
    }

}
