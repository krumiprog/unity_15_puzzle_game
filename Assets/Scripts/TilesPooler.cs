using System.Collections.Generic;
using UnityEngine;

public class TilesPooler : MonoBehaviour
{
    [SerializeField] private GameObject _tileToPool;
    private List<GameObject> _poolTiles;
    private int _amountToPool = 15;

    private static TilesPooler _instance;
    
    public static TilesPooler Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("is Null");

            return _instance;
        }
    }

    public GameObject GetPooledTile()
    {
        for (var i = 0; i < _amountToPool; i++)
        {
            if (!_poolTiles[i].activeInHierarchy)
            {
                return _poolTiles[i];
            }
        }

        return null;
    }

    public void HideTiles()
    {
        for (var i = 0; i < _amountToPool; i++)
        {
            _poolTiles[i].SetActive(false);
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _poolTiles = new List<GameObject>();

        for (var i = 0; i < _amountToPool; i++)
        {
            GameObject tile = Instantiate(_tileToPool);
            tile.SetActive(false);
            _poolTiles.Add(tile);
            tile.transform.SetParent(this.transform);
        }
    }
}
