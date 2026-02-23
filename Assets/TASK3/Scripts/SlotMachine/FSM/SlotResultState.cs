using AxGrid;
using AxGrid.FSM;

namespace TASK3.SlotMachine.FSM
{
    [State("SlotResult")]
    public class SlotResultState : SMBaseState
    {
        public SlotResultState(SlotMachineMain slotMachineMain) : base(slotMachineMain)
        {
        }

        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            Settings.Model.Set("LastSelectedItem", main.getMiddle());
            Settings.Model.Set("SpinState", "Result");

            // Блокируем кнопки на короткое время для показа результата
            Settings.Model.Set("CanStart", false);
            Settings.Model.Set("CanStop", false);

            // Отправляем события
            Settings.Model.EventManager.Invoke("OnItemSelected");
            Settings.Model.EventManager.Invoke("OnSlotResult");
        }

        [One(1.5f)]
        private void GoToIdle()
        {
            Log.Debug("Result shown, returning to idle");
            Parent.Change("SlotIdle");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}