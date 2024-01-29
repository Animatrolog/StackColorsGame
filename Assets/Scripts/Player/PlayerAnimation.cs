using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerSkinChanger _skinChanger;
    [SerializeField] private Transform _leftHandPlacement;
    [SerializeField] private Transform _rightHandPlacement;

    private PlayerSkinModel _skinModel;
    private Player _player;
    private Animator _animator;
    private Coroutine _speedChangeCoroutine;

    public void Initialize(Player player)
    {
        _player = player;
    }

    private void OnEnable()
    {
        GameStateManager.OnStateChange += HandleGameStateChange;
        _skinChanger.OnSkinChanged += HandleSkinChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HandleGameStateChange;
        _skinChanger.OnSkinChanged += HandleSkinChange;
    }

    private void HandleSkinChange(int index)
    {
        _skinModel = _skinChanger.CurrentSkinInstance;
        _animator = _skinModel.Animator;

        _skinModel.RightHandIkTarget.position = _rightHandPlacement.position;
        _skinModel.RightHandIkTarget.rotation = _rightHandPlacement.rotation;

        _skinModel.LeftHandIkTarget.position = _leftHandPlacement.position;
        _skinModel.LeftHandIkTarget.rotation = _leftHandPlacement.rotation;
    }

    private void HandleGameStateChange(GameState state)
    {
        if(state == GameState.Game)
        {
            _speedChangeCoroutine = StartCoroutine(SpeedChange());
        }
        else if (state == GameState.Defeat)
        {
            StartSadAnimation();
        }
        else if (state == GameState.Finish)
        {
            StartIdleAnimation();
        }
    }

    private IEnumerator SpeedChange()
    {
        while(true)
        {
            float speed = _player.Movement.CurrentSpeed;
            
            if(!_animator.GetBool("isRunning"))
            {
                _animator.SetBool("isRunning", speed > 0);
                yield return null;
            }

            if (speed <= 1f)
            {
                _animator.SetFloat("speed", speed);   
            }
            else
            {
                _animator.SetFloat("speed", 1f);
            }
            if(_skinModel.AnimatonRig.weight < (speed / 7.5f)) _skinModel.AnimatonRig.weight = (speed / 7.5f);
            _animator.speed = (speed / 7.5f);
            yield return null;
        }
    }

    public void StartKickAnimation()
    {
        if(_speedChangeCoroutine != null)
        {
            StopCoroutine(_speedChangeCoroutine);
            _speedChangeCoroutine = null;
        }
        _skinModel.AnimatonRig.weight = 0f;
        _animator.speed = 1f;
        _animator.SetTrigger("kick");
    }

    public void StartIdleAnimation()
    {
        if (_speedChangeCoroutine != null)
        {
            StopCoroutine(_speedChangeCoroutine);
            _speedChangeCoroutine = null;
        }
        _skinModel.AnimatonRig.weight = 0f;
        _animator.speed = 1f;
        _animator.SetBool("isRunning", true);
        _animator.SetFloat("speed", 0f);
    }

    public void StartSadAnimation()
    {
        if (_speedChangeCoroutine != null)
        {
            StopCoroutine(_speedChangeCoroutine);
            _speedChangeCoroutine = null;
        }
        _animator.SetBool("isRunning", false);
        _skinModel.AnimatonRig.weight = 0f;
        _animator.speed = 1f;
        _animator.SetTrigger("lose");
    }
}
