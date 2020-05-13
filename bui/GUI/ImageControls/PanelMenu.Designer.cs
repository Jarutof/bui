namespace bui.GUI
{
    partial class PanelMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelMenu));
            this.buttonSoftProt = new bui.GUI.ButtonImage();
            this.buttonCalibrBKN = new bui.GUI.ButtonImage();
            this.buttonCalibrBV2 = new bui.GUI.ButtonImage();
            this.buttonCalibrBV1 = new bui.GUI.ButtonImage();
            this.buttonClose = new bui.GUI.ButtonImage();
            this.SuspendLayout();
            // 
            // buttonSoftProt
            // 
            this.buttonSoftProt.BackColor = System.Drawing.Color.Transparent;
            this.buttonSoftProt.BackgroundImage = global::bui.Properties.Resources.btn_empty_big_cyan;
            this.buttonSoftProt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSoftProt.Disabled = false;
            this.buttonSoftProt.Display = 0;
            this.buttonSoftProt.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonSoftProt.Images")));
            this.buttonSoftProt.IsDirty = false;
            this.buttonSoftProt.IsHidden = false;
            this.buttonSoftProt.Location = new System.Drawing.Point(3, 252);
            this.buttonSoftProt.Name = "buttonSoftProt";
            this.buttonSoftProt.Size = new System.Drawing.Size(240, 60);
            this.buttonSoftProt.TabIndex = 5;
            // 
            // buttonCalibrBKN
            // 
            this.buttonCalibrBKN.BackColor = System.Drawing.Color.Transparent;
            this.buttonCalibrBKN.BackgroundImage = global::bui.Properties.Resources.btn_empty_big_cyan;
            this.buttonCalibrBKN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonCalibrBKN.Disabled = false;
            this.buttonCalibrBKN.Display = 0;
            this.buttonCalibrBKN.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonCalibrBKN.Images")));
            this.buttonCalibrBKN.IsDirty = true;
            this.buttonCalibrBKN.IsHidden = false;
            this.buttonCalibrBKN.Location = new System.Drawing.Point(3, 150);
            this.buttonCalibrBKN.Name = "buttonCalibrBKN";
            this.buttonCalibrBKN.Size = new System.Drawing.Size(240, 60);
            this.buttonCalibrBKN.TabIndex = 3;
            // 
            // buttonCalibrBV2
            // 
            this.buttonCalibrBV2.BackColor = System.Drawing.Color.Transparent;
            this.buttonCalibrBV2.BackgroundImage = global::bui.Properties.Resources.btn_empty_big_cyan;
            this.buttonCalibrBV2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonCalibrBV2.Disabled = false;
            this.buttonCalibrBV2.Display = 0;
            this.buttonCalibrBV2.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonCalibrBV2.Images")));
            this.buttonCalibrBV2.IsDirty = true;
            this.buttonCalibrBV2.IsHidden = false;
            this.buttonCalibrBV2.Location = new System.Drawing.Point(3, 90);
            this.buttonCalibrBV2.Name = "buttonCalibrBV2";
            this.buttonCalibrBV2.Size = new System.Drawing.Size(240, 60);
            this.buttonCalibrBV2.TabIndex = 2;
            // 
            // buttonCalibrBV1
            // 
            this.buttonCalibrBV1.BackColor = System.Drawing.Color.Transparent;
            this.buttonCalibrBV1.BackgroundImage = global::bui.Properties.Resources.btn_empty_big_cyan;
            this.buttonCalibrBV1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonCalibrBV1.Disabled = false;
            this.buttonCalibrBV1.Display = 0;
            this.buttonCalibrBV1.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonCalibrBV1.Images")));
            this.buttonCalibrBV1.IsDirty = true;
            this.buttonCalibrBV1.IsHidden = false;
            this.buttonCalibrBV1.Location = new System.Drawing.Point(3, 30);
            this.buttonCalibrBV1.Name = "buttonCalibrBV1";
            this.buttonCalibrBV1.Size = new System.Drawing.Size(240, 60);
            this.buttonCalibrBV1.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.Transparent;
            this.buttonClose.BackgroundImage = global::bui.Properties.Resources.btn_empty_big_cyan;
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Disabled = false;
            this.buttonClose.Display = 0;
            this.buttonClose.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonClose.Images")));
            this.buttonClose.IsDirty = false;
            this.buttonClose.IsHidden = false;
            this.buttonClose.Location = new System.Drawing.Point(3, 414);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(240, 60);
            this.buttonClose.TabIndex = 0;
            // 
            // PanelMenu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.buttonSoftProt);
            this.Controls.Add(this.buttonCalibrBKN);
            this.Controls.Add(this.buttonCalibrBV2);
            this.Controls.Add(this.buttonCalibrBV1);
            this.Controls.Add(this.buttonClose);
            this.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "PanelMenu";
            this.Size = new System.Drawing.Size(246, 474);
            this.ResumeLayout(false);

        }

        #endregion

        private ButtonImage buttonClose;
        private ButtonImage buttonCalibrBV1;
        private ButtonImage buttonCalibrBV2;
        private ButtonImage buttonCalibrBKN;
        private ButtonImage buttonSoftProt;
    }
}
