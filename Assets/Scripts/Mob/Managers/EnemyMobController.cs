using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobController : GenericMobController<EnemyMob>,IEnemy
{
   public void ResetFightEnemies()
    {
        if (lsMobs.Count == 0)
        {
            for (int i = 0; i < lsMobs.Count; i++)
            {
                lsMobs[i].OnResetFight();
            }
        }
    }
}
