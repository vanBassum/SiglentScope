
namespace SiglentScope
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            FRMLib.Scope.ScopeViewSettings scopeViewSettings1 = new FRMLib.Scope.ScopeViewSettings();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.scopeView1 = new FRMLib.Scope.Controls.ScopeView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(353, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "192.168.11.55:5024";
            // 
            // scopeView1
            // 
            this.scopeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scopeView1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scopeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scopeView1.DataSource = null;
            this.scopeView1.Location = new System.Drawing.Point(12, 38);
            this.scopeView1.Name = "scopeView1";
            scopeViewSettings1.BackgroundColor = System.Drawing.Color.Black;
            scopeViewSettings1.DrawScalePosHorizontal = FRMLib.Scope.DrawPosHorizontal.Bottom;
            scopeViewSettings1.DrawScalePosVertical = FRMLib.Scope.DrawPosVertical.Right;
            scopeViewSettings1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            scopeViewSettings1.GridZeroPosition = FRMLib.Scope.VerticalZeroPosition.Middle;
            scopeViewSettings1.HorizontalDivisions = 10;
            scopeViewSettings1.HorOffset = 0D;
            scopeViewSettings1.HorScale = 10D;
            scopeViewSettings1.NotifyOnChange = true;
            scopeViewSettings1.VerticalDivisions = 8;
            scopeViewSettings1.ZeroPosition = FRMLib.Scope.VerticalZeroPosition.Middle;
            this.scopeView1.Settings = scopeViewSettings1;
            this.scopeView1.Size = new System.Drawing.Size(1060, 400);
            this.scopeView1.TabIndex = 5;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 450);
            this.Controls.Add(this.scopeView1);
            this.Controls.Add(this.textBox2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox2;
        private FRMLib.Scope.Controls.ScopeView scopeView1;
        private System.Windows.Forms.Timer timer1;
    }
}

