using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    Mineral miningTarget;
    WorkerJobStatus jobStatus;
    Base dumpBase;
    Enemy enemy
    {
        get
        {
            return this.gameObject.GetComponent<Enemy>();
        }
    }

    void MoveToMineral()
    {
        enemy.Move();
    }

    void Mine()
    {
        Enemy.Stall();
    }

    void Dump()
    {
        Enemy.Stall();
        MoveToMineral();
    }
}
