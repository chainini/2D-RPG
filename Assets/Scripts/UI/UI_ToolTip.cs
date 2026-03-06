using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 100;

    public virtual void AdjustPosition()
    {
        Vector2 mousePos = Input.mousePosition;

        float new_xOffset = 0;
        float new_yOffset = 0;

        if (mousePos.x > xLimit)
            new_xOffset = -xOffset;
        else
            new_xOffset = xOffset;
        if (mousePos.y > yLimit)
            new_yOffset = -yOffset;
        else
            new_yOffset = yOffset;

        transform.position = new Vector2(mousePos.x + new_xOffset, mousePos.y + new_yOffset);
    }

    public virtual void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12)
            _text.fontSize = _text.fontSize * .8f;
    }
}
