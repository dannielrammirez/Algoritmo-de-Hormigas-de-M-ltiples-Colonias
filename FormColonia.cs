using ColoniaHormigas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace ColoniaHormigas
{
	public partial class FormColonia : Form
	{
		private static FormColonia instance = null;
		public static FormColonia Instance { get { return instance; } }

		private int longitudPixel = 0;
		private double factorMultiplicador;
		private double _numPoblacion = 0;
		public int longitud = 0;
		public int countDias = 0;
		public int[,] arrayPersonas;
		public double _probabilidadMorir = 0;
		public double _probabilidadMovimiento = 0;
		public double _probabilidadHospitalizacion = 0;
		public int _VecinasNecesariasParaInfeccion = 0;
		public int _diasEvolucionVirus = 0;
		public double _probabilidadInfeccion = 0;
		public int _comidaAcumulada = 0;

		private int _vlrNumeroComida = 0;
		private int _vlrNumeroHormigas = 0;

		public Pixel[,] PrevObjpersonas;
		public Pixel[,] Objpersonas;
		public Logica _objAutomata = null;
		public List<ResumenCiclo> _resumenDia;
		private Bitmap bmp = null;

		public FormColonia()
		{
			InitializeComponent();
			if (instance == null) instance = this;
			_objAutomata = new Logica();
			_resumenDia = new List<ResumenCiclo>();
		}

		private void ReiniciarRejilla()
		{
			for (int i = 0; i < longitud; i++)
				for (int j = 0; j < longitud; j++)
				{
					Objpersonas[i, j] = new Pixel(i, j);
					arrayPersonas[i, j] = 0;
				}

			PintarMatriz();
		}

		private bool ValidarInformacionInicial()
		{
			try
			{
				//_numSanos = GetValFromPorc(_numPoblacion, tbPorSanosIniciales.Text) * factorMultiplicador;
				//_numContagiados = GetValFromPorc(_numPoblacion, tbPorContagiadosIniciales.Text) * factorMultiplicador;
				//_numAsintomaticos = GetValFromPorc(_numPoblacion, tbPorAsintomaticosIniciales.Text) * factorMultiplicador;
				//_numInmunes = GetValFromPorc(_numPoblacion, tbPorInmunesIniciales.Text) * factorMultiplicador;
				//_numHospitalizados = GetValFromPorc(_numPoblacion, tbPorUCIIniciales.Text) * factorMultiplicador;
				//_numFallecidos = GetValFromPorc(_numPoblacion, tbPorFallecidosIniciales.Text) * factorMultiplicador;
				//_probabilidadMorir = !string.IsNullOrEmpty(tbProbabilidadMorir.Text) ? int.Parse(tbProbabilidadMorir.Text) : 0;
				//_probabilidadHospitalizacion = !string.IsNullOrEmpty(tbProbabilidadHospitalizacion.Text) ? int.Parse(tbProbabilidadHospitalizacion.Text) : 0;
				//_probabilidadInfeccion = !string.IsNullOrEmpty(tbProbabilidadInfeccion.Text) ? int.Parse(tbProbabilidadInfeccion.Text) : 50;
				//_probabilidadMovimiento = !string.IsNullOrEmpty(tbProbabilidadMovimiento.Text) ? int.Parse(tbProbabilidadMovimiento.Text) : 50;
				//_diasEvolucionVirus = !string.IsNullOrEmpty(tbDiasEvolucionVirus.Text) ? int.Parse(tbDiasEvolucionVirus.Text) : 0;

				//if (string.IsNullOrEmpty(tbInfeccionesNecesarias.Text)) return false;

				//_VecinasNecesariasParaInfeccion = int.Parse(tbInfeccionesNecesarias.Text);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Excepcion: FormAutomara->ValidarInformacionInicial: {ex}");
				return false;
			}
		}

		private double GetValFromPorc(double totalPoblacion, string prmStrValPorcentaje)
		{
			double response = 0;

			if (string.IsNullOrEmpty(prmStrValPorcentaje))
				response = 0;
			else
			{
				bool isValid = int.TryParse(prmStrValPorcentaje, out int vlrPorcentaje);
				if (isValid)
					return (vlrPorcentaje * totalPoblacion) / 100;
			}

			return response;
		}

		private void PindarDatos()
		{
			int totalPoblacion = 0;
			int totalLimite = 0;
			int totalIndeterminados = 0;

			int grupoPixeles = 0;
			PintarPixelAleatorio(_vlrNumeroHormigas, EnumEstado.HORMIGA, grupoPixeles);
			PintarPixelAleatorio(_vlrNumeroComida, EnumEstado.COMIDA, grupoPixeles);

			PintarMatriz();
		}

		private void PintarPixelAleatorio(double numMaxPintados, EnumEstado estadoPintar, int grupoPixeles)
		{
			int numPintados = 0;
			Random random = new Random();

			var tmpLongitud = (int)longitud / 10;

			while (numPintados < (int)numMaxPintados)
			{
				int ejeX = random.Next(0, longitud);
				int ejeY = estadoPintar == EnumEstado.HORMIGA ? random.Next(tmpLongitud * 5, longitud) : random.Next(0, tmpLongitud * 6);
				//int ejeY = estadoPintar == EnumEstado.HORMIGA ? random.Next(tmpLongitud * 5, longitud) : random.Next(0, tmpLongitud * 10);
				//int ejeY = random.Next(0, longitud);

				if (arrayPersonas[ejeX, ejeY] == grupoPixeles && !Objpersonas[ejeX, ejeY].IsPaint)
				{
					Objpersonas[ejeX, ejeY].Estado = estadoPintar;
					Objpersonas[ejeX, ejeY].IsPaint = true;
					arrayPersonas[ejeX, ejeY] = 1;
					numPintados++;
				}
			}
		}

		private void PintarPixelHormigaAleatorio(double numMaxPintados, EnumEstado estadoPintar, int grupoPixeles)
		{
			int numPintados = 0;
			Random random = new Random();

			var tmpLongitud = (int)longitud / 3;

			while (numPintados < (int)numMaxPintados)
			{
				int ejeX = random.Next(0, longitud);
				int ejeY = random.Next(tmpLongitud * 2, longitud);

				//if (ejeY < tmpLongitud * 2) continue;

				if (arrayPersonas[ejeX, ejeY] == grupoPixeles && !Objpersonas[ejeX, ejeY].IsPaint)
				{
					Objpersonas[ejeX, ejeY].Estado = estadoPintar;
					Objpersonas[ejeX, ejeY].IsPaint = true;
					arrayPersonas[ejeX, ejeY] = 1;
					numPintados++;
				}
			}
		}

		public int NumPixelesPorEstado(EnumEstado prmEstado)
		{
			int response = 0;

			Pixel[] unidimensional = Objpersonas.Cast<Pixel>().ToArray();

			var newObject = unidimensional.Count(x => x.Estado == prmEstado);

			response = newObject;

			return response;
		}

		public int NumPixeles()
		{
			int response = 0;

			Pixel[] unidimensional = Objpersonas.Cast<Pixel>().ToArray();

			var newObject = unidimensional.Count(x => !new EnumEstado[] { EnumEstado.VACIO, EnumEstado.COMIDA, EnumEstado.FEROMONA }.Contains(x.Estado));

			response = newObject;

			return response;
		}

		private void PintarMatriz()
		{
			bmp = new Bitmap(pbColonia.Width, pbColonia.Height);

			for (int x = 0; x < longitud; x++)
				for (int y = 0; y < longitud; y++)
				{
					var tempPersona = Objpersonas[x, y];
					var tempColor = GetColorPixel(tempPersona.Estado);
					Objpersonas[x, y].IsModified = false;
					PintarPixel(bmp, x, y, tempColor);
				}
		}

		private void PintarPixel(Bitmap bmp, int x, int y, Color prmColor)
		{
			for (int _x = 0; _x < longitudPixel; _x++)
				for (int _y = 0; _y < longitudPixel; _y++)
				{
					var ejeX = _x + (x * longitudPixel);
					var ejeY = _y + (y * longitudPixel);

					bmp.SetPixel(ejeX, ejeY, prmColor);
				}
		}

		private Color GetColorPixel(EnumEstado prmEstado)
		{
			_ = new Color();
			Color response;
			switch (prmEstado)
			{
				case EnumEstado.VACIO:
					response = Color.Black;
					break;
				case EnumEstado.HORMIGA:
					response = Color.Brown;
					break;
				case EnumEstado.COMIDA:
					response = Color.DarkOrange;
					break;
				case EnumEstado.HORMIGA_CON_COMIDA:
					response = Color.Red;
					break;
				case EnumEstado.FEROMONA:
					response = Color.AliceBlue;
					break;
				case EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO:
					response = Color.Brown;
					break;
				case EnumEstado.HORMIGA_ENRUTADA_HACIA_COLONIA:
					response = Color.Brown;
					break;
				default:
					response = Color.White;
					break;
			}

			return response;
		}

		private void PintarInformacionInicial()
		{
			int sumaPorcentajes = GetSumaPorcentajes();

			if (sumaPorcentajes > 100)
			{
				MessageBox.Show("La suma de los porcentajes es mayor a 100, verifique la información e intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			countDias = 0;
			_resumenDia.Clear();
			_numPoblacion = 0;
			CargarTamanios();

			arrayPersonas = new int[longitud, longitud];
			Objpersonas = new Pixel[longitud, longitud];
			bmp = new Bitmap(pbColonia.Width, pbColonia.Height);

			bool isValid = ValidarInformacionInicial();

			if (isValid)
			{
				ReiniciarRejilla();
				PindarDatos();
				ShowResumen();
				btnIniciar.Enabled = true;
				//btnAvanzar.Enabled = true;

				pbColonia.Image = bmp;
			}
			else
				MessageBox.Show("Datos incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

			stopwatch.Stop();
			//tSStatusLabel.Text = $"Pintar Patron Inicial - Tiempo de ejecución: (m/s) {stopwatch.ElapsedMilliseconds}";
		}

		private int GetSumaPorcentajes()
		{
			//int response = GetValorInput(tbPorSanosIniciales.Text) + GetValorInput(tbPorContagiadosIniciales.Text) + GetValorInput(tbPorAsintomaticosIniciales.Text);
			//response += GetValorInput(tbPorInmunesIniciales.Text) + GetValorInput(tbPorUCIIniciales.Text) + GetValorInput(tbPorFallecidosIniciales.Text);

			//return response;
			return 10;
		}

		private int GetValorInput(string prmValorInput)
		{
			int response = string.IsNullOrEmpty(prmValorInput) ? 0 : int.Parse(prmValorInput);
			return response;
		}

		private void CargarTamanios()
		{
			longitud = 38;
			longitudPixel = 16;
			factorMultiplicador = 0.125;
			_numPoblacion = _numPoblacion == 0 ? (longitud * longitud) * 8 : _numPoblacion;
		}

		private void ShowResumen()
		{
			int numHormigas = NumPixeles();
			int hormigasBuscando = NumPixelesPorEstado(EnumEstado.HORMIGA);
			int hormigasOcupadas = NumPixelesPorEstado(EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO);
			int hormigasCargadas = NumPixelesPorEstado(EnumEstado.HORMIGA_CON_COMIDA);
			int comidaAcumulada = _comidaAcumulada;

			lblBuscando.Text = $"Buscando: {hormigasBuscando}"; ;
			lblOcupadas.Text = $"Ocupadas: {hormigasOcupadas}";
			lblTransportando.Text = $"Transportando: {hormigasCargadas}";
			lblComidaAlmacenada.Text = $"Comida Almacenada: {comidaAcumulada}";

			if(numHormigas < 20)
			{
				//btnPausarSimulacion.Enabled = false;
				//btnStartProcess.Enabled = true;
				//timerColonia.Enabled = false;
			}
			//countDias++;

			//var objResumenDia = new ResumenCiclo()
			//{
			//	Dia = countDias,
			//	NumHormigas = numHormigas
			//};

			//_resumenDia.Add(objResumenDia);

			lblNumHormigas.Text = $"Hormigas: {numHormigas}";
		}

		private void BtnStartProcess_Click(object sender, EventArgs e)
		{
			timerColonia.Enabled = true;
			btnPausar.Enabled = true;
			btnIniciar.Enabled = false;
		}

		private Pixel[,] GetClonArrayObject(Pixel[,] arrayOriginal)
		{
			Pixel[,] arrayClonado = new Pixel[longitud, longitud];

			for (int i = 0; i < longitud; i++)
				for (int j = 0; j < longitud; j++)
					arrayClonado[i, j] = (Pixel)((ICloneable)arrayOriginal[i, j]).Clone();

			return arrayClonado;
		}

		private void ProcessEvolucion()
		{
			_objAutomata._longitud = longitud;
			_objAutomata._objPersonas = Objpersonas;
			_objAutomata._arrayPersonas = arrayPersonas;
			PrevObjpersonas = GetClonArrayObject(Objpersonas);
			//btnRetroceder.Enabled = true;
			//btnResumenEvolucion.Enabled = true;
			Pixel[,] newObjectpersonas;

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();


			//if (checkInfeccion.Checked)
			//{
			//	newObjectpersonas = _objAutomata.Process();
			//	Objpersonas = newObjectpersonas;
			//	PintarMatriz();
				ShowResumen();
			//	pbColonia.Image = bmp;
			//	_objAutomata._objPersonas = Objpersonas;
			//	pbColonia.Refresh();
			//}

			//if (checkMovimiento.Checked)
			//{
				newObjectpersonas = _objAutomata.Movimiento();
				Objpersonas = newObjectpersonas;
				PintarMatriz();
				pbColonia.Image = bmp;
				pbColonia.Refresh();
			//}

			stopwatch.Stop();

			//tSStatusLabel.Text = $"Tiempo de ejecución: (m/s) {stopwatch.ElapsedMilliseconds}";
		}

		private void BtnPausarSimulacion_Click(object sender, EventArgs e)
		{
			btnPausar.Enabled = false;
			btnIniciar.Enabled = true;
			timerColonia.Enabled = false;
		}

		private void BtnAvanzar_Click(object sender, EventArgs e)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			ProcessEvolucion();

			stopwatch.Stop();
			//tSStatusLabel.Text = $"Avanzar 1 dia - Tiempo de ejecución: (m/s) {stopwatch.ElapsedMilliseconds}";
		}

		private void BtnResumenEvolucion_Click(object sender, EventArgs e)
		{
			//var objFormResumen = new ResumenAutomata(_resumenDia);
			//objFormResumen.ShowDialog();
		}

		private void BtnRetroceder_Click(object sender, EventArgs e)
		{
			countDias -= 2;
			ShowResumen();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			Objpersonas = PrevObjpersonas;

			PintarMatriz();
			ShowResumen();

			pbColonia.Image = bmp;

			stopwatch.Stop();
			//btnRetroceder.Enabled = false;

			//tSStatusLabel.Text = $"Retroceder - Tiempo de ejecución: (m/s) {stopwatch.ElapsedMilliseconds}";
		}

		private void CbTamPixel_SelectedIndexChanged(object sender, EventArgs e)
		{
			//var tempTamanio = cbTamPixel.Text.Replace("px", "");
			//int tamPixel = int.Parse(tempTamanio);

			//if (tamPixel != 1)
			//{
			//	checkUsarMapa.Visible = false;
			//	checkUsarMapa.Checked = false;
			//}
			//else
			//{
			//	checkUsarMapa.Visible = true;
			//}
		}

		private void timerColonia_Tick(object sender, EventArgs e)
		{
			ProcessEvolucion();
		}

		private void btnRetroceder_Click_1(object sender, EventArgs e)
		{
			countDias -= 2;
			ShowResumen();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			Objpersonas = PrevObjpersonas;

			PintarMatriz();
			ShowResumen();

			pbColonia.Image = bmp;

			stopwatch.Stop();
			//btnRetroceder.Enabled = false;

			//tSStatusLabel.Text = $"Retroceder - Tiempo de ejecución: (m/s) {stopwatch.ElapsedMilliseconds}";
		}

		private void btnAdelantar_Click(object sender, EventArgs e)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			ProcessEvolucion();

			stopwatch.Stop();
		}

		private void btnIniciar_Click(object sender, EventArgs e)
		{
			timerColonia.Enabled = true;
			btnPausar.Enabled = true;
			btnIniciar.Enabled = false;
		}

		private void btnPintarInicial_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(tbCantidadComida.Text) || string.IsNullOrWhiteSpace(tbCantidadHormigas.Text))
					MessageBox.Show("Ingrese los valores!");

				_vlrNumeroComida = int.Parse(tbCantidadComida.Text);
				_vlrNumeroHormigas = int.Parse(tbCantidadHormigas.Text);

				PintarInformacionInicial();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ingrese valores correctos!");
			}
		}

		private void btnPausar_Click(object sender, EventArgs e)
		{
			btnPausar.Enabled = false;
			btnIniciar.Enabled = true;
			timerColonia.Enabled = false;
		}
	}
}