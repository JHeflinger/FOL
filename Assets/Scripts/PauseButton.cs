using System;
using Flora;

namespace Game
{
	public class PauseButton : Entity
	{
		LocalInput input;

		void OnCreate()
		{
			input = new LocalInput();
		}

		void OnDestroy()
		{

		}

		void OnUpdate(float ts)
		{
			if (input.IsMouseButtonReleased(MouseCode.ButtonLeft))
				if (Visual.GetHoveredEntityID() == ID)
					Scene.FindEntityByName("Game Controller").As<Controller>().Pause();
		}
	}
}