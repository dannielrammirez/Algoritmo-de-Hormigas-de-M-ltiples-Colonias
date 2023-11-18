using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace ColoniaHormigas
{
	public class LogicaColonia
	{
		private readonly ColoniaHormigas _formColonia;
		public Pixel[,] _objPixel;
		private Pixel[,] _clonPixel;

		public int[,] _arrayPersonas;
		public int _longitud;
		Random _random;
		double randomValue;
		const int VALOR_ESCALA_FEROMONA_PROMETEDORA = 300;
		const int VALOR_ESCALA_FEROMONA_INICIAL = 100;

		private const double Alpha = 2.0; // Controla la influencia de las feromonas
		private const double RandomFactor = 0.1; // Añade una cierta aleatoriedad en la decisión

		private static int seed = Environment.TickCount;

		private readonly static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
			new Random(Interlocked.Increment(ref seed))
		);

		public static Random GetThreadRandom()
		{
			return randomWrapper.Value;
		}

		public LogicaColonia()
		{
			_formColonia = ColoniaHormigas.Instance;
			_objPixel = _formColonia.ObjPixel;
			//_arrayPersonas = _formAutomata.arrayPersonas;
			_longitud = _formColonia.longitud;
			_random = new Random();
		}

		#region Proceso de Movimiento
		public Pixel[,] Movimiento()
		{
			_clonPixel = GetClonArrayObject(_objPixel);
			MoverHormiga();
			return _clonPixel;
		}

		public void MoverHormiga()
		{
			for (int x = 0; x < _longitud; x++)
			{
				for (int y = 0; y < _longitud; y++)
				{
					if (_objPixel[x, y].Estado == EnumEstado.HORMIGA)
						ProcessMovimiento(x, y);
					else if (new EnumEstado[] { EnumEstado.FEROMONA, EnumEstado.FEROMONA_OPTIMA }.Contains(_objPixel[x, y].Estado))
						ProcessFeromona(x, y);
				}
			}
		}

		private void ProcessFeromona(int x, int y)
		{
			_clonPixel[x, y].IntensidadFeromona--;

			if (_clonPixel[x, y].IntensidadFeromona <= 0 && _clonPixel[x, y].Estado != EnumEstado.HORMIGA)
				_clonPixel[x, y].Estado = EnumEstado.VACIO;
		}

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

		private void ProcessMovimiento(int x, int y)
		{
			_random = GetThreadRandom();

			if (!_objPixel[x, y].IsModified)
			{
				var posDisponibles = AnalizarVecinasByEstado(x, y, new EnumEstado[] { EnumEstado.VACIO, EnumEstado.FEROMONA, EnumEstado.FEROMONA_OPTIMA, EnumEstado.COLONIA });

				if (posDisponibles.Count() > 0)
				{
					var ultimasPosiciones = _clonPixel[x, y].ListHistoryPosition.OrderByDescending(t => t.Key).Take(3);
					posDisponibles = posDisponibles.Where(p => !ultimasPosiciones.Any(up => p == up.Value) && _clonPixel[p.X, p.Y].Estado != EnumEstado.HORMIGA).ToList();

					if (posDisponibles.Count() == 1)
					{
						var colo = posDisponibles.FirstOrDefault();

						if (_clonPixel[colo.X, colo.Y].Estado == EnumEstado.COLONIA)
						{
							ProcessLlegadaHormigaColonia(x, y);
							return;
						}
					}

					//var asddd = _clonPixel[x, y];
					var pixelBox = new Point();
					int intensidadFeromonaPrevia = 0;
					bool hayFeromonasCerca = posDisponibles.Where(t => _clonPixel[t.X, t.Y].Estado == EnumEstado.FEROMONA).Count() > 0;
					Dictionary<Point, double> probabilities;

					if (hayFeromonasCerca)
						probabilities = CalculateMovementProbabilities(posDisponibles, new Point(_objPixel[x, y].EjeX, _objPixel[x, y].EjeY));
					else
						probabilities = CalculateProbabilities(posDisponibles, new Point(_objPixel[x, y].EjeX, _objPixel[x, y].EjeY));

					
					Point nextPosition = new Point();

					if (probabilities != null)
					{
						if (hayFeromonasCerca)
							nextPosition = SelectNextPosition(probabilities);
						else
							nextPosition = ChooseNextPosition(posDisponibles, probabilities);
						
						if (nextPosition.IsEmpty)
						{
							if (_clonPixel[nextPosition.X, nextPosition.Y].Estado == EnumEstado.COLONIA)
								ProcessLlegadaHormigaColonia(x, y);

							return;
						}

						if (_clonPixel[nextPosition.X, nextPosition.Y].Estado == EnumEstado.COLONIA)
						{
							ProcessLlegadaHormigaColonia(x, y);
							return;
						}
						
						if(_clonPixel[nextPosition.X, nextPosition.Y].Estado == EnumEstado.VACIO && _clonPixel[x, y].BeforePixel == EnumEstado.FEROMONA_OPTIMA)
						{
							pixelBox = new Point(_clonPixel[x, y].EjeX, _clonPixel[x, y].EjeY);
							intensidadFeromonaPrevia = _clonPixel[nextPosition.X, nextPosition.Y].IntensidadFeromona;
						}

						_clonPixel[nextPosition.X, nextPosition.Y].BeforePixel = _clonPixel[nextPosition.X, nextPosition.Y].Estado;
						_clonPixel[nextPosition.X, nextPosition.Y].Estado = EnumEstado.HORMIGA;
						_clonPixel[nextPosition.X, nextPosition.Y].Colonia = _clonPixel[x, y].Colonia;
						_clonPixel[nextPosition.X, nextPosition.Y].Pasos = _clonPixel[x, y].Pasos + 1;
						_clonPixel[nextPosition.X, nextPosition.Y].PasosCoronar = _clonPixel[x, y].PasosCoronar;
						var pasos = _clonPixel[nextPosition.X, nextPosition.Y].Pasos;
						_clonPixel[nextPosition.X, nextPosition.Y].ListHistoryPosition = _objPixel[x, y].ListHistoryPosition;
						_clonPixel[nextPosition.X, nextPosition.Y].ListHistoryPosition.Add(pasos, new Point(_clonPixel[x, y].EjeX, _clonPixel[x, y].EjeY));
						_clonPixel[nextPosition.X, nextPosition.Y].LlegadasColonia = _clonPixel[x, y].LlegadasColonia;
						_clonPixel[nextPosition.X, nextPosition.Y].IsModified = true;
					}
					_clonPixel[x, y].Estado = _clonPixel[x, y].BeforePixel == EnumEstado.FEROMONA_OPTIMA ? EnumEstado.FEROMONA_OPTIMA : EnumEstado.VACIO;
				}
			}
		}

		private void ProcessLlegadaHormigaColonia(int x, int y)
		{
			var corono = _clonPixel[x, y];
			_clonPixel[x, y].Colonia = _clonPixel[x, y].Colonia == 'A' ? 'B' : 'A';
			_clonPixel[x, y].PasosCoronar = _clonPixel[x, y].LlegadasColonia == 0 ? _clonPixel[x, y].Pasos : _clonPixel[x, y].PasosCoronar;

			if (_clonPixel[x, y].LlegadasColonia == 0 || _clonPixel[x, y].Pasos <= _clonPixel[x, y].PasosCoronar)
			{
				_clonPixel[x, y].LlegadasColonia++;
				_clonPixel[x, y].PasosCoronar = _clonPixel[x, y].Pasos;
				_clonPixel[x, y].IntensidadFeromonasHijas = _clonPixel[x, y].LlegadasColonia * VALOR_ESCALA_FEROMONA_PROMETEDORA;

				var feromonasActivas = _clonPixel[x, y].ListHistoryPosition;

				foreach (var item in feromonasActivas)
				{
					if(_clonPixel[item.Value.X, item.Value.Y].Estado != EnumEstado.HORMIGA)
					{
						_clonPixel[item.Value.X, item.Value.Y].Estado = EnumEstado.FEROMONA_OPTIMA;
						_clonPixel[item.Value.X, item.Value.Y].IntensidadFeromona = _clonPixel[x, y].IntensidadFeromonasHijas;
					}
				}
			}

			_clonPixel[x, y].ListHistoryPosition = new Dictionary<int, Point>();
			_clonPixel[x, y].Pasos = 0;
		}

		private Point ChooseNextPosition(List<Point> adjacentPositions, Dictionary<Point, double> probabilities)
		{
			randomValue = _random.NextDouble();
			double cumulative = 0;

			foreach (var position in adjacentPositions)
			{
				cumulative += probabilities[position];
				if (randomValue < cumulative)
					return position;
			}

			return new Point();// Por si acaso, aunque no debería llegar aquí
		}

		private Dictionary<Point, double> CalculateMovementProbabilities(List<Point> listAdjacentPositions, Point currentPosition)
		{
			var probabilities = new Dictionary<Point, double>();

			double totalProbability = 0;

			foreach (var position in listAdjacentPositions)
			{
				double pheromoneLevel = new EnumEstado[]{ EnumEstado.FEROMONA, EnumEstado.FEROMONA_OPTIMA }.Contains(_clonPixel[position.X, position.Y].Estado) ? _clonPixel[position.X, position.Y].IntensidadFeromona : 0;
				double probability = Math.Pow(pheromoneLevel, Alpha) * (RandomFactor + 1.0); // Alpha es un parámetro que controla la influencia de las feromonas
				totalProbability += probability;
				probabilities[position] = probability;
			}

			foreach (var position in probabilities.Keys.ToList())
			{
				probabilities[position] /= totalProbability;
			}

			return probabilities;
		}

		private Point SelectNextPosition(Dictionary<Point, double> probabilities)
		{
			double randomValue = new Random().NextDouble();
			double cumulativeProbability = 0;

			foreach (var entry in probabilities)
			{
				cumulativeProbability += entry.Value;
				if (randomValue < cumulativeProbability)
					return entry.Key;
			}

			return probabilities.Keys.First();
		}

		private Dictionary<Point, double> CalculateProbabilities(List<Point> adjacentPositions, Point CurrentPosition)
		{
			var probabilities = new Dictionary<Point, double>();
			double total = 0;

			foreach (var position in adjacentPositions)
			{
				double probability = 1; // Probabilidad base

				probabilities[position] = probability;
				total += probability;
			}

			// Normalizar las probabilidades
			var keys = new List<Point>(probabilities.Keys);
			foreach (var key in keys)
			{
				probabilities[key] /= total;
			}

			return probabilities;
		}

		#endregion

		#region Analisis de celulas vecinas

		private List<Point> AnalizarVecinasByEstado(int x, int y, EnumEstado[] prmEstado)
		{
			var listResponse = new List<Point>();

			var coloniaPerteneciente = _clonPixel[x, y].Colonia;

			if (coloniaPerteneciente == 'A')
			{
				//vecina 5
				if (x < _longitud - 1)
				{
					int ejeX = x + 1, ejeY = y;
					if (prmEstado.Contains(_objPixel[ejeX, ejeY].Estado))
						listResponse.Add(new Point(ejeX, ejeY));
				}

				//vecina 7
				if (y < _longitud - 1)
				{
					int ejeX = x, ejeY = y + 1;
					if (prmEstado.Contains(_objPixel[ejeX, ejeY].Estado))
						listResponse.Add(new Point(ejeX, ejeY));
				}

				//vecina 8
				if (x < _longitud - 1 && y < _longitud - 1)
				{
					int ejeX = x + 1, ejeY = y + 1;
					if (prmEstado.Contains(_objPixel[ejeX, ejeY].Estado))
						listResponse.Add(new Point(ejeX, ejeY));
				}
			}
			else
			{
				//vecina 1
				if (x > 0 && y > 0)
				{
					int ejeX = x - 1, ejeY = y - 1;
					if (prmEstado.Contains(_objPixel[ejeX, ejeY].Estado))
						listResponse.Add(new Point(ejeX, ejeY));
				}

				//vecina 2
				if (y > 0)
				{
					int ejeX = x, ejeY = y - 1;
					if (prmEstado.Contains(_objPixel[ejeX, ejeY].Estado))
						listResponse.Add(new Point(ejeX, ejeY));
				}

				//vecina 4
				if (x > 0)
				{
					int ejeX = x - 1, ejeY = y;
					if (prmEstado.Contains(_objPixel[ejeX, ejeY].Estado))
						listResponse.Add(new Point(ejeX, ejeY));
				}
			}

			////vecina 3
			//if (x < _longitud - 1 && y > 0)
			//{
			//	int ejeX = x + 1, ejeY = y - 1;
			//	if (prmEstado.Contains(_objPixel[ejeX, ejeY].Estado))
			//		listResponse.Add(new Point(ejeX, ejeY));
			//}

			////vecina 6
			//if (x > 0 && y < _longitud - 1)
			//{
			//	int ejeX = x - 1, ejeY = y + 1;
			//	if (prmEstado.Contains(_objPixel[ejeX, ejeY].Estado))
			//		listResponse.Add(new Point(ejeX, ejeY));
			//}

			return listResponse;
		}

		#endregion
	}
}