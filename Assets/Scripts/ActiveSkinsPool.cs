using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActiveSkinsPool", menuName = "ScriptableObjects/ActiveSkinsPool", order = 1)]
public class ActiveSkinsPool : ScriptableObject
{
    [SerializeField] private List<PlayerSkinModel> _activeSkins;

    public List<PlayerSkinModel> ActiveSkins => _activeSkins;
}
