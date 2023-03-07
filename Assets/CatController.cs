using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatController : MonoBehaviour
{
    [SerializeField] private float _maxMovementSpeed = 1;
    [SerializeField] private float _sensitivity = 1;

    private Vector3 _clickPosition;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _clickPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var movementInput = new Vector3((Input.mousePosition - _clickPosition).x, 0, (Input.mousePosition - _clickPosition).y) * _sensitivity;
            var movementVector = movementInput.magnitude > _maxMovementSpeed ? movementInput.normalized * _maxMovementSpeed : movementInput;
            transform.position += movementVector * _maxMovementSpeed * Time.deltaTime;
            transform.LookAt(transform.position + movementVector);
        }
    }
}
