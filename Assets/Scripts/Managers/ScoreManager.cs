using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreManager : Singleton<ScoreManager>
{
    private MatchableGrid grid;

    [SerializeField] private Transform collectionPoint;

    private Text scoreText;
    private int score;
    public int Score
    {
        get
        {
            return score;
        }
    }
    protected override void Init()
    {
       scoreText = GetComponent<Text>();
    }
    private void Start()
    {
        grid = (MatchableGrid)MatchableGrid.Instance;
    }
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"Score: {score}";
    }
    public IEnumerator ResolveMatch(Match toResolve)
    {
        Matchable matchable;

        for (int i = 0; i != toResolve.Count; i++)
        {
            matchable = toResolve.Matchables[i];

            grid.RemoveItemAt(matchable.position);

            if (i == toResolve.Count - 1)
                StartCoroutine(matchable.Resolve(collectionPoint));
            else
                StartCoroutine(matchable.Resolve(collectionPoint));
        }

        AddScore(toResolve.Count * toResolve.Count);

        yield return null;
    }
}
