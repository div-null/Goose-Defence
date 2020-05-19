using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName ="Score Database Template", menuName = "Score Database")]
public class ScoreDB : ScriptableObject
{
    [Description("Количество свободных мест в топе")]
    public int TopNumber = 5;
    List<int> scores;
    
    private void OnDestroy ()
        {
        UpLoadScore();
        }

    public void Load ()
        {
        scores = new List<int>(LoadScore());
        scores.Sort();
        scores.Reverse();
        }

    /// <summary>
    /// Размещает очки в списке
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
        {
        List<int> result = new List<int>(scores);
        result.Add(score);
        result.Sort();
        result.Reverse();
        scores = result.GetRange(0, scores.Count);
        UpLoadScore();
        }

    /// <summary>
    /// Получает список очков
    /// </summary>
    /// <returns></returns>
    public IEnumerable<int> GetScore ()
        {
        return scores;
        }

    /// <summary>
    /// Загружает данные из хранилища
    /// </summary>
    /// <returns></returns>
    private IEnumerable<int> LoadScore ()
        {
        List<int> loaded = new List<int>();
        string key;
        for ( int i = 0; i < TopNumber; i++ )
            {
            key = $"score{i}";
            if ( PlayerPrefs.HasKey(key) )
                loaded.Add(PlayerPrefs.GetInt(key));
            else
                loaded.Add(-1);
            }
        return loaded; 
        }

    /// <summary>
    /// Выгружает данные в хранилище
    /// </summary>
    private void UpLoadScore ()
        {
        string key;
        for ( int i = 0; i < TopNumber; i++ )
            {
            key = $"score{i}";
            PlayerPrefs.SetInt(key, scores[i]);
            }
        }
}
