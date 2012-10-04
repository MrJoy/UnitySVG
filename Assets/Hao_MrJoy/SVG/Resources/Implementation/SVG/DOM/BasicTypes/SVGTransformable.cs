public class SVGTransformable {
  private SVGTransformList _inheritTransformList;
  private SVGTransformList _currentTransformList;
  private SVGTransformList _summaryTransformList;
  /**********************************************************************************/
  public SVGTransformList inheritTransformList {
    get { return _inheritTransformList; }
    set {
      int c = 0;
      if(_inheritTransformList != null)
        c += _inheritTransformList.Count;
      if(_currentTransformList != null)
        c += _currentTransformList.Count;
      _inheritTransformList = value;
      _summaryTransformList = new SVGTransformList(c);
      if(_inheritTransformList != null)
        _summaryTransformList.AppendItems(_inheritTransformList);
      if(_currentTransformList != null)
        _summaryTransformList.AppendItems(_currentTransformList);
    }
  }
  public SVGTransformList currentTransformList {
    get { return _currentTransformList; }
    set {
      _currentTransformList = value;
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
  }
  public SVGTransformList summaryTransformList {
    get { return _summaryTransformList; }
  }
  public float transformAngle {
    get {
      float _angle = 0.0f;
      for(int i = 0; i < _summaryTransformList.Count; i++ ) {
        SVGTransform _temp = _summaryTransformList[i];
        if(_temp.type == SVGTransformMode.Rotate)
          _angle += _temp.angle;
      }
      return _angle;
    }
  }
  public SVGMatrix transformMatrix {
    get { return summaryTransformList.Consolidate().matrix; }
  }
  /*********************************************************************************************/
  public SVGTransformable(SVGTransformList transformList) {
    inheritTransformList = transformList;
  }
}
