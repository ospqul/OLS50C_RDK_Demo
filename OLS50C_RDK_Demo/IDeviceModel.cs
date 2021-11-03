using System.Threading;

namespace OLS50C_RDK_Demo
{
    public interface IDeviceModel
    {
        string CheckStatus(string command, CancellationToken token);
        bool SendCommand(string command, CancellationToken token);
    }
}