using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeClueFinger : MonoBehaviour
{
    [SerializeField] private RectTransform _finger;
    [SerializeField] private List<UIButton> _buttons;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Vector3 _offset;

    private Coroutine _animationCoroutine;

    private void OnEnable()
    {
        _animationCoroutine = StartCoroutine(Animation());
    }

    private void OnDisable()
    {
        StopCoroutine(_animationCoroutine);
        _animationCoroutine = null;
    }

    private IEnumerator Animation()
    {
        float delay = _duration / _buttons.Count;
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            for(int i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];

                if(button.Interactable)
                {
                    _finger.gameObject.SetActive(true);
                    _finger.localPosition = button.transform.localPosition + _offset;
                    if (_finger.TryGetComponent(out CounterAnimation anim))
                        anim.Animate();
                }
                else _finger.gameObject.SetActive(false);

                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }
}
