﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public abstract class ManagerSingleton<T> : SerializedMonoBehaviour where T : MonoBehaviour
{
	[SerializeField] protected bool isDonDestory = false;
	public static T Instance;
	protected virtual void Awake()
	{
		if(isDonDestory == false)
		{
			Instance = this as T;
		}
		else
		{
			if(Instance == null)
			{
				Instance = this as T;
				DontDestroyOnLoad(Instance.gameObject);
			}
			else
				Destroy(this.gameObject);
		}
		
	}

	public static bool IsValid()
	{
		return Instance != null && Instance.gameObject != null;
	}

	/* protected virtual void OnDestroy()
	{
		Instance = null;
	} */


}
