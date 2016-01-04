using System;
using System.Globalization;

public enum SVGTransformMode : ushort {
  Unknown = 0,
  Matrix = 1,
  Translate = 2,
  Scale = 3,
  Rotate = 4,
  SkewX = 5,
  SkewY = 6
}

public class SVGTransform {
  private SVGTransformMode _type;
  private float _angle;

  public Matrix2x3 matrix { get; private set; }

  public float angle {
    get {
      switch(_type) {
      case SVGTransformMode.Rotate:
      case SVGTransformMode.SkewX:
      case SVGTransformMode.SkewY:
        return _angle;
      default:
        return 0.0f;
      }
    }
  }

  public SVGTransformMode type { get { return _type; } }

  public SVGTransform() {
    matrix = new Matrix2x3();
    _type = SVGTransformMode.Matrix;
  }

  public SVGTransform(Matrix2x3 matrix) {
    _type = SVGTransformMode.Matrix;
    this.matrix = matrix;
  }

  public SVGTransform(string strKey, string strValue) {
    string[] valuesStr = SVGStringExtractor.ExtractTransformValue(strValue);
    int len = valuesStr.Length;
    float[] values = new float[len];

    for(int i = 0; i < len; i++)
      values.SetValue(float.Parse(valuesStr[i], CultureInfo.InvariantCulture), i);
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
        new Matrix2x3(
          values[0],
          values[1],
          values[2],
          values[3],
          values[4],
          values[5]
        ));
      break;
    default:
      _type = SVGTransformMode.Unknown;
      break;
    }
  }

  public void SetMatrix(Matrix2x3 m) {
    _type = SVGTransformMode.Matrix;
    matrix = m;
  }

  public void SetTranslate(float tx, float ty) {
    _type = SVGTransformMode.Translate;
    matrix = new Matrix2x3().Translate(tx, ty);
  }

  public void SetScale(float sx, float sy) {
    _type = SVGTransformMode.Scale;
    matrix = new Matrix2x3().ScaleNonUniform(sx, sy);
  }

  public void SetRotate(float rotateAngle) {
    _type = SVGTransformMode.Rotate;
    _angle = rotateAngle;
    matrix = new Matrix2x3().Rotate(rotateAngle);
  }

  public void SetRotate(float rotateAngle, float cx, float cy) {
    _type = SVGTransformMode.Rotate;
    _angle = rotateAngle;
    matrix = new Matrix2x3().Translate(cx, cy).Rotate(angle).Translate(-cx, -cy);
  }

  public void SetSkewX(float skewAngle) {
    _type = SVGTransformMode.SkewX;
    _angle = skewAngle;
    matrix = new Matrix2x3().SkewX(angle);
  }

  public void SetSkewY(float skewAngle) {
    _type = SVGTransformMode.SkewY;
    _angle = skewAngle;
    matrix = new Matrix2x3().SkewY(angle);
  }
}
