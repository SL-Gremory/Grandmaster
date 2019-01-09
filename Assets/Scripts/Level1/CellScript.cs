using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : Square
{

    private void SetColor(Color color)
    {
        var highlight = transform.Find("Highlighter");
        var spriteRenderer = highlight.GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }


    public override Vector3 GetCellDimensions()
    {
        Vector3 dimensions = GetComponent<SpriteRenderer>().bounds.size;
        return dimensions;
    }


    public override void MarkAsHighlighted()
    {
        SetColor(new Color(0.8f, 0.8f, 0.8f, 0.5f));
    }

    public override void MarkAsPath()
    {
        SetColor(new Color(0, 1, 0, 0.5f));
    }

    public override void MarkAsReachable()
    {
        SetColor(new Color(1, 0.92f, 0.16f, 0.5f));
    }

    public override void UnMark()
    {
        SetColor(new Color(1, 1, 1, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Highlighter").GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
