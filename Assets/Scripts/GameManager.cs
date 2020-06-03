using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepsTxt;
    [SerializeField] private int countSteps = 0;
    [SerializeField] private float _stepTile = 1.2f;
    [SerializeField] private float _startPosZ = 3.6f;
    private float _startPosX = 0f;
    private float _startPosY = 0f;
    private List<int> _originList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0 };
    private List<int> _workList;

    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("is Null");

            return _instance;
        }
    }

    public MoveDirection DetermDirection(GameObject tileObj)
    {
        MoveDirection direction;
        int tileIndex = tileObj.GetComponent<MoveTile>().IndexInList;
        int tileRow = tileIndex / 4;
        int tileCol = tileIndex % 4;
        int voidIndex = _workList.IndexOf(0);
        int voidRow = voidIndex / 4;
        int voidCol = voidIndex % 4;

        if (tileRow - 1 == voidRow && tileCol == voidCol) direction = MoveDirection.Up;
        else if (tileCol + 1 == voidCol && tileRow == voidRow) direction = MoveDirection.Right;
        else if (tileRow + 1 == voidRow && tileCol == voidCol) direction = MoveDirection.Down;
        else if (tileCol - 1 == voidCol && tileRow == voidRow) direction = MoveDirection.Left;
        else return MoveDirection.None;

        tileObj.GetComponent<MoveTile>().IndexInList = voidIndex;
        _workList[voidIndex] = int.Parse(tileObj.GetComponentInChildren<TextMeshProUGUI>().text);
        _workList[tileIndex] = 0;

        stepsTxt.text = "Score: " + ++countSteps;

        return direction;
    }

    public void CheckSolved()
    {
        if (_originList.SequenceEqual(_workList))
        {
            StartCoroutine(DelayShowSolverScreen());
        }
    }

    public void StartGame()
    {
        _workList = new List<int>(_originList);

        do
        {
            ShuffleTiles();
        } while (!CanSolve());

        SpawnTiles();

        countSteps = 0;
        stepsTxt.text = "Score: " + countSteps;
    }

    public void RestartGame()
    {
        ScreenManager.Instance.ShowGameScreen();
        TilesPooler.Instance.HideTiles();
        StartGame();
    }

    private void Awake()
    {
        _instance = this;
    }

    private IEnumerator DelayShowSolverScreen()
    {
        yield return new WaitForSeconds(1f);
        ScreenManager.Instance.ShowSolvedScreen();
    }

    private void ShuffleTiles()
    {
        for (var i = 0; i < _workList.Count; i++)
        {
            _workList.Swap(_workList[i], _workList[Random.Range(i, _workList.Count)]);
        }
    }

    private bool CanSolve()
    {
        int inversion = 0;

        for (var i = 0; i < _workList.Count; i++)
        {
            if (_workList[i] != 0)
            {
                for (var j = 0; j < i; j++)
                {
                    if (_workList[j] > _workList[i]) inversion++;
                }
            }
            else
            {
                inversion += 1 + i / 4;
            }
        }

        return inversion % 2 == 0 ? true : false;
    }

    private void SpawnTiles()
    {
        for (var i = 0; i < _workList.Count; i++)
        {
            float x = i % 4 * _stepTile;
            float z = i / 4 * _stepTile;

            if (_workList[i] == 0) continue;

            GameObject pooledTile = TilesPooler.Instance.GetPooledTile();
            if (pooledTile != null)
            {
                pooledTile.SetActive(true);
                pooledTile.transform.position = new Vector3(_startPosX + x, _startPosY, _startPosZ - z);
                pooledTile.GetComponentInChildren<TextMeshProUGUI>().text = _workList[i].ToString();
                pooledTile.GetComponent<MoveTile>().IndexInList = i;
            }
        }
    }
}
