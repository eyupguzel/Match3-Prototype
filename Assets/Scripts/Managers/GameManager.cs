using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    MatchablePool pool;
    MatchableGrid grid;

    [SerializeField] private Vector2Int dimensions = Vector2Int.one;

    [SerializeField] private Text gridOutPut;

    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;

        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        pool.PoolObject(dimensions.x * dimensions.y * 2);
        grid.InitializeGrid(dimensions);

        yield return null;

        StartCoroutine(grid.PopulateGrid());
    }
}
