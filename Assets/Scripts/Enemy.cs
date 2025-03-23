using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    private Transform playerTransform;

    [HideInInspector]
    public UnityEvent OnEnemyDestroyed;

    public void Initialize(Transform player)
    {
        playerTransform = player;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke();
    }
}
