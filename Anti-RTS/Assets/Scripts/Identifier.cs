using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identifier : MonoBehaviour
{
    [SerializeField] IdEnum idEnum;

    public bool IsMineral()
    {
        return idEnum == IdEnum.Mineral;
    }

    public bool IsWall()
    {
        return idEnum == IdEnum.Wall;
    }

    public bool IsPlayer()
    {
        return idEnum == IdEnum.Player;
    }

    public bool IsWorker()
    {
        return idEnum == IdEnum.Worker;
    }

    public bool IsEnemy()
    {
        return idEnum == IdEnum.Worker || idEnum == IdEnum.Ranged || idEnum == IdEnum.Melee;
    }

    public bool IsMelee()
    {
        return idEnum == IdEnum.Melee;
    }

    public bool IsRanged()
    {
        return idEnum == IdEnum.Ranged;
    }

    public bool IsBullet()
    {
        return idEnum == IdEnum.Bullet;
    }

    public bool IsBase()
    {
        return idEnum == IdEnum.Base;
    }
}
 