namespace bui
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelProtection = new bui.GUI.ImageControls.PanelProtection();
            this.panelGetParameter = new bui.GUI.ImageControls.PanelGetParameter();
            this.panelSettings = new bui.GUI.ImageControls.PanelSettings();
            this.panelMenu = new bui.GUI.PanelMenu();
            this.panelParams = new bui.GUI.PanelParams();
            this.panelSCS = new bui.GUI.PanelSCS();
            this.confirmWindow = new bui.GUI.ConfirmWindow();
            this.SuspendLayout();
            // 
            // panelProtection
            // 
            this.panelProtection.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelProtection.BackgroundImage")));
            this.panelProtection.Display = 0;
            this.panelProtection.IsDirty = false;
            this.panelProtection.IsHidden = false;
            this.panelProtection.Location = new System.Drawing.Point(3, 3);
            this.panelProtection.Name = "panelProtection";
            this.panelProtection.Size = new System.Drawing.Size(388, 474);
            this.panelProtection.TabIndex = 7;
            this.panelProtection.Visible = false;
            // 
            // panelGetParameter
            // 
            this.panelGetParameter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelGetParameter.BackgroundImage")));
            this.panelGetParameter.Display = 0;
            this.panelGetParameter.IsDirty = false;
            this.panelGetParameter.IsHidden = false;
            this.panelGetParameter.Location = new System.Drawing.Point(3, 3);
            this.panelGetParameter.Name = "panelGetParameter";
            this.panelGetParameter.Size = new System.Drawing.Size(388, 474);
            this.panelGetParameter.TabIndex = 6;
            this.panelGetParameter.Visible = false;
            // 
            // panelSettings
            // 
            this.panelSettings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelSettings.BackgroundImage")));
            this.panelSettings.Display = 0;
            this.panelSettings.IsDirty = false;
            this.panelSettings.IsHidden = false;
            this.panelSettings.Location = new System.Drawing.Point(3, 3);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(388, 474);
            this.panelSettings.TabIndex = 5;
            // 
            // panelMenu
            // 
            this.panelMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelMenu.BackgroundImage")));
            this.panelMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelMenu.Display = 0;
            this.panelMenu.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelMenu.IsDirty = true;
            this.panelMenu.IsHidden = false;
            this.panelMenu.Location = new System.Drawing.Point(394, 3);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(246, 474);
            this.panelMenu.TabIndex = 4;
            // 
            // panelParams
            // 
            this.panelParams.BackColor = System.Drawing.Color.Transparent;
            this.panelParams.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelParams.BackgroundImage")));
            this.panelParams.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelParams.Display = 0;
            this.panelParams.IsDirty = false;
            this.panelParams.IsHidden = false;
            this.panelParams.Location = new System.Drawing.Point(394, 3);
            this.panelParams.Name = "panelParams";
            this.panelParams.Size = new System.Drawing.Size(246, 474);
            this.panelParams.TabIndex = 3;
            // 
            // panelSCS
            // 
            this.panelSCS.BackColor = System.Drawing.Color.Transparent;
            this.panelSCS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelSCS.BackgroundImage")));
            this.panelSCS.Display = 0;
            this.panelSCS.Font = new System.Drawing.Font("Tahoma", 16F);
            this.panelSCS.IsDirty = false;
            this.panelSCS.IsHidden = false;
            this.panelSCS.Location = new System.Drawing.Point(3, 3);
            this.panelSCS.Margin = new System.Windows.Forms.Padding(0);
            this.panelSCS.Name = "panelSCS";
            this.panelSCS.Size = new System.Drawing.Size(388, 474);
            this.panelSCS.TabIndex = 1;
            // 
            // confirmWindow
            // 
            this.confirmWindow.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.confirmWindow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("confirmWindow.BackgroundImage")));
            this.confirmWindow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.confirmWindow.Display = 0;
            this.confirmWindow.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.confirmWindow.ForeColor = System.Drawing.Color.White;
            this.confirmWindow.GetDisplay = 0;
            this.confirmWindow.IsDirty = true;
            this.confirmWindow.IsHidden = false;
            this.confirmWindow.Location = new System.Drawing.Point(0, 0);
            this.confirmWindow.Margin = new System.Windows.Forms.Padding(0);
            this.confirmWindow.Name = "confirmWindow";
            this.confirmWindow.Size = new System.Drawing.Size(640, 480);
            this.confirmWindow.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::bui.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.panelProtection);
            this.Controls.Add(this.panelGetParameter);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.panelParams);
            this.Controls.Add(this.panelSCS);
            this.Controls.Add(this.confirmWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private GUI.PanelSCS panelSCS;
        private GUI.ConfirmWindow confirmWindow;
        private GUI.PanelParams panelParams;
        private GUI.PanelMenu panelMenu;
        private GUI.ImageControls.PanelSettings panelSettings;
        private GUI.ImageControls.PanelGetParameter panelGetParameter;
        private GUI.ImageControls.PanelProtection panelProtection;
    }
}

