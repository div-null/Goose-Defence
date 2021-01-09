using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonobehaviourExtension
{
	public static bool StopRoutine (this MonoBehaviour behaviour, Coroutine routine)
	{
		if ( routine != null )
		{
			behaviour.StopCoroutine(routine);
			return true;
		}
		return false;
	}

	public static bool StopRoutine (this MonoBehaviour behaviour, params Coroutine[] routines)
	{
		bool allStopped = true;
		foreach (  var routine in routines)
			allStopped &= behaviour.StopRoutine(routine);
		return allStopped;
	}
}
