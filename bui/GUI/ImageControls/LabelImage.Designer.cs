namespace bui.GUI
{
    partial class LabelImage
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
            this.labelContent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelContent
            // 
            this.labelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelContent.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelContent.Location = new System.Drawing.Point(0, 0);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(150, 150);
            this.labelContent.TabIndex = 0;
            this.labelContent.Text = "label1";
            this.labelContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelContent);
            this.Name = "LabelImage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelContent;
    }
}
