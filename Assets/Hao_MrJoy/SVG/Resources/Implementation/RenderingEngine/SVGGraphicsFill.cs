using UnityEngine;
using System;
using System.Collections;

public class SVGGraphicsFill : ISVGPathDraw
{
	private const sbyte FILL_FLAG = -1;
	private float[,] _neighbor = new float[4, 2] {{-1.0f, 0.0f}, {0.0f, -1.0f}, {1.0f, 0.0f}, {0.0f, 1.0f}};

	private SVGGraphics _graphics;
	private SVGBasicDraw _basicDraw;

	private sbyte _flagStep;
	private sbyte[,] _flag;

	//Chieu rong va chieu dai cua picture;
	private int _width, _height;

	private int _translateX;
	private int _translateY;

	private int _subW, _subH;

	public SVGGraphicsFill(SVGGraphics graphics)
	{
		_graphics = graphics;
		_flagStep = 0;
		_width = 0;
		_height = 0;

		_translateX = 0;
		_translateY = 0;
		_subW = _subH = 0;
		//Basic Draw
		_basicDraw = new SVGBasicDraw();
		_basicDraw.SetPixelMethod = new SetPixelDelegate(SetPixelForFlag);
	}

	public void SetSize(float width, float height)
	{
		_width = (int) width;
		_height = (int) height;
		_subW = _width;
		_subH = _height;
		_flag = new sbyte[(int) width + 1, (int) height + 1];
	}

	public void SetColor(Color color)
	{
		_graphics.SetColor(color);
	}

	private void SetPixelForFlag(int x, int y)
	{
		int tx = x + _translateX;
		int ty = y + _translateY;
		if (isInZone(tx, ty))
		{
			_flag[tx, ty] = _flagStep;
		}
	}

	private int _inZoneL = 0, _inZoneT = 0;

	private bool isInZone(int x, int y)
	{
		if ((x >= _inZoneL && x < _subW + _inZoneL) && (y >= _inZoneT && y < _subH + _inZoneT))
		{
			return true;
		}
		return false;
	}

	private Vector2 _boundTopLeft, _boundBottomRight;

	private void ExpandBounds(Vector2 point)
	{
		if (point.x < _boundTopLeft.x) _boundTopLeft.x = point.x;
		if (point.y < _boundTopLeft.y) _boundTopLeft.y = point.y;

		if (point.x > _boundBottomRight.x) _boundBottomRight.x = point.x;
		if (point.y > _boundBottomRight.y) _boundBottomRight.y = point.y;
	}

	private void ExpandBounds(Vector2 point, float dx, float dy)
	{
		if (point.x - dy < _boundTopLeft.x) _boundTopLeft.x = point.x - dx;
		if (point.y - dx < _boundTopLeft.y) _boundTopLeft.y = point.y - dy;

		if (point.x + dx > _boundBottomRight.x) _boundBottomRight.x = point.x + dx;
		if (point.y + dy > _boundBottomRight.y) _boundBottomRight.y = point.y + dy;
	}

	//Tinh Bound cho Fill
	private void ExpandBounds(Vector2[] points)
	{
		int _length = points.Length;
		for (int i = 0; i < _length; i++)
			ExpandBounds(points[i]);
	}

	private void ExpandBounds(Vector2[] points, int deltax, int deltay)
	{
		int _length = points.Length;
		for (int i = 0; i < _length; i++)
			ExpandBounds(points[i], deltax, deltay);
	}

	//Fill se to lan tu vi tri(x,y)theo gia tri this._flagStep
	private static LiteStack<Vector2> _stack = new LiteStack<Vector2>();

	private void Fill(int x, int y)
	{
		if (!isInZone(x, y))
			return;
		_stack.Clear();

		Vector2 temp = new Vector2(x, y);
		_flag[(int) temp.x, (int) temp.y] = FILL_FLAG;
		_stack.Push(temp);

		while (_stack.Count > 0)
		{
			temp = _stack.Pop();
			for (int t = 0; t < 4; t++)
			{
				float tx, ty;
				tx = temp.x + _neighbor[t, 0];
				ty = temp.y + _neighbor[t, 1];
				if (isInZone((int) tx, (int) ty))
				{
					if (_flag[(int) tx, (int) ty] == 0)
					{
						_flag[(int) tx, (int) ty] = FILL_FLAG;
						_stack.Push(new Vector2(tx, ty));
					}
				}
			}
		}
	}

	public void Fill(float x, float y)
	{
		Fill((int) x, (int) y);
	}

	public void Fill(Vector2 point)
	{
		Fill((int) point.x, (int) point.y);
	}

	public void Fill(float x, float y, int flagStep)
	{
		_flagStep = (sbyte) flagStep;
		Fill((int) x, (int) y);
	}

	public void Fill(Vector2 point, int flagStep)
	{
		_flagStep = (sbyte) flagStep;
		Fill((int) point.x, (int) point.y);
	}

	public void BeginSubBuffer()
	{
		_boundTopLeft = new Vector2(+10000f, +10000f);
		_boundBottomRight = new Vector2(-10000f, -10000f);

		_subW = _width;
		_subH = _height;
		_inZoneL = 0;
		_inZoneT = 0;
		_translateX = 0;
		_translateY = 0;

		_flagStep = 0;
		for (int i = 0; i < _subW; i++)
			for (int j = 0; j < _subH; j++)
				_flag[i, j] = 0;
		_flagStep = 1;
	}

	private void PreEndSubBuffer()
	{
		_translateX = 0;
		_translateY = 0;

		if (_boundTopLeft.x < 0f)
			_boundTopLeft.x = 0f;
		if (_boundTopLeft.y < 0f)
			_boundTopLeft.y = 0f;
		if (_boundBottomRight.x >= _width)
			_boundBottomRight.x = _width - 1f;
		if (_boundBottomRight.y >= _height)
			_boundBottomRight.y = _height - 1f;

		_subW = (int) Math.Abs((int) _boundTopLeft.x - (int) _boundBottomRight.x) + 1 + (2*1);
		_subH = (int) Math.Abs((int) _boundTopLeft.y - (int) _boundBottomRight.y) + 1 + (2*1);

		_inZoneL = (int) _boundTopLeft.x - 1;
		_inZoneT = (int) _boundTopLeft.y - 1;

		_inZoneL = (_inZoneL < 0) ? 0 : _inZoneL;
		_inZoneT = (_inZoneT < 0) ? 0 : _inZoneT;

		_inZoneL = (_inZoneL >= _width) ? (_width - 1) : _inZoneL;
		_inZoneT = (_inZoneT >= _height) ? (_height - 1) : _inZoneT;

		_subW = (_subW + _inZoneL >= _width) ? (_width - _inZoneL - 1) : _subW;
		_subH = (_subH + _inZoneT >= _height) ? (_height - _inZoneT - 1) : _subH;

		//Fill
		Fill(_inZoneL, _inZoneT);
		if ((_inZoneL == 0) && (_inZoneT == 0))
		{
			Fill(_inZoneL + _subW - 1, _inZoneT + _subH - 1);
		}
	}

	private void FillInZone()
	{
		for (int i = _inZoneL; i < _subW + _inZoneL; i++)
			for (int j = _inZoneT; j < _subH + _inZoneT; j++)
				if (_flag[i, j] != -1)
					_graphics.SetPixel(i, j);
	}

	//Fill Solid color, No fill Stroke
	public void EndSubBuffer()
	{
		PreEndSubBuffer();

		Fill(_inZoneL, _inZoneT);
		FillInZone();
	}

	//Fill Solid color, No fill Stroke
	public void EndSubBuffer(Vector2[] points)
	{
		PreEndSubBuffer();

		for (int i = 0; i < points.GetLength(0); i++)
			Fill(points[i].x, points[i].y);
		FillInZone();
	}

	//Fill Solid color, No fill Stroke
	public void EndSubBuffer(Vector2 point)
	{
		PreEndSubBuffer();

		Fill(point.x, point.y);
		FillInZone();
	}

	//Fill Solid color, with fill Stroke
	public void EndSubBuffer(SVGColor? strokePathColor)
	{
		PreEndSubBuffer();

		FillInZone();

		_graphics.SetColor(strokePathColor.Value.color);

		FillInZone();
	}

	//Fill Linear Gradient, no fill Stroke
	public void EndSubBuffer(SVGLinearGradientBrush linearGradientBrush)
	{
		PreEndSubBuffer();
		for (int i = _inZoneL; i < _subW + _inZoneL; i++)
		{
			for (int j = _inZoneT; j < _subH + _inZoneT; j++)
			{
				if (_flag[i, j] == 0)
				{
					Color _color = linearGradientBrush.GetColor(i, j);
					_graphics.SetColor(_color);
					_graphics.SetPixel(i, j);
				}
			}
		}
	}

	//Fill Linear Gradient, with fill Stroke
	public void EndSubBuffer(SVGLinearGradientBrush linearGradientBrush, SVGColor? strokePathColor)
	{
		PreEndSubBuffer();

		for (int i = _inZoneL; i < _subW + _inZoneL; i++)
		{
			for (int j = _inZoneT; j < _subH + _inZoneT; j++)
			{
				if (_flag[i, j] != -1)
				{
					Color _color = linearGradientBrush.GetColor(i, j);
					_graphics.SetColor(_color);
					_graphics.SetPixel(i, j);
				}
			}
		}

		_graphics.SetColor(strokePathColor.Value.color);

		FillInZone();
	}

	//Fill Radial Gradient, no fill Stroke
	public void EndSubBuffer(SVGRadialGradientBrush radialGradientBrush)
	{
		PreEndSubBuffer();

		for (int i = _inZoneL; i < _subW + _inZoneL; i++)
		{
			for (int j = _inZoneT; j < _subH + _inZoneT; j++)
			{
				if (_flag[i, j] == 0)
				{
					Color _color = radialGradientBrush.GetColor(i, j);
					_graphics.SetColor(_color);
					_graphics.SetPixel(i, j);
				}
			}
		}
	}

	//Fill Radial Gradient, with fill Stroke
	public void EndSubBuffer(SVGRadialGradientBrush radialGradientBrush, SVGColor? strokePathColor)
	{
		PreEndSubBuffer();

		for (int i = _inZoneL; i < _subW + _inZoneL; ++i)
		{
			for (int j = _inZoneT; j < _subH + _inZoneT; ++j)
			{
				if (_flag[i, j] != -1)
				{
					Color _color = radialGradientBrush.GetColor(i, j);
					_graphics.SetColor(_color);
					_graphics.SetPixel(i, j);
				}
			}
		}

		_graphics.SetColor(strokePathColor.Value.color);

		FillInZone();
	}

	private void PreRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
	{
		BeginSubBuffer();

		ExpandBounds(p1);
		ExpandBounds(p2);
		ExpandBounds(p3);
		ExpandBounds(p4);

		_basicDraw.MoveTo(p1);
		_basicDraw.LineTo(p2);
		_basicDraw.LineTo(p3);
		_basicDraw.LineTo(p4);
		_basicDraw.LineTo(p1);
	}

	//-----
	public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
	{
		PreRect(p1, p2, p3, p4);
		EndSubBuffer();
	}

	//-----
	public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, SVGColor? strokeColor)
	{
		PreRect(p1, p2, p3, p4);
		EndSubBuffer(strokeColor);
	}

	//-----
	public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, SVGColor fillColor, SVGColor? strokeColor)
	{
		SetColor(fillColor.color);
		PreRect(p1, p2, p3, p4);
		EndSubBuffer(strokeColor);
	}

	//--------------------------------------------------------------------------------
	//Method: RoundedRect
	//--------------------------------------------------------------------------------
	private void PreRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7,
		Vector2 p8, float r1, float r2, float angle)
	{
		float dxy = ((r1 > r2) ? (int) r1 : (int) r2);

		BeginSubBuffer();

		ExpandBounds(p1, dxy, dxy);
		ExpandBounds(p2, dxy, dxy);
		ExpandBounds(p3, dxy, dxy);
		ExpandBounds(p4, dxy, dxy);
		ExpandBounds(p5, dxy, dxy);
		ExpandBounds(p6, dxy, dxy);
		ExpandBounds(p7, dxy, dxy);
		ExpandBounds(p8, dxy, dxy);

		_basicDraw.MoveTo(p1);
		_basicDraw.LineTo(p2);
		_basicDraw.ArcTo(r1, r2, angle, false, true, p3);

		_basicDraw.MoveTo(p3);
		_basicDraw.LineTo(p4);
		_basicDraw.ArcTo(r1, r2, angle, false, true, p5);

		_basicDraw.MoveTo(p5);
		_basicDraw.LineTo(p6);
		_basicDraw.ArcTo(r1, r2, angle, false, true, p7);

		_basicDraw.MoveTo(p7);
		_basicDraw.LineTo(p8);
		_basicDraw.ArcTo(r1, r2, angle, false, true, p1);
	}

	//-----
	public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
		float r1, float r2,
		float angle)
	{
		PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
			angle);
		EndSubBuffer();
	}

	//-----
	public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
		float r1, float r2,
		float angle, SVGColor? strokeColor)
	{
		PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
			angle);
		EndSubBuffer(strokeColor);
	}

	//-----
	public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
		float r1, float r2,
		float angle, SVGColor fillColor, SVGColor? strokeColor)
	{
		SetColor(fillColor.color);
		PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
			angle);
		EndSubBuffer(strokeColor);
	}

	//--------------------------------------------------------------------------------
	//Method: CircleFill
	//--------------------------------------------------------------------------------
	private void PreCircle(Vector2 p, float r)
	{
		BeginSubBuffer();
		ExpandBounds(p, (int) r + 2, (int) r + 2);

		_basicDraw.Circle(p, r);
	}

	//-----
	public void Circle(Vector2 p, float r)
	{
		PreCircle(p, r);
		EndSubBuffer();
	}

	//-----
	public void Circle(Vector2 p, float r, SVGColor? strokeColor)
	{
		PreCircle(p, r);
		EndSubBuffer(strokeColor);
	}

	//-----
	public void Circle(Vector2 p, float r, SVGColor fillColor, SVGColor? strokeColor)
	{
		SetColor(fillColor.color);
		PreCircle(p, r);
		EndSubBuffer();
	}

	//--------------------------------------------------------------------------------
	//Method: Ellipse
	//--------------------------------------------------------------------------------
	private void PreEllipse(Vector2 p, float rx, float ry, float angle)
	{
		int d = (rx > ry) ? (int) rx : (int) ry;
		ExpandBounds(p, d, d);

		_basicDraw.Ellipse(p, (int) rx, (int) ry, angle);
	}

	//-----
	public void Ellipse(Vector2 p, float rx, float ry, float angle)
	{
		PreEllipse(p, rx, ry, angle);
		EndSubBuffer();
	}

	//-----
	public void Ellipse(Vector2 p, float rx, float ry, float angle, SVGColor? strokeColor)
	{
		PreEllipse(p, rx, ry, angle);
		EndSubBuffer(strokeColor);
	}

	//-----
	public void Ellipse(Vector2 p, float rx, float ry, float angle, SVGColor fillColor, SVGColor? strokeColor)
	{
		SetColor(fillColor.color);
		PreEllipse(p, rx, ry, angle);
		EndSubBuffer(strokeColor);
	}

	//--------------------------------------------------------------------------------
	//Method: Polygon.
	//--------------------------------------------------------------------------------
	private void PrePolygon(Vector2[] points)
	{
		if ((points != null) && (points.GetLength(0) > 0))
		{
			BeginSubBuffer();
			ExpandBounds(points);

			_basicDraw.MoveTo(points[0]);
			int _length = points.GetLength(0);
			for (int i = 1; i < _length; i++)
				_basicDraw.LineTo(points[i]);
			_basicDraw.LineTo(points[0]);
		}
	}

	//-----
	public void Polygon(Vector2[] points)
	{
		PrePolygon(points);
		EndSubBuffer();
	}

	//-----
	public void Polygon(Vector2[] points, SVGColor? strokeColor)
	{
		PrePolygon(points);
		EndSubBuffer(strokeColor);
	}

	//-----
	public void Polygon(Vector2[] points, SVGColor fillColor, SVGColor? strokeColor)
	{
		SetColor(fillColor.color);
		PrePolygon(points);
		EndSubBuffer(strokeColor);
	}

	//--------------------------------------------------------------------------------
	//Method: Fill Path
	//--------------------------------------------------------------------------------
	public void FillPath(SVGGraphicsPath graphicsPath)
	{
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer();
	}

	//-----
	public void FillPath(SVGGraphicsPath graphicsPath, Vector2[] points)
	{
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer(points);
	}

	//-----
	public void FillPath(SVGGraphicsPath graphicsPath, Vector2 point)
	{
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer(point);
	}

	//-----
	//Path Solid Fill
	public void FillPath(SVGColor fillColor, SVGGraphicsPath graphicsPath)
	{
		SetColor(fillColor.color);
		FillPath(graphicsPath);
	}

	//-----
	public void FillPath(SVGColor fillColor, SVGColor? strokePathColor, SVGGraphicsPath graphicsPath)
	{
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		if (strokePathColor != null)
		{
			EndSubBuffer(strokePathColor);
		}
		else
		{
			EndSubBuffer();
		}
	}

	//-----
	//Path Linear Fill
	public void FillPath(SVGLinearGradientBrush linearGradientBrush, SVGGraphicsPath graphicsPath)
	{
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer(linearGradientBrush);
	}

	//-----
	public void FillPath(SVGLinearGradientBrush linearGradientBrush, SVGColor? strokePathColor,
		SVGGraphicsPath graphicsPath)
	{
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		if (strokePathColor != null)
		{
			EndSubBuffer(linearGradientBrush, strokePathColor);
		}
		else
		{
			EndSubBuffer(linearGradientBrush);
		}
	}

	//-----
	//Path Radial Fill
	public void FillPath(SVGRadialGradientBrush radialGradientBrush, SVGGraphicsPath graphicsPath)
	{
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer(radialGradientBrush);
	}

	//-----
	public void FillPath(SVGRadialGradientBrush radialGradientBrush, SVGColor? strokePathColor,
		SVGGraphicsPath graphicsPath)
	{
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		if (strokePathColor != null)
		{
			EndSubBuffer(radialGradientBrush, strokePathColor);
		}
		else
		{
			EndSubBuffer(radialGradientBrush);
		}
	}

	//================================================================================
	//--------------------------------------------------------------------------------
	//Method: CircleTo
	//--------------------------------------------------------------------------------
	public void CircleTo(Vector2 p, float r)
	{
		ExpandBounds(p, (int) r + 1, (int) r + 1);

		//---------------
		_basicDraw.Circle(p, r);
	}

	//--------------------------------------------------------------------------------
	//Method: EllipseTo
	//--------------------------------------------------------------------------------
	public void EllipseTo(Vector2 p, float rx, float ry, float angle)
	{
		int d = (rx > ry) ? (int) rx + 2 : (int) ry + 2;
		ExpandBounds(p, d, d);

		//---------------
		_basicDraw.Ellipse(p, (int) rx, (int) ry, angle);
	}

	//--------------------------------------------------------------------------------
	//Method: LineTo4Path
	//--------------------------------------------------------------------------------
	public void LineTo(Vector2 p)
	{
		ExpandBounds(p);
		//---------------
		_basicDraw.LineTo(p);
	}

	//--------------------------------------------------------------------------------
	//Method: MoveTo
	//--------------------------------------------------------------------------------
	public void MoveTo(Vector2 p)
	{
		ExpandBounds(p);
		//---------------
		_basicDraw.MoveTo(p);
	}

	/*-------------------------------------------------------------------------------
  //Method: Arc4Path
  /-------------------------------------------------------------------------------*/

	public void ArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, Vector2 p)
	{
		ExpandBounds(p, (r1 > r2) ? 2*(int) r1 + 2 : 2*(int) r2 + 2, (r1 > r2) ? 2*(int) r1 + 2 : 2*(int) r2 + 2);
		//---------------
		_basicDraw.ArcTo(r1, r2, angle, largeArcFlag, sweepFlag, p);
	}

	/*-------------------------------------------------------------------------------
  //Method: CubicCurveTo4Path
  /-------------------------------------------------------------------------------*/

	public void CubicCurveTo(Vector2 p1, Vector2 p2, Vector2 p)
	{
		/*Vector2[] points = new Vector2[3];
	points[0] = p1;
	points[1] = p2;
	points[2] = p;
	ExpandBounds(points);*/
		ExpandBounds(p1);
		ExpandBounds(p2);
		ExpandBounds(p);
		//---------------
		_basicDraw.CubicCurveTo(p1, p2, p);
	}

	/*-------------------------------------------------------------------------------
  //Method: QuadraticCurveTo4Path
  /-------------------------------------------------------------------------------*/

	public void QuadraticCurveTo(Vector2 p1, Vector2 p)
	{
		/*Vector2[] points = new Vector2[2];
	points[0] = p1;
	points[1] = p;
	ExpandBounds(points);*/
		ExpandBounds(p1);
		ExpandBounds(p);
		//---------------
		_basicDraw.QuadraticCurveTo(p1, p);
	}
}