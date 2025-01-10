using System.Drawing;
using System.Windows.Forms;

namespace GI_VideoVersions
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ListTagKeys = new ListView();
            ColTags = new ColumnHeader();
            ColKeys = new ColumnHeader();
            CtxMenuCopy = new ContextMenuStrip(components);
            CtxItemCopy = new ToolStripMenuItem();
            LabStatus = new Label();
            LabProcessId = new Label();
            LabStatusText = new Label();
            TxtProcessId = new TextBox();
            BtnDisconnect = new Button();
            BtnExport = new Button();
            BtnConnect = new Button();
            LabGameVer = new Label();
            LabGameVerText = new Label();
            LabLanguage = new Label();
            CmbLanguage = new ComboBox();
            BtnMerge = new Button();
            CtxMenuCopy.SuspendLayout();
            SuspendLayout();
            // 
            // ListTagKeys
            // 
            ListTagKeys.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ListTagKeys.Columns.AddRange(new ColumnHeader[] { ColTags, ColKeys });
            ListTagKeys.FullRowSelect = true;
            ListTagKeys.GridLines = true;
            ListTagKeys.LabelWrap = false;
            ListTagKeys.Location = new Point(12, 185);
            ListTagKeys.MultiSelect = false;
            ListTagKeys.Name = "ListTagKeys";
            ListTagKeys.Size = new Size(358, 320);
            ListTagKeys.TabIndex = 0;
            ListTagKeys.UseCompatibleStateImageBehavior = false;
            ListTagKeys.View = View.Details;
            ListTagKeys.MouseClick += ListTagKeys_MouseClick;
            // 
            // ColTags
            // 
            ColTags.Text = "Version";
            ColTags.Width = 120;
            // 
            // ColKeys
            // 
            ColKeys.Text = "Key";
            ColKeys.Width = 225;
            // 
            // CtxMenuCopy
            // 
            CtxMenuCopy.ImageScalingSize = new Size(20, 20);
            CtxMenuCopy.Items.AddRange(new ToolStripItem[] { CtxItemCopy });
            CtxMenuCopy.Name = "CtxMenuCopy";
            CtxMenuCopy.Size = new Size(117, 28);
            // 
            // CtxItemCopy
            // 
            CtxItemCopy.Name = "CtxItemCopy";
            CtxItemCopy.Size = new Size(116, 24);
            CtxItemCopy.Text = "Copy";
            CtxItemCopy.Click += CtxItemCopy_Click;
            // 
            // LabStatus
            // 
            LabStatus.AutoSize = true;
            LabStatus.Location = new Point(12, 110);
            LabStatus.Name = "LabStatus";
            LabStatus.Size = new Size(54, 20);
            LabStatus.TabIndex = 2;
            LabStatus.Text = "Status";
            // 
            // LabProcessId
            // 
            LabProcessId.AutoSize = true;
            LabProcessId.Location = new Point(12, 68);
            LabProcessId.Name = "LabProcessId";
            LabProcessId.Size = new Size(79, 20);
            LabProcessId.TabIndex = 3;
            LabProcessId.Text = "ProcessId";
            // 
            // LabStatusText
            // 
            LabStatusText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LabStatusText.AutoSize = true;
            LabStatusText.ForeColor = Color.Red;
            LabStatusText.Location = new Point(97, 110);
            LabStatusText.Name = "LabStatusText";
            LabStatusText.Size = new Size(90, 20);
            LabStatusText.TabIndex = 4;
            LabStatusText.Text = "Disconnect";
            // 
            // TxtProcessId
            // 
            TxtProcessId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtProcessId.Location = new Point(97, 65);
            TxtProcessId.Name = "TxtProcessId";
            TxtProcessId.Size = new Size(221, 27);
            TxtProcessId.TabIndex = 5;
            TxtProcessId.KeyPress += TxtProcessId_KeyPress;
            // 
            // BtnDisconnect
            // 
            BtnDisconnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnDisconnect.ForeColor = Color.IndianRed;
            BtnDisconnect.Location = new Point(330, 61);
            BtnDisconnect.Name = "BtnDisconnect";
            BtnDisconnect.Size = new Size(40, 35);
            BtnDisconnect.TabIndex = 6;
            BtnDisconnect.Text = "X";
            BtnDisconnect.UseVisualStyleBackColor = true;
            BtnDisconnect.Visible = false;
            BtnDisconnect.Click += BtnDisconnect_Click;
            // 
            // BtnExport
            // 
            BtnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnExport.ForeColor = SystemColors.Highlight;
            BtnExport.Location = new Point(220, 515);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(120, 30);
            BtnExport.TabIndex = 7;
            BtnExport.Text = "Export...";
            BtnExport.UseVisualStyleBackColor = true;
            BtnExport.Click += BtnExport_Click;
            // 
            // BtnConnect
            // 
            BtnConnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnConnect.ForeColor = Color.Green;
            BtnConnect.Location = new Point(330, 61);
            BtnConnect.Name = "BtnConnect";
            BtnConnect.Size = new Size(40, 35);
            BtnConnect.TabIndex = 8;
            BtnConnect.Text = "O";
            BtnConnect.UseVisualStyleBackColor = true;
            BtnConnect.Click += BtnConnect_Click;
            // 
            // LabGameVer
            // 
            LabGameVer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LabGameVer.AutoSize = true;
            LabGameVer.Location = new Point(12, 150);
            LabGameVer.Name = "LabGameVer";
            LabGameVer.Size = new Size(175, 20);
            LabGameVer.TabIndex = 9;
            LabGameVer.Text = "Support game version:";
            LabGameVer.Resize += LabGameVer_Resize;
            // 
            // LabGameVerText
            // 
            LabGameVerText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LabGameVerText.AutoSize = true;
            LabGameVerText.ForeColor = Color.MediumOrchid;
            LabGameVerText.Location = new Point(193, 150);
            LabGameVerText.Name = "LabGameVerText";
            LabGameVerText.Size = new Size(126, 20);
            LabGameVerText.TabIndex = 10;
            LabGameVerText.Text = "GAME_VERSION";
            // 
            // LabLanguage
            // 
            LabLanguage.AutoSize = true;
            LabLanguage.Location = new Point(12, 24);
            LabLanguage.Name = "LabLanguage";
            LabLanguage.Size = new Size(80, 20);
            LabLanguage.TabIndex = 11;
            LabLanguage.Text = "Language";
            // 
            // CmbLanguage
            // 
            CmbLanguage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbLanguage.FormattingEnabled = true;
            CmbLanguage.Items.AddRange(new object[] { "English", "简体中文" });
            CmbLanguage.Location = new Point(97, 21);
            CmbLanguage.Name = "CmbLanguage";
            CmbLanguage.Size = new Size(221, 28);
            CmbLanguage.TabIndex = 12;
            CmbLanguage.SelectedIndexChanged += CmbLanguage_SelectedIndexChanged;
            // 
            // BtnMerge
            // 
            BtnMerge.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnMerge.ForeColor = SystemColors.Highlight;
            BtnMerge.Location = new Point(50, 515);
            BtnMerge.Name = "BtnMerge";
            BtnMerge.Size = new Size(120, 30);
            BtnMerge.TabIndex = 13;
            BtnMerge.Text = "Merge...";
            BtnMerge.UseVisualStyleBackColor = true;
            BtnMerge.Click += BtnMerge_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 553);
            Controls.Add(BtnMerge);
            Controls.Add(CmbLanguage);
            Controls.Add(LabLanguage);
            Controls.Add(LabGameVerText);
            Controls.Add(LabGameVer);
            Controls.Add(BtnConnect);
            Controls.Add(BtnExport);
            Controls.Add(BtnDisconnect);
            Controls.Add(TxtProcessId);
            Controls.Add(LabStatusText);
            Controls.Add(LabProcessId);
            Controls.Add(LabStatus);
            Controls.Add(ListTagKeys);
            MinimumSize = new Size(400, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GI-VideoVersions";
            FormClosing += MainForm_FormClosing;
            Shown += MainForm_Shown;
            CtxMenuCopy.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView ListTagKeys;
        private ColumnHeader ColTags;
        private ColumnHeader ColKeys;
        private Label LabStatus;
        private Label LabProcessId;
        private Label LabStatusText;
        private TextBox TxtProcessId;
        private Button BtnDisconnect;
        private Button BtnExport;
        private Button BtnConnect;
        private ContextMenuStrip CtxMenuCopy;
        private ToolStripMenuItem CtxItemCopy;
        private Label LabGameVer;
        private Label LabGameVerText;
        private Label LabLanguage;
        private ComboBox CmbLanguage;
        private Button BtnMerge;
    }
}
