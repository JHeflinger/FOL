using System;
using System.Collections.Generic;
using Flora;

namespace Game
{
	public class Controller : Entity
	{
		public int Size;
		public float Frequency;
		private float Count;
		private List<List<Cell>> Cells;

		void CreateBoard()
		{
			Cells = new List<List<Cell>>();
			for (int r = 0; r < Size; r++)
            {
				List<Cell> row = new List<Cell>();
				for (int c = 0; c < Size; c++)
                {
					Cell cell = Scene.CreateEntity<Cell>("Cell");
					cell.Translation = new Vector3((float)r - ((float)Size/2), (float)c - ((float)Size /2), 0.0f);
					cell.Scale = new Vector3(0.9f, 0.9f, 1.0f);
					row.Add(cell);
                }
				Cells.Add(row);
            }
		}
		
		void LoadConfig(List<List<int>> arr)
        {
			for (int i = 0; i < arr.Count; i++)
            {
				Cells[arr[i][0]][arr[i][1]].Revive();
            }
        }

		void OnCreate()
		{
			CreateBoard();
			List<List<int>> config = new List<List<int>>()
			{
				new List<int> {21, 21},
				new List<int> {21, 22},
				new List<int> {22, 22},
				new List<int> {22, 21},
				new List<int> {23, 21},
				new List<int> {23, 20},
				new List<int> {23, 19},
				new List<int> {24, 20},
			};
			LoadConfig(config);
		}

		void OnDestroy()
		{
		
		}

		void OnUpdate(float ts)
		{
			Count += ts;
			if (Count > Frequency)
			{
				Count -= Frequency;
				for (int r = 0; r < Size; r++)
				{
					for (int c = 0; c < Size; c++)
					{
						Cells[r][c].Ready = true;
						if (Cells[r][c].Alive)
						{
							for (int i = 0; i < 3; i++)
							{
								for (int j = 0; j < 3; j++)
								{
									if ((i != 1 || j != 1) && ValidCoordinate(r - 1 + i, c - 1 + j))
									{
										Cells[r - 1 + i][c - 1 + j].Neighbors += 1;
									}
								}
							}
						}
					}
				}
			}
		}

		bool ValidCoordinate(int r, int c)
        {
			if (r < 0) return false;
			if (r > Size - 1) return false;
			if (c < 0) return false;
			if (c > Size - 1) return false;
			return true;
		}
	}
}