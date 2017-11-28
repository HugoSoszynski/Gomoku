namespace Gomoku.Commands.Classes
{
    public class About : ACommand
    {
        public About()
        :base(ECommand.ABOUT)
        {}
        
        public override string CreateOutput(DataCommand output)
        {
            throw new System.NotImplementedException();
        }
    }
}