using System;
using System.Globalization;

public static class SVGNumber {
  public static float ParseToFloat(string str) {
    float val;
    int index = str.IndexOfAny(new Char[]{'E','e'});
    if(index > -1) {
      float number = SVGNumber.ParseToFloat(str.Substring(0, index));
      float power = SVGNumber.ParseToFloat(str.Substring(index+1));

      val = (float)Math.Pow(10, power)* number;
    } else {
      try  {
        val = Single.Parse(str, CultureInfo.InvariantCulture);
      } catch(Exception e) {
        throw new DOMException(DOMExceptionType.SyntaxErr, "Input string was not in a correct format: " + str, e);
      }
    }
    return val;
  }

  //-------------------------------------------------------------------------------------------
  public static float CalcAngleDiff(float a1, float a2) {
    while(a1 < 0)a1 += 360;

    a1 %= 360;

    while(a2 < 0)a2 += 360;
    a2 %= 360;

    float diff = (a1-a2);

    while(diff<0)diff += 360;
    diff %= 360;

    return diff;
  }
  //-------------------------------------------------------------------------------------------
  public static float CalcAngleBisection(float a1, float a2) {
    float diff = CalcAngleDiff(a1, a2);
    float bisect = a1 - diff/2F;

    while(bisect<0)bisect += 360;

    bisect %= 360;
    return bisect;
  }
}
