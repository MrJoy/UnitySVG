using System.Collections.Generic;

public class uSVGPathSegList {
	private List<object> m_segList = new List<object>();
	/***********************************************************************************/
	public int numberOfItems {
		get{return this.m_segList.Count;}
	}
	
	/***********************************************************************************/
	public uSVGPathSegList() {
	}
	/***********************************************************************************/
	public void Clear() {
		this.m_segList.Clear();
	}
	//-----------
	public uSVGPathSeg Initialize(uSVGPathSeg newItem) {
		Clear();
		return AppendItem(newItem);
	}
	//-----------
	public uSVGPathSeg GetItem(int index) {
		if (index < 0 || index > numberOfItems) {
			return null;
		}
		return (uSVGPathSeg) this.m_segList[index];
	}
	//-----------
	public uSVGPathSeg AppendItem(uSVGPathSeg newItem) {
		this.m_segList.Add(newItem);
		f_SetListAndIndex(newItem, this.m_segList.Count - 1);
		return newItem;
	}
	/***********************************************************************************/
	internal uSVGPathSeg GetPreviousSegment(uSVGPathSeg seg) {
		int index =  this.m_segList.IndexOf(seg);
		if (index <= 0) {
			return null;
		} else {
			return (uSVGPathSeg) GetItem(index - 1);
		}
	}
	/***********************************************************************************/
	private void f_SetListAndIndex(uSVGPathSeg newItem, int index) {
		if (newItem != null) {
			newItem.SetList(this);
			newItem.SetIndex(index);
		}
	}
}