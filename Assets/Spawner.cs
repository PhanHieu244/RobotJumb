using System;
using ChuongCustom;
using Sirenix.OdinInspector;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform maxHeightMilestone;
    [SerializeField] private Transform standPos;
    [SerializeField] private float deltaHeight;
    [SerializeField] private BaseMover baseMover;
    
    private Vector3 spawnPos = Vector3.zero;
    private int count = 0;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float increase = 0.1f;
    [SerializeField] private int maxCount = 3;
    [SerializeField] private float maxSpeed = 12f;

    public float StandPos => standPos.position.y;
    
    public void MoveHigher(BaseMover mover)
    {
        var delta = mover.transform.position.y - maxHeightMilestone.position.y;
        if (delta > 0)
        {
            transform.position -= new Vector3(0, delta);
        }
        spawnPos += new Vector3(0, deltaHeight);
        
    }

    private BaseMover _mover;
    
    private void Update()
    {
        if (_mover == null) return;
        if (_mover.transform.position.x < 2f) return;
        InGameManager.Instance.Stop();
    }

    public void Spawn(out BaseMover mover)
    {
        count++;
        if (count >= maxCount && speed < maxSpeed)
        {
            count = 0;
            speed += increase;
        }
        mover = Instantiate(baseMover, startPos);
        mover.transform.localPosition = spawnPos;
        mover.Move(speed);
        standPos.position = mover.StandingPos;
        _mover = mover;
    }

    [Button]
    private void Spawn()
    {
        Spawn(out var mover);
    }
}
