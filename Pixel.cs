using System;
using System.Collections.Generic;
using System.Drawing;

namespace ColoniaHormigas
{
	public class Pixel : ICloneable
	{
		public Pixel(int ejex, int ejeY)
		{
			EjeX = ejex;
			EjeY = ejeY;
			Estado = EnumEstado.VACIO;
			NumDiasSano = 0;
			IsModified = false;
			IsPaint = false;
			FeromonaNumPasosDesdeComida = 0;
			ObjetivoY = 0;
			ListHistoryPosition = new Dictionary<int, Point>();
			Pasos = 0;
			PasosCoronar = 0;
			LlegadasColonia = 0;
			BeforePixel = EnumEstado.VACIO;
		}
		public int EjeX { get; set; }
		public int EjeY { get; set; }
		public EnumEstado Estado { get; set; }
		public int NumDiasSano { get; set; }
		public bool IsModified { get; set; }
		public bool IsPaint { get; set; }
		public int FeromonaNumPasosDesdeComida { get; set; }
		public int ObjetivoY { get; set; }
		public int Pasos { get; set; }
		public int PasosCoronar { get; set; }
		public int LlegadasColonia { get; set; }
		public int IntensidadFeromona { get; set; }
		public int IntensidadFeromonasHijas { get; set; }
		public char Colonia { get; set; }
		public EnumEstado BeforePixel { get; set; }
		public Dictionary<int, Point> ListHistoryPosition { get; set; }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}