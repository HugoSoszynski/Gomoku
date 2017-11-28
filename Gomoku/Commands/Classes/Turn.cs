namespace Gomoku.Commands.Classes
{
    public class Turn : ACommand
    {
        public Turn()
            :base(ECommand.TURN)
        {}

        public override string CreateOutput(DataCommand output)
        {
            throw new System.NotImplementedException();
        }
    }
}