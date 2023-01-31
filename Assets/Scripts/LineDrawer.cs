
using System.Collections.Generic;
using DrawAndRun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float width;
    [SerializeField] private float splitter = 50;
    [SerializeField] private float requiredMinimumDistance;

    private List<Vector2> _positions;

    private Vector2 _offset;
    [SerializeField] private RectTransform rect;

    public delegate void EndDraw(List<Vector2> positions, Vector2 sizeDelta);

    private void Start()
    {
        _positions = new List<Vector2>();
    }

    public event EndDraw OnEndDraw;

    private void AddPoint(Vector2 pos)
    {
        if (_positions.Count > 0)
        {
            var lastPos = _positions[_positions.Count - 1];
            if ((lastPos - pos).magnitude < requiredMinimumDistance)
                return;
            DrawEdge(lastPos, pos);
        }

        _positions.Add(pos);
    }

    private void DrawEdge(Vector2 lastPos, Vector2 pos)
    {
        var newRect = new GameObject("edge", typeof(RectTransform), typeof(Image)).GetComponent<RectTransform>();
        newRect.SetParent(transform);
        var delta = pos - lastPos;
        newRect.anchoredPosition = delta / 2 + lastPos;
        newRect.sizeDelta = new Vector2(delta.magnitude, width);
        newRect.localEulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.Clear();
        _positions.Clear();
        _offset = -rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var relativePos = eventData.position + _offset;
        AddPoint(relativePos);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        /*var positions = new Vector3[lineRenderer.positionCount];
        for (var i = 0; i < lineRenderer.positionCount; i++)
            positions[i] = lineRenderer.GetPosition(i);*/


        OnEndDraw?.Invoke(SplitEvenly(_positions), rect.sizeDelta);
    }

    private List<Vector2> SplitEvenly(IReadOnlyList<Vector2> positions)
    {
        var result = new List<Vector2>();
        for (var i = 1; i < _positions.Count; i++)
        {
            result.Add(positions[i - 1]);
            var delta = positions[i] - positions[i - 1];
            var splitCount = Mathf.FloorToInt(delta.magnitude / splitter);
            for (var k = 0; k < splitCount; k++)
                result.Add(result[result.Count - 1] + delta.normalized * splitter * k);
            result.Add(positions[i]);
        }

        return result;
    }
}