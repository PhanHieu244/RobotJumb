using System;
using System.Collections;
using ChuongCustom;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Robot : Singleton<Robot>
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite normal, jump;
    [SerializeField] private float speed = 2.4f;

    public event Action OnStanding;
    public event Action OnFailStanding;
    
    private Coroutine _down;

    public float posStand;

    [Button]
    public void Jump()
    {
        StopAllCoroutines();
        _renderer.sprite = jump;
        Move(Vector2.up, speed);   
    }

    [Button]
    public void Down(float stand)
    {
        posStand = stand;
        StopAllCoroutines();
        _renderer.sprite = normal;
        StartCoroutine(MovementHelper.IEMove(transform, Vector2.down, speed * 1.5f, StopCondition, OnDownCompleted));
    }

    public void FailDown()
    {
        StopAllCoroutines();
        _renderer.transform.DORotate(new Vector3(0f, 0f,-70f), 0.4f);
        _renderer.sprite = normal;
        StartCoroutine(MovementHelper.IEMove(transform, Vector2.down, speed * 1.5f , StopCondition, OnFailDown));
    }

    private bool StopCondition()
    {
        return transform.position.y <= posStand;
    }

    private void OnDownCompleted()
    {
        OnStop();
        DOVirtual.DelayedCall(0.15f, () =>
        {
            OnStanding?.Invoke();
        });
    }
    
    private void OnFailDown()
    {
        OnStop();
        DOVirtual.DelayedCall(0.15f, () =>
        {
            OnFailStanding?.Invoke();
        });
    }

    public void Revive()
    {
        _renderer.transform.rotation = new Quaternion();
    }
    
    private void OnStop()
    {
        var stand = transform.position;
        stand.y = posStand;
        transform.position = stand;
    }

    private void Move(Vector2 direction, float moveSpeed)
    {
        StartCoroutine(MovementHelper.IEMove(transform, direction, moveSpeed));
    }
    
}

public static class MovementHelper
{
    public static IEnumerator IEMove(Transform mover, Vector2 direction, float speed)
    {
        // Normalize the direction to ensure consistent speed
        direction.Normalize();
    
        // Loop indefinitely until the coroutine is stopped
        while (mover.gameObject.activeInHierarchy)
        {
            // Calculate the movement vector based on direction and speed
            Vector2 movement = direction * speed * Time.deltaTime;
        
            // Update the mover's position
            mover.position = (Vector2)mover.position + movement;
        
            // Yield until the next frame
            yield return null;
        }
    }
    
    public static IEnumerator IEMove(Transform mover, Vector2 direction, float speed, IMovementCondition condition)
    {
        // Normalize the direction to ensure consistent speed
        direction.Normalize();
    
        // Loop indefinitely until the coroutine is stopped
        while (condition.CanMove())
        {
            // Calculate the movement vector based on direction and speed
            Vector2 movement = direction * speed * Time.deltaTime;
        
            // Update the mover's position
            mover.position = (Vector2)mover.position + movement;
        
            // Yield until the next frame
            yield return null;
        }
    }
    
    public static IEnumerator IEMove(Transform mover, Vector2 direction, float speed, Func<bool> checkStop, Action onStop = null)
    {
        // Normalize the direction to ensure consistent speed
        direction.Normalize();
    
        // Loop indefinitely until the coroutine is stopped
        while (true)
        {
            // Calculate the movement vector based on direction and speed
            Vector2 movement = direction * speed * Time.deltaTime;
        
            // Update the mover's position
            mover.position = (Vector2)mover.position + movement;
            // Yield until the next frame
            yield return null;
            
            if (checkStop())
            {
                onStop?.Invoke();
                yield break;
            }
        }
    }
}

public interface IMovementCondition
{
    bool CanMove();
}

public class EndlessMove : IMovementCondition
{
    public bool CanMove()
    {
        return true;
    }
}

public class MoveToPosCondition : IMovementCondition
{
    
    
    public bool CanMove()
    {
        return true;
    }
}
