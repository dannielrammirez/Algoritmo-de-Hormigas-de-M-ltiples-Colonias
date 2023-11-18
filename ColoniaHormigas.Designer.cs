namespace ColoniaHormigas
{
	partial class ColoniaHormigas
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
			this.pnlColonia = new System.Windows.Forms.Panel();
			this.pbColonia = new System.Windows.Forms.PictureBox();
			this.btnIniciar = new System.Windows.Forms.Button();
			this.btnPausar = new System.Windows.Forms.Button();
			this.timerColonia = new System.Windows.Forms.Timer(this.components);
			this.btnAtras = new System.Windows.Forms.Button();
			this.btnAdelante = new System.Windows.Forms.Button();
			this.pnlColonia.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbColonia)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlColonia
			// 
			this.pnlColonia.Controls.Add(this.pbColonia);
			this.pnlColonia.Location = new System.Drawing.Point(12, 12);
			this.pnlColonia.Name = "pnlColonia";
			this.pnlColonia.Size = new System.Drawing.Size(608, 608);
			this.pnlColonia.TabIndex = 1;
			// 
			// pbColonia
			// 
			this.pbColonia.BackColor = System.Drawing.Color.White;
			this.pbColonia.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbColonia.Location = new System.Drawing.Point(0, 0);
			this.pbColonia.Name = "pbColonia";
			this.pbColonia.Size = new System.Drawing.Size(608, 608);
			this.pbColonia.TabIndex = 0;
			this.pbColonia.TabStop = false;
			// 
			// btnIniciar
			// 
			this.btnIniciar.Location = new System.Drawing.Point(626, 12);
			this.btnIniciar.Name = "btnIniciar";
			this.btnIniciar.Size = new System.Drawing.Size(207, 51);
			this.btnIniciar.TabIndex = 7;
			this.btnIniciar.Text = "INICIAR";
			this.btnIniciar.UseVisualStyleBackColor = true;
			this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
			// 
			// btnPausar
			// 
			this.btnPausar.Location = new System.Drawing.Point(626, 69);
			this.btnPausar.Name = "btnPausar";
			this.btnPausar.Size = new System.Drawing.Size(207, 51);
			this.btnPausar.TabIndex = 8;
			this.btnPausar.Text = "PAUSA";
			this.btnPausar.UseVisualStyleBackColor = true;
			this.btnPausar.Click += new System.EventHandler(this.button1_Click);
			// 
			// timerColonia
			// 
			this.timerColonia.Tick += new System.EventHandler(this.timerColonia_Tick);
			// 
			// btnAtras
			// 
			this.btnAtras.Location = new System.Drawing.Point(626, 126);
			this.btnAtras.Name = "btnAtras";
			this.btnAtras.Size = new System.Drawing.Size(101, 51);
			this.btnAtras.TabIndex = 9;
			this.btnAtras.Text = "ATRAS";
			this.btnAtras.UseVisualStyleBackColor = true;
			this.btnAtras.Click += new System.EventHandler(this.btnAtras_Click);
			// 
			// btnAdelante
			// 
			this.btnAdelante.Location = new System.Drawing.Point(733, 126);
			this.btnAdelante.Name = "btnAdelante";
			this.btnAdelante.Size = new System.Drawing.Size(101, 51);
			this.btnAdelante.TabIndex = 10;
			this.btnAdelante.Text = "ADELANTE";
			this.btnAdelante.UseVisualStyleBackColor = true;
			this.btnAdelante.Click += new System.EventHandler(this.btnAdelante_Click);
			// 
			// ColoniaHormigas
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(841, 632);
			this.Controls.Add(this.btnAdelante);
			this.Controls.Add(this.btnAtras);
			this.Controls.Add(this.btnPausar);
			this.Controls.Add(this.btnIniciar);
			this.Controls.Add(this.pnlColonia);
			this.Name = "ColoniaHormigas";
			this.Text = "ColoniaHormigas";
			this.Load += new System.EventHandler(this.ColoniaHormigas_Load);
			this.pnlColonia.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbColonia)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlColonia;
		private System.Windows.Forms.PictureBox pbColonia;
		private System.Windows.Forms.Button btnIniciar;
		private System.Windows.Forms.Button btnPausar;
		private System.Windows.Forms.Timer timerColonia;
		private System.Windows.Forms.Button btnAtras;
		private System.Windows.Forms.Button btnAdelante;
	}
}