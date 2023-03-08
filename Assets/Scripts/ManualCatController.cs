using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManualCatController : MonoBehaviour
{
    [SerializeField] private float _maxMovementSpeed = 1;
    [SerializeField] private float _sensitivity = 1;
    [SerializeField] private AudioSource _walkAudio;
    [SerializeField] private AudioSource _caughtAudio;
    [SerializeField] private bool _isControllable;

    private Vector3 _clickPosition;
    private Rigidbody _rigid;
    private Animator _animator;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventManager.OnLevelComplete.AddListener(ReactToWinning);
        EventManager.OnCatCaught.AddListener(ReactToBeingCaught);
    }

    private void OnDisable()
    {
        EventManager.OnLevelComplete.RemoveListener(ReactToWinning);
        EventManager.OnCatCaught.RemoveListener(ReactToBeingCaught);
    }

    private void ReactToWinning()
    {
        _animator.SetTrigger("Win");
    }

    private void ReactToBeingCaught()
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
