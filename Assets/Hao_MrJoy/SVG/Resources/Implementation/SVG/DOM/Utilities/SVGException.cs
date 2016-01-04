using System;
using System.Runtime.Serialization;

public enum SVGExceptionType {
  WrongType,
  InvalidValue,
  MatrixNotInvertable
}

public class SVGException : DOMException {
  public SVGException(SVGExceptionType errorCode) : this(errorCode, String.Empty, null) {
  }

  public SVGException(SVGExceptionType errorCode, string message) : this(errorCode, message, null) {
  }

  public SVGException(SVGExceptionType errorCode, string message, Exception innerException)
  : base(message, innerException) {
    code = errorCode;
  }

  protected SVGException(SerializationInfo info, StreamingContext context) : base(info, context) {
  }

  private SVGExceptionType code;

  public new SVGExceptionType Code { get { return code; } }
}
