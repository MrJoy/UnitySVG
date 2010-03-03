public enum uSVGPathSegTypes : ushort {
	PATHSEG_UNKNOWN 			= 0,
	PATHSEG_CLOSEPATH 			= 1,
	PATHSEG_MOVETO_ABS 			= 2,
	PATHSEG_MOVETO_REL 			= 3,
	PATHSEG_LINETO_ABS 			= 4,
	PATHSEG_LINETO_REL 			= 5,
	PATHSEG_CURVETO_CUBIC_ABS 		= 6,
	PATHSEG_CURVETO_CUBIC_REL 		= 7,
	PATHSEG_CURVETO_QUADRATIC_ABS 		= 8,
	PATHSEG_CURVETO_QUADRATIC_REL 		= 9,
	PATHSEG_ARC_ABS 			= 10,
	PATHSEG_ARC_REL 			= 11,
	PATHSEG_LINETO_HORIZONTAL_ABS 		= 12,
	PATHSEG_LINETO_HORIZONTAL_REL 		= 13,
	PATHSEG_LINETO_VERTICAL_ABS 		= 14,
	PATHSEG_LINETO_VERTICAL_REL 		= 15,
	PATHSEG_CURVETO_CUBIC_SMOOTH_ABS 	= 16,
	PATHSEG_CURVETO_CUBIC_SMOOTH_REL 	= 17,
	PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS 	= 18,
	PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL 	= 19
}
/*************************************************************************************************/
public abstract class uSVGPathSeg {
	private uSVGPathSegTypes m_pathSegType;
	protected uSVGPathSegList m_segList;
	protected int m_segIndex;
	/***********************************************************************************/
	public uSVGPathSegTypes pathSegType {
		get { return this.m_pathSegType; }
	}
	public string pathSegTypeAsLetter {
		get { return f_TypeToLetter(); }
	}
	/***********************************************************************************/
	public uSVGPathSeg(uSVGPathSegTypes type) {
		this.m_pathSegType = type;
	}
	public uSVGPathSeg(ushort type) {
		this.m_pathSegType = (uSVGPathSegTypes)type;
	}
	/***********************************************************************************/
	internal void SetList(uSVGPathSegList segList) {
		this.m_segList = segList;
	}
	internal void SetIndex(int segIndex) {
		this.m_segIndex = segIndex;
	}
	public uSVGPathSeg previousSeg {
		get {
			return m_segList.GetPreviousSegment(this);
		}
	}
	/***********************************************************************************/
	public abstract uSVGPoint currentPoint{get;}
	public uSVGPoint previousPoint {
		get {
			uSVGPoint m_return = new uSVGPoint(0f,0f);
			uSVGPathSeg m_prevSeg = previousSeg;
			if (m_prevSeg != null) {
				m_return.x = m_prevSeg.currentPoint.x;
				m_return.y = m_prevSeg.currentPoint.y;
			}
			return m_return;
		}
	}
	/***********************************************************************************/
	private string f_TypeToLetter() {
		switch(this.m_pathSegType)
		{
		case uSVGPathSegTypes.PATHSEG_UNKNOWN:
			return "";
		case uSVGPathSegTypes.PATHSEG_ARC_ABS:
			return "A";
		case uSVGPathSegTypes.PATHSEG_ARC_REL:
			return "a";
		case uSVGPathSegTypes.PATHSEG_CLOSEPATH:
			return "z";
		case uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_ABS:
			return "C";
		case uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_REL:
			return "c";
		case uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_SMOOTH_ABS:
			return "S";
		case uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_SMOOTH_REL:
			return "s";
		case uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_ABS:
			return "Q";
		case uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_REL:
			return "q";
		case uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS:
			return "T";
		case uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL:
			return "t";
		case uSVGPathSegTypes.PATHSEG_LINETO_ABS:
			return "L";
		case uSVGPathSegTypes.PATHSEG_LINETO_HORIZONTAL_ABS:
			return "H";
		case uSVGPathSegTypes.PATHSEG_LINETO_HORIZONTAL_REL:
			return "h";
		case uSVGPathSegTypes.PATHSEG_LINETO_REL:
			return "l";
		case uSVGPathSegTypes.PATHSEG_LINETO_VERTICAL_ABS:
			return "V";
		case uSVGPathSegTypes.PATHSEG_LINETO_VERTICAL_REL:
			return "v";
		case uSVGPathSegTypes.PATHSEG_MOVETO_ABS:
			return "M";
		case uSVGPathSegTypes.PATHSEG_MOVETO_REL:
			return "m";
		default:
			return "";
		}
	}
}