namespace Gomoku.Commands.Classes
{
    public class Start : ACommand
    {
        public Start()
            :base(ECommand.START)
        {}

        public override string CreateOutput(DataCommand output)
        {
            throw new System.NotImplementedException();
        }
    }
}