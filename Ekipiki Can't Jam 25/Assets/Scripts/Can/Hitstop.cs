using System;
using System.Collections;
using UnityEngine;

public class Hitstop : MonoBehaviour
{
    private static Hitstop _instance;
    public static Hitstop Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("Hitstop");
                _instance = obj.AddComponent<Hitstop>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private bool isHitstopActive = false;

    public void Trigger(float duration, Action onEnd = null)
    {
        if (!isHitstopActive)
            StartCoroutine(DoHitstop(duration, onEnd));
    }

    private IEnumerator DoHitstop(float duration, Action onEnd)
    {
        isHitstopActive = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        isHitstopActive = false;
        onEnd?.Invoke();
    }
}   