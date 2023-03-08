using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] private GameObject[] _rearDoors;
    [SerializeField] private GameObject[] _frontDoors;
    [SerializeField] private GameObject _arrows;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private float _arrowMoveDuration = .5f;
    [SerializeField] private float _arrowMoveDistance = 2f;
    [SerializeField] private float _doorMoveDuration = .5f;
    [SerializeField] private float _escalateDuration = 1f;
    [SerializeField] private float _escalateDistance = 1f;
    [SerializeField] private AudioSource _openDoorsSound;
    [SerializeField] private AudioSource _closeDoorsSound;

    private bool _elevated;

    private void Start()
    {
        foreach (var door in _frontDoors)
        {
            door.transform.localScale = new Vector3(0, 1, 1);
        }

        _arrows.transform.DOLocalMoveY(_arrows.transform.localPosition.y - _arrowMoveDistance, _arrowMoveDuration).SetEase(Ease.InCirc).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Cat") Escalate();
    }

    private void Escalate()
    {
        if(_elevated) return; _elevated = true;
        
        _canvas.SetActive(false);

        Sequence elevationSequence = DOTween.Sequence();

        float temp1 = 0;
        elevationSequence.Append(DOTween.To(() => temp1, x => temp1 = x, 1, _doorMoveDuration).OnStart(() => 
        { 
            _closeDoorsSound.Play(); 
        }).OnUpdate(() =>
        {
            foreach (var door in _frontDoors)
            {
                door.transform.localScale = new Vector3(temp1, 1, 1);
            }
            
        }));

        elevationSequence.Append(transform.DOMoveY(_escalateDistance, _escalateDuration));

        float temp2 = 1;
        elevationSequence.Append(DOTween.To(() => temp2, x => temp2 = x, 0, _doorMoveDuration).OnStart(() =>
        {
            _openDoorsSound.Play();
        }).OnUpdate(() =>
        {
            foreach (var door in _rearDoors)
            {
                door.transform.localScale = new Vector3(temp2, 1, 1);
            }   
        }));
    }
}
