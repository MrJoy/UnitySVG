using System;

public enum uSVGExceptionType
{
		SvgWrongTypeErr, 
		SvgInvalidValueErr, 
		SvgMatrixNotInvertable
}

public class uSVGException : uDOMException
{
		public uSVGException(uSVGExceptionType errorCode):this(errorCode, String.Empty, null)
		{
			
		}

		public uSVGException(uSVGExceptionType errorCode, string message):this(errorCode, message, null)
		{
		}

		public uSVGException(uSVGExceptionType errorCode, string message, Exception innerException):base(message, innerException)
		{
			code = errorCode;
		}

		protected uSVGException ( System.Runtime.Serialization.SerializationInfo info , System.Runtime.Serialization.StreamingContext context ) : base(info, context)
		{
		}

		private uSVGExceptionType code;
		public new uSVGExceptionType Code
		{
			get
			{
				return code;
			}
		}
}
