using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RadioController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _arrows;
    [SerializeField] private float _arrowMoveDuration = .5f;
    [SerializeField] private float _arrowMoveDistance = 2f;

    private void Start()
    {
        _particleSystem.Pause();
        _canvas.SetActive(false);

        _arrows.transform.DOLocalMoveY(_arrows.transform.localPosition.y - _arrowMoveDistance, _arrowMoveDuration).SetEase(Ease.InCirc).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        EventManager.OnReadyToOpenRadio.AddListener(EnableHint);
    }

    private void OnDisable()
    {
        EventManager.OnReadyToOpenRadio.RemoveListener(EnableHint);
    }

    private void EnableHint()
    {
        _canvas.SetActive(true);
    }

    private void OnMouseDown()
    {
        _particleSystem.Play();
        _audioSource.Play();
        _canvas.SetActive(false);
        EventManager.OnRadioTurnedOn.Invoke(gameObject);
    }
}
