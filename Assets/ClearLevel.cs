using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLevel : Singleton<ClearLevel>
{
    public void Clear()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Place"))
            GameObject.Destroy(item);

        foreach (var item in GameObject.FindGameObjectsWithTag("Tower"))
            GameObject.Destroy(item);

        foreach (var item in GameObject.FindGameObjectsWithTag("Goose"))
            GameObject.Destroy(item);

        foreach (var item in GameObject.FindGameObjectsWithTag("Projectile"))
            GameObject.Destroy(item);

		GooseFabric.Instance.Clear();

		Game.Instance.Clear();

	}
}
