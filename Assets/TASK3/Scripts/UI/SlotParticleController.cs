using UnityEngine;

namespace TASK3.UI
{
    public class SlotParticleController : MonoBehaviour
    {
        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem spinEffect;
        [SerializeField] private ParticleSystem winEffect;
        [SerializeField] private ParticleSystem sparkleEffect;

        [Header("Settings")]
        [SerializeField] private float spinSpeedMultiplier = 1f;

        private ParticleSystem.MainModule spinMain;
        private float baseSpinSpeed;

        private void Start()
        {
            if (spinEffect != null)
            {
                spinMain = spinEffect.main;
                baseSpinSpeed = spinMain.simulationSpeed;
            }
        }

        public void UpdateSpinSpeed(float speed)
        {
            if (spinEffect == null) return;

            float normalizedSpeed = Mathf.Clamp01(speed / 1000f);
            spinMain.simulationSpeed = baseSpinSpeed + normalizedSpeed * spinSpeedMultiplier;
        }

        public void PlayWinEffect()
        {
            if (winEffect != null)
                winEffect.Play();

            if (sparkleEffect != null)
                sparkleEffect.Play();
        }

        public void StopAllEffects()
        {
            if (spinEffect != null)
                spinEffect.Stop();

            if (winEffect != null)
                winEffect.Stop();

            if (sparkleEffect != null)
                sparkleEffect.Stop();

            if (spinEffect != null)
                spinMain.simulationSpeed = baseSpinSpeed;
        }
    }
}