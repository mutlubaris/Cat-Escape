using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RadioController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _canvas;

    private void Start()
    {
        _particleSystem.Pause();
    }

    private void OnMouseDown()
    {
        _particleSystem.Play();
        _audioSource.Play();
        _canvas.SetActive(false);
        EventManager.OnRadioTurnedOn.Invoke(gameObject);
    }
}
