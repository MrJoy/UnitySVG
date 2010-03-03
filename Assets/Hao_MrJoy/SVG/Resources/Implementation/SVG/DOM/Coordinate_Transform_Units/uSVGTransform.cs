using System;
using System.Collections.Generic;


public enum uSVGTransformType : ushort {        
	SVG_TRANSFORM_UNKNOWN 	= 0,
	SVG_TRANSFORM_MATRIX 	= 1,
	SVG_TRANSFORM_TRANSLATE = 2,
	SVG_TRANSFORM_SCALE 	= 3,
	SVG_TRANSFORM_ROTATE 	= 4,
	SVG_TRANSFORM_SKEWX 	= 5,
	SVG_TRANSFORM_SKEWY 	= 6
}

public class uSVGTransform {

	private uSVGTransformType m_type;
	private uSVGMatrix m_matrix;
	private double m_angle;

	//***********************************************************************************
	public uSVGMatrix matrix {
		get{return this.m_matrix;}
	}
	public float angle {
		get{
			switch(this.m_type) {
				case uSVGTransformType.SVG_TRANSFORM_ROTATE:
				case uSVGTransformType.SVG_TRANSFORM_SKEWX:
				case uSVGTransformType.SVG_TRANSFORM_SKEWY: {
					return (float)this.m_angle;
				}
				default: return 0.0f;
			}
		}
	}
	public uSVGTransformType type {
		get { return this.m_type; }
	}
	//***********************************************************************************
	public uSVGTransform() {
		this.m_matrix = new uSVGMatrix();
		this.m_type = uSVGTransformType.SVG_TRANSFORM_MATRIX;
	}

	public uSVGTransform(uSVGMatrix matrix) {
		this.m_type = uSVGTransformType.SVG_TRANSFORM_MATRIX;
		this.m_matrix = matrix;
	}
	
	//Chuyen doi 1 day gia tri "a b c d e f" => arr[] = string{a, b, c, d, e, f};
	public uSVGTransform(string strKey, string strValue) {
		List<string> valuesStr = uSVGStringExtractor.f_ExtractTransformValue(strValue);
		int len = valuesStr.Count;
		float[] values = new float[len];

		for (int i = 0; i<len; i++) {
			values.SetValue(uSVGNumber.ParseToFloat(valuesStr[i]), i);
		}
		switch (strKey) {
			case "translate":
				switch (len) {
					case 1:
						SetTranslate(values[0], 0);
						break;
					case 2:
						SetTranslate(values[0], values[1]);
						break;
					default:
						throw new ApplicationException("Wrong number of arguments in translate transform");
				}
			break;
			case "rotate":
				switch (len) {
					case 1:
						SetRotate(values[0]);
						break;
					case 3:
						SetRotate(values[0], values[1], values[2]);
						break;
					default:
						throw new ApplicationException("Wrong number of arguments in rotate transform");
				}
			break;
			case "scale":
				switch (len) {
					case 1:
						SetScale(values[0], values[0]);
					break;
					case 2:
						SetScale(values[0], values[1]);
					break;
					default:
						throw new ApplicationException("Wrong number of arguments in scale transform");
				}
			break;
			case "skewX":
				if (len != 1)
					throw new ApplicationException("Wrong number of arguments in skewX transform");
					SetSkewX(values[0]);
			break;
			case "skewY":
				if (len != 1)
					throw new ApplicationException("Wrong number of arguments in skewY transform");
				SetSkewY(values[0]);
			break;
			case "matrix":
				if(len != 6)
					throw new ApplicationException("Wrong number of arguments in matrix transform");
				SetMatrix(
					new uSVGMatrix(
						values[0],
						values[1],
						values[2],
						values[3],
						values[4],
						values[5]
						));
			break;
			default:
				this.m_type = uSVGTransformType.SVG_TRANSFORM_UNKNOWN;
			break;
		}
	}
	//***********************************************************************************
	public void SetMatrix(uSVGMatrix matrix) {
		this.m_type = uSVGTransformType.SVG_TRANSFORM_MATRIX;
		this.m_matrix = matrix;
	}

	public void SetTranslate(float tx, float ty) {
		this.m_type = uSVGTransformType.SVG_TRANSFORM_TRANSLATE;
		this.m_matrix = new uSVGMatrix().Translate(tx, ty);
	}

	public void SetScale(float sx, float sy) {
		this.m_type = uSVGTransformType.SVG_TRANSFORM_SCALE;
		this.m_matrix = new uSVGMatrix().ScaleNonUniform(sx, sy);
	}

	public void SetRotate(float angle) {
		this.m_type = uSVGTransformType.SVG_TRANSFORM_ROTATE;
		this.m_angle = angle;
		this.m_matrix = new uSVGMatrix().Rotate(angle);
	}

	public void SetRotate(float angle, float cx, float cy) {
		this.m_type = uSVGTransformType.SVG_TRANSFORM_ROTATE;
		this.m_angle = angle;
		this.m_matrix = new uSVGMatrix().Translate(cx, cy).Rotate(angle).Translate(-cx,-cy);
	}

	public void SetSkewX(float angle) {
		this.m_type = uSVGTransformType.SVG_TRANSFORM_SKEWX;
		this.m_angle = angle;
		this.m_matrix = new uSVGMatrix().SkewX(angle);
	}

	public void SetSkewY(float angle) {
		this.m_type = uSVGTransformType.SVG_TRANSFORM_SKEWY;
		this.m_angle = angle;
		this.m_matrix = new uSVGMatrix().SkewY(angle);
	}
}