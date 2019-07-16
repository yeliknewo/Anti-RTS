using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identifier : MonoBehaviour
{
    [SerializeField] IdEnum idEnum;

    public bool IsMineral()
    {
        return idEnum == IdEnum.MINERAL;
    }

    public bool IsWall()
    {
		return idEnum == IdEnum.WALL;
    }

    public bool IsPlayer()
    {
        return idEnum == IdEnum.PLAYER;
    }

    public bool IsWorker()
    {
        return idEnum == IdEnum.WORKER;
    }

    public bool IsEnemy()
    {
        return idEnum == IdEnum.WORKER || idEnum == IdEnum.RANGED || idEnum == IdEnum.MELEE;
    }

    public bool IsMelee()
    {
        return idEnum == IdEnum.MELEE;
    }

    public bool IsRanged()
    {
        return idEnum == IdEnum.RANGED;
    }

    public bool IsBullet()
    {
        return idEnum == IdEnum.BULLET;
    }

    public bool IsBase()
    {
        return idEnum == IdEnum.BASE;
    }
}
 