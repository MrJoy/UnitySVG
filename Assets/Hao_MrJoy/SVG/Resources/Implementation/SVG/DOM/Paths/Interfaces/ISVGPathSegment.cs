public interface ISVGPathSegment {
  void ExpandBounds(SVGGraphicsPath path);

  bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw);
}
