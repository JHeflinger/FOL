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
		private bool Playing = false;
		private List<List<Entity>> Cells;
		private List<List<int>> Neighbors;
		private List<List<bool>> Status;
		private LocalInput input;

		public void Play()
        {
			Playing = true;
        }

		public void Pause()
        {
			Playing = false;
        }

		public void Reset()
        {
			ResetBoard();
        }

		void ResetBoard()
		{
			for (int r = 0; r < Size; r++)
			{
				for (int c = 0; c < Size; c++)
				{
					Cells[r][c].GetComponent<SpriteRendererComponent>().Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
					Neighbors[r][c] = 0;
					Status[r][c] = false;
				}
			}
		}

		void CreateBoard()
		{
			Cells = new List<List<Entity>>();
			Neighbors = new List<List<int>>();
			Status = new List<List<bool>>();
			for (int r = 0; r < Size; r++)
            {
				List<Entity> row = new List<Entity>();
				List<int> nrow = new List<int>();
				List<bool> srow = new List<bool>();
				for (int c = 0; c < Size; c++)
                {
					Entity cell = Scene.CreateEntity("Cell");
					cell.Translation = new Vector3((float)r - ((float)Size/2), (float)c - ((float)Size /2), 0.0f);
					cell.Scale = new Vector3(0.9f, 0.9f, 1.0f); 
					cell.AddComponent<SpriteRendererComponent>();
					cell.GetComponent<SpriteRendererComponent>().Color = new Vector4(1);
					cell.GetComponent<SpriteRendererComponent>().Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
					row.Add(cell);
					nrow.Add(0);
					srow.Add(false);
                }
				Cells.Add(row);
				Neighbors.Add(nrow);
				Status.Add(srow);
            }
		}
		
		void LoadConfig(List<List<int>> arr)
        {
			for (int i = 0; i < arr.Count; i++)
			{
				Cells[arr[i][0]][arr[i][1]].GetComponent<SpriteRendererComponent>().Color = new Vector4(1.0f);
				Status[arr[i][0]][arr[i][1]] = true;
			}
        }

		void OnCreate()
		{
			input = new LocalInput();
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
			GridControl();
			if (Playing)
			{
				Count += ts;
				if (Count > Frequency)
				{
					Count -= Frequency;
					for (int r = 0; r < Size; r++)
					{
						for (int c = 0; c < Size; c++)
						{
							if (Status[r][c])
							{
								for (int i = 0; i < 3; i++)
								{
									for (int j = 0; j < 3; j++)
									{
										if ((i != 1 || j != 1) && ValidCoordinate(r - 1 + i, c - 1 + j))
										{
											Neighbors[r - 1 + i][c - 1 + j] += 1;
										}
									}
								}
							}
						}
					}
					ProcessGeneration();
				}
			}
		}

		void GridControl()
		{
			if (input.IsMouseButtonReleased(MouseCode.ButtonLeft))
			{
				long id = Visual.GetHoveredEntityID();
				for (int r = 0; r < Size; r++)
				{
					for (int c = 0; c < Size; c++)
					{
						if (id == Cells[r][c].ID)
                        {
							if (Status[r][c])
                            {
								Cells[r][c].GetComponent<SpriteRendererComponent>().Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
								Status[r][c] = false;
                            } else
							{
								Cells[r][c].GetComponent<SpriteRendererComponent>().Color = new Vector4(1.0f);
								Status[r][c] = true;
							}
                        }
					}
				}
			}
		}

		void ProcessGeneration()
		{
			for (int r = 0; r < Size; r++)
			{
				for (int c = 0; c < Size; c++)
				{
					if (Status[r][c])
					{
						if (Neighbors[r][c] != 2 && Neighbors[r][c] != 3)
						{
							Cells[r][c].GetComponent<SpriteRendererComponent>().Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
							Status[r][c] = false;
						}
					}
					else
					{
						if (Neighbors[r][c] == 3)
						{
							Cells[r][c].GetComponent<SpriteRendererComponent>().Color = new Vector4(1.0f);
							Status[r][c] = true;
						}
					}
					Neighbors[r][c] = 0;
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