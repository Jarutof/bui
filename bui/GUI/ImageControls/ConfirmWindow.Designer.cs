namespace bui.GUI
{
    partial class ConfirmWindow
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmWindow));
            this.buttonConfirm = new bui.GUI.ButtonImage();
            this.buttonCancel = new bui.GUI.ButtonImage();
            this.buttonOk = new bui.GUI.ButtonImage();
            this.SuspendLayout();
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
            this.buttonConfirm.Location = new System.Drawing.Point(114, 305);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(190, 60);
            this.buttonConfirm.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.BackgroundImage = global::bui.Properties.Resources.btn_empty_big;
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCancel.Disabled = false;
            this.buttonCancel.Display = 0;
            this.buttonCancel.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonCancel.Images")));
            this.buttonCancel.IsDirty = false;
            this.buttonCancel.Location = new System.Drawing.Point(337, 305);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(190, 60);
            this.buttonCancel.TabIndex = 1;
            // 
            // buttonOk
            // 
            this.buttonOk.BackColor = System.Drawing.Color.Transparent;
            this.buttonOk.BackgroundImage = global::bui.Properties.Resources.btn_empty_big_green;
            this.buttonOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonOk.Disabled = false;
            this.buttonOk.Display = 0;
            this.buttonOk.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonOk.Images")));
            this.buttonOk.IsDirty = false;
            this.buttonOk.Location = new System.Drawing.Point(225, 305);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(190, 60);
            this.buttonOk.TabIndex = 2;
            // 
            // ConfirmWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = global::bui.Properties.Resources.window_confirm;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonConfirm);
            this.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ConfirmWindow";
            this.Size = new System.Drawing.Size(640, 480);
            this.ResumeLayout(false);

        }

        #endregion

        private ButtonImage buttonConfirm;
        private ButtonImage buttonCancel;
        private ButtonImage buttonOk;
    }
}
