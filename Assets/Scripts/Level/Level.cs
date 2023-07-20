using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public LevelEnemies[] lvlEnemies;
    public LevelPlayers lvlPlayers;
    [System.Serializable]
    public class LevelEnemies
    {
        public int LEVEL;
        public PokemonTypes MOBTYPE;
        public float dmgMult = 1f;
        public float hpMult = 1f;
        public int posID;
        public float scale = 1f;
    }

    public class LevelPlayers
    {
        //public  string LevelForPogs = "0,3,6,15,12,9,18,21,0,3,6";
    }
}
