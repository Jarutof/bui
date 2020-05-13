using System.Drawing;
using System.Threading.Tasks;

namespace bui.GUI
{
    partial class PanelSCS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelSCS));
            this.label_BV1_U = new bui.GUI.LabelImage();
            this.label_BV1_I = new bui.GUI.LabelImage();
            this.buttonBV2 = new bui.GUI.ButtonImage();
            this.buttonBV1 = new bui.GUI.ButtonImage();
            this.label_BV2_I = new bui.GUI.LabelImage();
            this.label_BV2_U = new bui.GUI.LabelImage();
            this.stateImageKMbus = new bui.GUI.ImageControls.StateImage();
            this.stateImageKM1 = new bui.GUI.ImageControls.StateImage();
            this.label_KM1 = new bui.GUI.LabelImage();
            this.label_KMbus = new bui.GUI.LabelImage();
            this.label_BV1_temp = new bui.GUI.LabelImage();
            this.label_BV2_temp = new bui.GUI.LabelImage();
            this.stateImageK1 = new bui.GUI.ImageControls.StateImage();
            this.stateImageK2 = new bui.GUI.ImageControls.StateImage();
            this.label_K1 = new bui.GUI.LabelImage();
            this.label_K2 = new bui.GUI.LabelImage();
            this.buttonSmode = new bui.GUI.ButtonImage();
            this.buttonWmode = new bui.GUI.ButtonImage();
            this.stateImageKSrsk = new bui.GUI.ImageControls.StateImage();
            this.stateImageKSminus = new bui.GUI.ImageControls.StateImage();
            this.stateImageKSplus = new bui.GUI.ImageControls.StateImage();
            this.buttonProtections = new bui.GUI.ButtonImage();
            this.stateImageOS_BV1 = new bui.GUI.ImageControls.StateImage();
            this.stateImageOS_BV2 = new bui.GUI.ImageControls.StateImage();
            this.labelImageBV2 = new bui.GUI.LabelImage();
            this.labelImageBV1 = new bui.GUI.LabelImage();
            this.labelImageBKN = new bui.GUI.LabelImage();
            this.SuspendLayout();
            // 
            // label_BV1_U
            // 
            this.label_BV1_U.IsDirty = false;
            this.label_BV1_U.IsHidden = false;
            this.label_BV1_U.Location = new System.Drawing.Point(57, 151);
            this.label_BV1_U.Margin = new System.Windows.Forms.Padding(2);
            this.label_BV1_U.Name = "label_BV1_U";
            this.label_BV1_U.Size = new System.Drawing.Size(121, 34);
            this.label_BV1_U.TabIndex = 3;
            // 
            // label_BV1_I
            // 
            this.label_BV1_I.IsDirty = false;
            this.label_BV1_I.IsHidden = false;
            this.label_BV1_I.Location = new System.Drawing.Point(57, 118);
            this.label_BV1_I.Margin = new System.Windows.Forms.Padding(2);
            this.label_BV1_I.Name = "label_BV1_I";
            this.label_BV1_I.Size = new System.Drawing.Size(121, 34);
            this.label_BV1_I.TabIndex = 2;
            // 
            // buttonBV2
            // 
            this.buttonBV2.BackColor = System.Drawing.Color.Transparent;
            this.buttonBV2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonBV2.BackgroundImage")));
            this.buttonBV2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonBV2.Disabled = false;
            this.buttonBV2.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonBV2.Images")));
            this.buttonBV2.IsDirty = false;
            this.buttonBV2.IsHidden = false;
            this.buttonBV2.Location = new System.Drawing.Point(225, 210);
            this.buttonBV2.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBV2.Name = "buttonBV2";
            this.buttonBV2.Size = new System.Drawing.Size(138, 42);
            this.buttonBV2.TabIndex = 1;
            // 
            // buttonBV1
            // 
            this.buttonBV1.BackColor = System.Drawing.Color.Transparent;
            this.buttonBV1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonBV1.BackgroundImage")));
            this.buttonBV1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonBV1.Disabled = false;
            this.buttonBV1.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonBV1.Images")));
            this.buttonBV1.IsDirty = false;
            this.buttonBV1.IsHidden = false;
            this.buttonBV1.Location = new System.Drawing.Point(40, 210);
            this.buttonBV1.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBV1.Name = "buttonBV1";
            this.buttonBV1.Size = new System.Drawing.Size(138, 42);
            this.buttonBV1.TabIndex = 0;
            // 
            // label_BV2_I
            // 
            this.label_BV2_I.IsDirty = false;
            this.label_BV2_I.IsHidden = false;
            this.label_BV2_I.Location = new System.Drawing.Point(242, 118);
            this.label_BV2_I.Margin = new System.Windows.Forms.Padding(2);
            this.label_BV2_I.Name = "label_BV2_I";
            this.label_BV2_I.Size = new System.Drawing.Size(121, 34);
            this.label_BV2_I.TabIndex = 4;
            // 
            // label_BV2_U
            // 
            this.label_BV2_U.IsDirty = false;
            this.label_BV2_U.IsHidden = false;
            this.label_BV2_U.Location = new System.Drawing.Point(242, 151);
            this.label_BV2_U.Margin = new System.Windows.Forms.Padding(2);
            this.label_BV2_U.Name = "label_BV2_U";
            this.label_BV2_U.Size = new System.Drawing.Size(121, 34);
            this.label_BV2_U.TabIndex = 5;
            // 
            // stateImageKMbus
            // 
            this.stateImageKMbus.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageKMbus.Images")));
            this.stateImageKMbus.IsDirty = false;
            this.stateImageKMbus.IsHidden = false;
            this.stateImageKMbus.Location = new System.Drawing.Point(157, 317);
            this.stateImageKMbus.Name = "stateImageKMbus";
            this.stateImageKMbus.Size = new System.Drawing.Size(48, 36);
            this.stateImageKMbus.TabIndex = 6;
            // 
            // stateImageKM1
            // 
            this.stateImageKM1.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageKM1.Images")));
            this.stateImageKM1.IsDirty = false;
            this.stateImageKM1.IsHidden = false;
            this.stateImageKM1.Location = new System.Drawing.Point(157, 389);
            this.stateImageKM1.Name = "stateImageKM1";
            this.stateImageKM1.Size = new System.Drawing.Size(48, 36);
            this.stateImageKM1.TabIndex = 7;
            // 
            // label_KM1
            // 
            this.label_KM1.IsDirty = false;
            this.label_KM1.IsHidden = false;
            this.label_KM1.Location = new System.Drawing.Point(122, 389);
            this.label_KM1.Margin = new System.Windows.Forms.Padding(0);
            this.label_KM1.Name = "label_KM1";
            this.label_KM1.Size = new System.Drawing.Size(38, 36);
            this.label_KM1.TabIndex = 8;
            // 
            // label_KMbus
            // 
            this.label_KMbus.IsDirty = false;
            this.label_KMbus.IsHidden = false;
            this.label_KMbus.Location = new System.Drawing.Point(125, 317);
            this.label_KMbus.Margin = new System.Windows.Forms.Padding(0);
            this.label_KMbus.Name = "label_KMbus";
            this.label_KMbus.Size = new System.Drawing.Size(38, 36);
            this.label_KMbus.TabIndex = 9;
            // 
            // label_BV1_temp
            // 
            this.label_BV1_temp.IsDirty = false;
            this.label_BV1_temp.IsHidden = false;
            this.label_BV1_temp.Location = new System.Drawing.Point(20, 183);
            this.label_BV1_temp.Margin = new System.Windows.Forms.Padding(0);
            this.label_BV1_temp.Name = "label_BV1_temp";
            this.label_BV1_temp.Size = new System.Drawing.Size(158, 26);
            this.label_BV1_temp.TabIndex = 10;
            // 
            // label_BV2_temp
            // 
            this.label_BV2_temp.IsDirty = false;
            this.label_BV2_temp.IsHidden = false;
            this.label_BV2_temp.Location = new System.Drawing.Point(209, 183);
            this.label_BV2_temp.Margin = new System.Windows.Forms.Padding(0);
            this.label_BV2_temp.Name = "label_BV2_temp";
            this.label_BV2_temp.Size = new System.Drawing.Size(158, 26);
            this.label_BV2_temp.TabIndex = 11;
            // 
            // stateImageK1
            // 
            this.stateImageK1.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageK1.Images")));
            this.stateImageK1.IsDirty = false;
            this.stateImageK1.IsHidden = false;
            this.stateImageK1.Location = new System.Drawing.Point(92, 322);
            this.stateImageK1.Name = "stateImageK1";
            this.stateImageK1.Size = new System.Drawing.Size(30, 30);
            this.stateImageK1.TabIndex = 12;
            // 
            // stateImageK2
            // 
            this.stateImageK2.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageK2.Images")));
            this.stateImageK2.IsDirty = false;
            this.stateImageK2.IsHidden = false;
            this.stateImageK2.Location = new System.Drawing.Point(36, 322);
            this.stateImageK2.Name = "stateImageK2";
            this.stateImageK2.Size = new System.Drawing.Size(30, 30);
            this.stateImageK2.TabIndex = 13;
            // 
            // label_K1
            // 
            this.label_K1.IsDirty = false;
            this.label_K1.IsHidden = false;
            this.label_K1.Location = new System.Drawing.Point(64, 317);
            this.label_K1.Margin = new System.Windows.Forms.Padding(0);
            this.label_K1.Name = "label_K1";
            this.label_K1.Size = new System.Drawing.Size(34, 36);
            this.label_K1.TabIndex = 14;
            // 
            // label_K2
            // 
            this.label_K2.IsDirty = false;
            this.label_K2.IsHidden = false;
            this.label_K2.Location = new System.Drawing.Point(11, 317);
            this.label_K2.Margin = new System.Windows.Forms.Padding(0);
            this.label_K2.Name = "label_K2";
            this.label_K2.Size = new System.Drawing.Size(30, 36);
            this.label_K2.TabIndex = 15;
            // 
            // buttonSmode
            // 
            this.buttonSmode.BackColor = System.Drawing.Color.Transparent;
            this.buttonSmode.BackgroundImage = global::bui.Properties.Resources.btn_long_empty;
            this.buttonSmode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSmode.Disabled = false;
            this.buttonSmode.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonSmode.Images")));
            this.buttonSmode.IsDirty = false;
            this.buttonSmode.IsHidden = false;
            this.buttonSmode.Location = new System.Drawing.Point(3, 3);
            this.buttonSmode.Name = "buttonSmode";
            this.buttonSmode.Size = new System.Drawing.Size(190, 39);
            this.buttonSmode.TabIndex = 16;
            // 
            // buttonWmode
            // 
            this.buttonWmode.BackColor = System.Drawing.Color.Transparent;
            this.buttonWmode.BackgroundImage = global::bui.Properties.Resources.btn_long_empty;
            this.buttonWmode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonWmode.Disabled = false;
            this.buttonWmode.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonWmode.Images")));
            this.buttonWmode.IsDirty = false;
            this.buttonWmode.IsHidden = false;
            this.buttonWmode.Location = new System.Drawing.Point(194, 3);
            this.buttonWmode.Name = "buttonWmode";
            this.buttonWmode.Size = new System.Drawing.Size(190, 39);
            this.buttonWmode.TabIndex = 17;
            // 
            // stateImageKSrsk
            // 
            this.stateImageKSrsk.BackgroundImage = global::bui.Properties.Resources.KS_rsk_green;
            this.stateImageKSrsk.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageKSrsk.Images")));
            this.stateImageKSrsk.IsDirty = false;
            this.stateImageKSrsk.IsHidden = false;
            this.stateImageKSrsk.Location = new System.Drawing.Point(336, 411);
            this.stateImageKSrsk.Name = "stateImageKSrsk";
            this.stateImageKSrsk.Size = new System.Drawing.Size(38, 14);
            this.stateImageKSrsk.TabIndex = 18;
            // 
            // stateImageKSminus
            // 
            this.stateImageKSminus.BackgroundImage = global::bui.Properties.Resources.KS_rsk_green;
            this.stateImageKSminus.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageKSminus.Images")));
            this.stateImageKSminus.IsDirty = false;
            this.stateImageKSminus.IsHidden = false;
            this.stateImageKSminus.Location = new System.Drawing.Point(290, 411);
            this.stateImageKSminus.Name = "stateImageKSminus";
            this.stateImageKSminus.Size = new System.Drawing.Size(38, 14);
            this.stateImageKSminus.TabIndex = 19;
            // 
            // stateImageKSplus
            // 
            this.stateImageKSplus.BackgroundImage = global::bui.Properties.Resources.KS_rsk_green;
            this.stateImageKSplus.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageKSplus.Images")));
            this.stateImageKSplus.IsDirty = false;
            this.stateImageKSplus.IsHidden = false;
            this.stateImageKSplus.Location = new System.Drawing.Point(244, 411);
            this.stateImageKSplus.Name = "stateImageKSplus";
            this.stateImageKSplus.Size = new System.Drawing.Size(38, 14);
            this.stateImageKSplus.TabIndex = 20;
            // 
            // buttonProtections
            // 
            this.buttonProtections.BackColor = System.Drawing.Color.Transparent;
            this.buttonProtections.BackgroundImage = global::bui.Properties.Resources.protection_Icon;
            this.buttonProtections.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonProtections.Disabled = false;
            this.buttonProtections.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("buttonProtections.Images")));
            this.buttonProtections.IsDirty = false;
            this.buttonProtections.IsHidden = false;
            this.buttonProtections.Location = new System.Drawing.Point(293, 310);
            this.buttonProtections.Name = "buttonProtections";
            this.buttonProtections.Size = new System.Drawing.Size(80, 80);
            this.buttonProtections.TabIndex = 21;
            // 
            // stateImageOS_BV1
            // 
            this.stateImageOS_BV1.BackgroundImage = global::bui.Properties.Resources.sign_OS_on;
            this.stateImageOS_BV1.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageOS_BV1.Images")));
            this.stateImageOS_BV1.IsDirty = false;
            this.stateImageOS_BV1.IsHidden = false;
            this.stateImageOS_BV1.Location = new System.Drawing.Point(21, 245);
            this.stateImageOS_BV1.Name = "stateImageOS_BV1";
            this.stateImageOS_BV1.Size = new System.Drawing.Size(8, 8);
            this.stateImageOS_BV1.TabIndex = 22;
            // 
            // stateImageOS_BV2
            // 
            this.stateImageOS_BV2.BackgroundImage = global::bui.Properties.Resources.sign_OS_on;
            this.stateImageOS_BV2.Images = ((System.Collections.Generic.List<System.Drawing.Bitmap>)(resources.GetObject("stateImageOS_BV2.Images")));
            this.stateImageOS_BV2.IsDirty = false;
            this.stateImageOS_BV2.IsHidden = false;
            this.stateImageOS_BV2.Location = new System.Drawing.Point(207, 245);
            this.stateImageOS_BV2.Name = "stateImageOS_BV2";
            this.stateImageOS_BV2.Size = new System.Drawing.Size(8, 8);
            this.stateImageOS_BV2.TabIndex = 23;
            // 
            // labelImageBV2
            // 
            this.labelImageBV2.IsDirty = false;
            this.labelImageBV2.IsHidden = false;
            this.labelImageBV2.Location = new System.Drawing.Point(195, 91);
            this.labelImageBV2.Name = "labelImageBV2";
            this.labelImageBV2.Size = new System.Drawing.Size(184, 168);
            this.labelImageBV2.TabIndex = 25;
            // 
            // labelImageBV1
            // 
            this.labelImageBV1.IsDirty = false;
            this.labelImageBV1.IsHidden = false;
            this.labelImageBV1.Location = new System.Drawing.Point(9, 91);
            this.labelImageBV1.Name = "labelImageBV1";
            this.labelImageBV1.Size = new System.Drawing.Size(184, 168);
            this.labelImageBV1.TabIndex = 26;
            // 
            // labelImageBKN
            // 
            this.labelImageBKN.IsDirty = false;
            this.labelImageBKN.IsHidden = false;
            this.labelImageBKN.Location = new System.Drawing.Point(9, 265);
            this.labelImageBKN.Name = "labelImageBKN";
            this.labelImageBKN.Size = new System.Drawing.Size(370, 166);
            this.labelImageBKN.TabIndex = 27;
            // 
            // PanelSCS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::bui.Properties.Resources.blocks_shadow2;
            this.Controls.Add(this.labelImageBKN);
            this.Controls.Add(this.buttonProtections);
            this.Controls.Add(this.stateImageKSplus);
            this.Controls.Add(this.stateImageKSminus);
            this.Controls.Add(this.stateImageKSrsk);
            this.Controls.Add(this.buttonWmode);
            this.Controls.Add(this.buttonSmode);
            this.Controls.Add(this.stateImageK2);
            this.Controls.Add(this.stateImageK1);
            this.Controls.Add(this.stateImageKM1);
            this.Controls.Add(this.stateImageKMbus);
            this.Controls.Add(this.labelImageBV2);
            this.Controls.Add(this.labelImageBV1);
            this.Controls.Add(this.stateImageOS_BV2);
            this.Controls.Add(this.stateImageOS_BV1);
            this.Controls.Add(this.label_BV2_U);
            this.Controls.Add(this.label_BV2_I);
            this.Controls.Add(this.label_BV1_U);
            this.Controls.Add(this.label_BV1_I);
            this.Controls.Add(this.buttonBV2);
            this.Controls.Add(this.buttonBV1);
            this.Controls.Add(this.label_KMbus);
            this.Controls.Add(this.label_KM1);
            this.Controls.Add(this.label_BV1_temp);
            this.Controls.Add(this.label_BV2_temp);
            this.Controls.Add(this.label_K1);
            this.Controls.Add(this.label_K2);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PanelSCS";
            this.Size = new System.Drawing.Size(388, 474);
            this.ResumeLayout(false);

        }

        #endregion

        private ButtonImage buttonBV1;
        private ButtonImage buttonBV2;
        private LabelImage label_BV1_I;
        private LabelImage label_BV1_U;
        private LabelImage label_BV2_I;
        private LabelImage label_BV2_U;
        private ImageControls.StateImage stateImageKMbus;
        private ImageControls.StateImage stateImageKM1;
        private LabelImage label_KM1;
        private LabelImage label_KMbus;
        private LabelImage label_BV1_temp;
        private LabelImage label_BV2_temp;
        private ImageControls.StateImage stateImageK1;
        private ImageControls.StateImage stateImageK2;
        private LabelImage label_K1;
        private LabelImage label_K2;
        private ButtonImage buttonSmode;
        private ButtonImage buttonWmode;
        private ImageControls.StateImage stateImageKSrsk;
        private ImageControls.StateImage stateImageKSminus;
        private ImageControls.StateImage stateImageKSplus;
        private ButtonImage buttonProtections;
        private ImageControls.StateImage stateImageOS_BV1;
        private ImageControls.StateImage stateImageOS_BV2;
        private LabelImage labelImageBV2;
        private LabelImage labelImageBV1;
        private LabelImage labelImageBKN;
    }
}
