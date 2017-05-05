using System;
using System.Runtime.Serialization;

namespace EG {
public class InnerThreadAbortException : ApplicationException {
  public InnerThreadAbortException()
  : base("Работа потока прервана") {

  }

  public InnerThreadAbortException(string message)
  : base(message) {

  }

  public InnerThreadAbortException(SerializationInfo info, StreamingContext context)
  : base(info, context) {

  }

  public InnerThreadAbortException(string message, Exception innerException)
  : base(message, innerException) {

  }
}
}
