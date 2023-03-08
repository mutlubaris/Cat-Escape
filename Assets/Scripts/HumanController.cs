using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Image _sensorImage;
    [SerializeField] private float _sensorAngle = 90;
    [SerializeField] private float _idleDuration = 1f;
    [SerializeField] private float _moveDistance = 5f;
    [SerializeField] private float _moveDuration = 2f;
    [SerializeField] private float _rotateDuration = 1f;
    [SerializeField] private bool _standingStill;

    private Sequence sequence;
    
    private void Start()
    {
        _sensorImage.transform.localRotation= Quaternion.identity;
        _sensorImage.transform.Rotate(new Vector3(0, 0, _sensorAngle / 2));
        _sensorImage.fillAmount = _sensorAngle / 360;

        if (!_standingStill)
        {
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(transform.position + transform.forward * _moveDistance, _moveDuration));
            sequence.AppendInterval(_idleDuration);
            sequence.Append(transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 180, 0), _rotateDuration));
            sequence.Append(transform.DOMove(transform.position, _moveDuration));
            sequence.AppendInterval(_idleDuration);
            sequence.Append(transform.DORotate(transform.rotation.eulerAngles, _rotateDuration));

            sequence.SetLoops(-1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Cat")
        {
            var angleBetweenSelfAndCat = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), 
                new Vector2(other.transform.position.x - transform.position.x, other.transform.position.z - transform.position.z));
            if (angleBetweenSelfAndCat < _sensorAngle / 2) sequence.Kill();
        }
    }
}
