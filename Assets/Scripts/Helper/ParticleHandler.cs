using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{

    [SerializeField] ParticleSystem psMerge;

    public void MergeParticlePlay()
    {
        psMerge.Play();
    }

}
