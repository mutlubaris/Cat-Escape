using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Image _sensorImage;
    [SerializeField] private float _sensorAngle = 90;
    [SerializeField] private float _patrolIdleDuration = 1f;
    [SerializeField] private float _patrolDistance = 5f;
    [SerializeField] private float _patrolMoveDuration = 2f;
    [SerializeField] private float _patrolRotateDuration = 1f;
    [SerializeField] private float _quickRotateDuration = .5f;
    [SerializeField] private float _targetMoveDuration = 2f;
    [SerializeField] private float _targetOffsetDistance = 1.5f;
    [SerializeField] private float _reactDuration = 3f;
    [SerializeField] private bool _standingStill;
    [SerializeField] private bool _distractable;
    [SerializeField] private Transform _bodyTransform;

    private Sequence patrolSequence;
    private Sequence distractionSequence;
    private bool _caughtTheCat;

    private void OnEnable()
    {
        EventManager.OnRadioTurnedOn.AddListener(GoToTarget);
    }

    private void OnDisable()
    {
        EventManager.OnRadioTurnedOn.RemoveListener(GoToTarget);
    }

    private void GoToTarget(GameObject target)
    {
        if (_distractable)
        {
            patrolSequence.Kill();

            distractionSequence = DOTween.Sequence();
            var currentRotation = transform.rotation;
            transform.LookAt(target.transform);
            var targetRotation = transform.rotation;
            transform.rotation = currentRotation;
            distractionSequence.AppendInterval(_reactDuration);
            distractionSequence.Append(transform.DORotateQuaternion(targetRotation, _quickRotateDuration));
            distractionSequence.Append(transform.DOMove(target.transform.position - (target.transform.position - transform.position).normalized * _targetOffsetDistance, _targetMoveDuration));
        }
    }

    private void Start()
    {
        _sensorImage.transform.localRotation= Quaternion.identity;
        _sensorImage.transform.Rotate(new Vector3(0, 0, _sensorAngle / 2));
        _sensorImage.fillAmount = _sensorAngle / 360;

        if (!_standingStill)
        {
            patrolSequence = DOTween.Sequence();
            patrolSequence.Append(transform.DOMove(transform.position + transform.forward * _patrolDistance, _patrolMoveDuration));
            patrolSequence.AppendInterval(_patrolIdleDuration);
            patrolSequence.Append(transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 180, 0), _patrolRotateDuration));
            patrolSequence.Append(transform.DOMove(transform.position, _patrolMoveDuration));
            patrolSequence.AppendInterval(_patrolIdleDuration);
            patrolSequence.Append(transform.DORotate(transform.rotation.eulerAngles, _patrolRotateDuration));

            patrolSequence.SetLoops(-1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_caughtTheCat && other.tag == "Cat")
        {
            var angleBetweenSelfAndCat = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), 
                new Vector2(other.transform.position.x - transform.position.x, other.transform.position.z - transform.position.z));
            if (angleBetweenSelfAndCat < _sensorAngle / 2)
            {
                _caughtTheCat= true;
                patrolSequence.Kill();
                EventManager.OnCatCaught.Invoke();

                var currentRotation = transform.rotation;
                transform.LookAt(other.transform);
                var targetRotation = transform.rotation;
                transform.rotation = currentRotation;
                transform.DORotateQuaternion(targetRotation, .5f).OnComplete(() =>
                {
                    _bodyTransform.DOLocalRotate(new Vector3(0, 360, 0), _quickRotateDuration, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    });
                });
                
            }
        }
    }
}
