namespace VideoVersions
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
            BtnDumpList = new Button();
            BtnConnect = new Button();
            LabGameVer = new Label();
            LabGameVerText = new Label();
            CtxMenuCopy.SuspendLayout();
            SuspendLayout();
            // 
            // ListTagKeys
            // 
            ListTagKeys.Columns.AddRange(new ColumnHeader[] { ColTags, ColKeys });
            ListTagKeys.FullRowSelect = true;
            ListTagKeys.GridLines = true;
            ListTagKeys.LabelWrap = false;
            ListTagKeys.Location = new Point(12, 111);
            ListTagKeys.MultiSelect = false;
            ListTagKeys.Name = "ListTagKeys";
            ListTagKeys.Size = new Size(358, 403);
            ListTagKeys.TabIndex = 0;
            ListTagKeys.UseCompatibleStateImageBehavior = false;
            ListTagKeys.View = View.Details;
            ListTagKeys.MouseClick += ListTagKeys_MouseClick;
            // 
            // ColTags
            // 
            ColTags.Text = "Tag";
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
            LabStatus.Location = new Point(37, 65);
            LabStatus.Name = "LabStatus";
            LabStatus.Size = new Size(54, 20);
            LabStatus.TabIndex = 2;
            LabStatus.Text = "Status";
            // 
            // LabProcessId
            // 
            LabProcessId.AutoSize = true;
            LabProcessId.Location = new Point(12, 24);
            LabProcessId.Name = "LabProcessId";
            LabProcessId.Size = new Size(79, 20);
            LabProcessId.TabIndex = 3;
            LabProcessId.Text = "ProcessId";
            // 
            // LabStatusText
            // 
            LabStatusText.AutoSize = true;
            LabStatusText.ForeColor = Color.Red;
            LabStatusText.Location = new Point(103, 66);
            LabStatusText.Name = "LabStatusText";
            LabStatusText.Size = new Size(90, 20);
            LabStatusText.TabIndex = 4;
            LabStatusText.Text = "Disconnect";
            // 
            // TxtProcessId
            // 
            TxtProcessId.Location = new Point(103, 22);
            TxtProcessId.Name = "TxtProcessId";
            TxtProcessId.Size = new Size(221, 27);
            TxtProcessId.TabIndex = 5;
            TxtProcessId.KeyPress += TxtProcessId_KeyPress;
            // 
            // BtnDisconnect
            // 
            BtnDisconnect.ForeColor = Color.IndianRed;
            BtnDisconnect.Location = new Point(330, 18);
            BtnDisconnect.Name = "BtnDisconnect";
            BtnDisconnect.Size = new Size(40, 35);
            BtnDisconnect.TabIndex = 6;
            BtnDisconnect.Text = "X";
            BtnDisconnect.UseVisualStyleBackColor = true;
            BtnDisconnect.Visible = false;
            BtnDisconnect.Click += BtnDisconnect_Click;
            // 
            // BtnDumpList
            // 
            BtnDumpList.ForeColor = SystemColors.Highlight;
            BtnDumpList.Location = new Point(250, 63);
            BtnDumpList.Name = "BtnDumpList";
            BtnDumpList.Size = new Size(120, 32);
            BtnDumpList.TabIndex = 7;
            BtnDumpList.Text = "26236578.blk";
            BtnDumpList.UseVisualStyleBackColor = true;
            BtnDumpList.Visible = false;
            BtnDumpList.Click += BtnDumpList_Click;
            // 
            // BtnConnect
            // 
            BtnConnect.ForeColor = Color.Green;
            BtnConnect.Location = new Point(330, 17);
            BtnConnect.Name = "BtnConnect";
            BtnConnect.Size = new Size(40, 35);
            BtnConnect.TabIndex = 8;
            BtnConnect.Text = "O";
            BtnConnect.UseVisualStyleBackColor = true;
            BtnConnect.Click += BtnConnect_Click;
            // 
            // LabGameVer
            // 
            LabGameVer.AutoSize = true;
            LabGameVer.Location = new Point(12, 524);
            LabGameVer.Name = "LabGameVer";
            LabGameVer.Size = new Size(175, 20);
            LabGameVer.TabIndex = 9;
            LabGameVer.Text = "Support game version:";
            // 
            // LabGameVerText
            // 
            LabGameVerText.AutoSize = true;
            LabGameVerText.ForeColor = Color.Orange;
            LabGameVerText.Location = new Point(193, 524);
            LabGameVerText.Name = "LabGameVerText";
            LabGameVerText.Size = new Size(0, 20);
            LabGameVerText.TabIndex = 10;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 553);
            Controls.Add(LabGameVerText);
            Controls.Add(LabGameVer);
            Controls.Add(BtnConnect);
            Controls.Add(BtnDumpList);
            Controls.Add(BtnDisconnect);
            Controls.Add(TxtProcessId);
            Controls.Add(LabStatusText);
            Controls.Add(LabProcessId);
            Controls.Add(LabStatus);
            Controls.Add(ListTagKeys);
            MaximizeBox = false;
            MaximumSize = new Size(400, 600);
            MinimumSize = new Size(400, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GI-VideoVersions";
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
        private Button BtnDumpList;
        private Button BtnConnect;
        private ContextMenuStrip CtxMenuCopy;
        private ToolStripMenuItem CtxItemCopy;
        private Label LabGameVer;
        private Label LabGameVerText;
    }
}
