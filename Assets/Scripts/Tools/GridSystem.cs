using System.Collections.Generic;
using UnityEngine;

public class GridSystem<T> : Singleton<GridSystem<T>>
{
    private T[,] data;
    private Vector2Int diemnsions = new Vector2Int(1,1);
    public Vector2Int Diemnsions
    {
        get
        {
            return diemnsions;
        }
    }
    private bool isReady;
    public bool IsReady
    {
        get
        {
            return isReady;
        }
    }
    public void InitializeGrid(Vector2Int diemnsions)
    {
        if (diemnsions.x < 0 || diemnsions.y < 0)
            Debug.LogWarning("Grid diemnsions must be positive numbers.");

        this.diemnsions = diemnsions;
        data = new T[diemnsions.x, diemnsions.y];
    }
    public void Clear()
    {
        data = new T[diemnsions.x,diemnsions.y];
    }

    public bool CheckBounds(int x, int y)
    {
        return x >= 0 && x < diemnsions.x && y >= 0 && y < diemnsions.y;
    }
    public bool CheckBounds(Vector2Int positions)
    {
        if (!isReady)
            Debug.LogError("Grid has not been initialized");
        return CheckBounds(positions.x, positions.y);
    }
    public bool IsEmpty(int x,int y)
    {
        if(!CheckBounds(x, y)) 
             Debug.LogError($"({x}),({y}) are not on the grid.");

        return EqualityComparer<T>.Default.Equals(data[x,y],default(T));

    }
    public bool IsEmpty(Vector2Int positions)
    {
        return IsEmpty(positions.x, positions.y);
    }

    public bool PutItemAt(T item, int x, int y, bool allowOnWrite = false)
    {
        if (!CheckBounds(x, y))
            Debug.LogError($"({x}),({y}) are not on the grid.");

        if (!allowOnWrite && !IsEmpty(x, y))
            return false;

        data[x,y] = item;
        return true;
    }
    public bool PutItemAt(T item, Vector2Int positions, bool allowOnWrite = false)
    {
        return PutItemAt(item, positions.x, positions.y, allowOnWrite);
    }
    public T GetItemAt(int x, int y)
    {
        if (!CheckBounds(x, y))
            Debug.LogError($"({x}),({y}) are not on the grid.");

        return data[x,y];
    }
    public T GetItemAt(Vector2Int position)
    {
        return GetItemAt(position.x, position.y);
    }
    public T RemoveItemAt(int x, int y)
    {
        if (!CheckBounds(x, y))
            Debug.LogError($"({x}),({y}) are not on the grid.");

        T temp = data[x,y];
        data[x,y] = default(T);

        return temp;
    }

    public T RemoveItemAt(Vector2Int position)
    {
        return RemoveItemAt(position.x, position.y);
    }

    public void SwapItemsAt(int x1,int y1, int x2, int y2)
    {
        if (!CheckBounds(x1, y1))
            Debug.LogError($"({x1}),({y1}) are not on the grid.");

        if (!CheckBounds(x2, y2))
            Debug.LogError($"({x2}),({y2}) are not on the grid.");

        T temp = data[x1,y1];
        data[x1, y1] = data[x2,y2];
        data[x2,y2] = temp;
    }

    public void SwapItemsAt(Vector2Int position1, Vector2Int position2)
    {
        SwapItemsAt(position1.x,position1.y,position2.x,position2.y);
    }

    public override string ToString()
    {
        string s = "";

        for (int y = diemnsions.y - 1; y != -1; --y)
        {
            s += "[";
            for (int x = diemnsions.x - 1; x != diemnsions.x; ++x)
            {
                if (IsEmpty(x, y))
                    s += " ";
                else
                    s += data[x, y].ToString();

                if(x != diemnsions.x - 1)
                    s += ", ";
            }
        }
        return s;
    }
}
