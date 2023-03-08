using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float _restartDelay = 1f;
    
    private void OnEnable()
    {
        EventManager.OnLevelComplete.AddListener(InitiateRestart);
    }

    private void OnDisable()
    {
        EventManager.OnLevelComplete.AddListener(InitiateRestart);
    }

    private void InitiateRestart()
    {
        StartCoroutine(RestartLevelCo());
    }

    private IEnumerator RestartLevelCo()
    {
        yield return new WaitForSeconds(_restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
