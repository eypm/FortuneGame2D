using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class App : MonoBehaviour
{
    public static App instance;
    public static User user;
    public List<Snackbar> snackbars;

    void Awake()
    {
        instance = this;
        user = PreferenceHelper.GetUserFromPrefs();
    }
    public Coroutine WaitForSeconds(float waitForSeconds, Action action)
    {
        return StartCoroutine(_WaitForSecondsFinished(waitForSeconds, action));
    }
    private IEnumerator _WaitForSecondsFinished(float waitForSeconds, Action action)
    {
        yield return new WaitForSeconds(waitForSeconds);
        action.Invoke();
    }
}