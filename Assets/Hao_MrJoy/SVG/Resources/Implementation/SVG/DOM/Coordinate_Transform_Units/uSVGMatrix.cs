using UnityEngine;

// TODO: Rename Matrix2x3
public class uSVGMatrix {
		protected float m_a, m_b, m_c, m_d, m_e, m_f;
		const double radPerDegree = 2.0 * 3.1415926535 / 360.0;
		public uSVGMatrix() : this(1, 0, 0, 1, 0, 0)
		{}
		public uSVGMatrix(float a, float b, float c, float d, float e, float f) {
			this.m_a = a;
			this.m_b = b;
			this.m_c = c;
			this.m_d = d;
			this.m_e = e;
			this.m_f = f;		
		}
		public float a {
			get {return this.m_a;}
			set {this.m_a = value;}
		}
		public float b {
			get {return this.m_b;}
			set {this.m_b = value;}
		}
		public float c {
			get {return this.m_c;}
			set {this.m_c = value;}
		}
		public float d {
			get {return this.m_d;}
			set {this.m_d = value;}
		}
		public float e {
			get {return this.m_e;}
			set {this.m_e = value;}
		}
		public float f {
			get {return this.m_f;}
			set {this.m_f = value;}
		}
	
		//---------------------------------------
		public uSVGMatrix Multiply(uSVGMatrix secondMatrix) {
			float sa, sb, sc, sd, se, sf;
			sa = secondMatrix.a;
			sb = secondMatrix.b;
			sc = secondMatrix.c;
			sd = secondMatrix.d;
			se = secondMatrix.e;
			sf = secondMatrix.f;
			return new uSVGMatrix(	a*sa + c*sb, 	 b*sa + d*sb,
									a*sc + c*sd, 	 b*sc + d*sd,
									a*se + c*sf + e, b*se + d*sf + f);
		}
		public uSVGMatrix Inverse() {
			double det = a*d - c*b;
			if (det == 0.0) {
				throw new uSVGException(uSVGExceptionType.SvgMatrixNotInvertable);
			}
			return new uSVGMatrix(	(float) (d/det),				(float) (-b/det),
									(float) (-c/det),				(float) (a/det),
									(float) ((c*f - e*d) / det),	(float) ((e*b - a*f) / det));
		}
		public uSVGMatrix Scale(float scaleFactor) {
			return new uSVGMatrix(	a * scaleFactor, 	b * scaleFactor,
									c * scaleFactor, 	d * scaleFactor,
									e,					f);
		}
		public uSVGMatrix ScaleNonUniform(float scaleFactorX, float scaleFactorY) {
			return new uSVGMatrix (	a*scaleFactorX,	b*scaleFactorX,
									c*scaleFactorY, d*scaleFactorY,
									e,				f);
		}
		public uSVGMatrix Rotate(float angle) {
			double ca = Mathf.Cos((float)(angle * radPerDegree));
			double sa = Mathf.Sin((float)(angle * radPerDegree));
			
			return new uSVGMatrix(	(float) (a*ca + c*sa),	(float) (b*ca + d*sa),
									(float) (c*ca - a*sa),	(float) (d*ca - b*sa),
									e,						f);
		}
		public uSVGMatrix Translate(float x, float y) {
			return new uSVGMatrix ( a, b, c, d, a*x + c*y + e, b*x + d*y +f);
		}
		public uSVGMatrix SkewX(float angle) {
			double ta = Mathf.Tan((float) (angle*radPerDegree));
			return new uSVGMatrix(	a,					b,
									(float) (c + a*ta),	(float) (d + b*ta),
									e,					f);
		}
		public uSVGMatrix SkewY(float angle) {
			double ta = Mathf.Tan((float) (angle*radPerDegree));
			return new uSVGMatrix(	(float) (a + c*ta),	(float) (b + d*ta),
									c,					d,
									e,					f);
		}
}