using System;
using System.Windows.Forms;

namespace FreeDNS_Client
{
    partial class CustomTrayIcon
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem timerToolStripMenuItem;
        private ToolStripMenuItem updateToolStripMenuItem;
        private ToolStripMenuItem openLogToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private NotifyIcon notifyIcon;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip = new ContextMenuStrip(components);
            contextMenuStrip.SuspendLayout();
            timerToolStripMenuItem = new ToolStripMenuItem();
            updateToolStripMenuItem = new ToolStripMenuItem();
            openLogToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();

            // timerToolStripMenuItem
            timerToolStripMenuItem.Name = "timerToolStripMenuItem";
            timerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            timerToolStripMenuItem.Text = "Resume FreeDNS";
            timerToolStripMenuItem.Click += new EventHandler(StartStopTime);
            // UpdateToolStripMenuItem
            updateToolStripMenuItem.Name = "UpdateToolStripMenuItem";
            updateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            updateToolStripMenuItem.Text = "Update Now";
            updateToolStripMenuItem.Click += new EventHandler(Update);
            // openLogToolStripMenuItem
            openLogToolStripMenuItem.Name = "openLogToolStripMenuItem";
            openLogToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            openLogToolStripMenuItem.Text = "Open Log";

            // aboutToolStripMenuItem
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            aboutToolStripMenuItem.Text = "About FreeDNS Tray";
            
            // exitToolStripMenuItem
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += new EventHandler(Close);
            contextMenuStrip.ResumeLayout();

            contextMenuStrip.Items.AddRange(new ToolStripItem[] {
                timerToolStripMenuItem,
                updateToolStripMenuItem,
                openLogToolStripMenuItem,
                aboutToolStripMenuItem,
                exitToolStripMenuItem});
            contextMenuStrip.Name = "contextMenuStrip";

            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = contextMenuStrip,
                Icon = Properties.Resources.radio_white,
                Text = "FreeDNS Tray",
                Visible = true
            };
        }

        #endregion
    }
}

