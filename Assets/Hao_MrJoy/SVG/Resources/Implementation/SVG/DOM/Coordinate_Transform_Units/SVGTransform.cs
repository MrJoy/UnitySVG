using System;
using System.Collections.Generic;


public enum SVGTransformMode : ushort {
  Unknown   = 0,
  Matrix   = 1,
  Translate = 2,
  Scale   = 3,
  Rotate   = 4,
  SkewX   = 5,
  SkewY   = 6
}

public class SVGTransform {

  private SVGTransformMode _type;
  private SVGMatrix _matrix;
  private double _angle;

  //***********************************************************************************
  public SVGMatrix matrix {
    get{return this._matrix;}
  }
  public float angle {
    get{
      switch(this._type) {
        case SVGTransformMode.Rotate:
        case SVGTransformMode.SkewX:
        case SVGTransformMode.SkewY: {
          return(float)this._angle;
        }
        default: return 0.0f;
      }
    }
  }
  public SVGTransformMode type {
    get { return this._type; }
  }
  //***********************************************************************************
  public SVGTransform() {
    this._matrix = new SVGMatrix();
    this._type = SVGTransformMode.Matrix;
  }

  public SVGTransform(SVGMatrix matrix) {
    this._type = SVGTransformMode.Matrix;
    this._matrix = matrix;
  }

  //Chuyen doi 1 day gia tri "a b c d e f" => arr[] = string{a, b, c, d, e, f};
  public SVGTransform(string strKey, string strValue) {
    string[] valuesStr = SVGStringExtractor.ExtractTransformValue(strValue);
    int len = valuesStr.Length;
    float[] values = new float[len];

    for(int i = 0; i<len; i++) {
      values.SetValue(SVGNumber.ParseToFloat(valuesStr[i]), i);
    }
    switch(strKey) {
      case "translate":
        switch(len) {
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
        switch(len) {
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
        switch(len) {
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
        if(len != 1)
          throw new ApplicationException("Wrong number of arguments in skewX transform");
          SetSkewX(values[0]);
      break;
      case "skewY":
        if(len != 1)
          throw new ApplicationException("Wrong number of arguments in skewY transform");
        SetSkewY(values[0]);
      break;
      case "matrix":
        if(len != 6)
          throw new ApplicationException("Wrong number of arguments in matrix transform");
        SetMatrix(
          new SVGMatrix(
            values[0],
            values[1],
            values[2],
            values[3],
            values[4],
            values[5]
            ));
      break;
      default:
        this._type = SVGTransformMode.Unknown;
      break;
    }
  }
  //***********************************************************************************
  public void SetMatrix(SVGMatrix matrix) {
    this._type = SVGTransformMode.Matrix;
    this._matrix = matrix;
  }

  public void SetTranslate(float tx, float ty) {
    this._type = SVGTransformMode.Translate;
    this._matrix = new SVGMatrix().Translate(tx, ty);
  }

  public void SetScale(float sx, float sy) {
    this._type = SVGTransformMode.Scale;
    this._matrix = new SVGMatrix().ScaleNonUniform(sx, sy);
  }

  public void SetRotate(float angle) {
    this._type = SVGTransformMode.Rotate;
    this._angle = angle;
    this._matrix = new SVGMatrix().Rotate(angle);
  }

  public void SetRotate(float angle, float cx, float cy) {
    this._type = SVGTransformMode.Rotate;
    this._angle = angle;
    this._matrix = new SVGMatrix().Translate(cx, cy).Rotate(angle).Translate(-cx,-cy);
  }

  public void SetSkewX(float angle) {
    this._type = SVGTransformMode.SkewX;
    this._angle = angle;
    this._matrix = new SVGMatrix().SkewX(angle);
  }

  public void SetSkewY(float angle) {
    this._type = SVGTransformMode.SkewY;
    this._angle = angle;
    this._matrix = new SVGMatrix().SkewY(angle);
  }
}
