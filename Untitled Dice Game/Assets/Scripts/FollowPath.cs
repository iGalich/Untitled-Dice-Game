using UnityEngine;

public class FollowPath : MonoBehaviour
{
    
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private int _waypointIndex = 0;
    public bool _isAllowedToMove = false;
    private int _spacesMoved = 0;

    public int WayPointIndex { get => _waypointIndex; set => _waypointIndex = value; }
    public bool IsAllowedToMove { get => _isAllowedToMove; set => _isAllowedToMove = value; }
    public int SpacesMoved { get => _spacesMoved; set => _spacesMoved = value; }

    private void Start()
    {
        transform.position = GameManager.Instance.Waypoints[_waypointIndex].transform.position;
    }

    private void Update()
    {
        var delta = Time.deltaTime;

        if (_isAllowedToMove)
        {
            Move(delta);
        }
    }

    private void Move(float delta)
    {
        transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.Waypoints[_waypointIndex].transform.position, _moveSpeed * delta);

        if (transform.position == GameManager.Instance.Waypoints[_waypointIndex].transform.position)
        {
            _waypointIndex++;
        }

        if (_waypointIndex == GameManager.Instance.Waypoints.Length)
        {
            _waypointIndex = 0;
        }
    }
}