using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using System;
using System.Collections.Generic;
using TASK3.SlotMachine.FSM;
using UnityEngine;

namespace TASK3.SlotMachine
{
    public class SlotMachineMain : MonoBehaviourExtBind
    {
        [Header("Slot Settings")]
        [SerializeField] private int slotItemsCount = 20;
        [SerializeField] private float itemHeight = 100f;
        [SerializeField] private float maxScrollSpeed = 800f;
        [SerializeField] private int accelSlotCount = 3;
        [SerializeField] private int reduceSlotCount = 2;

        [Header("References")]
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private SlotItem slotItemPrefab;

        private List<SlotItem> slotItems = new List<SlotItem>();
        private float scrollPosition = 0f;
        private float speed;
        private float accelerate = 0f;

        [OnAwake]
        private void AwakeThis()
        {
            Log.Debug("SlotMachineMain Awake");
            InitializeSlotItems();
        }

        private float calcAccelerate(float V, int count)
        {
            return Mathf.Pow(V, 2) / ((itemHeight - scrollPosition + itemHeight * count) * 2);
        }

        private void startReduction()
        {
            accelerate = -calcAccelerate(speed, reduceSlotCount);
        }

        private void startAccelerate()
        {
            setSpeed(0);
            accelerate = calcAccelerate(maxScrollSpeed, accelSlotCount);
        }

        [OnStart]
        private void StartThis()
        {
            Log.Debug("SlotMachineMain Start");

            // Инициализация FSM
            Settings.Fsm = new AxGrid.FSM.FSM();
            Settings.Fsm.Add(new SlotInitState());
            Settings.Fsm.Add(new SlotIdleState());
            Settings.Fsm.Add(new SlotSpinningState());
            Settings.Fsm.Add(new SlotStoppingState());
            Settings.Fsm.Add(new SlotResultState());

            Settings.Fsm.Start("SlotInit");

            Settings.Model.EventManager.AddAction("OnSpinStarted", OnSpinStarted);
            Settings.Model.EventManager.AddAction("OnSpinStopping", OnSpinStopping);
            Settings.Model.EventManager.AddAction("OnCheckStopping", OnCheckStopping);
            Settings.Model.EventManager.AddAction("OnItemSelected", OnItemSelected);
        }

        [OnDestroy]
        private void OnDestory()
        {
            Settings.Model.EventManager.RemoveAction("OnSpinStarted", OnSpinStarted);
            Settings.Model.EventManager.RemoveAction("OnSpinStopping", OnSpinStopping);
            Settings.Model.EventManager.RemoveAction("OnCheckStopping", OnCheckStopping);
            Settings.Model.EventManager.RemoveAction("OnItemSelected", OnItemSelected);
        }

        private void OnItemSelected()
        {
            SlotItem selected = getMiddle();
            Settings.Model.Set("LastSelectedItem", selected);
            selected.Blink();
        }

        private void OnCheckStopping()
        {
            float adt = accelerate * Time.deltaTime;
            float new_speed = speed + adt;

            if (new_speed < adt)
            {
                Stop();
            }
        }

        private void OnSpinStarted()
        {
            startAccelerate();
        }

        private void OnSpinStopping()
        {
            startReduction();
        }

        [OnUpdate]
        private void UpdateThis()
        {
            Settings.Fsm.Update(Time.deltaTime);
            UpdateScroll();
        }

        private void Clear()
        {
            // Очищаем существующие элементы
            foreach (Transform child in contentPanel)
                Destroy(child.gameObject);
            slotItems.Clear();
        }

        private void InitializeSlotItems()
        {
            Clear();
            // Устанавливаем высоту контента
            contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, slotItemsCount * itemHeight);

            // Создаем новые элементы
            for (int i = 0; i < slotItemsCount; i++)
            {
                slotItems.Add(Instantiate(slotItemPrefab, contentPanel));
            }
            scrollPosition = 0;
            updateItemPositions();
        }

        private void updateItemPositions()
        {
            float y = contentPanel.sizeDelta.y / 2;
            // Применяем позицию ко всем элементам
            for (int i = 0; i < slotItems.Count; i++)
            {
                float baseY = y - i * itemHeight;
                float currentY = baseY - scrollPosition;

                // Зацикливание для бесконечного скролла
                if (currentY < -contentPanel.rect.height)
                    currentY += slotItems.Count * itemHeight;

                RectTransform rect = slotItems[i].GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, currentY);
            }

        }

        public void Stop()
        {
            setSpeed(0);
            accelerate = 0;
            scrollPosition = Mathf.Round(scrollPosition / itemHeight) * itemHeight;
            updatePosition();
            // Определяем результат
            Settings.Fsm.Change("SlotResult");
        }

        private void setSpeed(float value)
        {
            value = Mathf.Min(value, maxScrollSpeed);
            if (value != speed)
            {
                speed = value;
                Settings.Model.EventManager.Invoke("OnSpeedChanged", speed);
            }
        }

        private void updatePosition()
        {
            scrollPosition += speed * Time.deltaTime;
            // Зацикливаем позицию
            if (scrollPosition >= itemHeight)
            {
                scrollPosition -= itemHeight;
                RepositionItems();
            }
            updateItemPositions();
        }

        private void UpdateScroll()
        {
            // Обновляем позицию скролла
            setSpeed(speed + accelerate * Time.deltaTime);
            if (speed != 0)
            {
                updatePosition();
            }
        }

        private void RepositionItems()
        {
            int last = slotItems.Count - 1;
            SlotItem tmp = slotItems[last];
            slotItems.RemoveAt(last);
            slotItems.Insert(0, tmp);

            // Обновляем данные элемента
            tmp.ResetData();
        }

        private SlotItem getMiddle()
        {
            return slotItems[slotItems.Count / 2];
        }

        [Bind("OnStartClick")]
        private void OnStartButtonClick()
        {
            Log.Debug("SlotMachineMain: Start button clicked");
            Settings.Fsm.Invoke("StartSpin");
        }

        [Bind("OnStopClick")]
        private void OnStopButtonClick()
        {
            Log.Debug("SlotMachineMain: Stop button clicked");
            Settings.Fsm.Invoke("StopSpin", this);
        }
    }
}