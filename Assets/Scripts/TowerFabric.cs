using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerFabric : Singleton<TowerFabric>
{
	public const int MaxTowerCount = 6;

	/// <summary>
	/// Количество не пустых мест
	/// </summary>
	public int TowerCount => _places.Count(place => place.IsFree);

	public List<Tower> Towers;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject _placePrefab;

	[SerializeField]
	private GameObject[] _projectilePrefabs;

	[SerializeField]
	private GameObject[] _towerPrefabs;

	[SerializeField]
	private GameObject[] _place;

	[SerializeField]
	private Transform _battlefield;

	[Header("Stats")]
	[SerializeField]
	private List<TowerStats> _towerStatsList;

	[SerializeField]
	private TowerStats _colocolStats;

	[SerializeField]
	private TowerStats _wallStats;

	private Place[] _places;
	private List<Target> _targets;
	private Wall _wall;
	private BossBell _bell;


	/// <summary>
	/// Инициализирует уровень
	/// </summary>
	public void SpawnLocation ()
	{
		_battlefield = GameObject.Find("BattleField").transform;
		Towers = new List<Tower>();
		_targets = new List<Target>();
		_places = new Place[MaxTowerCount];
		for ( int placeId = 0; placeId < MaxTowerCount; placeId++ )
		{
			Vector3 placePosition = _place[placeId].transform.position;
			placePosition.z = -3 + placePosition.y / 10f + 1.5f;

			var placeObject = GameObject.Instantiate(_placePrefab, placePosition, Quaternion.identity, _battlefield);
			_places[placeId] = placeObject.GetComponent<Place>();
			_places[placeId].Initialize(placeId, true, placePosition);

			Towers.Add(null);
		}

		_spawnWall(new Vector3(-13.4f, 0.77f, 0));
		_spawnBell(new Vector3(-19.95f, -0.7f, 0));
		_spawnDummyTarget(new Vector3(1000f, -1.38f, 0));
	}

	/// <summary>
	/// Характеристика указанной башни
	/// </summary>
	/// <param name="order">Номер площадки</param>
	/// <returns></returns>
	public TowerStats SelectedTowerStats (int order)
	{
		if ( Towers[order] != null )
			return Towers[order].Stats;
		return null;
	}

	/// <summary>
	/// Получает характеристики
	/// </summary>
	/// <param name="type">Тип башни</param>
	/// <param name="Level">Уровень башни</param>
	/// <returns></returns>
	public TowerStats TowerStats (TowerType type, int Level = 1)
	{
		return _towerStatsList.Find(st => st.Type == type && st.Level == Level);
	}

	public TowerStats NextTowerStats (TowerStats stats)
	{
		return TowerStats(stats.Type, stats.Level + 1);
	}

	/// <summary>
	/// Ставит башню на позицию
	/// </summary>
	/// <param name="order">Номер площадки</param>
	/// <param name="type">Тип башни</param>
	/// <param name="Level">Уровень башни</param>
	public void PlaceTower (int order, TowerType type, int Level)
	{
		_setTower(order, TowerStats(type, Level));
	}

	/// <summary>
	/// Обновляет башню на позиции
	/// </summary>
	/// <param name="order">Номер площадки</param>
	public void UpgradeTower (int order)
	{
		TowerStats stats = _getStatsByOrder(order);
		var newPrefid = (int)stats.Type * 3 + stats.Level;
		var newStats = _towerStatsList[newPrefid];
		DestroyTower(order);
		_setTower(order, newStats);
	}

	/// <summary>
	/// Убирает указанную башню
	/// </summary>
	/// <param name="order">Номер площадки</param>
	public void DestroyTower (int order)
	{
		Tower tower = Towers[order];
		_targets.Remove(tower);
		tower.Destroyed -= onDeleteTower;
		tower.DestroySelf();

		Towers[order] = null;
		_places[order].IsFree = true;
	}

	/// <summary>
	/// Ищет ближайшую к точке поиска цель
	/// </summary>
	/// <param name="searchingPoint">Точка поиска</param>
	/// <param name="searchDistance">Радиус поиска</param>
	/// <returns></returns>
	public Target FindNearTarget (Vector3 searchingPoint, float searchDistance = 9999)
	{
		float closestDistance = searchDistance;
		Target foundTarget = null;
		foreach ( var target in _targets )
		{
			if ( target == null || target.HP <= 0 )
				continue;

			var distance = Vector3.Distance(target.transform.position, searchingPoint);
			if ( distance < closestDistance )
			{
				closestDistance = distance;
				foundTarget = target;
			}
		}
		return foundTarget;
	}

	private void _setTower (int placeId, TowerStats stats)
	{
		Vector3 spawnPosition = _places[placeId].Position;
		int towerPrefId = (int)stats.Type * 3 + stats.Level - 1;
		int projPrefId = (int)stats.Type;

		GameObject towerObject = GameObject.Instantiate(_towerPrefabs[towerPrefId], spawnPosition, Quaternion.identity, _battlefield);
		var tower = towerObject.AddComponent<Tower>();

		tower.Initialize(stats, _projectilePrefabs[projPrefId], placeId);
		tower.MakeDamage();
		tower.Destroyed += onDeleteTower;

		_targets.Add(tower);
		Towers[placeId] = tower;
		_places[placeId].IsFree = false;
	}

	private TowerStats _getStatsByOrder (int order)
	{
		return Towers[order].Stats;
	}

	private void _spawnWall (Vector3 position)
	{
		//////////////////////////
		/// СПАВН стены
		var wallObj = GameObject.Instantiate(_towerPrefabs[9], position, Quaternion.identity, _battlefield);
		wallObj.transform.tag = "Tower";

		Vector3 mutatedPosition = wallObj.transform.position;
		mutatedPosition.z = _places[0].transform.position.z + 0.001f;
		wallObj.transform.position = mutatedPosition;

		_wall = wallObj.GetComponent<Wall>();
		_wall.Destroyed += onWallDestroyed;
		_wall.Initialize(_wallStats.MaxHP);
		_targets.Add(_wall);
	}

	private void _spawnBell(Vector3 position)
	{
		var bellObject = GameObject.Instantiate(_towerPrefabs[10], position, Quaternion.identity, _battlefield);
		bellObject.transform.tag = "Tower";

		Vector3 mutatedPosition = bellObject.transform.position;
		mutatedPosition.z = -3 + Mathf.Abs(mutatedPosition.y / 10) + 0.1f;
		bellObject.transform.position = mutatedPosition;

		_bell = bellObject.GetComponent<BossBell>();
		_bell.Destroyed += onBellDestroyed;
		_bell.Initialize(_colocolStats.MaxHP);
		_targets.Add(_bell);
	}

	private void _spawnDummyTarget (Vector3 position)
	{
		var dummyObject = GameObject.Instantiate(_towerPrefabs[10], position, Quaternion.identity, _battlefield);
		dummyObject.transform.tag = "Tower";

		Vector3 mutatedPosition = dummyObject.transform.position;
		mutatedPosition.z = -3 + Mathf.Abs(mutatedPosition.y / 10) + 4f;
		dummyObject.transform.position = mutatedPosition;

		var tower = dummyObject.AddComponent<Tower>();
		tower.Initialize(_colocolStats, _projectilePrefabs[0], 10);
		_targets.Add(tower);
	}

	/// <summary>
	/// Обработчик уничтожения башни
	/// </summary>
	/// <param name="tower"></param>
	private void onDeleteTower (Target tower)
	{
		int index = Towers.IndexOf((Tower)tower);
		DestroyTower(index);
	}

	/// <summary>
	/// Обработчик уничтожения колокола
	/// </summary>
	/// <param name="bossBell"></param>
	private void onBellDestroyed (Target bell)
	{
		StartCoroutine(_bellDestroyed(bell));
	}

	/// <summary>
	/// Обработчик уничтожения стены
	/// </summary>
	private void onWallDestroyed (Target wall)
	{
		Debug.Log("Wall crushed");
		wall.DestroySelf();
		_targets.Remove(wall);
		GooseFabric.Instance.LoanchBoss();
	}

	private IEnumerator _bellDestroyed (Target bossBell)
	{
		Debug.Log("Bell Destroyed");
		_targets.Remove(bossBell);
		bossBell.DestroySelf();
		yield return new WaitForSeconds(2f);
		Game.Instance.LooseGame?.Invoke(false, Game.Instance.Score);
	}

}
