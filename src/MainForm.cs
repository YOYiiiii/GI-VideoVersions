using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GI_VideoVersions
{
    public partial class MainForm : Form
    {
        private readonly List<Process> blackList = [];
        private Process? genshinProc;

        public MainForm()
        {
            InitializeComponent();
            LoadLanguage();
            CmbLanguage.SelectedIndex = (int)Config.Language;
            LabGameVerText.Text = Config.GenshinVersion;
        }

        private void LoadLanguage()
        {
            LabProcessId.Text = Config.LoadString(nameof(LabProcessId));
            LabStatus.Text = Config.LoadString(nameof(LabStatus));
            LabStatusText.Text = Config.LoadString(
                genshinProc is null ? "TxtDisconnect" : "TxtConnected");
            LabLanguage.Text = Config.LoadString(nameof(LabLanguage));
            LabGameVer.Text = Config.LoadString(nameof(LabGameVer));
            CtxItemCopy.Text = Config.LoadString(nameof(CtxItemCopy));
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            while (true)
            {
                if (genshinProc is null)
                    CheckGenshinProcess();
                else
                    CheckGenshinConnect();

                await Task.Delay(2000);
            }
        }

        private void CmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.Language = (Config.LanguageType)CmbLanguage.SelectedIndex;
            LoadLanguage();
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
                Utils.ShowError(Config.LoadString("MsgDumpListFail"));
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
#pragma warning disable CA1869
                var fmt = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    IndentCharacter = '\t',
                    IndentSize = 1,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
#pragma warning restore CA1869
                File.WriteAllText(dialog.FileName,
                    JsonSerializer.Serialize(doc, fmt));
            }
            catch (Exception ex)
            {
                Utils.ShowError(string.Format(
                    Config.LoadString("MsgSaveFileFail")!,
                    dialog.FileName, ex.Message));
            }
        }

        private async Task<bool> TryConnectTo(Process process)
        {
            TxtProcessId.Text = process.Id.ToString();
            TxtProcessId.ReadOnly = true;
            LabStatusText.Text = Config.LoadString("TxtConnecting");
            LabStatusText.ForeColor = Color.Orange;
            BtnConnect.Enabled = false;

            try
            {
                if (!NativeHelper.LoadLibraryDll(
                (uint)process.Id, Config.DllName, out var error))
                    throw new Win32Exception(error);
                await PipeMessage.WaitConnectAsync();
            }
            catch (Exception ex)
            {
                Utils.ShowError(string.Format(
                    Config.LoadString("MsgConnectFail")!,
                    process.Id, ex.Message));

                Disconnect();
                BtnConnect.Enabled = true;
                return false;
            }

            genshinProc = process;
            LabStatusText.Text = Config.LoadString("TxtConnected");
            LabStatusText.ForeColor = Color.Green;
            BtnConnect.Visible = false;
            BtnDisconnect.Visible = true;
            BtnDumpList.Visible = true;
            return true;
        }

        private void Disconnect()
        {
            PipeMessage.Disconnect();
            genshinProc = null;
            TxtProcessId.Text = "";
            TxtProcessId.ReadOnly = false;
            LabStatusText.Text = Config.LoadString("TxtDisconnect");
            LabStatusText.ForeColor = Color.Red;
            BtnConnect.Visible = true;
            BtnDisconnect.Visible = false;
            BtnDumpList.Visible = false;
        }

        private async void CheckGenshinProcess()
        {
            if (!string.IsNullOrEmpty(TxtProcessId.Text))
                return;

            blackList.RemoveAll(p => p.HasExited);
            var processes = Process
                .GetProcessesByName(Config.GenshinProcName)
                .Where(x => (DateTime.Now - x.StartTime).Seconds > 10
                         && blackList.All(y => x.Id != y.Id));

            foreach (var process in processes)
            {
                blackList.Add(process);
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

        private void LabGameVer_Resize(object sender, EventArgs e)
        {
            LabGameVerText.Location = new(
                LabGameVer.Location.X + LabGameVer.Width,
                LabGameVerText.Location.Y);
        }
    }
}
