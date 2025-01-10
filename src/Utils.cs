using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GI_VideoVersions
{
    internal static class Utils
    {
        internal static string RootNamespace = "GI-VideoVersions";

        public static bool ShowConfirm(object? message)
        {
            return DialogResult.Yes == MessageBox.Show(message?.ToString(),
                RootNamespace, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static async void ShowInfo(object? message, bool async = false)
        {
            if (async) await Task.Run(() => ShowInfo(message));
            else MessageBox.Show(message?.ToString(),
                RootNamespace, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static async void ShowWarning(object? message, bool async = false)
        {
            if (async) await Task.Run(() => ShowWarning(message));
            else MessageBox.Show(message?.ToString(),
                RootNamespace, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static async void ShowError(object? message, bool async = false)
        {
            if (async) await Task.Run(() => ShowError(message));
            else MessageBox.Show(message?.ToString(),
                RootNamespace, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [DoesNotReturn]
        public static void ThrowLastError(int error = 0)
            => throw new Win32Exception(
                error == 0 ? Marshal.GetLastWin32Error() : error);
    }
}
