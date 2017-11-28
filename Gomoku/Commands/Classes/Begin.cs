namespace Gomoku.Commands.Classes
{
    public class Begin : ACommand
    {
        public Begin()
            :base(ECommand.BEGIN)
        {}

        public override string CreateOutput(DataCommand output)
        {
            throw new System.NotImplementedException();
        }        
    }
}