using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
public class Matchable : Movable
{
    private MatchablePool pool;
    private Cursor cursor;
    private int type;
    public int Type
    {
        get
        {
            return type;
        }
    }
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        cursor = Cursor.Instance;
        pool = (MatchablePool)MatchablePool.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector2Int position;
    private void OnMouseDown()
    {
        cursor.SelectFirst(this);
        //print($"Mouse down at ({position.x}) , ({position.y})");
    }
    private void OnMouseUp()
    {
        cursor.SelectFirst(null);
        //print($"Mouse up at ({position.x}) , ({position.y})");
    }
    private void OnMouseEnter()
    {
        cursor.SelectSecond(this);
       // print($"Mouse enter at ({position.x}) , ({position.y})");
    }
    public void SetType(int type, Sprite sprite, Color color)
    {
        this.type = type;
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
    }
    public IEnumerator Resolve(Transform collectionPoint)
    {
        spriteRenderer.sortingOrder = 2;

        yield return StartCoroutine(MoveToPosition(collectionPoint.position));

        spriteRenderer.sortingOrder = 1;

        pool.ReturnObjectToPool(this);
    }
    public override string ToString()
    {
        return gameObject.name;
    }
}
