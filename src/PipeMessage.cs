using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace GI_VideoVersions
{
    internal static class PipeMessage
    {
        private enum MsgType : uint
        {
            MsgListDump = 0xAE0DFA5B,
            MsgKeyDump = 0x7367339B
        }

        private static readonly NamedPipeServerStream pipeServer
            = new(Config.PipeName);

        public static async Task WaitConnectAsync()
        {
            using var cts = new CancellationTokenSource();
            try
            {
                await pipeServer
                    .WaitForConnectionAsync(cts.Token)
                    .WaitAsync(TimeSpan.FromSeconds(3), cts.Token);
            }
            catch (TimeoutException)
            {
                cts.Cancel();
                throw;
            }
        }

        public static void Disconnect()
        {
            try { pipeServer.Disconnect(); }
            catch { }
        }

        public static async Task<byte[]?> NotifyListDump()
        {
            try
            {
                var bytes = BitConverter.GetBytes((uint)MsgType.MsgListDump);
                await pipeServer.WriteAsync(bytes);
                await pipeServer.ReadExactlyAsync(bytes);
                if (BitConverter.ToUInt32(bytes) != ~(uint)MsgType.MsgListDump)
                    return null;

                await pipeServer.ReadExactlyAsync(bytes);
                uint size = BitConverter.ToUInt32(bytes);
                byte[] buffer = new byte[size];
                await pipeServer.ReadExactlyAsync(buffer);
                return buffer;
            }
            catch { return null; }
        }

        public static async Task<byte[]?> NotifyKeyDump()
        {
            try
            {
                var bytes = BitConverter.GetBytes((uint)MsgType.MsgKeyDump);
                await pipeServer.WriteAsync(bytes);
                await pipeServer.ReadExactlyAsync(bytes);
                if (BitConverter.ToUInt32(bytes) != ~(uint)MsgType.MsgKeyDump)
                    return null;

                await pipeServer.ReadExactlyAsync(bytes);
                uint size = BitConverter.ToUInt32(bytes);
                byte[] buffer = new byte[size];
                await pipeServer.ReadExactlyAsync(buffer);
                return buffer;
            }
            catch { return null; }
        }
    }
}
