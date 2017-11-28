namespace Gomoku.Commands
{
    public abstract class ACommand
    {
        public ECommand Type { get; }

        protected ACommand(ECommand type)
        {
            Type = type;
            CommandDictionary.CommandMap.Add(this);
        }

        public abstract string CreateOutput(DataCommand output);
        public virtual DataCommand CreateDataCommand(string input)
        { return new DataCommand() {CommandType = Type, Data = null}; }
    }
}