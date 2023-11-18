using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ColoniaHormigas
{
	public class Logica
	{
		private readonly FormColonia _formAutomata;
		public Pixel[,] _objPersonas;
		private Pixel[,] _clonObjPersonas;

		public int[,] _arrayPersonas;
		public int _longitud;
		Random _random;
		const int CONST_NUM_DIAS_UCI = 7;

		private static int seed = Environment.TickCount;

		private readonly static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
			new Random(Interlocked.Increment(ref seed))
		);

		public static Random GetThreadRandom()
		{
			return randomWrapper.Value;
		}

		public Logica()
		{
			_formAutomata = FormColonia.Instance;
			_objPersonas = _formAutomata.Objpersonas;
			_arrayPersonas = _formAutomata.arrayPersonas;
			_longitud = _formAutomata.longitud;
			_random = new Random();
		}

		#region Proceso de Movimiento
		public Pixel[,] Movimiento()
		{
			_clonObjPersonas = GetClonArrayObject(_objPersonas);

			MoverIndividuo();

			return _clonObjPersonas;
		}

		public void MoverIndividuo()
		{
			for (int x = 0; x < _longitud; x++)
			{
				for (int y = 0; y < _longitud; y++)
				{
					ProcessMovimiento(x, y);
				}
			}
		}

		private void ProcessMovimiento(int x, int y)
		{
			_random = GetThreadRandom();

			if (_objPersonas[x, y].Estado != EnumEstado.COMIDA && _objPersonas[x, y].Estado != EnumEstado.VACIO && _objPersonas[x, y].Estado != EnumEstado.FEROMONA && !_clonObjPersonas[x, y].IsModified)
			{
				var posDisponibles = AnalizarVecinas(x, y).ToArray();
				posDisponibles = posDisponibles.Where(pd => !new EnumEstado[]{ EnumEstado.HORMIGA, EnumEstado.HORMIGA_CON_COMIDA }.Contains(pd.Estado)).ToArray();

				if (posDisponibles.Length > 0)
				{
					int indiceRandom = _random.Next(0, posDisponibles.Length);
					var aleaPosDis = posDisponibles[indiceRandom];
					var currentClon = GetClonObject(_clonObjPersonas[x, y]);
					var aleaClon = GetClonObject(aleaPosDis);

					if (_objPersonas[x, y].Estado == EnumEstado.HORMIGA)
					{
						if (_clonObjPersonas[aleaClon.EjeX, aleaClon.EjeY].Estado == EnumEstado.HORMIGA || _clonObjPersonas[aleaClon.EjeX, aleaClon.EjeY].Estado == EnumEstado.HORMIGA_CON_COMIDA)
							return;

						var objNewComida = AnalizarVecinasPorEstado(x, y, EnumEstado.COMIDA);

						//if (_objPersonas[aleaClon.EjeX, aleaClon.EjeY].Estado == EnumEstado.COMIDA)
						if (objNewComida != null)
						{
							_clonObjPersonas[x, y].Estado = EnumEstado.VACIO;

							_clonObjPersonas[objNewComida.EjeX, objNewComida.EjeY + 1].Estado = EnumEstado.HORMIGA_CON_COMIDA;
							_clonObjPersonas[objNewComida.EjeX, objNewComida.EjeY + 1].FeromonaNumPasosDesdeComida = 0;
							_clonObjPersonas[objNewComida.EjeX, objNewComida.EjeY + 1].IsModified = true;

							_clonObjPersonas[objNewComida.EjeX, objNewComida.EjeY].Estado = EnumEstado.COMIDA;
							_clonObjPersonas[objNewComida.EjeX, objNewComida.EjeY].IsModified = true;

						}
						else if (_objPersonas[aleaClon.EjeX, aleaClon.EjeY].Estado == EnumEstado.VACIO)
						{
							_clonObjPersonas[x, y].Estado = EnumEstado.VACIO;
							_clonObjPersonas[x, y].IsModified = true;

							_clonObjPersonas[aleaClon.EjeX, aleaClon.EjeY].Estado = EnumEstado.HORMIGA;
							_clonObjPersonas[aleaClon.EjeX, aleaClon.EjeY].IsModified = true;
						}
						else if(_objPersonas[aleaClon.EjeX, aleaClon.EjeY].Estado == EnumEstado.FEROMONA)
						{
							_clonObjPersonas[x, y].Estado = EnumEstado.VACIO;
							_clonObjPersonas[x, y].IsModified = true;

							_clonObjPersonas[aleaClon.EjeX, aleaClon.EjeY].Estado = EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO;
							_clonObjPersonas[aleaClon.EjeX, aleaClon.EjeY].IsModified = true;
						}
					}
					else if (_objPersonas[x, y].Estado == EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO)
					{
						var newVector = AnalizarVecinasObjetivoPorEstado(x, y, EnumEstado.FEROMONA);
						var newComida = AnalizarVecinasObjetivoPorEstado(x, y, EnumEstado.COMIDA);
						var hormigaCruzadaObjetivo = AnalizarVecinasObjetivoPorEstado(x, y, EnumEstado.HORMIGA_CON_COMIDA);
						bool yaOcuparonLugar = false;

						if (newVector != null)
							yaOcuparonLugar = _clonObjPersonas[newVector.EjeX, newVector.EjeY].IsModified;

						if (yaOcuparonLugar)
							return;

						if (newComida != null)
						{
							_clonObjPersonas[x, y].Estado = EnumEstado.HORMIGA_CON_COMIDA;
							_clonObjPersonas[x, y].IsModified = true;
							_clonObjPersonas[x, y].FeromonaNumPasosDesdeComida = 0;

							_clonObjPersonas[newComida.EjeX, newComida.EjeY].Estado = EnumEstado.COMIDA;
							_clonObjPersonas[newComida.EjeX, newComida.EjeY].FeromonaNumPasosDesdeComida = 0;
							_clonObjPersonas[newComida.EjeX, newComida.EjeY].IsModified = true;
						}
						if (hormigaCruzadaObjetivo != null)
						{
							_clonObjPersonas[x, y].Estado = EnumEstado.HORMIGA_CON_COMIDA;
							_clonObjPersonas[x, y].IsModified = true;

							_clonObjPersonas[hormigaCruzadaObjetivo.EjeX, hormigaCruzadaObjetivo.EjeY].Estado = EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO;
							_clonObjPersonas[hormigaCruzadaObjetivo.EjeX, hormigaCruzadaObjetivo.EjeY].IsModified = true;
						}
						if (newVector != null)
						{
							//var listVectores = AnalizarVecinasOptimasObjetivoPorEstado(x, y, EnumEstado.FEROMONA);

							//if (listVectores.Count() > 1 && listVectores.Count() < 3)
							//	newVector = listVectores.OrderBy(t => t.FeromonaNumPasosDesdeComida).FirstOrDefault();

							_clonObjPersonas[x, y].Estado = EnumEstado.FEROMONA;
							_clonObjPersonas[x, y].IsModified = true;

							_clonObjPersonas[newVector.EjeX, newVector.EjeY].Estado = EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO;
							_clonObjPersonas[newVector.EjeX, newVector.EjeY].IsModified = true;
						}
					}
					else if (_objPersonas[x, y].Estado == EnumEstado.HORMIGA_CON_COMIDA)
					{
						// DEBE COMENZAR A DEVOLVERSE A LA COLONIA

						int newX = x > _longitud / 2 ? x - 1 : x + 1;
						int newY = y == (_longitud - 1) ? (_longitud - 1) : y + 1;

						if (newY != (_longitud - 1))
						{
							var varTest = _clonObjPersonas[newX, newY];

							if (varTest.Estado != EnumEstado.HORMIGA &&
								varTest.Estado != EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO &&
								varTest.Estado != EnumEstado.HORMIGA_CON_COMIDA &&
								!_clonObjPersonas[newX, newY].IsModified
							)
							{
								_clonObjPersonas[newX, newY].Estado = EnumEstado.HORMIGA_CON_COMIDA;
								_clonObjPersonas[newX, newY].FeromonaNumPasosDesdeComida = _objPersonas[x, y].FeromonaNumPasosDesdeComida + 1;
								_clonObjPersonas[newX, newY].IsModified = true;

								_clonObjPersonas[x, y].Estado = EnumEstado.FEROMONA;
								_clonObjPersonas[x, y].FeromonaNumPasosDesdeComida = _clonObjPersonas[newX, newY].FeromonaNumPasosDesdeComida;
								_clonObjPersonas[x, y].IsModified = true;
							}
						}
						if (newY == (_longitud - 1))
						{
							var varTest = _clonObjPersonas[newX, newY];
							_clonObjPersonas[x, y].Estado = EnumEstado.FEROMONA;
							_clonObjPersonas[x, y].IsModified = true;

							var vectorColmena = _clonObjPersonas[x, newY];

							int xCalc = x;
							int	yCalc = y;

							if (!new EnumEstado[] { EnumEstado.VACIO, EnumEstado.FEROMONA }.Contains(vectorColmena.Estado))
							{
								xCalc = xCalc + 1;
								yCalc =  y == (_longitud - 1) ? y : y + 1;
							}

							_clonObjPersonas[xCalc, newY].Estado = EnumEstado.HORMIGA_ENRUTADA_HACIA_OBJETIVO;
							_clonObjPersonas[xCalc, yCalc].IsModified = true;
							_clonObjPersonas[xCalc, yCalc].Estado = EnumEstado.FEROMONA;
							_clonObjPersonas[xCalc, yCalc].IsModified = true;

							FormColonia.Instance._comidaAcumulada++;
						}
					}
				}
			}

			//int asdas = NumPixeles();
			//if (asdas != 20)
			//{

			//}
		}
		#endregion

		#region Creacion de clones
		private Pixel[,] GetClonArrayObject(Pixel[,] arrayOriginal)
		{
			Pixel[,] arrayClonado = new Pixel[_longitud, _longitud];

			for (int i = 0; i < _longitud; i++)
			{
				for (int j = 0; j < _longitud; j++)
				{
					arrayClonado[i, j] = (Pixel)((ICloneable)arrayOriginal[i, j]).Clone();
				}
			}

			return arrayClonado;
		}

		private Pixel GetClonObject(Pixel objOriginal)
		{
			Pixel objClonado;

			objClonado = (Pixel)((ICloneable)objOriginal).Clone();

			return objClonado;
		}
		#endregion

		#region Analisis de celulas vecinas
		private Pixel AnalizarVecinasObjetivoPorEstado(int x, int y, EnumEstado prmEstado)
		{
			int[,] resp;

			//vecina 1
			if (x > 0 && y > 0)
			{
				int ejeX = x - 1, ejeY = y - 1;
				var test = _objPersonas[ejeX, ejeY];
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 2
			if (y > 0)
			{
				int ejeX = x, ejeY = y - 1;
				var test = _objPersonas[ejeX, ejeY];
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 3
			if (x < _longitud - 1 && y > 0)
			{
				int ejeX = x + 1, ejeY = y - 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 4
			if (x > 0)
			{
				int ejeX = x - 1, ejeY = y;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 5
			if (x < _longitud - 1)
			{
				int ejeX = x + 1, ejeY = y;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			return null;
		}

		private List<Pixel> AnalizarVecinasOptimasObjetivoPorEstado(int x, int y, EnumEstado prmEstado)
		{
			var tempList = new List<Pixel>();

			//vecina 1
			if (x > 0 && y > 0)
			{
				int ejeX = x - 1, ejeY = y - 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					tempList.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 2
			if (y > 0)
			{
				int ejeX = x, ejeY = y - 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					tempList.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 3
			if (x < _longitud - 1 && y > 0)
			{
				int ejeX = x + 1, ejeY = y - 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					tempList.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 4
			if (x > 0)
			{
				int ejeX = x - 1, ejeY = y;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					tempList.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 5
			if (x < _longitud - 1)
			{
				int ejeX = x + 1, ejeY = y;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					tempList.Add(_objPersonas[ejeX, ejeY]);
			}

			return tempList;
		}

		private Pixel AnalizarVecinasPorEstado(int x, int y, EnumEstado prmEstado)
		{
			int[,] resp;

			//vecina 1
			if (x > 0 && y > 0)
			{
				int ejeX = x - 1, ejeY = y - 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 2
			if (y > 0)
			{
				int ejeX = x, ejeY = y - 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 3
			if (x < _longitud - 1 && y > 0)
			{
				int ejeX = x + 1, ejeY = y - 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 4
			if (x > 0)
			{
				int ejeX = x - 1, ejeY = y;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 5
			if (x < _longitud - 1)
			{
				int ejeX = x + 1, ejeY = y;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 6
			if (x > 0 && y < _longitud - 1)
			{
				int ejeX = x - 1, ejeY = y + 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 7
			if (y < _longitud - 1)
			{
				int ejeX = x, ejeY = y + 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}


			//vecina 8
			if (x < _longitud - 1 && y < _longitud - 1)
			{
				int ejeX = x + 1, ejeY = y + 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			return null;
		}

		private Pixel AnalizarVecinasColoniaPorEstado(int x, int y, EnumEstado prmEstado)
		{
			int[,] resp;

			//vecina 4
			if (x > 0)
			{
				int ejeX = x - 1, ejeY = y;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 5
			if (x < _longitud - 1)
			{
				int ejeX = x + 1, ejeY = y;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 6
			if (x > 0 && y < _longitud - 1)
			{
				int ejeX = x - 1, ejeY = y + 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			//vecina 7
			if (y < _longitud - 1)
			{
				int ejeX = x, ejeY = y + 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}


			//vecina 8
			if (x < _longitud - 1 && y < _longitud - 1)
			{
				int ejeX = x + 1, ejeY = y + 1;
				if (_objPersonas[ejeX, ejeY].Estado == prmEstado)
					return _objPersonas[ejeX, ejeY];
			}

			return null;
		}

		private List<Pixel> AnalizarVecinas(int x, int y)
		{
			var listResponse = new List<Pixel>();

			//vecina 1
			if (x > 0 && y > 0)
			{
				int ejeX = x - 1, ejeY = y - 1;
				listResponse.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 2
			if (y > 0)
			{
				int ejeX = x, ejeY = y - 1;
				listResponse.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 3
			if (x < _longitud - 1 && y > 0)
			{
				int ejeX = x + 1, ejeY = y - 1;
				listResponse.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 4
			if (x > 0)
			{
				int ejeX = x - 1, ejeY = y;
				listResponse.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 5
			if (x < _longitud - 1)
			{
				int ejeX = x + 1, ejeY = y;
				listResponse.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 6
			if (x > 0 && y < _longitud - 1)
			{
				int ejeX = x - 1, ejeY = y + 1;
				listResponse.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 7
			if (y < _longitud - 1)
			{
				int ejeX = x, ejeY = y + 1;
				listResponse.Add(_objPersonas[ejeX, ejeY]);
			}

			//vecina 8
			if (x < _longitud - 1 && y < _longitud - 1)
			{
				int ejeX = x + 1, ejeY = y + 1;
				listResponse.Add(_objPersonas[ejeX, ejeY]);
			}

			return listResponse;
		}
		#endregion

		#region Evolución
		public Pixel[,] Process()
		{
			_clonObjPersonas = GetClonArrayObject(_objPersonas);

			//for (int x = 0; x < _longitud; x++)
			//{
			//	for (int y = 0; y < _longitud; y++)
			//	{
			//		if (_clonObjPersonas[x, y].Estado != EnumEstado.VACIO)
			//			ReglaEvolucion(x, y);
			//	}
			//}

			return _clonObjPersonas;
		}

		public int NumPixeles()
		{
			int response = 0;

			Pixel[] unidimensional = _clonObjPersonas.Cast<Pixel>().ToArray();

			var newObject = unidimensional.Count(x => !new EnumEstado[] { EnumEstado.VACIO, EnumEstado.COMIDA, EnumEstado.FEROMONA }.Contains(x.Estado));

			response = newObject;

			return response;
		}

		private void ReglaEvolucion(int x, int y)
		{
			EnumEstado newStatus;
			var randomUCI = _random.Next(0, 100);
			var randomMorir = _random.Next(0, 100);
			var randomInfeccion = _random.Next(0, 100);

			//if (_objPersonas[x, y].Estado != EnumEstado.INMUNE && _objPersonas[x, y].Estado != EnumEstado.LIMITE && _objPersonas[x, y].Estado != EnumEstado.FALLECIDO && _objPersonas[x, y].Estado != EnumEstado.INDETERMINADO)
			//{
			//	if (_objPersonas[x, y].Estado == EnumEstado.CONTAGIADO)
			//	{
			//		_clonObjPersonas[x, y].NumDiasContagiado++;
			//		newStatus = _objPersonas[x, y].Estado;
			//		var vlrProbabilidadUCI = double.Parse(randomUCI.ToString());


			//		if (_objPersonas[x, y].NumDiasContagiado >= CONST_NUM_DIAS_UCI)
			//		{
			//			var formProbabilidadHospitalizacion = FormColonia.Instance._probabilidadHospitalizacion;

			//			if (vlrProbabilidadUCI <= formProbabilidadHospitalizacion)
			//				newStatus = EnumEstado.UCI;
			//		}
			//		else
			//		{
			//			if (_objPersonas[x, y].NumDiasContagiado >= FormColonia.Instance._diasEvolucionVirus)
			//			{
			//				_clonObjPersonas[x, y].NumDiasContagiado = 0;
			//				newStatus = EnumEstado.INMUNE;
			//			}
			//		}

			//	}
			//	else if (_objPersonas[x, y].Estado == EnumEstado.UCI)
			//	{
			//		_clonObjPersonas[x, y].NumDiasContagiado++;
			//		var vlrProbabilidadMorir = double.Parse(randomMorir.ToString());

			//		if (vlrProbabilidadMorir < _formAutomata._probabilidadMorir)
			//			newStatus = EnumEstado.FALLECIDO;
			//		else
			//		{
			//			if (_objPersonas[x, y].NumDiasContagiado >= FormColonia.Instance._diasEvolucionVirus)
			//			{
			//				_clonObjPersonas[x, y].NumDiasContagiado = 0;
			//				newStatus = EnumEstado.INMUNE;
			//			}
			//			else
			//				newStatus = EnumEstado.UCI;
			//		}
			//	}
			//	else if (_objPersonas[x, y].Estado == EnumEstado.FALLECIDO)
			//	{
			//		newStatus = EnumEstado.FALLECIDO;
			//	}
			//	else if (_objPersonas[x, y].Estado == EnumEstado.VACIO)
			//	{
			//		newStatus = EnumEstado.VACIO;
			//	}
			//	else
			//	{
			//		int vecinasContagiadas = AnalizarVecinasObjetivoPorEstado(x, y, EnumEstado.CONTAGIADO);

			//		var formProbabilidadInfeccion = FormColonia.Instance._probabilidadInfeccion;
			//		var formVecinasParaInfeccion = FormColonia.Instance._VecinasNecesariasParaInfeccion;

			//		if (vecinasContagiadas >= formVecinasParaInfeccion && randomInfeccion < formProbabilidadInfeccion)
			//			newStatus = EnumEstado.CONTAGIADO;
			//		else
			//			newStatus = EnumEstado.SANO;
			//	}
			//}
			//else
			//	newStatus = _objPersonas[x, y].Estado;

			//_clonObjPersonas[x, y].Estado = newStatus;
		}
		#endregion
	}
}