public abstract class uSVGPathSegCurvetoQuadratic : uSVGPathSeg{

	/***********************************************************************************/
	public uSVGPathSegCurvetoQuadratic(uSVGPathSegTypes type) : base(type) {
	}
	public uSVGPathSegCurvetoQuadratic(ushort type) : base(type) {
	}
	/***********************************************************************************/
	public abstract override uSVGPoint currentPoint{get;} 
	public abstract uSVGPoint controlPoint1{get;}
}
