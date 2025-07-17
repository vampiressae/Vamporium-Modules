using UnityEngine;
using UnityEngine.UI;

public class ShapeBehaviourTest : MonoBehaviour
{
    public RectTransform RT;
    [SerializeField] private Image _image;
    [SerializeField] private Color _activeColor, _inactiveColor;

    private bool _active;

    public void Init(Vector2Int index, float size, Vector2Int area)
    {
        var position = new Vector2(index.x - (area.x * 0.5f) + 0.5f, index.y - (area.y * 0.5f) + 0.5f) * size;

        RT.anchoredPosition = position;
        RT.sizeDelta = Vector2.one * size;

        SetActive(false, true);
    }

    public void SetActive(bool active, bool force = false)
    {
        if (!force && _active == active) return;

        _active = active;
        _image.color = _active ? _activeColor : _inactiveColor;
    }
}
