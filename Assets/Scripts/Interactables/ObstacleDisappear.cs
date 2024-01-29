using UnityEngine;

public class ObstacleDisappear : MonoBehaviour
{
    [Header("Disabled components")]
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private ObstacleRotator _rotator;
    [Header("Falling setting")]
    [SerializeField] private bool _isFalling;
    [SerializeField] [Range(1, 3)] private int _angle;
    [SerializeField] private float _fallingToValue;
    [SerializeField] private float _speedFalling;
    /*
    [Header("Compressing setting")]
    [SerializeField] private bool _isCompressing;
    [SerializeField] private Vector3 _compressToValue;
    [SerializeField] private float _speedCompressing;
    */
    [Header("Animation setting")]
    [SerializeField] private bool _isNeedAnimator;
    [SerializeField] private Animator _animator;

    private int _angleX = 1;
    private int _angleY = 2;
    private int _angleZ = 3;
    private bool _isFalled = false;
    //private bool _isCompressed = false;

    private void OnEnable()
    {
        if (_isNeedAnimator == false)
        {
            DisablingRotator();
        }

        DisablingCollieders();
        TryStartingAnimator();
    }

    private void Update()
    {
        if (_isNeedAnimator == false)
        {
            if (_isFalling == true && _isFalled == false)
            {
                _isFalled = TryFalling();
                Debug.Log("Falling");
            }            
            else
            {
                Debug.Log("Off");
                DisablingObject();
            }

            /*
            else if (_isCompressing == true && _isCompressed == false)
            {
                TryCompressing();
            }
            */
        }
    }

    public void DisablingObject()
    {
        gameObject.SetActive(false);
    }

    private bool TryFalling()
    {       
        bool result = false;

        if (_angle == _angleX 
            && transform.localPosition.x > _fallingToValue)
        {
            transform.Translate(-_speedFalling * Time.deltaTime, 0f, 0f);
        }
        else if (_angle == _angleY 
            && transform.localPosition.y > _fallingToValue)
        {
            transform.Translate(0f, -_speedFalling * Time.deltaTime, 0f);
        }
        else if (_angle == _angleZ 
            && transform.localPosition.z > _fallingToValue)
        {            
            transform.Translate(0f, 0f, -_speedFalling * Time.deltaTime);
        }
        else
        {
            result = true;
        }

        return result;
    }

    /*
    private bool TryCompressing()
    {
        bool result = false;

        if (transform.localScale.x > _compressToValue.x)
        {
            transform.localScale -= new Vector3
                    (_speedCompressing * Time.deltaTime, 0f, 0f);
        }
        
        if (transform.localScale.y > _compressToValue.y)
        {
            transform.localScale -= new Vector3
                    (0f, _speedCompressing * Time.deltaTime, 0f);
        }

        if (transform.localScale.z > _compressToValue.z)
        {
            transform.localScale -= new Vector3
                    (0f, 0f, _speedCompressing * Time.deltaTime);
        }

        return result;
    }
    */

    private void DisablingCollieders()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
    }

    private void DisablingRotator()
    {
        if (_rotator != null)
        {
            _rotator.enabled = false;
        }
    }

    private void TryStartingAnimator()
    {
        if (_animator != null)
        {
            if (_isNeedAnimator == true)
            {
                _animator.enabled = true;
                _animator.SetTrigger("Disappear");
            }
            else
            {
                _animator.enabled = false;
            }
        }
    }
}