using UnityEngine;

// TODO: Would it be beneficial to move to Unity's Matrix classes?
public class Matrix2x3 {
  public float a, b, c, d, e, f;

  public Matrix2x3() : this(1, 0, 0, 1, 0, 0) {
  }

  public Matrix2x3(Matrix2x3 m) : this(m.a, m.b, m.c, m.d, m.e, m.f) {
  }

  public Matrix2x3(float a, float b, float c, float d, float e, float f) {
    SetValues(a, b, c, d, e, f);
  }

  public void SetValues(float a, float b, float c, float d, float e, float f) {
    this.a = a;
    this.b = b;
    this.c = c;
    this.d = d;
    this.e = e;
    this.f = f;
  }

  public void Multiply(Matrix2x3 secondMatrix) {
    float sa = secondMatrix.a, sb = secondMatrix.b, sc = secondMatrix.c, sd = secondMatrix.d, se = secondMatrix.e,
          sf = secondMatrix.f;
    SetValues(a * sa + c * sb, b * sa + d * sb,
              a * sc + c * sd, b * sc + d * sd,
              a * se + c * sf + e, b * se + d * sf + f);
  }

  public Matrix2x3 Inverse() {
    float det = a * d - c * b;
    if(det == 0)
      throw new SVGException(SVGExceptionType.MatrixNotInvertable);
    SetValues(d / det, -b / det,
              -c / det, a / det,
              (c * f - e * d) / det, (e * b - a * f) / det);
    return this;
  }

  public Matrix2x3 Scale(float scaleFactor) {
    SetValues(a * scaleFactor, b * scaleFactor,
              c * scaleFactor, d * scaleFactor,
              e, f);
    return this;
  }

  public Matrix2x3 ScaleNonUniform(float scaleFactorX, float scaleFactorY) {
    SetValues(a * scaleFactorX, b * scaleFactorX,
              c * scaleFactorY, d * scaleFactorY,
              e, f);
    return this;
  }

  public Matrix2x3 Rotate(float angle) {
    float ca = Mathf.Cos(angle * Mathf.Deg2Rad);
    float sa = Mathf.Sin(angle * Mathf.Deg2Rad);

    SetValues(a * ca + c * sa, b * ca + d * sa,
              c * ca - a * sa, d * ca - b * sa,
              e, f);
    return this;
  }

  public Matrix2x3 Translate(float x, float y) {
    SetValues(a, b, c, d, a * x + c * y + e, b * x + d * y + f);
    return this;
  }

  public Matrix2x3 SkewX(float angle) {
    float ta = Mathf.Tan(angle * Mathf.Deg2Rad);
    SetValues(a, b,
              c + a * ta, d + b * ta,
              e, f);
    return this;
  }

  public Matrix2x3 SkewY(float angle) {
    float ta = Mathf.Tan(angle * Mathf.Deg2Rad);
    SetValues(a + c * ta, b + d * ta,
              c, d,
              e, f);
    return this;
  }

  public Vector2 Transform(Vector2 point) {
    return new Vector2(a * point.x + c * point.y + e, b * point.x + d * point.y + f);
  }
}
