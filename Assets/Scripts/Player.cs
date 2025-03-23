using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectilePrefab;

    [SerializeField]
    private Transform _projectileSpawnPoint;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private AudioClip _moveClip, _pointClip, _loseClip;

    private PlayerProjectile _currentProjectile;
    private float _shootCooldown = 2f;
    private float _timeSinceLastShot;
    private float _spawnAngle = 0f;

    private void Start()
    {
        SpawnProjectile();
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        //Debug.Log("Time: " + _timeSinceLastShot);

        if (_currentProjectile == null && _timeSinceLastShot >= _shootCooldown)
        {
            //Debug.Log("Spawn");
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        GameObject projectileObject = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity);
        _currentProjectile = projectileObject.GetComponent<PlayerProjectile>();
        _currentProjectile.Initialize(this, _spawnAngle);
        _timeSinceLastShot = 0f;
    }

    public void OnProjectileShot()
    {
        _spawnAngle = _currentProjectile.RotateAngle;
        _currentProjectile = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.Tags.Enemy))
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if(_currentProjectile)
        {
            Destroy(_currentProjectile.gameObject);
        }

        AudioManager.Instance.PlaySound(_loseClip);
        Destroy(Instantiate(_explosionPrefab, transform.position, Quaternion.identity), 5f);
        GameManager.Instance.EndGame();
        Destroy(gameObject);
    }
}
