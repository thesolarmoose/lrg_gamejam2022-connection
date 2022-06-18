namespace Hook.ConnectionsResponses
{
    public interface IConnectionResponse
    {
        void Execute(Connection connection);
    }
}