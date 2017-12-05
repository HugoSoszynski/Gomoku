namespace Gomoku.Commands
{
    public struct DataCommand
    {
        public ECommand CommandType { get; set; }
        public object Data { get; set; }
    }
}