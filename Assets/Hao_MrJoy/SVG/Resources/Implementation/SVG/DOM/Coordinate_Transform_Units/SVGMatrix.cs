using UnityEngine;
using System;

// TODO: Rename Matrix2x3
public class SVGMatrix {
    protected float _a, _b, _c, _d, _e, _f;
    const double radPerDegree = 2.0 * 3.1415926535 / 360.0;
    public SVGMatrix() : this(1, 0, 0, 1, 0, 0)
    {}
    public SVGMatrix(float a, float b, float c, float d, float e, float f) {
      this._a = a;
      this._b = b;
      this._c = c;
      this._d = d;
      this._e = e;
      this._f = f;
    }
    public float a {
      get {return this._a;}
      set {this._a = value;}
    }
    public float b {
      get {return this._b;}
      set {this._b = value;}
    }
    public float c {
      get {return this._c;}
      set {this._c = value;}
    }
    public float d {
      get {return this._d;}
      set {this._d = value;}
    }
    public float e {
      get {return this._e;}
      set {this._e = value;}
    }
    public float f {
      get {return this._f;}
      set {this._f = value;}
    }

    //---------------------------------------
    public SVGMatrix Multiply(SVGMatrix secondMatrix) {
      float sa, sb, sc, sd, se, sf;
      sa = secondMatrix.a;
      sb = secondMatrix.b;
      sc = secondMatrix.c;
      sd = secondMatrix.d;
      se = secondMatrix.e;
      sf = secondMatrix.f;
      return new SVGMatrix(  a*sa + c*sb,    b*sa + d*sb,
                  a*sc + c*sd,    b*sc + d*sd,
                  a*se + c*sf + e, b*se + d*sf + f);
    }
    public SVGMatrix Inverse() {
      double det = a*d - c*b;
      if(det == 0.0) {
        throw new SVGException(SVGExceptionType.MatrixNotInvertable);
      }
      return new SVGMatrix( (float)(d/det),       (float)(-b/det),
                 (float)(-c/det),       (float)(a/det),
                 (float)((c*f - e*d)/ det), (float)((e*b - a*f)/ det));
    }
    public SVGMatrix Scale(float scaleFactor) {
      return new SVGMatrix(  a * scaleFactor,   b * scaleFactor,
                  c * scaleFactor,   d * scaleFactor,
                  e,          f);
    }
    public SVGMatrix ScaleNonUniform(float scaleFactorX, float scaleFactorY) {
      return new SVGMatrix(  a*scaleFactorX,  b*scaleFactorX,
                  c*scaleFactorY, d*scaleFactorY,
                  e,        f);
    }
    public SVGMatrix Rotate(float angle) {
      double ca = (float)Math.Cos((float)(angle * radPerDegree));
      double sa = (float)Math.Sin((float)(angle * radPerDegree));

      return new SVGMatrix( (float)(a*ca + c*sa), (float)(b*ca + d*sa),
                 (float)(c*ca - a*sa), (float)(d*ca - b*sa),
                  e,            f);
    }
    public SVGMatrix Translate(float x, float y) {
      return new SVGMatrix( a, b, c, d, a*x + c*y + e, b*x + d*y +f);
    }
    public SVGMatrix SkewX(float angle) {
      double ta = Math.Tan((float)(angle*radPerDegree));
      return new SVGMatrix(  a,          b,
                 (float)(c + a*ta), (float)(d + b*ta),
                  e,          f);
    }
    public SVGMatrix SkewY(float angle) {
      double ta = Math.Tan((float)(angle*radPerDegree));
      return new SVGMatrix( (float)(a + c*ta), (float)(b + d*ta),
                  c,          d,
                  e,          f);
    }

  public Vector2 Transform(Vector2 point) {
    return new Vector2(a*point.x + c*point.y + e, b*point.x + d*point.y +f);
  }
}
