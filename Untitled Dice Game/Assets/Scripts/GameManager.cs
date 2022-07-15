using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private GameObject _player1, _player2;
    private int _diceSideThrown = 0;
    public int _player1StartWayPoint = 0;
    public int _player2StartWayPoint = 0;

    public Transform[] Waypoints => _waypoints;
    public int DiceSideThrown { get => _diceSideThrown; set => _diceSideThrown = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (_player1.GetComponent<FollowPath>().WayPointIndex > _player1StartWayPoint + _diceSideThrown)
        {
            _player1.GetComponent<FollowPath>().IsAllowedToMove = false;
            _player1StartWayPoint = _player1.GetComponent<FollowPath>().WayPointIndex - 1;
        }

        if (_player2.GetComponent<FollowPath>().WayPointIndex > _player2StartWayPoint + _diceSideThrown)
        {
            _player2.GetComponent<FollowPath>().IsAllowedToMove = false;
            _player2StartWayPoint = _player2.GetComponent<FollowPath>().WayPointIndex - 1;
        }

        //if (_player1.GetComponent<FollowPath>().WayPointIndex == GameManager.Instance.Waypoints.Length)
        //{
        //    _player1.transform.position = GameManager.Instance.Waypoints[0].transform.position;
        //    _player1StartWayPoint = 0;
        //}

        //if (_player2.GetComponent<FollowPath>().WayPointIndex == GameManager.Instance.Waypoints.Length)
        //{
        //    _player2.transform.position = GameManager.Instance.Waypoints[0].transform.position;
        //    _player2StartWayPoint = 0;
        //}
    }

    public void MovePlayer(int playerToMove)
    {
        switch (playerToMove)
        {
            case 1:
                _player1.GetComponent<FollowPath>().IsAllowedToMove = true;
                _player1.GetComponent<FollowPath>().SpacesMoved = _diceSideThrown;
                break;
            case 2:
                _player2.GetComponent<FollowPath>().IsAllowedToMove = true;
                _player2.GetComponent<FollowPath>().SpacesMoved = _diceSideThrown;
                break;
        }
    }
}