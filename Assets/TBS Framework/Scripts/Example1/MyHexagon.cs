using UnityEngine;

class MyHexagon : Hexagon
{
    private Renderer hexagonRenderer;
    private Renderer outlineRenderer;

    public void Awake()
    {
        hexagonRenderer = GetComponent<Renderer>();

        var outline = transform.Find("Outline");
        outlineRenderer = outline.GetComponent<Renderer>();

        SetColor(hexagonRenderer, Color.white);
        SetColor(outlineRenderer, Color.black);
    }

    public override Vector3 GetCellDimensions()
    {
        var outline = transform.Find("Outline");
        var outlineRenderer = outline.GetComponent<Renderer>();
        return outlineRenderer.bounds.size;
    }

    public override void MarkAsReachable()
    {
        SetColor(hexagonRenderer, Color.yellow);
    }
    public override void MarkAsPath()
    {
        SetColor(hexagonRenderer, Color.green);;
    }
    public override void MarkAsHighlighted()
    {
        SetColor(outlineRenderer, Color.blue);
    }
    public override void UnMark()
    {
        SetColor(hexagonRenderer, Color.white);
        SetColor(outlineRenderer, Color.black);
    }

    private void SetColor(Renderer renderer, Color color)
    {
        renderer.material.color = color;
    }
}

