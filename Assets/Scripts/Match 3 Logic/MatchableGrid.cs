using System;
using System.Collections;
using UnityEngine;

public class MatchableGrid : GridSystem<Matchable>
{
    private MatchablePool pool;
    private ScoreManager score;
    [SerializeField] private Vector3 offScreenOffset;
    private Vector3 onScreenPosition;
    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        score = ScoreManager.Instance;
    }
    public IEnumerator PopulateGrid(bool allowMatches = false)
    {
        Matchable newMatchable;

        for (int y = 0; y != Dimensions.y; ++y)
            for (int x = 0; x != Dimensions.x; ++x)
            {
                newMatchable = pool.GetRandomMatchable();

                // newMatchable.transform.position = transform.position + new Vector3(x, y);
                onScreenPosition = transform.position + new Vector3(x, y);
                newMatchable.transform.position = onScreenPosition + offScreenOffset;

                newMatchable.gameObject.SetActive(true);

                newMatchable.position = new Vector2Int(x, y);

                PutItemAt(newMatchable, x, y);
                int initialType = newMatchable.Type;

                while (!allowMatches && IsPartOfAMatch(newMatchable))
                {
                    yield return null;
                    if (pool.NextType(newMatchable) == initialType)
                        break;
                }

                StartCoroutine(newMatchable.MoveToPosition(onScreenPosition));

                yield return new WaitForSeconds(0.14f);
            }
    }

    private bool IsPartOfAMatch(Matchable toMatch)
    {
        int horizontalMatches = 0, verticalMatches = 0;

        horizontalMatches += CountMatchesInDirection(toMatch, Vector2Int.left);
        horizontalMatches += CountMatchesInDirection(toMatch, Vector2Int.right);

        if (horizontalMatches > 1)
            return true;

        verticalMatches += CountMatchesInDirection(toMatch, Vector2Int.up);
        verticalMatches += CountMatchesInDirection(toMatch, Vector2Int.down);

        if (verticalMatches > 1)
            return true;

        return false;
    }

    private int CountMatchesInDirection(Matchable toMatch,Vector2Int direction)
    {
        int matches = 0;
        Vector2Int position = toMatch.position + direction;

        while(CheckBounds(position) && !IsEmpty(position) && GetItemAt(position).Type == toMatch.Type)
        {
            ++matches;
            position += direction;
        }
        return matches;
    }

    private Match GetMatchesInDirection(Matchable toMatch, Vector2Int direction)
    {
        Vector2Int position = toMatch.position + direction;
        Match match = new Match();
        Matchable next;

        while (CheckBounds(position) && !IsEmpty(position))
        {
            next = GetItemAt(position);

            if (next.Type == toMatch.Type && next.Idle)
            {

                match.AddMatchable(next);
                position += direction;
            }
            else
                break;
        }

        return match;
    }

    public IEnumerator TrySwap(Matchable[] toBeSwapped)
    {
        Matchable[] copies = new Matchable[2];
        copies[0] = toBeSwapped[0];
        copies[1] = toBeSwapped[1];

        yield return StartCoroutine(Swap(copies));

        Match[] matches = new Match[2];

        matches[0] = GetMatch(copies[0]);
        matches[1] = GetMatch(copies[1]);

        if (matches[0] != null)
            StartCoroutine(score.ResolveMatch(matches[0]));

        if (matches[1] != null)
            StartCoroutine(score.ResolveMatch(matches[1]));

        if (matches[0] == null && matches[1] == null) 
             StartCoroutine(Swap(copies));
    }
    private Match GetMatch(Matchable toMatch)
    {
        Match match = new Match(toMatch);

        Match horizontalMatch,verticalMatch;

        horizontalMatch = GetMatchesInDirection(toMatch, Vector2Int.left);
        horizontalMatch.Merge(GetMatchesInDirection(toMatch, Vector2Int.right));

        verticalMatch = GetMatchesInDirection(toMatch, Vector2Int.up);
        verticalMatch.Merge(GetMatchesInDirection(toMatch, Vector2Int.down));


        if (horizontalMatch.Count > 1)
        {
            match.Merge(horizontalMatch);
        }
        if (verticalMatch.Count > 1)
        {
            match.Merge(verticalMatch);
        }

        if (match.Count == 1)
            return null;

        return match;
    }
    private IEnumerator Swap(Matchable[] toBeSwapped)
    {
        SwapItemsAt(toBeSwapped[0].position,toBeSwapped[1].position);

        Vector2Int temp = toBeSwapped[0].position;
        toBeSwapped[0].position = toBeSwapped[1].position;
        toBeSwapped[1].position = temp;

        Vector3[] worldPosition = new Vector3[2];

        worldPosition[0] = toBeSwapped[0].transform.position;
        worldPosition[1] = toBeSwapped[1].transform.position;

                        StartCoroutine(toBeSwapped[0].MoveToPosition(worldPosition[1]));
        yield return    StartCoroutine(toBeSwapped[1].MoveToPosition(worldPosition[0]));
    }
}
