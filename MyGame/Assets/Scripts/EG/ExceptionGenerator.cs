using System;
using System.Threading;

using EG.Misc;

namespace EG {
public static class ExceptionGenerator {

  #region Создание исключения

  public static Exception CreateException(Type expType, string message, params object[] args) {
    string header = StringExtensions.SafeFormat(message, args);

    Exception newEx = null;
    try {
      if (expType == typeof(ThreadAbortException)) {
        expType = typeof(InnerThreadAbortException);
      }

      if (String.IsNullOrEmpty(header)) {
        newEx = (Exception)Activator.CreateInstance(expType);
      }
      else {
        object[] exArgs = new object[1] { header };
        newEx = (Exception)Activator.CreateInstance(expType, exArgs);
      }
    }
    catch (Exception innerEx) {
      newEx = new Exception(header);
      string sInner =
        StringExtensions.SafeFormat("exception in CreateException of type {0}:\nИсключение: {1}\nТип исключения:{2}\nИсточник:{3}\nПроизошло:\n{4}"
                                    , expType, innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);
      SetData(sInner, ref newEx);
    }
    return newEx;
  }

  public static Exception CreateException(Type expType, Exception ex, string message,
                                          params object[] args) {
    string header = StringExtensions.SafeFormat(message, args);

    Exception newEx = null;
    try {
      if (String.IsNullOrEmpty(header)) {
        header = ex.Message;
      }
      else if (!header.IsEqual(ex.Message)) {
        SetData(ex.Message, ref ex);
      }
      object[] exArgs = new object[2] { header, ex };
      if (expType == typeof(ThreadAbortException)) {
        newEx = (Exception)Activator.CreateInstance(typeof(InnerThreadAbortException), exArgs);
      }
      else {
        newEx = (Exception)Activator.CreateInstance(expType, exArgs);
      }
    }
    catch (Exception innerEx) {
      newEx = new Exception(header, ex);
      string sInner =
        StringExtensions.SafeFormat("exception in CreateException of type {0}:\nИсключение: {1}\nТип исключения:{2}\nИсточник:{3}\nПроизошло:\n{4}"
                                    , expType, innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);
      SetData(sInner, ref newEx);
    }
    finally {
      try {
        foreach (var key in ex.Data.Keys) {
          var val = (string)ex.Data[key];
          if (!header.IsEqual(val)) {
            newEx.Data.Add(key, val);
          }
        }
      }
      catch (Exception) {

      }
    }
    return newEx;
  }

  #endregion

  #region Генерация нового исключения по переданному сообщению и возвращает объект исключение

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  public static T GetByDefault<T>(string message, params object[] args)
  where T : Exception, new() {
    T ex = new T();
    string s = StringExtensions.SafeFormat(message, args);

    Exception ex_ = (Exception)ex;
    SetData(s, ref ex_);

    WriteToLog(s, ref ex_);

    return ex;
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="header">заголовок исключения - сообщение</param>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  public static Exception Get<T>(string header, string message, params object[] args)
  where T : Exception, new() {
    if (String.IsNullOrEmpty(header)) {
      return GetByDefault<T>(message, args);
    }

    T ex = default(T);
    try {
      ex = (T)CreateException(typeof(T), header);
      string s = StringExtensions.SafeFormat(message, args);

      Exception ex_ = (Exception)ex;
      SetData(s, ref ex_);

      WriteToLog(s, ref ex_);
    }
    catch (Exception innerEx) {
      Exception ex__ = new Exception(header, ex);
      string sInner =
        StringExtensions.SafeFormat("exception in Run():\nИсключение: {0}\nТип исключения:{1}\nИсточник:{2}\nПроизошло:\n{3}"
                                    , innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);
      SetData(sInner, ref ex__);
      string s = StringExtensions.SafeFormat(message, args);

      Exception ex_ = ex__;
      SetData(s, ref ex_);

      WriteToLog(s, ref ex_);
      return ex__;
    }

    return ex;
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="header">заголовок исключения - сообщение</param>
  public static Exception Get<T>(string header) where T : Exception, new() {
    return Get<T>(header, String.Empty);
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  public static Exception Get<T>() where T : Exception, new() {
    return Get<T>(String.Empty);
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="header">заголовок исключения - сообщение</param>
  /// <param name="args">аргументы заголовка исключения - сообщения</param>
  public static Exception Get<T>(string header, params object[] args)
  where T : Exception, new() {
    string sheader = StringExtensions.SafeFormat(header, args);
    return Get<T>(sheader);
  }

  #endregion

  #region Генерация нового исключения по переданному сообщению

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  public static void RunByDefault<T>(string message, params object[] args)
  where T : Exception, new() {
    throw GetByDefault<T>(message, args);
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="header">заголовок исключения - сообщение</param>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  public static void Run<T>(string header, string message, params object[] args)
  where T : Exception, new() {
    throw Get<T>(header, message, args);
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="header">заголовок исключения - сообщение</param>
  public static void Run<T>(string header) where T : Exception, new() {
    Run<T>(header, String.Empty);
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  public static void Run<T>() where T : Exception, new() {
    Run<T>(String.Empty);
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="header">заголовок исключения - сообщение</param>
  /// <param name="args">аргументы заголовка исключения - сообщения</param>
  public static void Run<T>(string header, params object[] args)
  where T : Exception, new() {
    string sheader = StringExtensions.SafeFormat(header, args);
    Run<T>(sheader);
  }

  #endregion

  #region Возвращает новый объект исключение с новым типом по переданному исключению и сообщению

  /// <summary>
  /// Добавляет дополнительную информацию в исключение, сохраняет информацию лог и возвращает созданный объект исключение по переданному исключению
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="ex">внутреннее исключение</param>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  /// <returns>объект исключение</returns>
  public static Exception Run<T>(Exception ex, string message, params object[] args)
  where T : Exception, new() {
    string s = StringExtensions.SafeFormat(message, args);

    WriteToLog(s, ref ex);

    Exception newEx = CreateException(typeof(T), ex, s);
    return newEx;
  }

  /// <summary>
  /// Добавляет дополнительную информацию в исключение, сохраняет информацию лог и возвращает созданный объект исключение по переданному исключению
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="ex">внутреннее исключение</param>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  /// <returns>объект исключение</returns>
  public static Exception Run<T>(string header, string message, Exception ex, params object[] args)
  where T : Exception, new() {
    if (header == null) {
      header = String.Empty;
    }

    string s = StringExtensions.SafeFormat(message, args);
    SetData(s, ref ex);

    WriteToLog(s, ref ex);

    Exception newEx = CreateException(typeof(T), ex, header);
    return newEx;
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="header">заголовок исключения - сообщение</param>
  /// <param name="ex">внутреннее исключение</param>
  /// <param name="args">аргументы заголовка исключения - сообщения</param>
  public static Exception Run<T>(string header, Exception ex, params object[] args)
  where T : Exception, new() {
    string sheader = StringExtensions.SafeFormat(header, args);
    return Run<T>(sheader, ex);
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="ex">внутреннее исключение</param>
  public static Exception Run<T>(Exception ex) where T : Exception, new() {
    return Run<T>(ex, String.Empty);
  }

  /// <summary>
  /// Генерирует исключение
  /// </summary>
  /// <typeparam name="T">тип исключения</typeparam>
  /// <param name="header">заголовок исключения - сообщение</param>
  /// <param name="ex">внутреннее исключение</param>
  /// <returns>объект исключение</returns>
  public static Exception Run<T>(string header, Exception ex) where T : Exception, new() {
    return Run<T>(header, String.Empty, ex);
  }

  #endregion

  #region Возвращает объект исключение по переданному исключению и сообщению

  /// <summary>
  /// Добавляет дополнительную информацию в исключение, сохраняет информацию лог и возвращает созданный объект исключение по переданному исключению
  /// </summary>
  /// <param name="ex">внутреннее исключение</param>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  /// <returns>объект исключение</returns>
  public static Exception Run(Exception ex, string message, params object[] args) {
    string s = StringExtensions.SafeFormat(message, args);

    WriteToLog(s, ref ex);

    Exception newEx = CreateException(ex.GetType(), ex, s);
    return newEx;
  }

  /// <summary>
  /// Добавляет дополнительную информацию в исключение, сохраняет информацию лог и возвращает созданный объект исключение по переданному исключению
  /// </summary>

  /// <param name="ex">внутреннее исключение</param>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  /// <returns>объект исключение</returns>
  public static Exception Run(Exception ex) {
    return Run(ex, String.Empty);
  }

  /// <summary>
  /// Добавляет дополнительную информацию в исключение, возвращает созданный объект исключение по переданному исключению
  /// </summary>
  /// <param name="header">заголовок исключения</param>
  /// <param name="ex">внутреннее исключение</param>
  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  /// <returns>объект исключение</returns>
  public static Exception Run(string header, Exception ex, string message, params object[] args) {
    string s = StringExtensions.SafeFormat(message, args);

    SetData(s, ref ex);

    WriteToLog(s, ref ex);

    Exception newEx = CreateException(ex.GetType(), ex, header);
    return newEx;
  }

  /// <summary>
  /// Возвращает созданный объект исключение по переданному исключению
  /// </summary>
  /// <param name="header">заголовок исключения</param>
  /// <param name="ex">внутреннее исключение</param>
  /// <returns>объект исключение</returns>
  public static Exception Run(string header, Exception ex) {
    return Run(header, ex, String.Empty);
  }

  #endregion

  #region Генерация нового исключение заданного типа

  public static void RunArgumentOutOfRangeException(int index, int min, int max) {
    ExceptionGenerator.Run<ArgumentOutOfRangeException>("Индекс \"{0}\" должен быть не меньше \"{1}\" и меньше \"{2}\"",
        index, min, max);
  }

  public static void RunIndexOutOfRangeException(int index, int min, int max) {
    ExceptionGenerator.Run<IndexOutOfRangeException>("Индекс \"{0}\" должен быть не меньше \"{1}\" и меньше \"{2}\"",
        index, min, max);
  }

  #endregion

  /// <summary>
  /// Добавляет дополнительную информацию в исключение
  /// </summary>
  /// <param name="s">добавляемое сообщение</param>
  /// <param name="ex">внутреннее исключение</param>
  private static void SetData(string s, ref Exception ex) {
    if (String.IsNullOrEmpty(s)) {
      return;
    }

    if (ex.Data.Contains("info")) {
      string sKey = "info_0001";
      int count = 0;
      while (ex.Data.Contains(sKey)) {
        sKey = sKey.Increment();
        count++;
        if (count > 100) {
          sKey = "info_ex_0001";
          if (!ex.Data.Contains(sKey)) {
            ex.Data.Add(sKey, s);
            return;
          }
          // ничего не добавляет, так очень много информации
          return;
        }
      }
      ex.Data.Add(sKey, s);
    }
    else {
      ex.Data.Add("info", s);
    }
  }

  /// <summary>
  /// Записать в лог информацию об исключении
  /// </summary>

  /// <param name="message">сообщение</param>
  /// <param name="args">аргументы сообщения</param>
  private static void WriteToLog(string message, ref Exception ex) {
    try {
      DebugLogger.WriteError(message, ex);
    }
    catch (Exception innerEx) {
      string sData = StringExtensions.SafeFormat(
                       "exception in WriteToLog():\nИсключение: {0}\nТип исключения:{1}\nИсточник:{2}\nПроизошло:\n{3}",
                       innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace);

      if (!ex.Data.Contains("log_exception_info")) {
        ex.Data.Add("log_exception_info", sData);
      }
    }
  }
}
}
