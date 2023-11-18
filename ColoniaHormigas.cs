using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColoniaHormigas
{
	public partial class ColoniaHormigas : Form
	{
		private static ColoniaHormigas instance = null;
		public static ColoniaHormigas Instance { get { return instance; } }
		public Pixel[,] ObjPixel;
		public Pixel[,] ObjColonia;
		public Pixel[,] ObjHormiga;
		public Pixel[,] PrevObjColonia;

		private Bitmap bmp = null;
		//public int longitud = 38;
		//public int longitudPixel = 16;
		public int longitud = 76;
		public int longitudPixel = 8;
		public LogicaColonia _objColonia = null;
		public ColoniaHormigas()
		{
			InitializeComponent();
			if (instance == null) instance = this;
			_objColonia = new LogicaColonia();
		}
		private void ReiniciarRejilla()
		{
			for (int i = 0; i < longitud; i++)
				for (int j = 0; j < longitud; j++)
				{
					ObjPixel[i, j] = new Pixel(i, j);
					ObjColonia[i, j] = new Pixel(i, j);
					PrevObjColonia[i, j] = new Pixel(i, j);
				}
		}

		private void btnIniciar_Click(object sender, EventArgs e)
		{
			timerColonia.Enabled = true;
		}

		private void ProcessStart()
		{
			ObjPixel = new Pixel[longitud, longitud];
			ObjColonia = new Pixel[longitud, longitud];
			PrevObjColonia = new Pixel[longitud, longitud];
			bmp = new Bitmap(pbColonia.Width, pbColonia.Height);

			ReiniciarRejilla();

			ObjPixel[0, 0].Estado = EnumEstado.COLONIA;
			ObjPixel[75, 75].Estado = EnumEstado.COLONIA;

			GenerarColoniaA();
			GenerarColoniaB();


			PintarMatriz();

			pbColonia.Image = bmp;
		}

		private void GenerarColoniaA()
		{
			ObjPixel[0, 1].Estado = EnumEstado.HORMIGA;
			ObjPixel[0, 1].Colonia = 'A';

			ObjPixel[1, 0].Estado = EnumEstado.HORMIGA;
			ObjPixel[1, 0].Colonia = 'A';

			ObjPixel[1, 1].Estado = EnumEstado.HORMIGA;
			ObjPixel[1, 1].Colonia = 'A';

			ObjPixel[10, 1].Estado = EnumEstado.HORMIGA;
			ObjPixel[10, 1].Colonia = 'A';

			ObjPixel[1, 10].Estado = EnumEstado.HORMIGA;
			ObjPixel[1, 10].Colonia = 'A';

			ObjPixel[10, 10].Estado = EnumEstado.HORMIGA;
			ObjPixel[10, 10].Colonia = 'A';

			ObjPixel[20, 1].Estado = EnumEstado.HORMIGA;
			ObjPixel[20, 1].Colonia = 'A';

			ObjPixel[1, 20].Estado = EnumEstado.HORMIGA;
			ObjPixel[1, 20].Colonia = 'A';

			ObjPixel[20, 20].Estado = EnumEstado.HORMIGA;
			ObjPixel[20, 20].Colonia = 'A';

			ObjPixel[30, 1].Estado = EnumEstado.HORMIGA;
			ObjPixel[30, 1].Colonia = 'A';

			ObjPixel[1, 30].Estado = EnumEstado.HORMIGA;
			ObjPixel[1, 30].Colonia = 'A';

			//ObjPixel[30, 30].Estado = EnumEstado.HORMIGA;
			//ObjPixel[30, 30].Colonia = 'A';
		}

		private void GenerarColoniaB()
		{
			ObjPixel[74, 74].Estado = EnumEstado.HORMIGA;
			ObjPixel[74, 74].Colonia = 'B';

			ObjPixel[75, 74].Estado = EnumEstado.HORMIGA;
			ObjPixel[75, 74].Colonia = 'B';

			ObjPixel[74, 75].Estado = EnumEstado.HORMIGA;
			ObjPixel[74, 75].Colonia = 'B';

			ObjPixel[64, 64].Estado = EnumEstado.HORMIGA;
			ObjPixel[64, 64].Colonia = 'B';

			ObjPixel[64, 74].Estado = EnumEstado.HORMIGA;
			ObjPixel[64, 74].Colonia = 'B';

			ObjPixel[74, 64].Estado = EnumEstado.HORMIGA;
			ObjPixel[74, 64].Colonia = 'B';

			ObjPixel[54, 54].Estado = EnumEstado.HORMIGA;
			ObjPixel[54, 54].Colonia = 'B';

			ObjPixel[54, 74].Estado = EnumEstado.HORMIGA;
			ObjPixel[54, 74].Colonia = 'B';

			ObjPixel[74, 54].Estado = EnumEstado.HORMIGA;
			ObjPixel[74, 54].Colonia = 'B';

			//ObjPixel[44, 44].Estado = EnumEstado.HORMIGA;
			//ObjPixel[44, 44].Colonia = 'B';

			ObjPixel[44, 74].Estado = EnumEstado.HORMIGA;
			ObjPixel[44, 74].Colonia = 'B';

			ObjPixel[74, 44].Estado = EnumEstado.HORMIGA;
			ObjPixel[74, 44].Colonia = 'B';

			//ObjPixel[64, 65].Estado = EnumEstado.HORMIGA;
			//ObjPixel[64, 65].Colonia = 'B';
		}

		private void PintarMatriz()
		{
			bmp = new Bitmap(pbColonia.Width, pbColonia.Height);

			for (int x = 0; x < longitud; x++)
				for (int y = 0; y < longitud; y++)
				{
					var tempPersona = ObjPixel[x, y];
					var tempColor = GetColorPixel(tempPersona.Estado);
					ObjPixel[x, y].IsModified = false;
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

		private void ProcessEvolucion()
		{
			Pixel[,] newObjectPixel;
			_objColonia._objPixel = ObjPixel;
			PrevObjColonia = GetClonArrayObject(ObjPixel);

			newObjectPixel = _objColonia.Movimiento();
			ObjPixel = newObjectPixel;
			PintarMatriz();
			pbColonia.Image = bmp;
			pbColonia.Refresh();
		}

		private Pixel[,] GetClonArrayObject(Pixel[,] arrayOriginal)
		{
			Pixel[,] arrayClonado = new Pixel[longitud, longitud];

			for (int i = 0; i < longitud; i++)
				for (int j = 0; j < longitud; j++)
					arrayClonado[i, j] = (Pixel)((ICloneable)arrayOriginal[i, j]).Clone();

			return arrayClonado;
		}

		private Color GetColorPixel(EnumEstado prmEstado)
		{
			_ = new Color();
			Color response;
			switch (prmEstado)
			{
				case EnumEstado.VACIO:
					response = Color.White;
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
					response = Color.Orange;
					break;
				case EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO:
					response = Color.Brown;
					break;
				case EnumEstado.HORMIGA_ENRUTADA_HACIA_COLONIA:
					response = Color.Brown;
					break;
				case EnumEstado.COLONIA:
					response = Color.Red;
					break;
				case EnumEstado.FEROMONA_OPTIMA:
					response = Color.OrangeRed;
					break;
				default:
					response = Color.White;
					break;
			}

			return response;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			timerColonia.Enabled = false;
		}

		private void timerColonia_Tick(object sender, EventArgs e)
		{
			ProcessEvolucion();
		}

		private void ColoniaHormigas_Load(object sender, EventArgs e)
		{
			ProcessStart();
			//timerColonia.Enabled = true;
		}

		private void btnAtras_Click(object sender, EventArgs e)
		{
			ObjPixel = PrevObjColonia;

			PintarMatriz();

			pbColonia.Image = bmp;
		}

		private void btnAdelante_Click(object sender, EventArgs e)
		{
			ProcessEvolucion();
		}
	}
}
