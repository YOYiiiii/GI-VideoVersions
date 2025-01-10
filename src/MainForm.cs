using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GI_VideoVersions
{
    public partial class MainForm : Form
    {
        private readonly VideoVersions versions = new();
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
            BtnMerge.Text = Config.LoadString(nameof(BtnMerge));
            BtnExport.Text = Config.LoadString(nameof(BtnExport));
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
                    Utils.ThrowLastError(error);
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
            if (genshinProc is null) return;

            try
            {
                VideoVersions.InitVideoFiles(genshinProc);
                var result = await PipeMessage.NotifyListDump();
                var other = VideoVersions.FromJson(result);
                other.TrimVersions();
                versions.MergeFrom(other);
            }
            catch (Exception ex)
            {
                Utils.ShowError(string.Format(
                    Config.LoadString("MsgDumpListFail")!, ex.Message));
                Disconnect();
            }
        }

        private async void CheckGenshinConnect()
        {
            var result = await PipeMessage.NotifyKeyDump();
            if (result is not null)
                MergeTagKeys(result);
            else Disconnect();
        }

        private void MergeTagKeys(ReadOnlySpan<byte> json)
        {
            try
            {
                if (json.IsEmpty) return;

                var dict = JsonSerializer
                    .Deserialize<Dictionary<string, ulong>>(json);
                if (dict is null) return;

                versions.MergeTagKeys(dict);
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ListTagKeys.Items.Count > 0 &&
                !Utils.ShowConfirm(Config.LoadString("MsgConfirmExit")))
                e.Cancel = true;
        }

        private void CmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.Language = (Config.LanguageType)CmbLanguage.SelectedIndex;
            LoadLanguage();
        }

        private void TxtProcessId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                BtnConnect_Click(sender, e);
            else if (e.KeyChar == (char)Keys.Escape)
                TxtProcessId.Text = "";
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
            if (Utils.ShowConfirm(Config.LoadString("MsgConfirmDisconnect")))
                Disconnect();
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

        private void BtnMerge_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog()
            {
                FileName = "versions.json",
                Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = "json",
            };
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var file = File.ReadAllBytes(dialog.FileName);
                    versions.MergeFrom(VideoVersions.FromJson(file));
                }
            }
            catch (Exception ex)
            {
                Utils.ShowError(string.Format(
                    Config.LoadString("MsgOpenFileFail")!,
                    dialog.FileName, ex.Message));
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog()
            {
                FileName = "versions.json",
                Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = "json",
            };
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    File.WriteAllText(dialog.FileName, versions.ToJson());
            }
            catch (Exception ex)
            {
                Utils.ShowError(string.Format(
                    Config.LoadString("MsgSaveFileFail")!,
                    dialog.FileName, ex.Message));
            }
        }
    }
}
