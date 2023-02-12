using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TDGrid : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(16, 16);
    public TDCell CellPrefab;
    public Dictionary<Vector2Int, TDCell> cells = new Dictionary<Vector2Int, TDCell>();
    public List<TDCell> wayPoints = new List<TDCell>();

    public GameObject moveObject;
    public float moveSpeed = 1;
    private float t;
    private bool isMoving;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var c = GetCellPosition(x, y);
                var cell = Instantiate(CellPrefab, new Vector3(c.x, 0, c.y), Quaternion.identity);
                cell.transform.parent = transform;
                cell.Init();
                cells.Add(new Vector2Int(x, y), cell);
            }
        }
    }

    public bool IsNeighbour(TDCell cell, TDCell targetCell)
    {
        var target = cells.FirstOrDefault(a => a.Value == cell).Key;

        var w = cells.TryGetValue(target + Vector2Int.up, out cell);
        if (cell == targetCell)
            return true;

        var a = cells.TryGetValue(target + Vector2Int.left, out cell);
        if (cell == targetCell)
            return true;

        var s = cells.TryGetValue(target + Vector2Int.down, out cell);
        if (cell == targetCell)
            return true;

        var d = cells.TryGetValue(target + Vector2Int.right, out cell);
        if (cell == targetCell)
            return true;

        return false;
    }

    void Update()
    {
        if (Physics.Raycast(Camera.allCameras[0].ScreenPointToRay(Input.mousePosition), out var raycastHit) && Input.GetMouseButton(0) && !isMoving)
        {
            if(raycastHit.collider.TryGetComponent(out TDCell cell))
            {
                if(wayPoints.Count > 0)
                {
                    if (IsNeighbour(wayPoints[^1], cell) && !wayPoints.Contains(cell)) 
                    {
                        cell.mat.color = Color.green;
                        wayPoints.Add(cell);
                    }
                }
                else
                {
                    cell.mat.color = Color.green;
                    wayPoints.Add(cell);
                }
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            isMoving = true;
        }
        if (isMoving)
        {
            t += Time.deltaTime * moveSpeed;
            Vector3 targetPosition = Multilerp.MultilerpFunction(wayPoints.Select(a => a.transform.position).ToArray(), Mathf.PingPong(t, 1));
            moveObject.transform.position = targetPosition;
        }
    }

    private Vector2Int GetCellPosition(int x, int y)
    {
        return new Vector2Int(gridSize.x / 2 - x, gridSize.y / 2 - y);
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var c = GetCellPosition(x, y);
                Gizmos.DrawWireCube(new Vector3(c.x, 0, c.y), Vector3.one);
            }
        }
    }
}
