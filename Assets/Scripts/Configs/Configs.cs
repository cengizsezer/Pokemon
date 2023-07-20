using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Configs
{
    public static class Player
    {
        public static float speed = 10f;
        public static float[] attackInterval = { 3f, 2.9f, 2.8f,
        3f, 2.9f, 2.8f,
        3f, 2.9f, 2.8f,
        3f, 2.9f, 2.8f,
        3f, 2.9f, 2.8f,
        3f, 2.9f, 2.8f,
        3f, 2.9f, 2.8f,
        3f, 2.9f, 2.8f };

        public static float[] damages =
        {         10f, 20f, 40f
                , 10f, 20f, 40f
                , 12f, 24f, 48f
                , 12f, 23f,46f
                , 10f, 7f, 12f
                , 10f, 20f, 30f
                , 12f, 20f, 39f
                , 12f, 22f, 40f
        };
        public static float[] HPs =
        {
                  30f, 60f, 70f
                , 45f, 90f, 180f
                , 50f, 100f,200f
                , 30f, 60f, 120f
                , 40f, 80f, 140f
                , 30f, 60f, 120f
                , 30f, 60f, 120f
                , 45f, 90f, 180f
        };
    }
    public static class UI
    {
        public static float FadeOutTime = .2f;

    }

}
   
