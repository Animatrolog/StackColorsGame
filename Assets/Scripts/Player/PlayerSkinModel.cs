using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerSkinModel : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _bodyRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rig _animatonRig;
    [SerializeField] private Transform _rightHandIkTarget;
    [SerializeField] private Transform _leftHandIkTarget;
    [SerializeField] private Sprite _skinSprite;

    public SkinnedMeshRenderer BodyRenderer => _bodyRenderer;
    public Animator Animator => _animator;
    public Rig AnimatonRig => _animatonRig;
    public Transform RightHandIkTarget => _rightHandIkTarget;
    public Transform LeftHandIkTarget => _leftHandIkTarget;
    public Sprite SkinSprite => _skinSprite;
}
