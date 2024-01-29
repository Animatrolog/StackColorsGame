using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _speed;    

    private void FixedUpdate()
    {
        transform.Rotate(_speed * Time.deltaTime);
    }
}