namespace Gomoku.Commands
{
    public abstract class ACommand
    {
        public ECommand Type { get; }

        protected ACommand(ECommand type)
        {
            Type = type;
            CommandList.ListOfCommand.Add(this);
        }

        public virtual DataCommand CreateDataCommand(string input)
        { return new DataCommand() {CommandType = Type, Data = null}; }
    }
}