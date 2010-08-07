using System.Collections.Generic;

public class uSVGTransformList {
  private List<uSVGTransform> _listTransform;
  /*********************************************************************************************/
  public int Count {
    get { return _listTransform.Count; }
  }
  private uSVGMatrix _totalMatrix = null;
  public uSVGMatrix totalMatrix {
    get {
      if(_totalMatrix == null) {
        if(_listTransform.Count == 0) {
          _totalMatrix = new uSVGMatrix(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);
        } else {
          uSVGMatrix matrix = _listTransform[0].matrix;
          for(int i = 1; i < _listTransform.Count; i++)
            matrix = matrix.Multiply(_listTransform[i].matrix);
          _totalMatrix = matrix;
        }
      }
      return _totalMatrix;
    }
  }
  /*********************************************************************************************/
  public uSVGTransformList() {
    _listTransform = new List<uSVGTransform>();
  }
  public uSVGTransformList(int capacity) {
    _listTransform = new List<uSVGTransform>(capacity);
  }
  public uSVGTransformList(string listString) {
    _listTransform = uSVGStringExtractor.f_ExtractTransformList(listString);
  }

  /*********************************************************************************************/
  public void Clear() {
    _listTransform.Clear();
    _totalMatrix = null;
  }
  public void AppendItem(uSVGTransform newItem) {
    _listTransform.Add(newItem);
    _totalMatrix = null;
  }
  public void AppendItems(uSVGTransformList newListItem) {
    _listTransform.AddRange(newListItem._listTransform);
    _totalMatrix = null;
  }
  public uSVGTransform this[int index] {
    get {
      if((index < 0) || (index >= _listTransform.Count))
        throw new uDOMException(uDOMExceptionType.IndexSizeErr);
      return _listTransform[index];
    }
  }

  public uSVGTransform Consolidate() {
    return new uSVGTransform(totalMatrix);
  }
}
