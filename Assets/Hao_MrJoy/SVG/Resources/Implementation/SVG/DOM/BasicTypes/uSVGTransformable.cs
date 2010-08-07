public class uSVGTransformable {
  private uSVGTransformList _inheritTransformList;
  private uSVGTransformList _currentTransformList;
  private uSVGTransformList _summaryTransformList;
  /**********************************************************************************/
  public uSVGTransformList inheritTransformList {
    get { return _inheritTransformList; }
    set {
      int c = 0;
      if(_inheritTransformList != null)c += _inheritTransformList.Count;
      if(_currentTransformList != null)c += _currentTransformList.Count;
      _inheritTransformList = value;
      _summaryTransformList = new uSVGTransformList(c);
      if(_inheritTransformList != null)
        _summaryTransformList.AppendItems(_inheritTransformList);
      if(_currentTransformList != null)
        _summaryTransformList.AppendItems(_currentTransformList);
    }
  }
  public uSVGTransformList currentTransformList {
    get { return _currentTransformList; }
    set {
      _currentTransformList = value;
      int c = 0;
      if(_inheritTransformList != null)c += _inheritTransformList.Count;
      if(_currentTransformList != null)c += _currentTransformList.Count;
      _summaryTransformList = new uSVGTransformList(c);
      if(_inheritTransformList != null)
        _summaryTransformList.AppendItems(_inheritTransformList);
      if(_currentTransformList != null)
        _summaryTransformList.AppendItems(_currentTransformList);
    }
  }
  public uSVGTransformList summaryTransformList {
    get { return _summaryTransformList; }
  }
  public float transformAngle {
    get {
      float _angle = 0.0f;
      for(int i = 0; i < _summaryTransformList.Count; i++ ) {
        uSVGTransform _temp = _summaryTransformList[i];
        if(_temp.type == uSVGTransformType.SVG_TRANSFORM_ROTATE)
          _angle += _temp.angle;
      }
      return _angle;
    }
  }
  public uSVGMatrix transformMatrix {
    get { return summaryTransformList.Consolidate().matrix; }
  }
  /*********************************************************************************************/
  public uSVGTransformable(uSVGTransformList transformList) {
    inheritTransformList = transformList;
  }
}
