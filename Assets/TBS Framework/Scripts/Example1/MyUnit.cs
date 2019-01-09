using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyUnit : Unit
{
    public Color PlayerColor;
    public string UnitName;
    private Transform Highlighter;

    private float offset = 2.45f;

    public override void Initialize()
    {
        base.Initialize();

        Highlighter = transform.Find("Highlighter");

        SetColor(PlayerColor);
        gameObject.transform.localPosition = Cell.transform.localPosition + new Vector3(0, offset, 0);
    }

    protected override void Defend(Unit other, int damage)
    {
        base.Defend(other, damage);
        UpdateHpBar();
    }

    public override void Move(Cell destinationCell, List<Cell> path)
    {
        base.Move(destinationCell, path);
    }

    public override void MarkAsAttacking(Unit other)
    {
        StartCoroutine(Jerk(other));
    }
    public override void MarkAsDefending(Unit other)
    {
        StartCoroutine(Glow(new Color(1, 0.5f, 0.5f), 1));
    }
    public override void MarkAsDestroyed()
    {
    }

    private IEnumerator Jerk(Unit other)
    {
        var heading = other.transform.localPosition - transform.localPosition;
        var direction = heading / heading.magnitude;
        float startTime = Time.time;

        while (startTime + 0.25f > Time.time)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + (direction / 2.5f), ((startTime + 0.25f) - Time.time));
            yield return 0;
        }
        startTime = Time.time;
        while (startTime + 0.25f > Time.time)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition - (direction / 2.5f), ((startTime + 0.25f) - Time.time));
            yield return 0;
        }
        transform.localPosition = Cell.transform.localPosition + new Vector3(0, offset, 0); ;
    }
    private IEnumerator Glow(Color color, float cooloutTime)
    {
        float startTime = Time.time;

        while (startTime + cooloutTime > Time.time)
        {
            SetColor(Color.Lerp(PlayerColor, color, (startTime + cooloutTime) - Time.time));
            yield return 0;
        }

        SetColor(PlayerColor);
    }

    public override void MarkAsFriendly()
    {
        SetHighlighterColor(new Color(0.8f,1,0.8f));
    }
    public override void MarkAsReachableEnemy()
    {
        SetHighlighterColor(Color.red);
    }
    public override void MarkAsSelected()
    {
        SetHighlighterColor(new Color(0,1,0));
    }
    public override void MarkAsFinished()
    {
        SetColor(PlayerColor - Color.gray);
        SetHighlighterColor(new Color(0.8f, 1, 0.8f));
    }
    public override void UnMark()
    {
        SetColor(PlayerColor);
        SetHighlighterColor(Color.white);
    }

    private void UpdateHpBar()
    {
        if (GetComponentInChildren<Image>() != null)
        {
            GetComponentInChildren<Image>().transform.localScale = new Vector3((float)((float)HitPoints / (float)TotalHitPoints), 1, 1);
            GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green,
                (float)((float)HitPoints / (float)TotalHitPoints));
        }
    }
    private void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
    private void SetHighlighterColor(Color color)
    {
        Highlighter.GetComponent<Renderer>().material.color = color;
    }
}