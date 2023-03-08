using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticCatController : MonoBehaviour
{
    [SerializeField] private float _radioWaitDuration = 4;
    [SerializeField] private PathFollower _pathFollower;
    [SerializeField] private AudioSource _walkAudio;

    private Rigidbody _rigid;
    private Animator _animator;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("Speed", _pathFollower.speed);
    }

    private void OnEnable()
    {
        EventManager.OnLevelComplete.AddListener(ReactToWinning);
        EventManager.OnRadioTurnedOn.AddListener(ResumeMovement);
        EventManager.OnReadyToOpenRadio.AddListener(PauseMovement);
    }

    private void OnDisable()
    {
        EventManager.OnLevelComplete.RemoveListener(ReactToWinning);
        EventManager.OnRadioTurnedOn.RemoveListener(ResumeMovement);
        EventManager.OnReadyToOpenRadio.RemoveListener(PauseMovement);
    }

    private void ReactToWinning()
    {
        _animator.SetTrigger("Win");
    }

    private void PauseMovement()
    {
        _pathFollower.enabled = false;
        _walkAudio.enabled = false;
        _animator.SetFloat("Speed", 0);
    }

    private void ResumeMovement(GameObject target)
    {
        StartCoroutine(WaitBeforeResumingCo());
    }

    private IEnumerator WaitBeforeResumingCo()
    {
        yield return new WaitForSeconds(_radioWaitDuration);
        _pathFollower.enabled = true;
        _walkAudio.enabled = true;
        _animator.SetFloat("Speed", _pathFollower.speed);
    }
}
