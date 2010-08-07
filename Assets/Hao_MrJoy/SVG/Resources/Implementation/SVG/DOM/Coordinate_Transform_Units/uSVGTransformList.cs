using System.Collections.Generic;

public class uSVGTransformList {
	private List<uSVGTransform> m_listTransform;
	/*********************************************************************************************/
	public int Count {
		get{ return m_listTransform.Count; }
	}
	public uSVGMatrix totalMatrix {
		get {
			if (m_listTransform.Count == 0) {
				return new uSVGMatrix(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);
			} else {
				uSVGMatrix matrix = m_listTransform[0].matrix;
				for (int i = 1; i < m_listTransform.Count; i++) {
					matrix = matrix.Multiply(m_listTransform[i].matrix);
				}
				return matrix;
			}
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
	}
	public void AppendItems(uSVGTransformList newListItem) {
	  m_listTransform.AddRange(newListItem.m_listTransform);
	}
	public uSVGTransform this[int index] {
	  get {
  		if ((index < 0) || (index >= m_listTransform.Count)) {
  			throw new uDOMException(uDOMExceptionType.IndexSizeErr);
  		}
  		return m_listTransform[index];
		}
	}

	public void InsertItemBefore(uSVGTransform newItem, int index) {
		this.m_listTransform.Insert(index, newItem);
	}

	public void ReplaceItem(uSVGTransform newItem, int index) {
		this.m_listTransform[index] = newItem;
	}

	public uSVGTransform CreateSVGTransformFromMatrix(uSVGMatrix matrix) {
		return new uSVGTransform(matrix);
	}
	public uSVGTransform Consolidate() {
		uSVGTransform result = CreateSVGTransformFromMatrix(totalMatrix);
		return result;
	}
}