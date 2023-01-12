using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Particle
{

    public class ParticleManager : MonoBehaviour
    {
        public ParticleUnit[] particles;
        public static ParticleManager instance;




        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

        }



        public void Play(string name)
        {
            ParticleUnit particle = System.Array.Find(particles, p => p.Name == name);
            if (particle == null)
                return;
            particle.ParticleEffect.Play();
        }
    }

}