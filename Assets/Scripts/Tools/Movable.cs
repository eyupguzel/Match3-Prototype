using System.Collections;
using UnityEngine;

public class Movable : MonoBehaviour
{
    //coroutine move from current position to new position

    private Vector3 from, to;
    private float howFar;

    [SerializeField] private float speed = 1f;

    private bool idle = true;
    public bool Idle
    {
        get
        {
            return idle;
        }
    }

    public IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        if (speed < 0)
            Debug.LogWarning("Speed must be a positive number!");
        from = transform.position;
        to = targetPosition;
        howFar = 0;

        idle = false;
        do
        {
            howFar +=  speed * Time.deltaTime;
            if (howFar > 1)
                howFar = 1;
            transform.position = Vector3.LerpUnclamped(from, to, Easing(howFar));
            yield return null;
        }
        while (howFar != 1);
        idle = true;
    }

    private float Easing(float t)
    {
        return t;
    }
}
