using System.ComponentModel;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace VideoVersions
{
    public partial class MainForm : Form
    {
        private readonly List<Process> _blackList = [];
        private Process? _genshinProc;

        public MainForm()
        {
            InitializeComponent();
            LabGameVerText.Text = Config.GenshinVersion;
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            while (true)
            {
                if (_genshinProc is null)
                    CheckGenshinProcess();
                else
                    CheckGenshinConnect();

                await Task.Delay(2000);
            }
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                uint pid = uint.Parse(TxtProcessId.Text);
                Process process = Process.GetProcessById((int)pid);
                await TryConnectTo(process);
            }
            catch (Exception ex)
            {
                Utils.ShowError(ex.Message);
            }
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            if (Utils.ShowConfirm("Are you sure to disconnect?\n" +
                "The same process will not be attached repeatedly."))
                Disconnect();
        }

        private async void BtnDumpList_Click(object sender, EventArgs e)
        {
            var result = await PipeMessage.NotifyListDump();
            if (result is null)
            {
                Utils.ShowError("Failed to dump 26236578.blk!");
                return;
            }

            using var dialog = new SaveFileDialog()
            {
                FileName = "versions.json",
                Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = "json",
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                var doc = JsonDocument.Parse(result);
#pragma warning disable IDE0079
#pragma warning disable CA1869
                var fmt = new JsonSerializerOptions()
#pragma warning restore CA1869
#pragma warning restore IDE0079
                {
                    WriteIndented = true,
                    IndentCharacter = '\t',
                    IndentSize = 1,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                File.WriteAllText(dialog.FileName,
                    JsonSerializer.Serialize(doc, fmt));
            }
            catch
            {
                Utils.ShowError($"Failed to save file to {dialog.FileName}!");
            }

        }

        private async Task<bool> TryConnectTo(Process process)
        {
            if (!NativeHelper.LoadLibraryDll(
                (uint)process.Id, Config.DllName, out var error))
            {
                Utils.ShowError($"Failed to connect to process: {process.Id}\n"
                    + "Error: " + new Win32Exception(error).Message);
                return false;
            }

            if (!await PipeMessage.TryConnectAsync())
                return false;

            _genshinProc = process;
            TxtProcessId.Text = process.Id.ToString();
            TxtProcessId.ReadOnly = true;
            LabStatusText.Text = "Connected";
            LabStatusText.ForeColor = Color.Green;
            BtnConnect.Visible = false;
            BtnDisconnect.Visible = true;
            BtnDumpList.Visible = true;
            return true;
        }

        private void Disconnect()
        {
            PipeMessage.Disconnect();
            _genshinProc = null;
            TxtProcessId.Text = "";
            TxtProcessId.ReadOnly = false;
            LabStatusText.Text = "Disonnect";
            LabStatusText.ForeColor = Color.Red;
            BtnConnect.Visible = true;
            BtnDisconnect.Visible = false;
            BtnDumpList.Visible = false;
        }

        private async void CheckGenshinProcess()
        {
            if (!string.IsNullOrEmpty(TxtProcessId.Text))
                return;

            _blackList.RemoveAll(p => p.HasExited);
            var processes = Process
                .GetProcessesByName(Config.GenshinProcName)
                .Where(x => (DateTime.Now - x.StartTime).Seconds > 10
                         && _blackList.All(y => x.Id != y.Id));

            foreach (var process in processes)
            {
                _blackList.Add(process);
                if (await TryConnectTo(process))
                    break;
            }
        }

        private async void CheckGenshinConnect()
        {
            var result = await PipeMessage.NotifyKeyDump();
            if (result is null)
                Disconnect();
            else MergeTagKeys(result);
        }

        private void MergeTagKeys(ReadOnlySpan<byte> json)
        {
            try
            {
                if (json.IsEmpty) return;

                var dict = JsonSerializer
                    .Deserialize<Dictionary<string, ulong>>(json);
                if (dict is null) return;

                var toBeAdd = dict
                    .Where(kv => !ListTagKeys.Items.ContainsKey(kv.Key))
                    .Select(kv =>
                    {
                        var item = new ListViewItem()
                        {
                            Name = kv.Key,
                            Text = kv.Key
                        };
                        item.SubItems.Add(kv.Value.ToString());
                        return item;
                    });
                ListTagKeys.Items.AddRange([.. toBeAdd]);
            }
            catch { }
        }

        private void TxtProcessId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                BtnConnect_Click(sender, e);
            else if (e.KeyChar == (char)Keys.Escape)
                TxtProcessId.Text = "";
        }

        private void CtxItemCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ListTagKeys
                .SelectedItems[0].SubItems[1].Text);
        }

        private void ListTagKeys_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right ||
                ListTagKeys.SelectedItems.Count == 0)
                return;

            CtxMenuCopy.Show(ListTagKeys, e.Location);
        }
    }
}
