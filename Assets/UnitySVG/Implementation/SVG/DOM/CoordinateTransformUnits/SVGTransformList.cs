using System.Collections.Generic;

public class SVGTransformList {
  private readonly List<SVGTransform> _listTransform;

  public int Count { get { return _listTransform.Count; } }

  private Matrix2x3 _totalMatrix;

  public Matrix2x3 totalMatrix {
    get {
      if(_totalMatrix == null) {
        _totalMatrix = new Matrix2x3();
        for(int i = 0; i < _listTransform.Count; ++i)
          _totalMatrix.Multiply(_listTransform[i].matrix);
      }
      return _totalMatrix;
    }
  }

  public SVGTransformList() {
    _listTransform = new List<SVGTransform>();
  }

  public SVGTransformList(int capacity) {
    _listTransform = new List<SVGTransform>(capacity);
  }

  public SVGTransformList(string listString) {
    _listTransform = SVGStringExtractor.ExtractTransformList(listString);
  }

  public void Clear() {
    _listTransform.Clear();
    _totalMatrix = null;
  }

  public void AppendItem(SVGTransform newItem) {
    _listTransform.Add(newItem);
    _totalMatrix = null;
  }

  public void AppendItems(SVGTransformList newListItem) {
    _listTransform.AddRange(newListItem._listTransform);
    _totalMatrix = null;
  }

  public SVGTransform this[int index] {
    get {
      if((index < 0) || (index >= _listTransform.Count))
        throw new DOMException(DOMExceptionType.IndexSizeErr);
      return _listTransform[index];
    }
  }

  public SVGTransform Consolidate() {
    return new SVGTransform(totalMatrix);
  }
}
