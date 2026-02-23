using AxGrid;
using AxGrid.FSM;

namespace TASK3.SlotMachine.FSM
{
    [State("SlotStopping")]
    public class SlotStoppingState : SMBaseState
    {
        public SlotStoppingState(SlotMachineMain slotMachineMain) : base(slotMachineMain)
        {
        }

        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            main.StartReduction();

            // Блокируем обе кнопки во время остановки
            Settings.Model.Set("CanStart", false);
            Settings.Model.Set("CanStop", false);
            Settings.Model.Set("SpinState", "Stopping");

            // Отправляем событие в UI
            Settings.Model.EventManager.Invoke("OnSpinStopping");
            Settings.Model.EventManager.Invoke("OnSlotStopping");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}