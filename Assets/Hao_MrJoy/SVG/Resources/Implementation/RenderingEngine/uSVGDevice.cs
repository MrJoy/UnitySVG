using UnityEngine;

public class uSVGDevice {
	private Texture2D m_texture;
	
	private int m_width;
	private int m_height;

	private Color[,] m_buffer;

	private Color m_color = Color.white;
	/***********************************************************************************/
	public void f_SetDevice(float width, float height) {
		this.f_SetDevice( (int)width, (int)height);
	}
	public void f_SetDevice(int width, int height) {
		this.m_texture = new Texture2D(width, height);
		this.m_buffer = new Color[width + 1, height + 1];
		this.m_width = width;
		this.m_height = height;
	}

	public void SetPixel(int x, int y) {
		if ((x >= 0) && ( x < this.m_width) && (y >= 0) && ( y < this.m_height)) {
			this.m_buffer[x, y] = this.m_color;
		}
	}
	public Color GetPixel(int x, int y) {
		return this.m_buffer[x, y] ;
	}

	public void SetColor(Color color) {
		this.m_color.r = color.r;
		this.m_color.g = color.g;
		this.m_color.b = color.b;
	}

	public Texture2D Render() {
		for(int i = 0; i < this.m_width; i++) {
			for (int j = 0; j < this.m_height; j++) {
				this.m_texture.SetPixel(i, j, m_buffer[this.m_width - i -1,j]);
			}
		}
		this.m_texture.Apply();
		return this.m_texture;
	}

	public void GetBufferSize(ref int width, ref int height) {
		width = this.m_width;
		height = this.m_height;
	}
	/***********************************************************************************/
}