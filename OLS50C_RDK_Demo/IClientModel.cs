namespace OLS50C_RDK_Demo
{
    public interface IClientModel
    {
        void Close();
        void Connect(string ip, int port);
        string Receive();
        void Write(string command);
    }
}