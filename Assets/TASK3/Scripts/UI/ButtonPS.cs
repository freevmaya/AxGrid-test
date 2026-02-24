using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace TASK3.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonPS : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem particleSystemBtn;
        [SerializeField]
        private float _timeLifePS = 2;
        private Button button => GetComponent<Button>();

        private void Awake()
        {
            button.onClick.AddListener(onClick);
        }

        private void onClick()
        {
            ParticleSystem ps = Instantiate(particleSystemBtn, transform);
            ps.transform.position = transform.position;
            ps.Play();

            StartCoroutine(WaitForParticles(_timeLifePS, () => {
                Destroy(ps.gameObject);
            }));
        }

        IEnumerator WaitForParticles(float delay, System.Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
    }
}
