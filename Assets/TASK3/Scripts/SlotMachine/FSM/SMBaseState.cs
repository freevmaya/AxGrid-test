using AxGrid.FSM;

namespace TASK3.SlotMachine.FSM
{
    public class SMBaseState : FSMState
    {
        private SlotMachineMain _main;
        protected SlotMachineMain main => _main;

        public SMBaseState(SlotMachineMain slotMachineMain)
        {
            _main = slotMachineMain;
        }
    }
}
