using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] private GameObject[] _rearDoors;
    [SerializeField] private GameObject[] _frontDoors;
    [SerializeField] private float _doorMoveDuration = .5f;
    [SerializeField] private float _escalateDuration = 1f;
    [SerializeField] private float _escalateDistance = 1f;

    private void Start()
    {
        foreach (var door in _frontDoors)
        {
            door.transform.localScale = new Vector3(0, 1, 1);
        }
    }

    private void Escalate()
    {
        Sequence elevationSequence = DOTween.Sequence();

        float temp1 = 0;
        elevationSequence.Append(DOTween.To(() => temp1, x => temp1 = x, 1, _doorMoveDuration).OnUpdate(() =>
        {
            foreach (var door in _frontDoors)
            {
                door.transform.localScale = new Vector3(temp1, 1, 1);
            }
        }));

        elevationSequence.Append(transform.DOMoveY(_escalateDistance, _escalateDuration));

        float temp2 = 1;
        elevationSequence.Append(DOTween.To(() => temp2, x => temp2 = x, 0, _doorMoveDuration).OnUpdate(() =>
        {
            foreach (var door in _rearDoors)
            {
                door.transform.localScale = new Vector3(temp2, 1, 1);
            }
        }));
    }
}
