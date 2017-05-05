using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using EG.Misc;

namespace EG {
public static class FatalExceptionHandler {
  public static ActionRef<Exception> OnExceptionHandler;

  public static bool IsErrorExisted {
    get { return Errors.Length > 0; }
  }

  static StringBuilder Errors = new StringBuilder();

  /// <summary>
  /// Обрабатывает исключение
  /// </summary>
  /// <param name="ex">исключение</param>
  public static void Handle(Exception ex) {
    if (ex == null) {
      return;
    }

    try {
      if (OnExceptionHandler != null) {
        OnExceptionHandler(ref ex);
      }
    }
    catch (Exception innerEx) {
      if (!ex.Data.Contains("OnExceptionHandler exception")) {
        string sData = StringExtensions.SafeFormat(
                         "OnExceptionHandler exception in Handle():\r\nИсключение: {0}\r\nТип исключения:{1}\r\nИсточник:{2}\r\nПроизошло:\r\n{3}",
                         innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);
        ex.Data.Add("OnExceptionHandler exception", sData);
      }
    }

    try {
      string sData = String.Empty;
      try {
        foreach (var key in ex.Data.Keys) {
          sData += String.Format("\r\n\tKey: \"{0}\", Value: \"{1}\"; ", key.ToString(),
                                 (ex.Data[key] != null ? ex.Data[key].ToString() : String.Empty));
        }
      }
      catch (Exception innerEx) {
        sData = StringExtensions.SafeFormat(
                  "data exception in Handle():\r\nИсключение: {0}\r\nТип исключения:{1}\r\nИсточник:{2}\r\nПроизошло:\r\n{3}",
                  innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);
      }


      string message = StringExtensions.SafeFormat(CultureInfo.CurrentCulture,
                       "\r\nИсключение: {0}\r\nТип исключения:{1}\r\nИсточник:{2}\r\nПроизошло:\r\n{3}\r\nПользовательские данные:{4}\r\n",
                       ex.Message, ex.GetType().Name, ex.Source, ex.StackTrace, sData);

      string innerExpMsg = ex.GetInnerMessage();

      message += innerExpMsg;

      if (String.IsNullOrEmpty(message) || message.IsEqual(StringExtensions.ExceptionInFormat)) {
        message += "\r\n";
        message += (ex.Message + ";;; " + ex.GetType().Name + ";;; " + ex.Source + ";;; " + ex.StackTrace);
      }

      AddIfNeeded(message);

      try {
        DebugLogger.WriteError(StringExtensions.SafeFormat("{0}\r\n", message));
      }
      catch (Exception innerEx) {
        sData = StringExtensions.SafeFormat(
                  "log exception in Handle():\r\nИсключение: {0}\r\nТип исключения:{1}\r\nИсточник:{2}\r\nПроизошло:\r\n{3}",
                  innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);
        AddIfNeeded(message + sData);
      }
    }
    catch (Exception innerEx) {
      var sData = StringExtensions.SafeFormat(
                    "exception in Handle():\nИсключение: {0}\nТип исключения:{1}\nИсточник:{2}\nПроизошло:\n{3}",
                    innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);

      try {
        DebugLogger.WriteError(String.Format("{0}", sData));
      }
      catch (Exception subInnerEx) {
        var sSubData = StringExtensions.SafeFormat(
                         "log exception in Handle():\nИсключение: {0}\nТип исключения:{1}\nИсточник:{2}\nПроизошло:\n{3}",
                         subInnerEx.Message, subInnerEx.GetType().Name, subInnerEx.Source, subInnerEx.StackTrace);
        AddIfNeeded(sData);
        AddIfNeeded(sSubData);
        return;
      }

      AddIfNeeded(sData);
    }
  }

  /*public static string GetErrorsMessage() {
    if (Errors.Length == 0) {
      return String.Empty;
    }

    try {
      // снова логируем ошибки для удобного поиска
      DebugLogger.WriteInfo("\r\n\r\nСообщения об необработанных ошибках:\r\n");
      DebugLogger.WriteCriticalError(LogAreaCode.Core, Errors.ToString());
    }
    catch (Exception innerEx) {
      string sData = StringExtensions.SafeFormat(
                       "log exception in GetErrorsMessage():\r\nИсключение: {0}\r\nТип исключения:{1}\r\nИсточник:{2}\r\nПроизошло:\r\n{3}",
                       innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);
      AddIfNeeded(sData);
    }

    try {
      FileInfo fi = new FileInfo(NotEnginePath.PathSimpleCombine(AppDomain.CurrentDomain.BaseDirectory,
                                 AppDomain.CurrentDomain.FriendlyName));

      DateTimeFormatInfo fmt = new CultureInfo(String.Empty).DateTimeFormat;
      fmt.ShortDatePattern = "yyyy-MM-dd";

      string exeFileName = StringExtensions.SafeFormat(
                             "Версия редактора: \"{0}\".\r\nДата обновления редактора пользователем \"{1}\": {2}\r\nВремя запуска редактора: {3}",
                             AppInfo.GetInstance().Version, AppDomain.CurrentDomain.FriendlyName,
                             fi.LastWriteTime.ToStrOrDefault(), AppInfo.GetInstance().StartAppTime);

      Errors.Insert(0, "\r\n\r\n");
      Errors.Insert(0, exeFileName);
    }
    catch (Exception innerEx) {
      string sData = StringExtensions.SafeFormat(
                       "exception in GetErrorsMessage():\r\nИсключение: {0}\r\nТип исключения:{1}\r\nИсточник:{2}\r\nПроизошло:\r\n{3}",
                       innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);
      AddIfNeeded(sData);
    }

    return Errors.ToString();
    }*/

  private static void AddIfNeeded(string message) {
    if (String.IsNullOrEmpty(message)) {
      return;
    }

    TErrorInfo findedErrorInfo = _exceptionMessageList.Find((it => it.message.CompareTo(message) == 0));
    if (findedErrorInfo != null) {
      try {
        DateTime dateTime = DateTime.Now;
        string sDataTime = dateTime.ToStr();
        // если есть такое же исключение: тип, название и само сообщение, то добавляем ссылку на предыдущее исключение
        Errors.Append(StringExtensions.SafeFormat(
                        "\r\nСсылка на ошибку (идентификатор: {0})\r\nВремя возникновения ошибки: {1}\r\n\r\n",
                        findedErrorInfo.id, sDataTime));
        return;
      }
      catch (Exception ex) {
        if (!(ex is FormatException || ex is ArgumentException || ex is ArgumentNullException ||
              ex is InvalidOperationException || ex is NotSupportedException || ex is StackOverflowException)) {
          //
          FatalExceptionHandler.Handle(ex);
          return;
        }
        // если есть такое же исключение: тип, название и само сообщение, то добавляем ссылку на предыдущее исключение
        Errors.Append(
          StringExtensions.SafeFormat("\r\nСсылка на ошибку (идентификатор: {0})\r\n\r\n",
                                      findedErrorInfo.id));
        return;
      }
    }

    TErrorInfo errorInfo = new TErrorInfo();
    errorInfo.id = -1;
    errorInfo.info = String.Empty;
    errorInfo.message = message;

    try {
      DateTime dateTime = DateTime.Now;
      string sDataTime = dateTime.ToStr();
      errorInfo.info = String.Format(
                         "Идентификатор ошибки: {0}\r\nВремя возникновения ошибки: {1}",
                         _exceptionMessageList.Count, sDataTime);
      errorInfo.id = _exceptionMessageList.Count;
    }
    catch (Exception ex) {
      if (!(ex is FormatException || ex is ArgumentException || ex is ArgumentNullException ||
            ex is InvalidOperationException || ex is NotSupportedException || ex is StackOverflowException)) {
        //
        FatalExceptionHandler.Handle(ex);
        return;
      }
    }
    finally {
      if (Errors.Length + errorInfo.message.Length + 8 < Errors.MaxCapacity) {
        Errors.Append(StringExtensions.SafeFormat("\r\n{0}{1}\r\n", errorInfo.info, errorInfo.message));
        _exceptionMessageList.Add(errorInfo);
      }
    }
  }

  class TErrorInfo {
    public int id;
    public string info;
    public string message;
  }

  private static List<TErrorInfo> _exceptionMessageList = new List<TErrorInfo>();
}
}
