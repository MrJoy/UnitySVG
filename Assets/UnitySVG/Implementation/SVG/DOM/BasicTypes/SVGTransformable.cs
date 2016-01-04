public class SVGTransformable {
  private SVGTransformList _inheritTransformList;
  private SVGTransformList _currentTransformList;
  private SVGTransformList _summaryTransformList;

  protected SVGTransformable(SVGTransformList transformList) {
    inheritTransformList = transformList;
  }

  protected SVGTransformList inheritTransformList {
    get { return _inheritTransformList; }
    set {
      _inheritTransformList = value;
      // TODO: Measure if (and how much) redundant work is happening by doing this here.  Consider a dirty state and
      // TODO: final-reconciliation-before-rendering pass.
      UpdateSummaryTransformList();
    }
  }

  protected SVGTransformList currentTransformList {
    get { return _currentTransformList; }
    set {
      _currentTransformList = value;
      UpdateSummaryTransformList();
    }
  }

  protected SVGTransformList summaryTransformList { get { return _summaryTransformList; } }

  protected void UpdateSummaryTransformList() {
    int c = 0;
    if(_inheritTransformList != null)
      c += _inheritTransformList.Count;
    if(_currentTransformList != null)
      c += _currentTransformList.Count;
    _summaryTransformList = new SVGTransformList(c);
    if(_inheritTransformList != null)
      _summaryTransformList.AppendItems(_inheritTransformList);
    if(_currentTransformList != null)
      _summaryTransformList.AppendItems(_currentTransformList);
  }

  public float transformAngle {
    get {
      float _angle = 0.0f;
      for(int i = 0; i < _summaryTransformList.Count; i++) {
        SVGTransform _temp = _summaryTransformList[i];
        if(_temp.type == SVGTransformMode.Rotate)
          _angle += _temp.angle;
      }
      return _angle;
    }
  }

  protected Matrix2x3 transformMatrix { get { return summaryTransformList.Consolidate().matrix; } }
}
