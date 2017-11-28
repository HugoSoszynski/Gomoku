namespace Gomoku.Commands.Classes
{
    public class CommandBoard : ACommand
    {
        public CommandBoard()
            :base(ECommand.BOARD)
        {}

        public override string CreateOutput(DataCommand output)
        {
            throw new System.NotImplementedException();
        }
    }
}