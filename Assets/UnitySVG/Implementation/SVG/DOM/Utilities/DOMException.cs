using System;
using System.Runtime.Serialization;

public enum DOMExceptionType {
  IndexSizeErr,
  DomstringSizeErr,
  HierarchyRequestErr,
  WrongDocumentErr,
  InvalidCharacterErr,
  NoDataAllowedErr,
  NoModificationAllowedErr,
  NotFoundErr,
  NotSupportedErr,
  InuseAttributeErr,
  InvalidStateErr,
  SyntaxErr,
  InvalidModificationErr,
  NamespaceErr,
  InvalidAccessErr
}

[Serializable]
public class DOMException : Exception {
  protected DOMException(string msg, Exception innerException) : base(msg, innerException) {
  }

  public DOMException(DOMExceptionType code) : this(code, String.Empty) {
  }

  public DOMException(DOMExceptionType code, string msg) : this(code, msg, null) {
  }

  public DOMException(DOMExceptionType code,
                      string msg,
                      Exception innerException) : base(msg, innerException) {
    this.code = code;
  }

  protected DOMException(SerializationInfo info,
                         StreamingContext context) : base(info, context) {
  }

  private DOMExceptionType code;

  public DOMExceptionType Code {
    get { return code; }
  }
}
