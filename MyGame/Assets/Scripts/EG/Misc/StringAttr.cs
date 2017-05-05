using System;

namespace EG.Misc {
/// <summary>
/// Класс - атрибут строки (формат атрибут строки <имя атрибута><разделитель><левая граница значения><значение><правая граница значения>
/// <левая граница значения> и <правая граница значения> - необязательные
/// </summary>
[Serializable]
public sealed class StringAttr {
  /// <summary>
  /// Имя атрибута
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Индекс атрибута в строке
  /// </summary>
  public int StartIndex { get; set; }

  /// <summary>
  /// Значение атрибута
  /// </summary>
  public string Value { get; set; }

  /// <summary>
  /// Индекс правой границы значения (или индекс окончания атрибута, если неопределена правая граница)
  /// </summary>
  public int EndIndex { get; set; }

  /// <summary>
  /// Разделитель между значением и именем атрибута
  /// </summary>
  public string Separator { get; set; }

  /// <summary>
  /// Левая граница
  /// </summary>
  public string LeftBorder { get; set; }

  /// <summary>
  /// Правая граница
  /// </summary>
  public string RightBorder { get; set; }

  /// <summary>
  /// Индекс значения атрибута в строке
  /// </summary>
  public int Index {
    get { return StartIndex + Name.Length + Separator.Length + LeftBorder.Length; }
  }

  /// <summary>
  /// Длина атрибута (все данные)
  /// </summary>
  public int Length {
    get { return EndIndex + RightBorder.Length - StartIndex; }
  }

  /// <summary>
  /// <c>true</c>, значение атрибута пустое
  /// </summary>
  public bool IsEmpty {
    get { return String.IsNullOrEmpty(Value); }
  }

  /// <summary>
  /// Текст атрибута (имя атрибута, разделитель, значение и границы значения)
  /// </summary>
  public string Text {
    get { return String.Format("{0}{1}{2}{3}{4}", Name, Separator, LeftBorder, Value, RightBorder); }
  }

  /// <summary>
  /// Явное преобразование атрибута в строку (берется только значение атрибута)
  /// </summary>
  /// <param name="attr">атрибут</param>
  /// <returns>значение атрибута</returns>
  public static implicit operator string(StringAttr attr) {
    return attr.Value;
  }
}
}
