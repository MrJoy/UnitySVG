using System;

public class uSVGNumber {

	private float m_value;
	//*************************************************************************************
	public float value {
		get {
			return m_value;
		}
		set	{
			this.m_value = value;
		}
	}
	//*********************************************************************************
	public uSVGNumber(float val) {
		m_value = val;
	}

	public uSVGNumber(string str) {
		m_value = uSVGNumber.ParseToFloat(str);
	}
	//************************************************************************************
	public static float ParseToFloat(string str) {
		float val;
		int index = str.IndexOfAny(new Char[]{'E','e'});
		if(index > -1) {
			float number = uSVGNumber.ParseToFloat(str.Substring(0, index));
			float power = uSVGNumber.ParseToFloat(str.Substring(index+1));

			val = (float) Math.Pow(10, power) * number;
		} else {
			try	{
				val = Single.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
			} catch (Exception e) {
				throw new uDOMException(uDOMExceptionType.SyntaxErr, "Input string was not in a correct format: " + str, e);
			}
		}
		return val;
	}
		
	//-------------------------------------------------------------------------------------------
	public static float CalcAngleDiff(float a1, float a2) {
		while (a1 < 0) a1 += 360;
		
		a1 %= 360;

		while(a2 < 0) a2 += 360;
		a2 %= 360;

		float diff = (a1-a2);

		while(diff<0) diff += 360;
		diff %= 360;
            
		return diff;
	}
	//-------------------------------------------------------------------------------------------
	public static float CalcAngleBisection(float a1, float a2) {
		float diff = CalcAngleDiff(a1, a2);
		float bisect = a1 - diff/2F;

		while(bisect<0) bisect += 360;

		bisect %= 360;
		return bisect;
	}
}
//}
