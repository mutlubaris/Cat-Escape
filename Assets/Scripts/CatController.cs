using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatController : MonoBehaviour
{
    [SerializeField] private float _maxMovementSpeed = 1;
    [SerializeField] private float _sensitivity = 1;
    [SerializeField] private AudioSource _walkAudio;
    [SerializeField] private AudioSource _caughtAudio;

    private Vector3 _clickPosition;
    private bool _isControllable;
    private Rigidbody _rigid;
    private Animator _animator;

    private void Start()
    {
        EnableControl();
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventManager.OnEnabledDisabled.AddListener(EnableControl);
        EventManager.OnControlDisabled.AddListener(DisableControl);
    }

    private void OnDisable()
    {
        EventManager.OnEnabledDisabled.RemoveListener(EnableControl);
        EventManager.OnControlDisabled.RemoveListener(DisableControl);
    }

    private void EnableControl()
    {
        _isControllable = true;
    }

    private void DisableControl()
    {
        _isControllable = false;
        _caughtAudio.Play();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _clickPosition = Input.mousePosition;
        }

        if (_isControllable && Input.GetMouseButton(0))
        {
            var movementInput = new Vector3((Input.mousePosition - _clickPosition).x, 0, (Input.mousePosition - _clickPosition).y) * _sensitivity;
            var movementVector = movementInput.magnitude > _maxMovementSpeed ? movementInput.normalized * _maxMovementSpeed : movementInput;
            _rigid.velocity = new Vector3(movementVector.x * _maxMovementSpeed, _rigid.velocity.y, movementVector.z * _maxMovementSpeed) ;
            transform.LookAt(transform.position + movementVector);
            _walkAudio.enabled = true;
        }

        else
        {
            _rigid.velocity = new Vector3(0, _rigid.velocity.y, 0);
            _walkAudio.enabled = false;
        }

        _animator.SetFloat("Speed", new Vector3(_rigid.velocity.x, 0, _rigid.velocity.z).magnitude);
    }
}
