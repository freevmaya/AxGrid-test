using AxGrid;
using AxGrid.FSM;

namespace TASK3.SlotMachine.FSM
{
    [State("SlotStopping")]
    public class SlotStoppingState : FSMState
    {

        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            // Блокируем обе кнопки во время остановки
            Settings.Model.Set("CanStart", false);
            Settings.Model.Set("CanStop", false);
            Settings.Model.Set("SpinState", "Stopping");

            // Отправляем событие в UI
            Settings.Model.EventManager.Invoke("OnSpinStopping");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }

        [Loop(0.01f)]
        private void OnUpdate()
        {
            Settings.Model.EventManager.Invoke("OnCheckStopping");
        }
    }
}