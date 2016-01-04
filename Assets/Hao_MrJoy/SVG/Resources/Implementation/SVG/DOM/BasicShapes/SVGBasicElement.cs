using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitySVG {
  public abstract class SVGBasicElement : SVGTransformable, ISVGDrawable {
    private readonly SVGGraphics _render;
    private readonly SVGPaintable _paintable;

    protected SVGGraphicsPath _graphicsPath;

    protected SVGBasicElement(Dictionary<string, string> attrList,
                              SVGTransformList inheritTransformList,
                              SVGPaintable inheritPaintable,
                              SVGGraphics render) : base(inheritTransformList) {
      _render = render;
      _paintable = new SVGPaintable(inheritPaintable, attrList);
    }

    protected abstract void CreateGraphicsPath();

    private void Draw() {
      if(_paintable.strokeColor == null)
        return;

      _render.DrawPath(_graphicsPath, _paintable.strokeWidth,
                       _paintable.strokeColor);
    }

    public void BeforeRender(SVGTransformList transformList) {
      inheritTransformList = transformList;
    }

    public void Render() {
      CreateGraphicsPath();
      _render.StrokeLineCap = _paintable.strokeLineCap;
      _render.StrokeLineJoin = _paintable.strokeLineJoin;
      switch(_paintable.GetPaintType()) {
      case SVGPaintMethod.SolidGradientFill:
        _render.FillPath(_paintable.fillColor.Value, _graphicsPath);
        Draw();
        break;
      case SVGPaintMethod.LinearGradientFill: {
        SVGLinearGradientBrush _linearGradBrush = _paintable.GetLinearGradientBrush(_graphicsPath);

        if(_linearGradBrush != null)
          _render.FillPath(_linearGradBrush, _graphicsPath);
        Draw();
        break;
      }
      case SVGPaintMethod.RadialGradientFill: {
        SVGRadialGradientBrush _radialGradBrush = _paintable.GetRadialGradientBrush(_graphicsPath);

        if(_radialGradBrush != null)
          _render.FillPath(_radialGradBrush, _graphicsPath);
        Draw();
        break;
      }
      case SVGPaintMethod.PathDraw:
        Draw();
        break;
      }
    }
  }
}
