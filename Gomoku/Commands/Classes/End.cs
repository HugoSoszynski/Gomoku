namespace Gomoku.Commands.Classes
{
    public class End : ACommand
    {
        public End()
            :base(ECommand.END)
        {}

        public override string CreateOutput(DataCommand output)
        {
            throw new System.NotImplementedException();
        }        
    }
}