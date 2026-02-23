using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace TASK3.SlotMachine.FSM
{
    [State("SlotSpinning")]
    public class SlotSpinningState : FSMState
    {

        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            // Обновляем состояние кнопок
            Settings.Model.Set("CanStart", false);
            Settings.Model.Set("CanStop", false); // Стоп станет доступным через 3 секунды
            Settings.Model.Set("SpinState", "Spinning");

            // Отправляем событие в UI для запуска вращения
            Settings.Model.EventManager.Invoke("OnSpinStarted");
            Settings.Model.EventManager.Invoke("OnSlotSpinning");
        }

        [One(3f)]  // Вызов через 3 секунды после входа в состояние
        private void DelayedMethod()
        {
            Log.Debug("Прошло 3 секунды");
            Settings.Model.Set("CanStop", true);
        }

        [Bind("StopSpin")]
        private void OnStartSpin()
        {
            Log.Debug($"{Parent.CurrentStateName} StopSpin event received");
            Parent.Change("SlotStopping");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}