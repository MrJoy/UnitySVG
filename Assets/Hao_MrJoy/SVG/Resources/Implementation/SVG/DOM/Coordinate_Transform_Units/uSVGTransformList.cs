using System.Collections.Generic;

public class uSVGTransformList {
	private List<uSVGTransform> m_listTransform;
	/*********************************************************************************************/
	public int Count {
		get{ return m_listTransform.Count; }
	}
	private uSVGMatrix _totalMatrix = null;
	public uSVGMatrix totalMatrix {
		get {
		  if(_totalMatrix == null) {
  			if (m_listTransform.Count == 0) {
  				_totalMatrix = new uSVGMatrix(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);
  			} else {
  				uSVGMatrix matrix = m_listTransform[0].matrix;
  				for (int i = 1; i < m_listTransform.Count; i++) {
  					matrix = matrix.Multiply(m_listTransform[i].matrix);
  				}
  				_totalMatrix = matrix;
  			}
			}
			return _totalMatrix;
		}
	}
	/*********************************************************************************************/
	public uSVGTransformList() {
		m_listTransform = new List<uSVGTransform>();
	}
	public uSVGTransformList(int capacity) {
		m_listTransform = new List<uSVGTransform>(capacity);
	}
	public uSVGTransformList(string listString) {
		m_listTransform = uSVGStringExtractor.f_ExtractTransformList(listString);
	}

	/*********************************************************************************************/
	public void AppendItem(uSVGTransform newItem) {
		m_listTransform.Add(newItem);
	  _totalMatrix = null;
	}
	public void AppendItems(uSVGTransformList newListItem) {
	  m_listTransform.AddRange(newListItem.m_listTransform);
	  _totalMatrix = null;
	}
	public uSVGTransform this[int index] {
	  get {
  		if ((index < 0) || (index >= m_listTransform.Count)) {
  			throw new uDOMException(uDOMExceptionType.IndexSizeErr);
  		}
  		return m_listTransform[index];
		}
	}

	public uSVGTransform Consolidate() {
		return new uSVGTransform(totalMatrix);
	}
}
