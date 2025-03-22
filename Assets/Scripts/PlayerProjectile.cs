using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    //[SerializeField]
    //private Transform _bodyRoot;

    [SerializeField]
    private float _rotateRadius, _rotateSpeed, _moveSpeed;

    private Vector3 centerPos;

    private bool canMove;
    private bool canRotate;
    private bool canShoot;
    private bool shooting;

    private float rotateAngle;
    public float RotateAngle => rotateAngle;
    private Vector3 moveDirection;
    private Vector3 _startCenterPos;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private AudioClip _moveClip, _pointClip, _loseClip;

    private Player _player;

    public void Initialize(Player player, float angle)
    {
        _player = player;
        canRotate = true;
        canShoot = true;
        canMove = false;
        _startCenterPos = player.transform.position;
        centerPos = _startCenterPos;
        rotateAngle = angle;
    }

    private void Update()
    {
        if (canShoot && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (canRotate)
        {
            rotateAngle += _rotateSpeed * Time.fixedDeltaTime;
            moveDirection = new Vector3(Mathf.Cos(rotateAngle * Mathf.Deg2Rad)
                , Mathf.Sin(rotateAngle * Mathf.Deg2Rad), 0f).normalized;
            transform.position = centerPos + _rotateRadius * moveDirection;
            if (rotateAngle >= 360f) rotateAngle = 0f;
        }
        else if (canMove)
        {
            transform.position += _moveSpeed * Time.fixedDeltaTime * moveDirection;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.Tags.Obstacle))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag(Constants.Tags.Enemy))
        {
            AudioManager.Instance.PlaySound(_pointClip);
            GameManager.Instance.UpdateScore();

            // Destroy the enemy and spawn destruction particle
            Destroy(Instantiate(_explosionPrefab, collision.transform.position, Quaternion.identity), 5f);
            Destroy(collision.gameObject);
        }
    }

    private void Shoot()
    {
        AudioManager.Instance.PlaySound(_moveClip);
        canRotate = false;
        canShoot = false;
        canMove = true;
        moveDirection = (transform.position - centerPos).normalized;
        _player.OnProjectileShot();
    }
}
