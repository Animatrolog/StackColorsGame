using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class StartPanel : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _tutorial;

    public Action OnPanelTouched;

    private void Start()
    {
        if (GameDataManager.GameData.Level == 2)
            _tutorial.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameStateManager.Instance.SetState(GameState.Game);
        OnPanelTouched?.Invoke();
    }
}
