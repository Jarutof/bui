namespace bui.GUI.ImageControls
{
    partial class PanelGetParameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelGetParameter));
            this.panelKeyboard = new bui.GUI.ImageControls.PanelKeyboard();
            this.buttonConfirm = new bui.GUI.ButtonImage();
            this.labelImageMessage = new bui.GUI.LabelImage();
            this.labelTop = new bui.GUI.LabelImage();
            this.labelBottom = new bui.GUI.LabelImage();
            this.buttonCancel = new bui.GUI.ButtonImage();
            this.labelInput = new bui.GUI.LabelImage();
            this.labelMeasure = new bui.GUI.LabelImage();
            this.SuspendLayout();
            // 
            // panelKeyboard
            // 
            this.panelKeyboard.Display = 0;
            this.panelKeyboard.IsDirty = true;
            this.panelKeyboard.Location = new System.Drawing.Point(5, 210);
            this.panelKeyboard.Name = "panelKeyboard";
            this.panelKeyboard.Size = new System.Drawing.Size(378, 200);
            this.panelKeyboard.TabIndex = 9;
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.BackColor = System.Drawing.Color.Transparent;
            this.buttonConfirm.BackgroundImage = global::bui.Properties.Resources.btn_empty_big_green;
            this.buttonConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonConfirm.Disabled = false;
            this.buttonConfirm.Display = 0;
            this.buttonConfirm.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonConfirm.Images")));
            this.buttonConfirm.IsDirty = false;
            this.buttonConfirm.Location = new System.Drawing.Point(3, 411);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(186, 60);
            this.buttonConfirm.TabIndex = 10;
            // 
            // labelImageMessage
            // 
            this.labelImageMessage.Display = 0;
            this.labelImageMessage.IsDirty = false;
            this.labelImageMessage.Location = new System.Drawing.Point(5, 3);
            this.labelImageMessage.Name = "labelImageMessage";
            this.labelImageMessage.Size = new System.Drawing.Size(378, 113);
            this.labelImageMessage.TabIndex = 11;
            // 
            // labelTop
            // 
            this.labelTop.Display = 0;
            this.labelTop.IsDirty = false;
            this.labelTop.Location = new System.Drawing.Point(5, 122);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(216, 37);
            this.labelTop.TabIndex = 12;
            // 
            // labelBottom
            // 
            this.labelBottom.Display = 0;
            this.labelBottom.IsDirty = false;
            this.labelBottom.Location = new System.Drawing.Point(5, 167);
            this.labelBottom.Name = "labelBottom";
            this.labelBottom.Size = new System.Drawing.Size(216, 37);
            this.labelBottom.TabIndex = 13;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.BackgroundImage = global::bui.Properties.Resources.btn_empty_big_green;
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCancel.Disabled = false;
            this.buttonCancel.Display = 0;
            this.buttonCancel.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonCancel.Images")));
            this.buttonCancel.IsDirty = false;
            this.buttonCancel.Location = new System.Drawing.Point(199, 411);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(186, 60);
            this.buttonCancel.TabIndex = 14;
            // 
            // labelInput
            // 
            this.labelInput.BackColor = System.Drawing.Color.Transparent;
            this.labelInput.Display = 0;
            this.labelInput.IsDirty = false;
            this.labelInput.Location = new System.Drawing.Point(227, 122);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(150, 40);
            this.labelInput.TabIndex = 15;
            // 
            // labelMeasure
            // 
            this.labelMeasure.BackColor = System.Drawing.Color.Transparent;
            this.labelMeasure.Display = 0;
            this.labelMeasure.IsDirty = false;
            this.labelMeasure.Location = new System.Drawing.Point(227, 167);
            this.labelMeasure.Name = "labelMeasure";
            this.labelMeasure.Size = new System.Drawing.Size(150, 40);
            this.labelMeasure.TabIndex = 16;
            // 
            // PanelGetParameter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::bui.Properties.Resources.get_parameter;
            this.Controls.Add(this.labelMeasure);
            this.Controls.Add(this.labelInput);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelTop);
            this.Controls.Add(this.labelBottom);
            this.Controls.Add(this.labelImageMessage);
            this.Controls.Add(this.panelKeyboard);
            this.Controls.Add(this.buttonConfirm);
            this.Name = "PanelGetParameter";
            this.Size = new System.Drawing.Size(388, 474);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelKeyboard panelKeyboard;
        private ButtonImage buttonConfirm;
        private LabelImage labelImageMessage;
        private LabelImage labelTop;
        private LabelImage labelBottom;
        private ButtonImage buttonCancel;
        private LabelImage labelInput;
        private LabelImage labelMeasure;
    }
}
