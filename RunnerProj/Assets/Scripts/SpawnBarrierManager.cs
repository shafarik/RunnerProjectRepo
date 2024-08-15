using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnBarrierManager : MonoBehaviour
{

    [SerializeField] private int _distanceBetweenBarriers = 10;
    [SerializeField] private int _firstBarrierDistance = 50;

    [Header("Components")]
    [SerializeField] private Player _player;
    [SerializeField] private List<GameObject> _pool;
    [SerializeField] private List<GameObject> _linesList;
    private int _playersScore;


    [SerializeField] private int _safeDistance = 10;

    private int _newBarrierXPosition;

    // Start is called before the first frame update
    void Start()
    {
        ResetBarrierxPosition();
    }

    // Update is called once per frame
    void Update()
    {
        _playersScore = (int)_player.transform.position.x;

        if (_playersScore >= _newBarrierXPosition)
        {
            SpawnBarrier();
            _newBarrierXPosition += _distanceBetweenBarriers;
        }
    }

    private void SpawnBarrier()
    {
        Instantiate(_pool[Random.Range(0, _pool.Count)],
            new Vector3(_newBarrierXPosition + _firstBarrierDistance, 0, _linesList[Random.Range(0, _linesList.Count)].transform.position.z),
            Quaternion.identity);
    }

    private void OnDestroy()
    {

    }

    public void ResetBarrierxPosition()
    {
        _newBarrierXPosition = _safeDistance;
    }
}
