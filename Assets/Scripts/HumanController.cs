using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Image _sensorImage;
    [SerializeField] private float _sensorAngle = 90;
    
    private void Start()
    {
        _sensorImage.transform.localRotation= Quaternion.identity;
        _sensorImage.transform.Rotate(new Vector3(0, 0, _sensorAngle / 2));
        _sensorImage.fillAmount = _sensorAngle / 360;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Cat")
        {
            var angleBetweenSelfAndCat = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), 
                new Vector2(other.transform.position.x - transform.position.x, other.transform.position.z - transform.position.z));
            if (angleBetweenSelfAndCat < _sensorAngle / 2) print("Caught");
        }
    }
}
