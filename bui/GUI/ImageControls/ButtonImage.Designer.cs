namespace bui.GUI
{
    partial class ButtonImage
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
            this.contentLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // contentLabel
            // 
            this.contentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentLabel.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.contentLabel.ForeColor = System.Drawing.Color.White;
            this.contentLabel.Location = new System.Drawing.Point(0, 0);
            this.contentLabel.Name = "contentLabel";
            this.contentLabel.Size = new System.Drawing.Size(221, 56);
            this.contentLabel.TabIndex = 0;
            this.contentLabel.Text = "label1";
            this.contentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ButtonImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::bui.Properties.Resources.btn_empty;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.contentLabel);
            this.DoubleBuffered = true;
            this.Name = "ButtonImage";
            this.Size = new System.Drawing.Size(221, 56);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label contentLabel;
    }
}
