using System;
using UnityEngine;

public class UpgradeEffects : MonoBehaviour
{
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _upgradeClip;
    [SerializeField] private ParticleSystem _particlePrefab;

    private void OnEnable()
    {
        _upgradeManager.OnUpgrade += HandleUpgrade;
    }

    private void OnDisable()
    {
        _upgradeManager.OnUpgrade -= HandleUpgrade;
    }

    private void HandleUpgrade(UpgradeType type)
    {
        if(GameDataManager.GameData.IsSoundEnabled)
            _audioSource.PlayOneShot(_upgradeClip);

        Transform player = GameManager.Instance.Player.transform;
        ParticleSystem particles = Instantiate(_particlePrefab, player);
    }
}
