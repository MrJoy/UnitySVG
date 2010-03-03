using UnityEngine;

public struct uSVGColorExtractor {
	public static Color aliceblue { get {return change(240, 248, 255);} }
	public static Color antiquewhite { get {return change(250, 235, 215);} }
	public static Color aqua { get {return change( 0, 255, 255);} }
	public static Color aquamarine { get {return change(127, 255, 212);} }
	public static Color azure { get {return change(240, 255, 255);} }
	public static Color beige { get {return change(245, 245, 220);} }
	public static Color bisque { get {return change(255, 228, 196);} }
	public static Color black { get {return change( 0, 0, 0);} }
	public static Color blanchedalmond { get {return change(255, 235, 205);} }
	public static Color blue { get {return change( 0, 0, 255);} }
	public static Color blueviolet { get {return change(138, 43, 226);} }
	public static Color brown { get {return change(165, 42, 42);} }
	public static Color burlywood { get {return change(222, 184, 135);} }
	public static Color cadetblue { get {return change( 95, 158, 160);} }
	public static Color chartreuse { get {return change(127, 255, 0);} }
	public static Color chocolate { get {return change(210, 105, 30);} }
	public static Color coral { get {return change(255, 127, 80);} }
	public static Color cornflowerblue { get {return change(100, 149, 237);} }
	public static Color cornsilk { get {return change(255, 248, 220);} }
	public static Color crimson { get {return change(220, 20, 60);} }
	public static Color cyan { get {return change( 0, 255, 255);} }
	public static Color darkblue { get {return change( 0, 0, 139);} }
	public static Color darkcyan { get {return change( 0, 139, 139);} }
	public static Color darkgoldenrod { get {return change(184, 134, 11);} }
	public static Color darkgray { get {return change(169, 169, 169);} }
	public static Color darkgreen { get {return change( 0, 100, 0);} }
	public static Color darkgrey { get {return change(169, 169, 169);} }
	public static Color darkkhaki { get {return change(189, 183, 107);} }
	public static Color darkmagenta { get {return change(139, 0, 139);} }
	public static Color darkolivegreen { get {return change( 85, 107, 47);} }
	public static Color darkorange { get {return change(255, 140, 0);} }
	public static Color darkorchid { get {return change(153, 50, 204);} }
	public static Color darkred { get {return change(139, 0, 0);} }
	public static Color darksalmon { get {return change(233, 150, 122);} }
	public static Color darkseagreen { get {return change(143, 188, 143);} }
	public static Color darkslateblue { get {return change( 72, 61, 139);} }
	public static Color darkslategray { get {return change( 47, 79, 79);} }
	public static Color darkslategrey { get {return change( 47, 79, 79);} }
	public static Color darkturquoise { get {return change( 0, 206, 209);} }
	public static Color darkviolet { get {return change(148, 0, 211);} }
	public static Color deeppink { get {return change(255, 20, 147);} }
	public static Color deepskyblue { get {return change( 0, 191, 255);} }
	public static Color dimgray { get {return change(105, 105, 105);} }
	public static Color dimgrey { get {return change(105, 105, 105);} }
	public static Color dodgerblue { get {return change( 30, 144, 255);} }
	public static Color firebrick { get {return change(178, 34, 34);} }
	public static Color floralwhite { get {return change(255, 250, 240);} }
	public static Color forestgreen { get {return change( 34, 139, 34);} }
	public static Color fuchsia { get {return change(255, 0, 255);} }
	public static Color gainsboro { get {return change(220, 220, 220);} }
	public static Color ghostwhite { get {return change(248, 248, 255);} }
	public static Color gold { get {return change(255, 215, 0);} }
	public static Color goldenrod { get {return change(218, 165, 32);} }
	public static Color gray { get {return change(128, 128, 128);} }
	public static Color grey { get {return change(128, 128, 128);} }
	public static Color green { get {return change( 0, 128, 0);} }
	public static Color greenyellow { get {return change(173, 255, 47);} }
	public static Color honeydew { get {return change(240, 255, 240);} }
	public static Color hotpink { get {return change(255, 105, 180);} }
	public static Color indianred { get {return change(205, 92, 92);} }
	public static Color indigo { get {return change( 75, 0, 130);} }
	public static Color ivory { get {return change(255, 255, 240);} }
	public static Color khaki { get {return change(240, 230, 140);} }
	public static Color lavender { get {return change(230, 230, 250);} }
	public static Color lavenderblush { get {return change(255, 240, 245);} }
	public static Color lawngreen { get {return change(124, 252, 0);} }
	public static Color lemonchiffon { get {return change(255, 250, 205);} }
	public static Color lightblue { get {return change(173, 216, 230);} }
	public static Color lightcoral { get {return change(240, 128, 128);} }
	public static Color lightcyan { get {return change(224, 255, 255);} }
	public static Color lightgoldenrodyellow { get {return change(250, 250, 210);} }
	public static Color lightgray { get {return change(211, 211, 211);} }
	public static Color lightgreen { get {return change(144, 238, 144);} }
	public static Color lightgrey { get {return change(211, 211, 211);} }
	public static Color lightpink { get {return change(255, 182, 193);} }
	public static Color lightsalmon { get {return change(255, 160, 122);} }
	public static Color lightseagreen { get {return change( 32, 178, 170);} }
	public static Color lightskyblue { get {return change(135, 206, 250);} }
	public static Color lightslategray { get {return change(119, 136, 153);} }
	public static Color lightslategrey { get {return change(119, 136, 153);} }
	public static Color lightsteelblue { get {return change(176, 196, 222);} }
	public static Color lightyellow { get {return change(255, 255, 224);} }
	public static Color lime { get {return change( 0, 255, 0);} }
	public static Color limegreen { get {return change( 50, 205, 50);} }
	public static Color linen { get {return change(250, 240, 230);} }
	public static Color magenta { get {return change(255, 0, 255);} }
	public static Color maroon { get {return change(128, 0, 0);} }
	public static Color mediumaquamarine { get {return change(102, 205, 170);} }
	public static Color mediumblue { get {return change( 0, 0, 205);} }
	public static Color mediumorchid { get {return change(186, 85, 211);} }
	public static Color mediumpurple { get {return change(147, 112, 219);} }
	public static Color mediumseagreen { get {return change( 60, 179, 113);} }
	public static Color mediumslateblue { get {return change(123, 104, 238);} }
	public static Color mediumspringgreen { get {return change( 0, 250, 154);} }
	public static Color mediumturquoise { get {return change( 72, 209, 204);} }
	public static Color mediumvioletred { get {return change(199, 21, 133);} }
	public static Color midnightblue { get {return change( 25, 25, 112);} }
	public static Color mintcream { get {return change(245, 255, 250);} }
	public static Color mistyrose { get {return change(255, 228, 225);} }
	public static Color moccasin { get {return change(255, 228, 181);} }
	public static Color navajowhite { get {return change(255, 222, 173);} }
	public static Color navy { get {return change( 0, 0, 128);} }
	public static Color oldlace { get {return change(253, 245, 230);} }
	public static Color olive { get {return change(128, 128, 0);} }
	public static Color olivedrab { get {return change(107, 142, 35);} }
	public static Color orange { get {return change(255, 165, 0);} }
	public static Color orangered { get {return change(255, 69, 0);} }
	public static Color orchid { get {return change(218, 112, 214);} }
	public static Color palegoldenrod { get {return change(238, 232, 170);} }
	public static Color palegreen { get {return change(152, 251, 152);} }
	public static Color paleturquoise { get {return change(175, 238, 238);} }
	public static Color palevioletred { get {return change(219, 112, 147);} }
	public static Color papayawhip { get {return change(255, 239, 213);} }
	public static Color peachpuff { get {return change(255, 218, 185);} }
	public static Color peru { get {return change(205, 133, 63);} }
	public static Color pink { get {return change(255, 192, 203);} }
	public static Color plum { get {return change(221, 160, 221);} }
	public static Color powderblue { get {return change(176, 224, 230);} }
	public static Color purple { get {return change(128, 0, 128);} }
	public static Color red { get {return change(255, 0, 0);} }
	public static Color rosybrown { get {return change(188, 143, 143);} }
	public static Color royalblue { get {return change( 65, 105, 225);} }
	public static Color saddlebrown { get {return change(139, 69, 19);} }
	public static Color salmon { get {return change(250, 128, 114);} }
	public static Color sandybrown { get {return change(244, 164, 96);} }
	public static Color seagreen { get {return change( 46, 139, 87);} }
	public static Color seashell { get {return change(255, 245, 238);} }
	public static Color sienna { get {return change(160, 82, 45);} }
	public static Color silver { get {return change(192, 192, 192);} }
	public static Color skyblue { get {return change(135, 206, 235);} }
	public static Color slateblue { get {return change(106, 90, 205);} }
	public static Color slategray { get {return change(112, 128, 144);} }
	public static Color slategrey { get {return change(112, 128, 144);} }
	public static Color snow { get {return change(255, 250, 250);} }
	public static Color springgreen { get {return change( 0, 255, 127);} }
	public static Color steelblue { get {return change( 70, 130, 180);} }
	public static Color tan { get {return change(210, 180, 140);} }
	public static Color teal { get {return change( 0, 128, 128);} }
	public static Color thistle { get {return change(216, 191, 216);} }
	public static Color tomato { get {return change(255, 99, 71);} }
	public static Color turquoise { get {return change( 64, 224, 208);} }
	public static Color violet { get {return change(238, 130, 238);} }
	public static Color wheat { get {return change(245, 222, 179);} }
	public static Color white { get {return change(255, 255, 255);} }
	public static Color whitesmoke { get {return change(245, 245, 245);} }
	public static Color yellow { get {return change(255, 255, 0);} }
	public static Color yellowgreen { get {return change(154, 205, 50);} }

	private static Color change(int r, int g, int b) {
		return new Color((float)r/255.0f, (float)g/255.0f, (float)b/255.0f);
	}
	public static Color f_GetColor(string name) {
		switch(name.ToLower()) {
			case "aliceblue": return aliceblue;
			case "antiquewhite": return antiquewhite;
			case "aqua": return aqua;
			case "aquamarine": return aquamarine;
			case "azure": return azure;
			case "beige": return beige;
			case "bisque": return bisque;
			case "black": return black;
			case "blanchedalmond": return blanchedalmond;
			case "blue": return blue;
			case "blueviolet": return blueviolet;
			case "brown": return brown;
			case "burlywood": return burlywood;
			case "cadetblue": return cadetblue;
			case "chartreuse": return chartreuse;
			case "chocolate": return chocolate;
			case "coral": return coral;
			case "cornflowerblue": return cornflowerblue;
			case "cornsilk": return cornsilk;
			case "crimson": return crimson;
			case "cyan": return cyan;
			case "darkblue": return darkblue;
			case "darkcyan": return darkcyan;
			case "darkgoldenrod": return darkgoldenrod;
			case "darkgray": return darkgray;
			case "darkgreen": return darkgreen;
			case "darkgrey": return darkgrey;
			case "darkkhaki": return darkkhaki;
			case "darkmagenta": return darkmagenta;
			case "darkolivegreen": return darkolivegreen;
			case "darkorange": return darkorange;
			case "darkorchid": return darkorchid;
			case "darkred": return darkred;
			case "darksalmon": return darksalmon;
			case "darkseagreen": return darkseagreen;
			case "darkslateblue": return darkslateblue;
			case "darkslategray": return darkslategray;
			case "darkslategrey": return darkslategrey;
			case "darkturquoise": return darkturquoise;
			case "darkviolet": return darkviolet;
			case "deeppink": return deeppink;
			case "deepskyblue": return deepskyblue;
			case "dimgray": return dimgray;
			case "dimgrey": return dimgrey;
			case "dodgerblue": return dodgerblue;
			case "firebrick": return firebrick;
			case "floralwhite": return floralwhite;
			case "forestgreen": return forestgreen;
			case "fuchsia": return fuchsia;
			case "gainsboro": return gainsboro;
			case "ghostwhite": return ghostwhite;
			case "gold": return gold;
			case "goldenrod": return goldenrod;
			case "gray": return gray;
			case "grey": return grey;
			case "green": return green;
			case "greenyellow": return greenyellow;
			case "honeydew": return honeydew;
			case "hotpink": return hotpink;
			case "indianred": return indianred;
			case "indigo": return indigo;
			case "ivory": return ivory;
			case "khaki": return khaki;
			case "lavender": return lavender;
			case "lavenderblush": return lavenderblush;
			case "lawngreen": return lawngreen;
			case "lemonchiffon": return lemonchiffon;
			case "lightblue": return lightblue;
			case "lightcoral": return lightcoral;
			case "lightcyan": return lightcyan;
			case "lightgoldenrodyellow": return lightgoldenrodyellow;
			case "lightgray": return lightgray;
			case "lightgreen": return lightgreen;
			case "lightgrey": return lightgrey;
			case "lightpink": return lightpink;
			case "lightsalmon": return lightsalmon;
			case "lightseagreen": return lightseagreen;
			case "lightskyblue": return lightskyblue;
			case "lightslategray": return lightslategray;
			case "lightslategrey": return lightslategrey;
			case "lightsteelblue": return lightsteelblue;
			case "lightyellow": return lightyellow;
			case "lime": return lime;
			case "limegreen": return limegreen;
			case "linen": return linen;
			case "magenta": return magenta;
			case "maroon": return maroon;
			case "mediumaquamarine": return mediumaquamarine;
			case "mediumblue": return mediumblue;
			case "mediumorchid": return mediumorchid;
			case "mediumpurple": return mediumpurple;
			case "mediumseagreen": return mediumseagreen;
			case "mediumslateblue": return mediumslateblue;
			case "mediumspringgreen": return mediumspringgreen;
			case "mediumturquoise": return mediumturquoise;
			case "mediumvioletred": return mediumvioletred;
			case "midnightblue": return midnightblue;
			case "mintcream": return mintcream;
			case "mistyrose": return mistyrose;
			case "moccasin": return moccasin;
			case "navajowhite": return navajowhite;
			case "navy": return navy;
			case "oldlace": return oldlace;
			case "olive": return olive;
			case "olivedrab": return olivedrab;
			case "orange": return orange;
			case "orangered": return orangered;
			case "orchid": return orchid;
			case "palegoldenrod": return palegoldenrod;
			case "palegreen": return palegreen;
			case "paleturquoise": return paleturquoise;
			case "palevioletred": return palevioletred;
			case "papayawhip": return papayawhip;
			case "peachpuff": return peachpuff;
			case "peru": return peru;
			case "pink": return pink;
			case "plum": return plum;
			case "powderblue": return powderblue;
			case "purple": return purple;
			case "red": return red;
			case "rosybrown": return rosybrown;
			case "royalblue": return royalblue;
			case "saddlebrown": return saddlebrown;
			case "salmon": return salmon;
			case "sandybrown": return sandybrown;
			case "seagreen": return seagreen;
			case "seashell": return seashell;
			case "sienna": return sienna;
			case "silver": return silver;
			case "skyblue": return skyblue;
			case "slateblue": return slateblue;
			case "slategray": return slategray;
			case "slategrey": return slategrey;
			case "snow": return snow;
			case "springgreen": return springgreen;
			case "steelblue": return steelblue;
			case "tan": return tan;
			case "teal": return teal;
			case "thistle": return thistle;
			case "tomato": return tomato;
			case "turquoise": return turquoise;
			case "violet": return violet;
			case "wheat": return wheat;
			case "white": return white;
			case "whitesmoke": return whitesmoke;
			case "yellow": return yellow;
			case "yellowgreen": return yellowgreen;
			default: {
				 return f_HexColor(name);
			}
		}
	}
	//------------
	public static bool IsConstName(string textColor) {
		switch(textColor.ToLower()) {
			case "aliceblue":
			case "antiquewhite":
			case "aqua":
			case "aquamarine":
			case "azure":
			case "beige":
			case "bisque":
			case "black":
			case "blanchedalmond":
			case "blue":
			case "blueviolet":
			case "brown":
			case "burlywood":
			case "cadetblue":
			case "chartreuse":
			case "chocolate":
			case "coral":
			case "cornflowerblue":
			case "cornsilk":
			case "crimson":
			case "cyan":
			case "darkblue":
			case "darkcyan":
			case "darkgoldenrod":
			case "darkgray":
			case "darkgreen":
			case "darkgrey":
			case "darkkhaki":
			case "darkmagenta":
			case "darkolivegreen":
			case "darkorange":
			case "darkorchid":
			case "darkred":
			case "darksalmon":
			case "darkseagreen":
			case "darkslateblue":
			case "darkslategray":
			case "darkslategrey":
			case "darkturquoise":
			case "darkviolet":
			case "deeppink":
			case "deepskyblue":
			case "dimgray":
			case "dimgrey":
			case "dodgerblue":
			case "firebrick":
			case "floralwhite":
			case "forestgreen":
			case "fuchsia":
			case "gainsboro":
			case "ghostwhite":
			case "gold":
			case "goldenrod":
			case "gray":
			case "grey":
			case "green":
			case "greenyellow":
			case "honeydew":
			case "hotpink":
			case "indianred":
			case "indigo":
			case "ivory":
			case "khaki":
			case "lavender":
			case "lavenderblush":
			case "lawngreen":
			case "lemonchiffon":
			case "lightblue":
			case "lightcoral":
			case "lightcyan":
			case "lightgoldenrodyellow":
			case "lightgray":
			case "lightgreen":
			case "lightgrey":
			case "lightpink":
			case "lightsalmon":
			case "lightseagreen":
			case "lightskyblue":
			case "lightslategray":
			case "lightslategrey":
			case "lightsteelblue":
			case "lightyellow":
			case "lime":
			case "limegreen":
			case "linen":
			case "magenta":
			case "maroon":
			case "mediumaquamarine":
			case "mediumblue":
			case "mediumorchid":
			case "mediumpurple":
			case "mediumseagreen":
			case "mediumslateblue":
			case "mediumspringgreen":
			case "mediumturquoise":
			case "mediumvioletred":
			case "midnightblue":
			case "mintcream":
			case "mistyrose":
			case "moccasin":
			case "navajowhite":
			case "navy":
			case "oldlace":
			case "olive":
			case "olivedrab":
			case "orange":
			case "orangered":
			case "orchid":
			case "palegoldenrod":
			case "palegreen":
			case "paleturquoise":
			case "palevioletred":
			case "papayawhip":
			case "peachpuff":
			case "peru":
			case "pink":
			case "plum":
			case "powderblue":
			case "purple":
			case "red":
			case "rosybrown":
			case "royalblue":
			case "saddlebrown":
			case "salmon":
			case "sandybrown":
			case "seagreen":
			case "seashell":
			case "sienna":
			case "silver":
			case "skyblue":
			case "slateblue":
			case "slategray":
			case "slategrey":
			case "snow":
			case "springgreen":
			case "steelblue":
			case "tan":
			case "teal":
			case "thistle":
			case "tomato":
			case "turquoise":
			case "violet":
			case "wheat":
			case "white":
			case "whitesmoke":
			case "yellow":
			case "yellowgreen": {
				return true;
			}
		}
		return false;
	}
	public static bool IsHexColor(string textColor) {
		string temp = textColor.Trim();
		if (temp.Length > 0) {
			if (temp[0] == '#') {
				if ((temp.Length == 4) || (temp.Length == 7)) {
					return true;
				}
			} 
		}
		return false;
	}
	private static Color f_HexColor(string textColor) {
		string temp;
		int r=0, g=0, b=0;
		string sr="", sg="", sb="";
		temp = textColor.Trim();
		if (temp.Length > 0) {
			if (temp[0] == '#') {
				if (temp.Length == 4) {
					sr += temp.Substring(1,1) + temp.Substring(1,1);
					sg += temp.Substring(2,1) + temp.Substring(2,1);
					sb += temp.Substring(3,1) + temp.Substring(3,1);
					
					r = int.Parse(sr, System.Globalization.NumberStyles.HexNumber);
					g = int.Parse(sg, System.Globalization.NumberStyles.HexNumber);
					b = int.Parse(sb, System.Globalization.NumberStyles.HexNumber);
				} else if (temp.Length == 7) {
					sr = temp.Substring(1,2);
					sg = temp.Substring(3,2);
					sb = temp.Substring(5,2);

					r = int.Parse(sr, System.Globalization.NumberStyles.HexNumber);
					g = int.Parse(sg, System.Globalization.NumberStyles.HexNumber);
					b = int.Parse(sb, System.Globalization.NumberStyles.HexNumber);
				}
			} 
		}
		return change(r, g, b);
	}
}