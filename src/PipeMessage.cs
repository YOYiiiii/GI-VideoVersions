using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace VideoVersions
{
    internal static class PipeMessage
    {
        private enum MsgType : uint
        {
            MsgListDump = 0xAE0DFA5B,
            MsgKeyDump = 0x7367339B
        }

        private static readonly NamedPipeServerStream _pipeServer
            = new(Config.PipeName);

        public static async Task<bool> TryConnectAsync()
        {
            using var cts = new CancellationTokenSource();
            try
            {
                await _pipeServer
                    .WaitForConnectionAsync(cts.Token)
                    .WaitAsync(TimeSpan.FromSeconds(3), cts.Token);
                return true;
            }
            catch
            {
                cts.Cancel();
                return false;
            }
        }

        public static void Disconnect()
        {
            try { _pipeServer.Disconnect(); }
            catch { }
        }

        public static async Task<byte[]?> NotifyListDump()
        {
            try
            {
                var bytes = BitConverter.GetBytes((uint)MsgType.MsgListDump);
                await _pipeServer.WriteAsync(bytes);
                await _pipeServer.ReadExactlyAsync(bytes);
                if (BitConverter.ToUInt32(bytes) != ~(uint)MsgType.MsgListDump)
                    return null;

                await _pipeServer.ReadExactlyAsync(bytes);
                uint size = BitConverter.ToUInt32(bytes);
                byte[] buffer = new byte[size];
                await _pipeServer.ReadExactlyAsync(buffer);
                return buffer;
            }
            catch { return null; }
        }

        public static async Task<byte[]?> NotifyKeyDump()
        {
            try
            {
                var bytes = BitConverter.GetBytes((uint)MsgType.MsgKeyDump);
                await _pipeServer.WriteAsync(bytes);
                await _pipeServer.ReadExactlyAsync(bytes);
                if (BitConverter.ToUInt32(bytes) != ~(uint)MsgType.MsgKeyDump)
                    return null;

                await _pipeServer.ReadExactlyAsync(bytes);
                uint size = BitConverter.ToUInt32(bytes);
                byte[] buffer = new byte[size];
                await _pipeServer.ReadExactlyAsync(buffer);
                return buffer;
            }
            catch { return null; }
        }
    }
}
