using UnityEngine;
using System.Collections.Generic;
using UnitySVG;

public class SVGPolygonElement : SVGBasicElement {
  private readonly List<Vector2> _listPoints;

  public List<Vector2> listPoints { get { return _listPoints; } }

  public SVGPolygonElement(Dictionary<string, string> attrList,
                           SVGTransformList inheritTransformList,
                           SVGPaintable inheritPaintable,
                           SVGGraphics render) : base(attrList, inheritTransformList, inheritPaintable, render) {
    _listPoints = ExtractPoints(attrList.GetValue("points"));
  }

  private static List<Vector2> ExtractPoints(string inputText) {
    List<Vector2> _return = new List<Vector2>();
    string[] _lstStr = SVGStringExtractor.ExtractTransformValue(inputText);

    int len = _lstStr.Length;

    for(int i = 0; i < len - 1; i++) {
      string value1 = _lstStr[i];
      string value2 = _lstStr[i + 1];
      SVGLength _length1 = new SVGLength(value1);
      SVGLength _length2 = new SVGLength(value2);
      Vector2 _point = new Vector2(_length1.value, _length2.value);
      _return.Add(_point);
      i++;
    }
    return _return;
  }

  protected override void CreateGraphicsPath() {
    _graphicsPath = new SVGGraphicsPath();

    _graphicsPath.Add(this);
    _graphicsPath.transformList = summaryTransformList;
  }
}
