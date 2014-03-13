namespace BioSCADA
{
    partial class Chart
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.pDockChart = new System.Windows.Forms.Panel();
            this.pDockChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // zg1
            // 
            this.zg1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.zg1.BackColor = System.Drawing.Color.Transparent;
            this.zg1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zg1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zg1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zg1.ForeColor = System.Drawing.Color.Transparent;
            this.zg1.IsAntiAlias = true;
            this.zg1.IsAutoScrollRange = true;
            this.zg1.Location = new System.Drawing.Point(0, 0);
            this.zg1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.zg1.Name = "zg1";
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(505, 237);
            this.zg1.TabIndex = 0;
            this.toolTip1.SetToolTip(this.zg1, "Gráfico de Meditação e Atenção");
            this.zg1.Load += new System.EventHandler(this.zg1_Load);
            this.zg1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.zg1_MouseClick);
            // 
            // pDockChart
            // 
            this.pDockChart.Controls.Add(this.zg1);
            this.pDockChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDockChart.Location = new System.Drawing.Point(0, 0);
            this.pDockChart.Name = "pDockChart";
            this.pDockChart.Size = new System.Drawing.Size(505, 237);
            this.pDockChart.TabIndex = 4;
            // 
            // Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pDockChart);
            this.Font = new System.Drawing.Font("Myriad Web Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Chart";
            this.Size = new System.Drawing.Size(505, 237);
            this.Resize += new System.EventHandler(this.CharPanel_Resize);
            this.pDockChart.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zg1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pDockChart; 
    }
}
