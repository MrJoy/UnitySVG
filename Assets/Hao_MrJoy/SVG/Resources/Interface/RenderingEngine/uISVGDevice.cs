using UnityEngine;
//using System.Collections;

//namespace Unity.SVG {
	public interface uISVGDevice {
		//private Color m_color = new Color(1.0f, 1.0f, 1.0f);
		/***********************************************************************************/
		void SetPixel(int x, int y);
		//Color GetPixel(int x, int y);
		void SetColor(Color color);
		//void Fill(int x, int y);
		Texture2D Render();
		//void BeginSubBuffer();
		//void EndSubBuffer();
		//void CombineSubBuffer();
		void GetBufferSize(ref int width, ref int height);		
		//void GetSpotForFill(int xIn, int yIn, ref int xOut, ref int yOut);
		void f_SetDevice(float width, float height);
		void f_SetDevice(int width, int height);
	}
//}