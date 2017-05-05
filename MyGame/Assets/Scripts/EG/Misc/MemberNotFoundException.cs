using System;
using System.Runtime.Serialization;

namespace EG.Misc {
class MemberNotFoundException : ApplicationException {
  public MemberNotFoundException()
  : base() {

  }

  public MemberNotFoundException(string message)
  : base(message) {

  }

  public MemberNotFoundException(SerializationInfo info, StreamingContext context)
  : base(info, context) {

  }

  public MemberNotFoundException(string message, Exception innerException)
  : base(message, innerException) {

  }
}
}
