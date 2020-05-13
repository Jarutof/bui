namespace bui.GUI.ImageControls
{
    partial class PanelSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelSettings));
            this.buttonConfirm = new bui.GUI.ButtonImage();
            this.labelUset = new bui.GUI.LabelImage();
            this.labelUmax = new bui.GUI.LabelImage();
            this.labelUmin = new bui.GUI.LabelImage();
            this.labelImax = new bui.GUI.LabelImage();
            this.buttonUset = new bui.GUI.ButtonImage();
            this.buttonUmin = new bui.GUI.ButtonImage();
            this.buttonUmax = new bui.GUI.ButtonImage();
            this.buttonImax = new bui.GUI.ButtonImage();
            this.panelKeyboard = new bui.GUI.ImageControls.PanelKeyboard();
            this.SuspendLayout();
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.BackColor = System.Drawing.Color.Transparent;
            this.buttonConfirm.BackgroundImage = global::bui.Properties.Resources.btn_empty_big;
            this.buttonConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonConfirm.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonConfirm.Images")));
            this.buttonConfirm.IsDirty = true;
            this.buttonConfirm.Location = new System.Drawing.Point(74, 411);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(240, 60);
            this.buttonConfirm.TabIndex = 0;
            // 
            // labelUset
            // 
            this.labelUset.BackColor = System.Drawing.Color.Transparent;
            this.labelUset.IsDirty = true;
            this.labelUset.Location = new System.Drawing.Point(213, 25);
            this.labelUset.Name = "labelUset";
            this.labelUset.Size = new System.Drawing.Size(172, 47);
            this.labelUset.TabIndex = 1;
            // 
            // labelUmax
            // 
            this.labelUmax.BackColor = System.Drawing.Color.Transparent;
            this.labelUmax.IsDirty = true;
            this.labelUmax.Location = new System.Drawing.Point(213, 118);
            this.labelUmax.Name = "labelUmax";
            this.labelUmax.Size = new System.Drawing.Size(172, 47);
            this.labelUmax.TabIndex = 2;
            // 
            // labelUmin
            // 
            this.labelUmin.BackColor = System.Drawing.Color.Transparent;
            this.labelUmin.IsDirty = true;
            this.labelUmin.Location = new System.Drawing.Point(213, 71);
            this.labelUmin.Name = "labelUmin";
            this.labelUmin.Size = new System.Drawing.Size(172, 47);
            this.labelUmin.TabIndex = 3;
            // 
            // labelImax
            // 
            this.labelImax.BackColor = System.Drawing.Color.Transparent;
            this.labelImax.IsDirty = true;
            this.labelImax.Location = new System.Drawing.Point(213, 165);
            this.labelImax.Name = "labelImax";
            this.labelImax.Size = new System.Drawing.Size(172, 47);
            this.labelImax.TabIndex = 2;
            // 
            // buttonUset
            // 
            this.buttonUset.BackColor = System.Drawing.Color.Transparent;
            this.buttonUset.BackgroundImage = global::bui.Properties.Resources.Uset;
            this.buttonUset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonUset.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonUset.Images")));
            this.buttonUset.IsDirty = true;
            this.buttonUset.Location = new System.Drawing.Point(3, 25);
            this.buttonUset.Name = "buttonUset";
            this.buttonUset.Size = new System.Drawing.Size(205, 45);
            this.buttonUset.TabIndex = 4;
            // 
            // buttonUmin
            // 
            this.buttonUmin.BackColor = System.Drawing.Color.Transparent;
            this.buttonUmin.BackgroundImage = global::bui.Properties.Resources.Uset;
            this.buttonUmin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonUmin.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonUmin.Images")));
            this.buttonUmin.IsDirty = true;
            this.buttonUmin.Location = new System.Drawing.Point(3, 71);
            this.buttonUmin.Name = "buttonUmin";
            this.buttonUmin.Size = new System.Drawing.Size(205, 45);
            this.buttonUmin.TabIndex = 5;
            // 
            // buttonUmax
            // 
            this.buttonUmax.BackColor = System.Drawing.Color.Transparent;
            this.buttonUmax.BackgroundImage = global::bui.Properties.Resources.Uset;
            this.buttonUmax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonUmax.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonUmax.Images")));
            this.buttonUmax.IsDirty = true;
            this.buttonUmax.Location = new System.Drawing.Point(3, 118);
            this.buttonUmax.Name = "buttonUmax";
            this.buttonUmax.Size = new System.Drawing.Size(205, 45);
            this.buttonUmax.TabIndex = 6;
            // 
            // buttonImax
            // 
            this.buttonImax.BackColor = System.Drawing.Color.Transparent;
            this.buttonImax.BackgroundImage = global::bui.Properties.Resources.Uset;
            this.buttonImax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonImax.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonImax.Images")));
            this.buttonImax.IsDirty = true;
            this.buttonImax.Location = new System.Drawing.Point(3, 165);
            this.buttonImax.Name = "buttonImax";
            this.buttonImax.Size = new System.Drawing.Size(205, 45);
            this.buttonImax.TabIndex = 7;
            // 
            // panelKeyboard
            // 
            this.panelKeyboard.IsDirty = true;
            this.panelKeyboard.Location = new System.Drawing.Point(5, 210);
            this.panelKeyboard.Name = "panelKeyboard";
            this.panelKeyboard.Size = new System.Drawing.Size(378, 200);
            this.panelKeyboard.TabIndex = 8;
            // 
            // PanelSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::bui.Properties.Resources.settings_back;
            this.Controls.Add(this.panelKeyboard);
            this.Controls.Add(this.buttonImax);
            this.Controls.Add(this.buttonUmax);
            this.Controls.Add(this.buttonUmin);
            this.Controls.Add(this.buttonUset);
            this.Controls.Add(this.labelImax);
            this.Controls.Add(this.labelUmin);
            this.Controls.Add(this.labelUmax);
            this.Controls.Add(this.labelUset);
            this.Controls.Add(this.buttonConfirm);
            this.Name = "PanelSettings";
            this.Size = new System.Drawing.Size(388, 474);
            this.ResumeLayout(false);

        }

        #endregion
        private ButtonImage buttonConfirm;
        private LabelImage labelUset;
        private LabelImage labelUmax;
        private LabelImage labelUmin;
        private LabelImage labelImax;
        private ButtonImage buttonUset;
        private ButtonImage buttonUmin;
        private ButtonImage buttonUmax;
        private ButtonImage buttonImax;
        private PanelKeyboard panelKeyboard;
    }
}
