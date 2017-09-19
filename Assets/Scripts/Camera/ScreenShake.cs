using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
	public static ScreenShake Singleton;

    public string ScreenShakeName;

    void Awake ()
	{
		if (ScreenShake.Singleton == null) {
			ScreenShake.Singleton = this;
		} else {
			Destroy (gameObject);
		}
	}
}
