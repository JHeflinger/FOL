using System;
using Flora;

namespace Game
{
	public class Cell : Entity
	{
		private bool m_Alive = false;
		private int m_Neighbors = 0;
		private bool m_Ready = false;

		public bool Ready
        {
            get
            {
				return m_Ready;
            }
			set
            {
				m_Ready = value;
            }
        }

		public int Neighbors
        {
            get
            {
				return m_Neighbors;
            }
            set
            {
				m_Neighbors = value;
            }
        }

		public bool Alive
        {
            get
            {
				return m_Alive;
            }
        }

		public void Kill()
        {
			m_Alive = false;
			GetComponent<SpriteRendererComponent>().Visible = m_Alive;
		}

		public void Revive()
        {
			m_Alive = true;
			GetComponent<SpriteRendererComponent>().Visible = m_Alive;
        }

		void OnCreate()
		{
			AddComponent<SpriteRendererComponent>();
			GetComponent<SpriteRendererComponent>().Color = new Vector4(1);
			Kill();
		}

		void OnDestroy()
		{
		
		}

		void OnUpdate(float ts)
		{
			if (m_Ready)
			{
				if (m_Neighbors > 0) Console.WriteLine(m_Neighbors.ToString());
				if (m_Alive)
				{
					if (m_Neighbors != 2 && m_Neighbors != 3)
					{
						Kill();
					}
				}
				else
				{
					if (m_Neighbors == 3)
					{
						Revive();
					}
				}
				m_Neighbors = 0;
				m_Ready = false;
			}
		}
	}
}