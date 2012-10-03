using System;

public static class SVGNumber {
  public static float CalcAngleDiff(float a1, float a2) {
    while(a1 < 0)
      a1 += 360;
    a1 %= 360;

    while(a2 < 0)
      a2 += 360;
    a2 %= 360;

    float diff = (a1 - a2);

    while(diff < 0)
      diff += 360;
    diff %= 360;

    return diff;
  }
  //-------------------------------------------------------------------------------------------
  public static float CalcAngleBisection(float a1, float a2) {
    float diff = CalcAngleDiff(a1, a2);
    float bisect = a1 - diff / 2f;

    while(bisect < 0)
      bisect += 360;

    bisect %= 360;
    return bisect;
  }
}
