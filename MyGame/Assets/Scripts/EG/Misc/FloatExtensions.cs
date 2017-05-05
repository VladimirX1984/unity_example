using System;

namespace EG.Misc {
/// <summary>
///     Класс-расширение для добавления дополнительных методов структуре float
/// </summary>
public static class FloatExtensions {
  /// <summary>
  /// Порог для сравнения 2 чисел
  /// </summary>
  private const float CompareThreshold = 0.0001f;

  /// <summary>
  /// Определяем равно ли (~)данное значение переданному аргументу
  /// </summary>
  /// <param name="compareValue1">Первое число для сравнения.</param>
  /// <param name="compareValue2">Первое число для сравнения.</param>
  /// <returns>
  /// <c>true</c> если числа равные; иначе, <c>false</c>.
  /// </returns>
  public static bool IsEqual(this float compareValue1, float compareValue2) {
    var absValue = Math.Abs(compareValue1 - compareValue2);
    return absValue < CompareThreshold;
  }

  /// <summary>
  /// Определяем равно ли (~)данное значение переданному аргументу
  /// </summary>
  /// <param name="compareValue1">Первое число для сравнения.</param>
  /// <param name="compareValue2">Второе число для сравнения.</param>
  /// <param name="threshold">Порог для сравнения.</param>
  /// <returns>
  /// <c>true</c> если числа равные; иначе, <c>false</c>.
  /// </returns>
  public static bool IsEqual(this float compareValue1, float compareValue2, float threshold) {
    var absValue = Math.Abs(compareValue1 - compareValue2);
    return absValue < threshold;
  }

  public static bool IsNotEqual(this float compareValue1, float compareValue2) {
    return compareValue1.IsEqual(compareValue2) == false;
  }

  public static bool IsNotEqual(this float compareValue1, float compareValue2, float threshold) {
    return compareValue1.IsEqual(compareValue2, threshold) == false;
  }

  /// <summary>
  /// Определяем больше ли (~)данное значение переданному аргументу
  /// </summary>
  /// <param name="compareValue1">Первое число для сравнения.</param>
  /// <param name="compareValue2">Первое число для сравнения.</param>
  /// <returns>
  /// <c>true</c> если 1 число больше; иначе, <c>false</c>.
  /// </returns>
  public static bool IsGreater(this float compareValue1, float compareValue2) {
    return compareValue1 > compareValue2 && !IsEqual(compareValue1, compareValue2);
  }

  /// <summary>
  /// Определяем больше ли (~)данное значение переданному аргументу
  /// </summary>
  /// <param name="compareValue1">Первое число для сравнения.</param>
  /// <param name="compareValue2">Первое число для сравнения.</param>
  /// <param name="threshold">Порог для сравнения.</param>
  /// <returns>
  /// <c>true</c> если 1 число больше; иначе, <c>false</c>.
  /// </returns>
  public static bool IsGreater(this float compareValue1, float compareValue2, float threshold) {
    return compareValue1 > compareValue2 && !IsEqual(compareValue1, compareValue2, threshold);
  }

  /// <summary>
  /// Определяем меньше ли (~)данное значение переданному аргументу
  /// </summary>
  /// <param name="compareValue1">Первое число для сравнения.</param>
  /// <param name="compareValue2">Первое число для сравнения.</param>
  /// <returns>
  /// <c>true</c> если 1 число меньше; иначе, <c>false</c>.
  /// </returns>
  public static bool IsLess(this float compareValue1, float compareValue2) {
    return compareValue1 < compareValue2 && !IsEqual(compareValue1, compareValue2);
  }

  /// <summary>
  /// Определяем меньше ли (~)данное значение переданному аргументу
  /// </summary>
  /// <param name="compareValue1">Первое число для сравнения.</param>
  /// <param name="compareValue2">Первое число для сравнения.</param>
  /// <param name="threshold">Порог для сравнения.</param>
  /// <returns>
  /// <c>true</c> если 1 число меньше; иначе, <c>false</c>.
  /// </returns>
  public static bool IsLess(this float compareValue1, float compareValue2, float threshold) {
    return compareValue1 < compareValue2 && !IsEqual(compareValue1, compareValue2, threshold);
  }

  /// <summary>
  /// Преобразовывает числовое значение данного экземпляра в эквивалентное ему строковое представление с заданной точностью с отбрасыванием "пустых" нулей в конце числа
  /// </summary>
  /// <param name="value">значение</param>
  /// <param name="numberDecimalDigits">число цифр после запятой</param>
  /// <returns></returns>
  public static string ToStr(this float value, int numberDecimalDigits) {
    if (numberDecimalDigits < 0 || numberDecimalDigits > 99) {
      ExceptionGenerator.Run<ArgumentOutOfRangeException>("numberDecimalDigits = \"{1}\"",
          numberDecimalDigits);
    }

    return value.ToStr(0, numberDecimalDigits);
  }

  /// <summary>
  /// Преобразовывает числовое значение данного экземпляра в эквивалентное ему строковое представление с заданной точностью с отбрасыванием "пустых" нулей в конце числа
  /// </summary>
  /// <param name="value">значение</param>
  /// <param name="minNumberDecimalDigits">минимальное число цифр после запятой</param>
  /// <param name="numberDecimalDigits">число цифр после запятой</param>
  /// <returns></returns>
  public static string ToStr(this float value, int minNumberDecimalDigits, int numberDecimalDigits) {
    if (minNumberDecimalDigits < 0 || minNumberDecimalDigits > 99 || numberDecimalDigits < 0 ||
        numberDecimalDigits > 99) {
      ExceptionGenerator.Run<ArgumentOutOfRangeException>("minNumberDecimalDigits \"{0}\", numberDecimalDigits = \"{1}\"",
          minNumberDecimalDigits,
          numberDecimalDigits);
    }

    if (minNumberDecimalDigits > numberDecimalDigits) {
      ExceptionGenerator.Run<ArgumentOutOfRangeException>("minNumberDecimalDigits \"{0}\" должно быть не больше, чем numberDecimalDigits = \"{1}\"",
          minNumberDecimalDigits, numberDecimalDigits);
    }

    if (value.IsEqual((float)Math.Floor(value))) {
      return value.ToString(String.Format("F{0}", minNumberDecimalDigits));
    }

    var temp_value = value;
    for (int i = minNumberDecimalDigits + 1; i < numberDecimalDigits; ++i) {
      temp_value = 10.0f * temp_value;
      if (temp_value.IsEqual((float)Math.Floor(temp_value))) {
        return value.ToString(String.Format("F{0}", i));
      }
    }

    return value.ToString(String.Format("F{0}", numberDecimalDigits)); ;
  }

  /// <summary>
  /// Преобразует числовое значение данного экземпляра в эквивалентное ему числовое значение двойной точности
  /// </summary>
  /// <param name="value">значение</param>
  /// <param name="numberDecimalDigits">число цифр после запятой</param>
  /// <returns></returns>
  public static double ToDouble(this float value, int numberDecimalDigits) {
    return value.ToDouble(0, numberDecimalDigits);
  }

  /// <summary>
  /// Преобразует числовое значение данного экземпляра в эквивалентное ему числовое значение двойной точности
  /// </summary>
  /// <param name="value">значение</param>
  /// <param name="minNumberDecimalDigits">минимальное число цифр после запятой</param>
  /// <param name="numberDecimalDigits">число цифр после запятой</param>
  /// <returns></returns>
  public static double ToDouble(this float value, int minNumberDecimalDigits,
                                int numberDecimalDigits) {
    string s = value.ToStr(minNumberDecimalDigits, numberDecimalDigits);
    return Convert.ToDouble(s);
  }

  public static bool IsInteger(this float value) {
    return value.IsEqual((float)Math.Floor(value));
  }

  public static bool IsZero(this float value) {
    return value.IsEqual(0.0f);
  }
}
}
