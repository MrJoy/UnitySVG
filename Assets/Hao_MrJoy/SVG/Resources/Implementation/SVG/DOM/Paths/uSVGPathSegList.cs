using System.Collections.Generic;

public class uSVGPathSegList {
  private List<object> _segList = new List<object>();
  /***********************************************************************************/
  public int numberOfItems {
    get{return this._segList.Count;}
  }

  /***********************************************************************************/
  public uSVGPathSegList() {
  }
  /***********************************************************************************/
  public void Clear() {
    this._segList.Clear();
  }
  //-----------
  public uSVGPathSeg Initialize(uSVGPathSeg newItem) {
    Clear();
    return AppendItem(newItem);
  }
  //-----------
  public uSVGPathSeg GetItem(int index) {
    if(index < 0 || index > numberOfItems) {
      return null;
    }
    return(uSVGPathSeg)this._segList[index];
  }
  //-----------
  public uSVGPathSeg AppendItem(uSVGPathSeg newItem) {
    this._segList.Add(newItem);
    SetListAndIndex(newItem, this._segList.Count - 1);
    return newItem;
  }
  /***********************************************************************************/
  internal uSVGPathSeg GetPreviousSegment(uSVGPathSeg seg) {
    int index =  this._segList.IndexOf(seg);
    if(index <= 0) {
      return null;
    } else {
      return(uSVGPathSeg)GetItem(index - 1);
    }
  }
  /***********************************************************************************/
  private void SetListAndIndex(uSVGPathSeg newItem, int index) {
    if(newItem != null) {
      newItem.SetList(this);
      newItem.SetIndex(index);
    }
  }
}