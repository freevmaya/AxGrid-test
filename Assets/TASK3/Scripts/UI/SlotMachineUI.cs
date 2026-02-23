using AxGrid;
using AxGrid.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TASK3.SlotMachine;

namespace TASK3.UI
{
    public class SlotMachineUI : MonoBehaviourExt
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI resultText;

        [Header("Visuals")]
        [SerializeField] private Image glowEffect;
        [SerializeField] private float glowIntensity = 1.5f;

        [Header("Particles")]
        [SerializeField] private ParticleSystem spinParticles;
        [SerializeField] private ParticleSystem winParticles;

        private Color originalGlowColor;
        private Coroutine glowCoroutine;

        [OnStart]
        private void StartThis()
        {
            Log.Debug("SlotMachineUI Start");

            if (glowEffect != null)
                originalGlowColor = glowEffect.color;

            // Подписываемся на события модели
            Settings.Model.EventManager.AddAction("OnSlotIdle", OnSlotIdle);
            Settings.Model.EventManager.AddAction("OnSlotSpinning", OnSlotSpinning);
            Settings.Model.EventManager.AddAction("OnSlotStopping", OnSlotStopping);
            Settings.Model.EventManager.AddAction("OnSlotResult", OnSlotResult);
            Settings.Model.EventManager.AddAction("OnStopTooEarly", OnStopTooEarly);
            Settings.Model.EventManager.AddAction("OnItemSelected", OnItemSelected);
            Settings.Model.EventManager.AddAction("OnSpeedChanged", OnSpeedChanged);
        }

        private void OnSlotIdle()
        {
            SetStatus("Готов к запуску");
            SetResult("");
            SetSpeed(0);

            if (glowCoroutine != null)
                StopCoroutine(glowCoroutine);

            if (glowEffect != null)
                glowEffect.color = originalGlowColor;

            if (spinParticles != null)
                spinParticles.Stop();
        }

        private void OnSlotSpinning()
        {
            SetStatus("Вращение...");

            if (glowCoroutine != null)
                StopCoroutine(glowCoroutine);
            glowCoroutine = StartCoroutine(AnimateGlowCoroutine());

            if (spinParticles != null)
                spinParticles.Play();
        }

        private void OnSlotStopping()
        {
            SetStatus("Остановка...");
        }

        private void OnSlotResult()
        {
            SetStatus("Результат!");

            if (winParticles != null)
                winParticles.Play();
        }

        private void OnStopTooEarly()
        {
            SetStatus("Подождите 3 секунды!");
            StartCoroutine(FlashText(statusText, Color.red, 0.5f));
        }

        private void OnItemSelected()
        {
            SlotItem item = Settings.Model.Get("LastSelectedItem", null) as SlotItem;
            string itemName = item ? item.ItemName : "Неизвестно";

            SetResult($"Выпало: {itemName}");
        }

        private void OnSpeedChanged(params object[] args)
        {
            if (args.Length > 0 && args[0] is float speed)
            {
                SetSpeed(speed);
            }
        }

        private void SetStatus(string text)
        {
            if (statusText != null)
                statusText.text = text;
        }

        private void SetResult(string text)
        {
            if (resultText != null)
                resultText.text = text;
        }

        private void SetSpeed(float speed)
        {
            if (speedText != null)
                speedText.text = $"Скорость: {speed:F0}";
        }

        private IEnumerator AnimateGlowCoroutine()
        {
            if (glowEffect == null) yield break;

            float time = 0;
            while (Settings.Model.GetString("SpinState") == "Spinning")
            {
                time += Time.deltaTime * 2;
                float intensity = 1 + Mathf.Sin(time) * 0.5f;
                glowEffect.color = originalGlowColor * intensity;
                yield return null;
            }

            glowEffect.color = originalGlowColor;
        }

        private IEnumerator FlashText(TextMeshProUGUI text, Color flashColor, float duration)
        {
            if (text == null) yield break;

            Color originalColor = text.color;
            text.color = flashColor;
            yield return new WaitForSeconds(duration);
            text.color = originalColor;
        }

        [OnDestroy]
        private void DestroyThis()
        {
            Settings.Model.EventManager.RemoveAction("OnSlotIdle", OnSlotIdle);
            Settings.Model.EventManager.RemoveAction("OnSlotSpinning", OnSlotSpinning);
            Settings.Model.EventManager.RemoveAction("OnSlotStopping", OnSlotStopping);
            Settings.Model.EventManager.RemoveAction("OnSlotResult", OnSlotResult);
            Settings.Model.EventManager.RemoveAction("OnStopTooEarly", OnStopTooEarly);
            Settings.Model.EventManager.RemoveAction("OnItemSelected", OnItemSelected);
            Settings.Model.EventManager.RemoveAction("OnSpeedChanged", OnSpeedChanged);
        }
    }
}