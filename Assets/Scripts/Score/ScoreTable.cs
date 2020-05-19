using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTable : Singleton<ScoreTable>
{
    /// <summary>
    /// Настройки бд
    /// </summary>
    [SerializeField]
    ScoreDB DB;

    /// <summary>
    /// Полотно для дочерних элементов
    /// </summary>
    [SerializeField]
    Canvas canv;

    /// <summary>
    /// Лейбл "ТОП 5"
    /// </summary>
    [SerializeField]
    Text TopScore;

    /// <summary>
    /// Префаб панели счёта
    /// </summary>
    [SerializeField]
    GameObject textPanelPref;

    /// <summary>
    /// Контейнер для счёта
    /// </summary>
    [SerializeField]
    Transform Content;
    private void Start ()
        {
        canv.enabled = false;
        DB.Load();
        }

    private void Reset ()
        {
        DB = new ScoreDB();
        canv = GetComponent<Canvas>();
        TopScore = transform.Find("TopScore").GetComponent<Text>();
        Content = transform.Find("Table").Find("Viewport").Find("Content");
        }

    public void Show (int current)
        {
        canv.enabled = true;

        DB.AddScore(current);
        TopScore.text = $"Top {DB.TopNumber} scores:";
        List<int> scores = new List<int>(DB.GetScore());

        // Spawn new panels
        for ( int i = 0; i < scores.Count; i++ )
            {
            var label = Instantiate(textPanelPref, Content, false).GetComponent<Text>();
            if ( scores[i] == -1 )
                label.text = $"{i + 1}. ----------";
            else
                label.text = $"{i + 1}. {scores[i]}";
            }
        }

    public void Hide ()
        {
        canv.enabled = false;
        // Destroy child objects
        foreach ( var field in Content.GetComponentsInChildren<Text>() )
            Destroy(field);
        }
}
