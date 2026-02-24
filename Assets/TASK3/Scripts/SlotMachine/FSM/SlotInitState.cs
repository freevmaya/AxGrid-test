using AxGrid;
using AxGrid.FSM;

namespace TASK3.SlotMachine.FSM
{
    [State("SlotInit")]
    public class SlotInitState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            // Инициализация модели
            Settings.Model.Set("CanStart", true);
            Settings.Model.Set("CanStop", false);

            // Переходим в состояние ожидания
            Parent.Change("SlotIdle");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}