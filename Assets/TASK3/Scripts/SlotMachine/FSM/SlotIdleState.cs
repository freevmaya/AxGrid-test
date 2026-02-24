using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace TASK3.SlotMachine.FSM
{
    [State("SlotIdle")]
    public class SlotIdleState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            // Обновляем состояние кнопок
            Settings.Model.Set("CanStart", true);
            Settings.Model.Set("CanStop", false);

            Settings.Model.Set("SpinState", "Idle");

            // Отправляем событие в UI
            Settings.Model.EventManager.Invoke("OnSlotIdle");
        }

        [Bind("StartSpin")]
        private void OnStartSpin()
        {
            Log.Debug($"{Parent.CurrentStateName} StartSpin event received");
            Parent.Change("SlotSpinning");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}