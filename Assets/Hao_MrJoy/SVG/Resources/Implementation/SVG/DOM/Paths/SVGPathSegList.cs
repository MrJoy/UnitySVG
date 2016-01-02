using System.Collections.Generic;

public class SVGPathSegList {
  private List<object> _segList;

  public SVGPathSegList(int size) {
    _segList = new List<object>(size);
  }

  public int Count { get { return _segList.Count; } }

  public void Clear() {
    this._segList.Clear();
  }

  public SVGPathSeg GetItem(int index) {
    if(index < 0 || index > _segList.Count)
      return null;
    return (SVGPathSeg)this._segList[index];
  }

  public SVGPathSeg AppendItem(SVGPathSeg newItem) {
    this._segList.Add(newItem);
    SetList(newItem);
    return newItem;
  }

  internal SVGPathSeg GetPreviousSegment(SVGPathSeg seg) {
    int index = this._segList.IndexOf(seg);
    if(index <= 0)
      return null;
    else
      return (SVGPathSeg)GetItem(index - 1);
  }

  private void SetList(SVGPathSeg newItem) {
    if(newItem != null)
      newItem.SetList(this);
  }
}
