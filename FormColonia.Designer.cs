namespace ColoniaHormigas
{
	partial class FormColonia
	{
		/// <summary>
		/// Variable del diseñador necesaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpiar los recursos que se estén usando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.pnlColonia = new System.Windows.Forms.Panel();
			this.pbColonia = new System.Windows.Forms.PictureBox();
			this.timerColonia = new System.Windows.Forms.Timer(this.components);
			this.lblNumHormigas = new System.Windows.Forms.Label();
			this.btnRetroceder = new System.Windows.Forms.Button();
			this.btnAdelantar = new System.Windows.Forms.Button();
			this.btnIniciar = new System.Windows.Forms.Button();
			this.btnPintarInicial = new System.Windows.Forms.Button();
			this.btnPausar = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tbCantidadComida = new System.Windows.Forms.TextBox();
			this.tbCantidadHormigas = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lblBuscando = new System.Windows.Forms.Label();
			this.lblOcupadas = new System.Windows.Forms.Label();
			this.lblTransportando = new System.Windows.Forms.Label();
			this.lblComidaAlmacenada = new System.Windows.Forms.Label();
			this.pnlColonia.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbColonia)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlColonia
			// 
			this.pnlColonia.Controls.Add(this.pbColonia);
			this.pnlColonia.Location = new System.Drawing.Point(225, 12);
			this.pnlColonia.Name = "pnlColonia";
			this.pnlColonia.Size = new System.Drawing.Size(608, 608);
			this.pnlColonia.TabIndex = 0;
			// 
			// pbColonia
			// 
			this.pbColonia.BackColor = System.Drawing.Color.Black;
			this.pbColonia.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbColonia.Location = new System.Drawing.Point(0, 0);
			this.pbColonia.Name = "pbColonia";
			this.pbColonia.Size = new System.Drawing.Size(608, 608);
			this.pbColonia.TabIndex = 0;
			this.pbColonia.TabStop = false;
			// 
			// timerColonia
			// 
			this.timerColonia.Tick += new System.EventHandler(this.timerColonia_Tick);
			// 
			// lblNumHormigas
			// 
			this.lblNumHormigas.AutoSize = true;
			this.lblNumHormigas.Location = new System.Drawing.Point(13, 627);
			this.lblNumHormigas.Name = "lblNumHormigas";
			this.lblNumHormigas.Size = new System.Drawing.Size(0, 13);
			this.lblNumHormigas.TabIndex = 3;
			// 
			// btnRetroceder
			// 
			this.btnRetroceder.Location = new System.Drawing.Point(563, 700);
			this.btnRetroceder.Name = "btnRetroceder";
			this.btnRetroceder.Size = new System.Drawing.Size(132, 51);
			this.btnRetroceder.TabIndex = 4;
			this.btnRetroceder.Text = "RETROCEDER";
			this.btnRetroceder.UseVisualStyleBackColor = true;
			this.btnRetroceder.Click += new System.EventHandler(this.btnRetroceder_Click_1);
			// 
			// btnAdelantar
			// 
			this.btnAdelantar.Location = new System.Drawing.Point(701, 700);
			this.btnAdelantar.Name = "btnAdelantar";
			this.btnAdelantar.Size = new System.Drawing.Size(132, 51);
			this.btnAdelantar.TabIndex = 5;
			this.btnAdelantar.Text = "AVANZAR";
			this.btnAdelantar.UseVisualStyleBackColor = true;
			this.btnAdelantar.Click += new System.EventHandler(this.btnAdelantar_Click);
			// 
			// btnIniciar
			// 
			this.btnIniciar.Location = new System.Drawing.Point(8, 206);
			this.btnIniciar.Name = "btnIniciar";
			this.btnIniciar.Size = new System.Drawing.Size(207, 51);
			this.btnIniciar.TabIndex = 6;
			this.btnIniciar.Text = "INICIAR";
			this.btnIniciar.UseVisualStyleBackColor = true;
			this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
			// 
			// btnPintarInicial
			// 
			this.btnPintarInicial.Location = new System.Drawing.Point(8, 149);
			this.btnPintarInicial.Name = "btnPintarInicial";
			this.btnPintarInicial.Size = new System.Drawing.Size(207, 51);
			this.btnPintarInicial.TabIndex = 7;
			this.btnPintarInicial.Text = "PATRON INICIAL";
			this.btnPintarInicial.UseVisualStyleBackColor = true;
			this.btnPintarInicial.Click += new System.EventHandler(this.btnPintarInicial_Click);
			// 
			// btnPausar
			// 
			this.btnPausar.Location = new System.Drawing.Point(225, 700);
			this.btnPausar.Name = "btnPausar";
			this.btnPausar.Size = new System.Drawing.Size(203, 54);
			this.btnPausar.TabIndex = 8;
			this.btnPausar.Text = "PAUSAR";
			this.btnPausar.UseVisualStyleBackColor = true;
			this.btnPausar.Click += new System.EventHandler(this.btnPausar_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 65);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "Cantidad Comida";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 101);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Cantidad Hormigas";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(194, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(240, 55);
			this.label3.TabIndex = 11;
			this.label3.Text = "COLONIA";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.label3);
			this.panel1.Location = new System.Drawing.Point(225, 617);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(608, 77);
			this.panel1.TabIndex = 12;
			// 
			// tbCantidadComida
			// 
			this.tbCantidadComida.Location = new System.Drawing.Point(121, 62);
			this.tbCantidadComida.Name = "tbCantidadComida";
			this.tbCantidadComida.Size = new System.Drawing.Size(100, 20);
			this.tbCantidadComida.TabIndex = 13;
			// 
			// tbCantidadHormigas
			// 
			this.tbCantidadHormigas.Location = new System.Drawing.Point(121, 98);
			this.tbCantidadHormigas.Name = "tbCantidadHormigas";
			this.tbCantidadHormigas.Size = new System.Drawing.Size(100, 20);
			this.tbCantidadHormigas.TabIndex = 14;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(14, 305);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(52, 13);
			this.label4.TabIndex = 15;
			this.label4.Text = "Resumen";
			// 
			// lblBuscando
			// 
			this.lblBuscando.AutoSize = true;
			this.lblBuscando.Location = new System.Drawing.Point(14, 364);
			this.lblBuscando.Name = "lblBuscando";
			this.lblBuscando.Size = new System.Drawing.Size(72, 13);
			this.lblBuscando.TabIndex = 16;
			this.lblBuscando.Text = "No Buscando";
			// 
			// lblOcupadas
			// 
			this.lblOcupadas.AutoSize = true;
			this.lblOcupadas.Location = new System.Drawing.Point(14, 401);
			this.lblOcupadas.Name = "lblOcupadas";
			this.lblOcupadas.Size = new System.Drawing.Size(73, 13);
			this.lblOcupadas.TabIndex = 17;
			this.lblOcupadas.Text = "No Ocupadas";
			// 
			// lblTransportando
			// 
			this.lblTransportando.AutoSize = true;
			this.lblTransportando.Location = new System.Drawing.Point(14, 439);
			this.lblTransportando.Name = "lblTransportando";
			this.lblTransportando.Size = new System.Drawing.Size(93, 13);
			this.lblTransportando.TabIndex = 18;
			this.lblTransportando.Text = "No Transportando";
			// 
			// lblComidaAlmacenada
			// 
			this.lblComidaAlmacenada.AutoSize = true;
			this.lblComidaAlmacenada.Location = new System.Drawing.Point(14, 475);
			this.lblComidaAlmacenada.Name = "lblComidaAlmacenada";
			this.lblComidaAlmacenada.Size = new System.Drawing.Size(83, 13);
			this.lblComidaAlmacenada.TabIndex = 19;
			this.lblComidaAlmacenada.Text = "No Almacenado";
			// 
			// FormColonia
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(845, 761);
			this.Controls.Add(this.lblComidaAlmacenada);
			this.Controls.Add(this.lblTransportando);
			this.Controls.Add(this.lblOcupadas);
			this.Controls.Add(this.lblBuscando);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tbCantidadHormigas);
			this.Controls.Add(this.tbCantidadComida);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnPausar);
			this.Controls.Add(this.btnPintarInicial);
			this.Controls.Add(this.btnIniciar);
			this.Controls.Add(this.btnAdelantar);
			this.Controls.Add(this.btnRetroceder);
			this.Controls.Add(this.lblNumHormigas);
			this.Controls.Add(this.pnlColonia);
			this.Name = "FormColonia";
			this.Text = "Form1";
			this.pnlColonia.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbColonia)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlColonia;
		private System.Windows.Forms.PictureBox pbColonia;
		private System.Windows.Forms.Timer timerColonia;
		private System.Windows.Forms.Label lblNumHormigas;
		private System.Windows.Forms.Button btnRetroceder;
		private System.Windows.Forms.Button btnAdelantar;
		private System.Windows.Forms.Button btnIniciar;
		private System.Windows.Forms.Button btnPintarInicial;
		private System.Windows.Forms.Button btnPausar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox tbCantidadComida;
		private System.Windows.Forms.TextBox tbCantidadHormigas;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblBuscando;
		private System.Windows.Forms.Label lblOcupadas;
		private System.Windows.Forms.Label lblTransportando;
		private System.Windows.Forms.Label lblComidaAlmacenada;
	}
}

