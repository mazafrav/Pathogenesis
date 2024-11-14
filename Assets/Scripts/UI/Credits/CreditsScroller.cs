using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    #region References
    [SerializeField] private RectTransform _textRectTransform;

    #endregion

    #region Parameters
    [SerializeField] private float _totalScrollingTime;
    #endregion

    #region Attributes
    private float _time = 0;
    private Vector2 _originalPosition = Vector2.zero;
    #endregion

    #region Methods

    private void Start()
    {
        _originalPosition = _textRectTransform.anchoredPosition;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time < _totalScrollingTime)
        {
            _textRectTransform.anchoredPosition = Vector2.Lerp(_originalPosition, Vector2.zero, _time/ _totalScrollingTime);
        }
    }

    #endregion
}
