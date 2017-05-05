using System;
using System.Diagnostics;
using System.Reflection;

using EG.Misc;

namespace EG {

/// <summary>
/// Зоны логгирования
/// </summary>
[Flags]
public enum LogZone {
  /// <summary>
  /// Утверждения. В идеале никто и никогда не должен их видеть !!
  /// </summary>
  Assert = 1,
  /// <summary>
  /// Критические сообщения.
  /// </summary>
  Critical = 2,
  /// <summary>
  /// Ошибки и сбои в работе.
  /// </summary>
  Error = 4,
  /// <summary>
  /// Предупреждения, не желательное поведение в работе.
  /// </summary>
  Warning = 8,
  /// <summary>
  /// Информационные сообщения.
  /// </summary>
  Info = 16,
  /// <summary>
  /// Дополнительные сведения, идут в дополнения к информационным сообщениям.
  /// </summary>
  Verbose = 32,
  /// <summary>
  /// Трассировка данных.
  /// </summary>
  Trace = 64,
}

public class DebugLogger {

  /// <summary>
  /// Включить зону логгирования.
  /// </summary>
  /// <param name="zone">Зона логгирования.</param>
  public static void EnableZone(LogZone zone) {
    _enableZones |= (int) zone;
  }

  /// <summary>
  /// Установить зоны логгирования по переданной маске.
  /// </summary>
  /// <param name="enableZones">Разрешенные для логгирования зоны.</param>
  public static void SetAvailableZones(int enableZones) {
    var allZones = 0;
    foreach (var zone in Enum.GetValues(typeof(LogZone))) {
      allZones |= (int)zone;
    }

    _enableZones = enableZones & allZones;
  }

  /// <summary>
  /// Определяет используется ли указанная зона логгирования.
  /// </summary>
  /// <param name="zone">Зона логгирования.</param>
  /// <returns>
  /// <c>true</c> если зона включена; иначе, <c>false</c>.
  /// </returns>
  public static bool IsZoneEnable(LogZone zone) {
    return (_enableZones & (int) zone) > 0;
  }

  /// <summary>
  /// Выключить зону логгирования.
  /// </summary>
  /// <param name="zone">Зона логгирования.</param>
  public static void DisableZone(LogZone zone) {
    _enableZones &= ~(int) zone;
  }

  /// <summary>
  /// Записать лог-сообщения.
  /// </summary>
  /// <param name="zone">Зона логгирования.</param>
  /// <param name="message">Текст форматируемого сообщения.</param>
  /// <param name="args">Дополнительные аргументы для форматируемого сообщения.</param>
  public static void WriteMessage(LogZone zone, string message,
                                  params object[] args) {
    if (IsZoneEnable(zone) == false) {
      return;
    }

    UnityEngine.Debug.Log(String.Format("{0}: {1}: {2}", DateTime.Now.ToLongTimeString(), zone,
                                        StringExtensions.SafeFormat(message, args)));
  }

  /// <summary>
  /// Записать лог-сообщения в расширенную информационную зону ошибок.
  /// </summary>
  /// <param name="areaCode">Область регистрируемой записи.</param>
  /// <param name="message">Текст расширенного информационного сообщения.</param>
  /// <param name="args">Дополнительные аргументы для форматируемого сообщения.</param>
  [Conditional("TRACE")]
  public static void WriteVerbose(string message, params object[] args) {
    WriteMessage(LogZone.Verbose, message, args);
  }


  /// <summary>
  /// Записать лог-сообщения в стандартную информационную зону.
  /// </summary>
  /// <param name="message">Текст сообщения.</param>
  /// <param name="args">Дополнительные аргументы для форматируемого сообщения.</param>
  public static void WriteInfo(string message, params object[] args) {
    WriteMessage(LogZone.Info, message, args);
  }

  /// <summary>
  /// Записать лог-сообщения в информационную зону предупреждений.
  /// </summary>
  /// <param name="message">Текст предупреждения.</param>
  /// <param name="args">Дополнительные аргументы для форматируемого сообщения.</param>
  public static void WriteWarning(string message, params object[] args) {
    WriteMessage(LogZone.Warning, message, args);
  }

  /// <summary>
  /// Записать лог-сообщения в информационную зону ошибок.
  /// </summary>
  /// <param name="message">Текст ошибки.</param>
  /// <param name="args">Дополнительные аргументы для форматируемого сообщения.</param>
  public static void WriteError(string message, params object[] args) {
    WriteMessage(LogZone.Error, message, args);
  }

  /// <summary>
  /// Записать лог-сообщения в информационную зону ошибок.
  /// </summary>
  /// <param name="message">Текст ошибки.</param>
  /// <param name="exception">Исключение.</param>
  /// <param name="args">Дополнительные аргументы для форматируемого сообщения.</param>
  public static void WriteError(string message, Exception exp, params object[] args) {
    string sData = String.Empty;
    foreach (var key in exp.Data.Keys) {
      sData += "\n\tKey: \"" + key + "\", Value: \"" + exp.Data[key] + "\"; ";
    }
    var text =
      StringExtensions.SafeFormat("{0}\nТип исключения: {1}\nИсточник: {2}\nДополнительные сведения: {3}\nСтек вызовов: {4}\nДанные: {5}{6}\n",
                                  message, exp.GetType(), exp.Source, exp.Message, exp.StackTrace, sData, exp.GetInnerMessage());
    WriteMessage(LogZone.Error, text, args);
  }

  /// <summary>
  /// Записать лог-сообщения в информационную зону ошибок.
  /// </summary>
  /// <param name="message">Текст ошибки.</param>
  /// <param name="exception">Исключение.</param>
  /// <param name="args">Дополнительные аргументы для форматируемого сообщения.</param>
  public static void WriteError(Exception exp) {
    string sData = String.Empty;
    foreach (var key in exp.Data.Keys) {
      sData += "\n\tKey: \"" + key + "\", Value: \"" + exp.Data[key] + "\"; ";
    }
    var text =
      StringExtensions.SafeFormat("{0}\nТип исключения: {1}\nИсточник: {2}\nСтек вызовов: {3}\nДанные: {4}{5}\n",
                                  exp.Message, exp.GetType(), exp.Source, exp.StackTrace, sData, exp.GetInnerMessage());
    WriteMessage(LogZone.Error, text);
  }

  /// <summary>
  /// Получить имя вызвавшего метода.
  /// </summary>
  /// <param name="stackTraceFrameUp">Насколько кадров подниматься вверх по стеку.</param>
  /// <returns>Имя родительского метода</returns>
  private static string GetParentMethodName(int stackTraceFrameUp) {
    var stackTrace = new StackTrace();
    StackFrame stackFrame = stackTrace.GetFrame(stackTraceFrameUp);
    MethodBase methodBase = stackFrame.GetMethod();
    return methodBase.DeclaringType != null ? methodBase.DeclaringType.FullName + "." +
           methodBase.Name : "." + methodBase.Name;
  }

  /// <summary>
  /// Зоны, разрешенные для трассировки (по умолчанию).
  /// </summary>
  private static int _enableZones = (int)LogZone.Critical + (int)LogZone.Error +
                                    (int)LogZone.Warning + (int)LogZone.Info;
}
}