using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMobController : GenericMobController<PlayerMob>,IPlayer
{
    public void ResetFightPlayerMobs()
    {
        for (int i = 0; i < lsMobs.Count; i++)
        {
            lsMobs[i].OnResetFight();
        }
    }


    public bool CheckTutorialPokemon()
    {
        for (int i = 0; i < lsMobs.Count; i++)
        {
            for (int x = 0; x < lsMobs.Count; x++)
            {
                if (lsMobs[i].MOBTYPE == lsMobs[x].MOBTYPE && i != x)
                {
                    Debug.Log(lsMobs[i].MOBTYPE + " +" + " " + i);
                    Debug.Log(lsMobs[x].MOBTYPE + " +" + " " + x);
                    return true;
                }
            }
        }
        

        return false;
    }
}
