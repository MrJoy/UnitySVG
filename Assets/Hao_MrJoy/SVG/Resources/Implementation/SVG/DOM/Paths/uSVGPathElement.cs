using System.Collections.Generic;

public class uSVGPathElement : uSVGTransformable, uISVGDrawable {
	private uSVGPathSegList m_segList;
	/***********************************************************************************/
	private uISVGGraphics m_render;
	private AttributeList m_attrList;
	private uSVGPaintable m_paintable;
	/***********************************************************************************/
	public uSVGPathElement(AttributeList attrList,
							uSVGAnimatedTransformList inheritTransformList,
							uSVGPaintable inheritPaintable,
							uISVGGraphics m_render) : base (inheritTransformList) {
		this.m_attrList = attrList;
		this.m_paintable = new uSVGPaintable(inheritPaintable, attrList);
		this.m_render = m_render;
		f_Initial();
	}
	/***********************************************************************************/
	private void f_Initial() {
		this.currentTransformList = new uSVGAnimatedTransformList(
											this.m_attrList.GetValue("TRANSFORM", true));
		m_segList = new uSVGPathSegList();
		
		//-----------
		string m_d = this.m_attrList.GetValue("d");
		List<string> m_charList = new List<string>();
		List<string> m_valueList = new List<string>();

		uSVGStringExtractor.f_ExtractPathSegList(m_d, ref m_charList, ref m_valueList);
		
		string m_char, m_value;
		List<string> m_valuesOfChar = new List<string>();
		for(int i = 0; i < m_charList.Count; i++) {
			m_char = m_charList[i];
			m_value = m_valueList[i];
			m_valuesOfChar = uSVGStringExtractor.f_ExtractTransformValue(m_value);
			switch (m_char) {
				case "Z" :
				case "z" : {
					uSVGPathSegClosePath temp = CreateSVGPathSegClosePath();
					m_segList.AppendItem(temp);
				break;
				}
				case "M" : {
					uSVGPathSegMovetoAbs temp = CreateSVGPathSegMovetoAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]));
					m_segList.AppendItem(temp);
				break;
				}
				case "m" : {
					uSVGPathSegMovetoRel temp = CreateSVGPathSegMovetoRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]));
					m_segList.AppendItem(temp);
				break;
				}
				case "L" : {
					uSVGPathSegLinetoAbs temp = CreateSVGPathSegLinetoAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]));
					m_segList.AppendItem(temp);
					break;
				}
				case "l" : {
					uSVGPathSegLinetoRel temp = CreateSVGPathSegLinetoRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]));
					m_segList.AppendItem(temp);
					break;
				}
				case "C" : {
					uSVGPathSegCurvetoCubicAbs temp = CreateSVGPathSegCurvetoCubicAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]),
													uSVGLength.GetPXLength(m_valuesOfChar[2]),
													uSVGLength.GetPXLength(m_valuesOfChar[3]),
													uSVGLength.GetPXLength(m_valuesOfChar[4]),
													uSVGLength.GetPXLength(m_valuesOfChar[5]));
					m_segList.AppendItem(temp);
					break;
				}
				case "c" : {
					uSVGPathSegCurvetoCubicRel temp = CreateSVGPathSegCurvetoCubicRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]),
													uSVGLength.GetPXLength(m_valuesOfChar[2]),
													uSVGLength.GetPXLength(m_valuesOfChar[3]),
													uSVGLength.GetPXLength(m_valuesOfChar[4]),
													uSVGLength.GetPXLength(m_valuesOfChar[5]));
					m_segList.AppendItem(temp);
					break;
				}
				case "S" : {
					uSVGPathSegCurvetoCubicSmoothAbs temp =
												CreateSVGPathSegCurvetoCubicSmoothAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]),
													uSVGLength.GetPXLength(m_valuesOfChar[2]),
													uSVGLength.GetPXLength(m_valuesOfChar[3]));
					m_segList.AppendItem(temp);
					break;
				}
				case "s" : {
					uSVGPathSegCurvetoCubicSmoothRel temp =
												CreateSVGPathSegCurvetoCubicSmoothRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]),
													uSVGLength.GetPXLength(m_valuesOfChar[2]),
													uSVGLength.GetPXLength(m_valuesOfChar[3]));
					m_segList.AppendItem(temp);
					break;
				}
				case "Q" : {
					uSVGPathSegCurvetoQuadraticAbs temp = CreateSVGPathSegCurvetoQuadraticAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]),
													uSVGLength.GetPXLength(m_valuesOfChar[2]),
													uSVGLength.GetPXLength(m_valuesOfChar[3]));
					m_segList.AppendItem(temp);
					break;
				}
				case "q" : {
					uSVGPathSegCurvetoQuadraticRel temp = CreateSVGPathSegCurvetoQuadraticRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]),
													uSVGLength.GetPXLength(m_valuesOfChar[2]),
													uSVGLength.GetPXLength(m_valuesOfChar[3]));
					m_segList.AppendItem(temp);
					break;
				}
				case "T" : {
					uSVGPathSegCurvetoQuadraticSmoothAbs temp =
													CreateSVGPathSegCurvetoQuadraticSmoothAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]));
					m_segList.AppendItem(temp);
					break;
				}
				case "t" : {
					uSVGPathSegCurvetoQuadraticSmoothRel temp =
												CreateSVGPathSegCurvetoQuadraticSmoothRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]));
					m_segList.AppendItem(temp);
					break;
				}
				case "A" : {
					uSVGPathSegArcAbs temp = CreateSVGPathSegArcAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]),
													uSVGLength.GetPXLength(m_valuesOfChar[2]),
													uSVGLength.GetPXLength(m_valuesOfChar[3]),
													uSVGLength.GetPXLength(m_valuesOfChar[4]),
													uSVGLength.GetPXLength(m_valuesOfChar[5]),
													uSVGLength.GetPXLength(m_valuesOfChar[6]));
					m_segList.AppendItem(temp);
					break;
				}
				case "a" : {
					uSVGPathSegArcRel temp = CreateSVGPathSegArcRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]),
													uSVGLength.GetPXLength(m_valuesOfChar[1]),
													uSVGLength.GetPXLength(m_valuesOfChar[2]),
													uSVGLength.GetPXLength(m_valuesOfChar[3]),
													uSVGLength.GetPXLength(m_valuesOfChar[4]),
													uSVGLength.GetPXLength(m_valuesOfChar[5]),
													uSVGLength.GetPXLength(m_valuesOfChar[6]));
					m_segList.AppendItem(temp);
					break;
				}
				case "H" : {
					uSVGPathSegLinetoHorizontalAbs temp = CreateSVGPathSegLinetoHorizontalAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]));
					m_segList.AppendItem(temp);
					break;
				}
				case "h" : {
					uSVGPathSegLinetoHorizontalRel temp = CreateSVGPathSegLinetoHorizontalRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]));
					m_segList.AppendItem(temp);
					break;
				}
				case "V" : {
					uSVGPathSegLinetoVerticalAbs temp = CreateSVGPathSegLinetoVerticalAbs(
													uSVGLength.GetPXLength(m_valuesOfChar[0]));
					m_segList.AppendItem(temp);
					break;
				}
				case "v" : {
					uSVGPathSegLinetoVerticalRel temp = CreateSVGPathSegLinetoVerticalRel(
													uSVGLength.GetPXLength(m_valuesOfChar[0]));
					m_segList.AppendItem(temp);
					break;
				}
			}	
		}
	}
	/***********************************************************************************/
	//Create Methods
	public uSVGPathSegClosePath CreateSVGPathSegClosePath()	{
		uSVGPathSegMovetoAbs m_firstPoint = m_segList.GetItem(0) as uSVGPathSegMovetoAbs;
		if (m_firstPoint == null) {
			uSVGPathSegMovetoRel m_firstPoint1 = m_segList.GetItem(0) as uSVGPathSegMovetoRel;
			if (m_firstPoint1 != null) {
				return new uSVGPathSegClosePath(m_firstPoint1.x, m_firstPoint1.y);
			}
		} else {
			return new uSVGPathSegClosePath(m_firstPoint.x, m_firstPoint.y);
		}
		
		return new uSVGPathSegClosePath(-1f, -1f);
	}
	//--------------
	//MoveToAbs
	public uSVGPathSegMovetoAbs CreateSVGPathSegMovetoAbs(float x, float y)	{
		return new uSVGPathSegMovetoAbs(x, y);
	}
	//--------------							
	//MoveToRel
	public uSVGPathSegMovetoRel CreateSVGPathSegMovetoRel(float x, float y) {
		return new uSVGPathSegMovetoRel(x, y);
	}
	
	//--------------
	//LineToAbs
	public uSVGPathSegLinetoAbs CreateSVGPathSegLinetoAbs(float x, float y) {
		return new uSVGPathSegLinetoAbs(x, y);
	}
	//--------------
	//LineToRel
	public uSVGPathSegLinetoRel CreateSVGPathSegLinetoRel(float x, float y) {
		return new uSVGPathSegLinetoRel(x, y);
	}
	//--------------
	//CubicCurveAbs
	public uSVGPathSegCurvetoCubicAbs CreateSVGPathSegCurvetoCubicAbs(float x1, 
									float y1, float x2, float y2, float x, float y) {
		return new uSVGPathSegCurvetoCubicAbs(x1, y1, x2, y2, x, y);
	}
	//--------------
	//CubicCurveRel
	public uSVGPathSegCurvetoCubicRel CreateSVGPathSegCurvetoCubicRel(float x1, 
									float y1, float x2, float y2, float x, float y) {
		return new uSVGPathSegCurvetoCubicRel(x1, y1, x2, y2, x, y);
	}
	//--------------
	//SmoothCubicCurveAbs (S)
	public uSVGPathSegCurvetoCubicSmoothAbs CreateSVGPathSegCurvetoCubicSmoothAbs(float x2,
																float y2, float x, float y) {
		return new uSVGPathSegCurvetoCubicSmoothAbs(x2, y2, x, y);
	}
	//--------------
	//SmoothCubicCurveRel (s)
	public uSVGPathSegCurvetoCubicSmoothRel CreateSVGPathSegCurvetoCubicSmoothRel(float x2,
																float y2, float x, float y) {
		return new uSVGPathSegCurvetoCubicSmoothRel(x2, y2, x, y);
	}
	//--------------
	//QuadraticCurveAbs (Q)
	public uSVGPathSegCurvetoQuadraticAbs CreateSVGPathSegCurvetoQuadraticAbs(float x1, 
									float y1, float x, float y) {
		return new uSVGPathSegCurvetoQuadraticAbs(x1, y1, x, y);
	}
	//--------------
	//QuadraticCurveAbs (q)
	public uSVGPathSegCurvetoQuadraticRel CreateSVGPathSegCurvetoQuadraticRel(float x1, 
									float y1, float x, float y) {
		return new uSVGPathSegCurvetoQuadraticRel(x1, y1, x, y);
	}
	//--------------
	//SmoothQuadraticCurveAbs (T)
	public uSVGPathSegCurvetoQuadraticSmoothAbs CreateSVGPathSegCurvetoQuadraticSmoothAbs(float x,
																float y) {
		return new uSVGPathSegCurvetoQuadraticSmoothAbs(x, y);
	}
	//--------------
	//SmoothQuadraticCurveRel (t)
	public uSVGPathSegCurvetoQuadraticSmoothRel CreateSVGPathSegCurvetoQuadraticSmoothRel(float x,
																float y) {
		return new uSVGPathSegCurvetoQuadraticSmoothRel(x, y);
	}
	//--------------
	//ArcAbs (A)
	public uSVGPathSegArcAbs CreateSVGPathSegArcAbs(float r1, float r2, float angle,
													float largeArcFlag, float sweepFlag,
													float x, float y) {
		return new uSVGPathSegArcAbs(r1, r2, angle, largeArcFlag == 1.0f, sweepFlag == 1.0f, x, y);
	}
	//--------------
	//ArcRel (a)
	public uSVGPathSegArcRel CreateSVGPathSegArcRel(float r1, float r2, float angle,
													float largeArcFlag, float sweepFlag,
													float x, float y) {
		return new uSVGPathSegArcRel(r1, r2, angle, largeArcFlag == 1.0f, sweepFlag == 1.0f, x, y);
	}
	//--------------
	//LinetoHorizontalAbs (H)
	public uSVGPathSegLinetoHorizontalAbs CreateSVGPathSegLinetoHorizontalAbs(float x) {
		return new uSVGPathSegLinetoHorizontalAbs(x);
	}
	//--------------
	//LinetoHorizontalRel (h)
	public uSVGPathSegLinetoHorizontalRel CreateSVGPathSegLinetoHorizontalRel(float x) {
		return new uSVGPathSegLinetoHorizontalRel(x);
	}
	//--------------
	//LinetVerticalAbs (V)
	public uSVGPathSegLinetoVerticalAbs CreateSVGPathSegLinetoVerticalAbs(float y) {
		return new uSVGPathSegLinetoVerticalAbs(y);
	}
	//--------------
	//LinetoVerticalRel (v)
	public uSVGPathSegLinetoVerticalRel CreateSVGPathSegLinetoVerticalRel(float y) {
		return new uSVGPathSegLinetoVerticalRel(y);
	}
	//================================================================================
	private uSVGGraphicsPath m_graphicsPath;
	private void f_CreateGraphicsPath() {
		this.m_graphicsPath = new uSVGGraphicsPath();
		for (int i = 0; i < this.m_segList.numberOfItems; i++) {
			uISVGDrawableSeg temp = this.m_segList.GetItem(i) as uISVGDrawableSeg;
			if (temp != null) {
				temp.f_Render(this.m_graphicsPath);
			}
		}
		this.m_graphicsPath.transformList = this.summaryTransformList;
	}
	//-----
	private void f_Draw() {
		if (this.m_paintable.strokeColor == null) return;
		
		this.m_render.DrawPath(this.m_graphicsPath, this.m_paintable.strokeWidth,
														this.m_paintable.strokeColor);
	}
	//================================================================================
	//Thuc thi Interface Drawable
	public void f_BeforeRender (uSVGAnimatedTransformList transformList) {
		this.inheritTransformList = transformList;
		for (int i = 0; i < this.m_segList.numberOfItems; i++) {
			uISVGDrawable temp = this.m_segList.GetItem(i) as uISVGDrawable;
			if (temp != null) {
				temp.f_BeforeRender(this.summaryTransformList);
			}
		}
	}
	
	//------
	public void f_Render () {
		f_CreateGraphicsPath();
		this.m_render.SetStrokeLineCap(this.m_paintable.strokeLineCap);
		this.m_render.SetStrokeLineJoin(this.m_paintable.strokeLineJoin);
		switch(this.m_paintable.GetPaintType()) {
			case uSVGPaintTypes.SVG_PAINT_SOLID_GRADIENT_FILL : {
				this.m_render.FillPath(this.m_paintable.fillColor, this.m_graphicsPath);
				f_Draw();
				break;
			}
			case uSVGPaintTypes.SVG_PAINT_LINEAR_GRADIENT_FILL : {

				uSVGLinearGradientBrush m_linearGradBrush = 
									this.m_paintable.GetLinearGradientBrush(this.m_graphicsPath);

				if (m_linearGradBrush != null) {
					this.m_render.FillPath(m_linearGradBrush, m_graphicsPath);
				}
				f_Draw();
				break;
			}
			case uSVGPaintTypes.SVG_PAINT_RADIAL_GRADIENT_FILL : {
				uSVGRadialGradientBrush m_radialGradBrush = 
									this.m_paintable.GetRadialGradientBrush(this.m_graphicsPath);

				if (m_radialGradBrush != null) {
					this.m_render.FillPath(m_radialGradBrush, m_graphicsPath);
				}
				f_Draw();
				break;
			}
			case uSVGPaintTypes.SVG_PAINT_PATH_DRAW : {
				f_Draw();
				break;
			}
		}
	}
}