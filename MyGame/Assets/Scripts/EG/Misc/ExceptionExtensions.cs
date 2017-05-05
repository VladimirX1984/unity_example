using System;

namespace EG.Misc {
public static class ExceptionExtensions {
  public static string GetInnerMessage(this Exception exp) {
    string sIndent = String.Empty;
    string sIndent2 = "\t";
    Exception innerExp = null;
    string innerExpMsg = String.Empty;
    innerExp = exp.InnerException;
    while (innerExp != null) {
      innerExpMsg += StringExtensions.SafeFormat(
                       "\r\n{0}Внутреннее исключение: {1}\r\n{5}Тип исключения:{2}\r\n{5}Источник: {3}\r\n{5}Стек вызовов: {4}"
                       , sIndent, innerExp.Message, innerExp.GetType().Name, innerExp.Source, innerExp.StackTrace,
                       sIndent2);
      innerExp = innerExp.InnerException;
      sIndent += "\t";
      sIndent2 += "\t";
    }
    return innerExpMsg;
  }
}
}
