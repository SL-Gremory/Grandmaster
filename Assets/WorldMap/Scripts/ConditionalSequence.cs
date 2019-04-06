using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionalSequence : MonoBehaviour, ISequence
{
    [SerializeField]
    string hasFlag;
    [SerializeField]
    string doesntHaveFlag;
    [SerializeField]
    UnityEngine.Object sequence;
    [SerializeField]
    float endAfterTime;
    [SerializeField]
    UnityEvent functionOnStart;
    [SerializeField]
    UnityEvent functionOnEnd;

    bool ended = false;

    public void BeginSequence()
    {
        if (!PlayerFlags.HasFlag(hasFlag) || PlayerFlags.HasAnyFlag(doesntHaveFlag)){
            ended = true;
            return;
        }
        ended = false;
        functionOnStart.Invoke();
        if (sequence != null)
        {
            (sequence as ISequence).BeginSequence();
            StartCoroutine(WaitUntilSeqEndCoroutine());
        }
        else {
            StartCoroutine(WaitUntilTimeCoroutine());
        }
    }

    IEnumerator WaitUntilSeqEndCoroutine() {
        while (!(sequence as ISequence).HasSequenceEnded())
        {
            yield return null;
        }
        functionOnEnd.Invoke();
        ended = true;
    }

    IEnumerator WaitUntilTimeCoroutine()
    {
        yield return new WaitForSeconds(endAfterTime);
        functionOnEnd.Invoke();
        ended = true;
    }

    public bool HasSequenceEnded()
    {
        return ended;
    }

    private void OnValidate()
    {
        if (sequence != null && !(sequence is ISequence))
        {
            if (sequence is GameObject)
            {
                sequence = (sequence as GameObject).GetComponent<ISequence>() as Component;
                if (sequence == null)
                    Debug.LogWarning("No component on this GameObject implements ISequence interface.");
            }
            else
            {
                sequence = null;
                Debug.LogWarning("A Sequence must implement ISequence interface.");
            }
        }

        if (endAfterTime != 0 && sequence != null) {
            endAfterTime = 0;
            Debug.LogWarning("You can only 'end after time' when sequence is null.");
        }

    }
}
