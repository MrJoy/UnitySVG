using System;

public enum uDOMExceptionType {
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
public class uDOMException : Exception
{
    protected uDOMException(string msg, Exception innerException) : base(msg, innerException)
    {
    }

    public uDOMException(uDOMExceptionType code) : this(code, String.Empty)
    {
    }

    public uDOMException(uDOMExceptionType code, string msg) : this(code, msg, null)
    {
    }

    public uDOMException(  uDOMExceptionType code,
                string msg,
                Exception innerException) : base(msg, innerException)
    {
      this.code = code;
    }

    protected uDOMException(   System.Runtime.Serialization.SerializationInfo info ,
                  System.Runtime.Serialization.StreamingContext context ) : base(info, context)
    {
    }

    private uDOMExceptionType code;
    public uDOMExceptionType Code
    {
      get{return code;}
    }
}
