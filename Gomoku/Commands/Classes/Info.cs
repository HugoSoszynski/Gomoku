namespace Gomoku.Commands.Classes
{
    public class Info : ACommand
    {
        public Info()
            :base(ECommand.INFO)
        {}

        public override string CreateOutput(DataCommand output)
        {
            throw new System.NotImplementedException();
        }
    }
}