using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

using EG.Misc;

namespace EG.Calc {
  public static class MathEx {

    #region Преобразование слогов в IntPtr

    public static class IntPtrParse {
      private static int GetUnchecked(IntPtr value) {
        return IntPtr.Size == 8 ? unchecked((int)value.ToInt64()) : value.ToInt32();
      }

      public static int GetLow(IntPtr value) {
        return unchecked((short)GetUnchecked(value));
      }

      public static int GetHigh(IntPtr value) {
        return unchecked((short)(((uint)GetUnchecked(value)) >> 16));
      }
    }

    #endregion

    #region Точное сравнение чисел

    public static int Compare(double val1, double val2) {
      return val1 > val2 ? 1 : (val1 < val2 ? -1 : 0);
    }

    public static int Compare(float val1, float val2) {
      return val1 > val2 ? 1 : (val1 < val2 ? -1 : 0);
    }

    public static int Compare<T>(object val1, object val2) {
      if (typeof(T) == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        return Compare(val1_, val2_);
      }
      if (typeof(T) == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        return Compare(val1_, val2_);
      }
      if (typeof(T) == typeof(byte)) {
        byte val1_ = Convert.ToByte(val1);
        byte val2_ = Convert.ToByte(val2);
        return val1_.CompareTo(val2_);
      }
      if (typeof(T) == typeof(sbyte)) {
        sbyte val1_ = Convert.ToSByte(val1);
        sbyte val2_ = Convert.ToSByte(val2);
        return val1_.CompareTo(val2_);
      }
      if (typeof(T) == typeof(short)) {
        short val1_ = Convert.ToInt16(val1);
        short val2_ = Convert.ToInt16(val2);
        return val1_.CompareTo(val2_);
      }
      if (typeof(T) == typeof(ushort)) {
        ushort val1_ = Convert.ToUInt16(val1);
        ushort val2_ = Convert.ToUInt16(val2);
        return val1_.CompareTo(val2_);
      }
      if (typeof(T) == typeof(int)) {
        int val1_ = Convert.ToInt32(val1);
        int val2_ = Convert.ToInt32(val2);
        return val1_.CompareTo(val2_);
      }
      if (typeof(T) == typeof(uint)) {
        uint val1_ = Convert.ToUInt32(val1);
        uint val2_ = Convert.ToUInt32(val2);
        return val1_.CompareTo(val2_);
      }
      if (typeof(T) == typeof(long)) {
        long val1_ = Convert.ToInt64(val1);
        long val2_ = Convert.ToInt64(val2);
        return val1_.CompareTo(val2_);
      }
      if (typeof(T) == typeof(ulong)) {
        ulong val1_ = Convert.ToUInt64(val1);
        ulong val2_ = Convert.ToUInt64(val2);
        return val1_.CompareTo(val2_);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return 0;
    }

    #endregion

    #region Сравнение чисел c определенной точностью

    public static bool IsEqual(double val1, double val2) {
      return val1.IsEqual(val2);
    }

    public static bool IsEqual(float val1, float val2) {
      return val1.IsEqual(val2);
    }

    public static bool IsEqual<T>(object val1, object val2) {
      if (typeof(T) == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        return IsEqual(val1_, val2_);
      }
      if (typeof(T) == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        return IsEqual(val1_, val2_);
      }

      try {
        object val1_ = Convert.ChangeType(val1, typeof(T));
        object val2_ = Convert.ChangeType(val2, typeof(T));
        return val1_.IsEquals(val2_);
      }
      catch (Exception ex) {
        throw ExceptionGenerator.Run<InvalidCastException>(ex,
            "Тип [{0}] не корректный", typeof(T).ToString());
      }
    }

    public static bool IsEqual(double val1, double val2, double threshold) {
      return val1.IsEqual(val2, threshold);
    }

    public static bool IsEqual(float val1, float val2, float threshold) {
      return val1.IsEqual(val2, threshold);
    }

    public static bool IsEqual<T>(object val1, object val2, object threshold) {
      if (typeof(T) == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        double threshold_ = Convert.ToDouble(threshold);
        return IsEqual(val1_, val2_, threshold_);
      }
      if (typeof(T) == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        float threshold_ = Convert.ToSingle(threshold);
        return IsEqual(val1_, val2_, threshold_);
      }

      try {
        object val1_ = Convert.ChangeType(val1, typeof(T));
        object val2_ = Convert.ChangeType(val2, typeof(T));
        return val1_.IsEquals(val2_);
      }
      catch (Exception ex) {
        throw ExceptionGenerator.Run<InvalidCastException>(ex, "Тип [{0}] не корректный",
            typeof(T).ToString());
      }
    }

    public static bool IsGreater(object val1, object val2) {
      Type type1 = val1.GetType();
      Type type2 = val2.GetType();

      if (type1 != type2) {
        ExceptionGenerator.Run<InvalidCastException>("Типы [{0} и {1}] данных должны быть одинаковы",
            type1.ToString(), type2.ToString());
      }

      if (type1 == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        return val1_.IsGreater(val2_);
      }
      if (type1 == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        return val1_.IsGreater(val2_);
      }
      if (type1 == typeof(sbyte)) {
        sbyte val1_ = Convert.ToSByte(val1);
        sbyte val2_ = Convert.ToSByte(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(byte)) {
        byte val1_ = Convert.ToByte(val1);
        byte val2_ = Convert.ToByte(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(short)) {
        short val1_ = Convert.ToInt16(val1);
        short val2_ = Convert.ToInt16(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(ushort)) {
        ushort val1_ = Convert.ToUInt16(val1);
        ushort val2_ = Convert.ToUInt16(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(int)) {
        int val1_ = Convert.ToInt32(val1);
        int val2_ = Convert.ToInt32(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(uint)) {
        uint val1_ = Convert.ToUInt32(val1);
        uint val2_ = Convert.ToUInt32(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(long)) {
        long val1_ = Convert.ToInt64(val1);
        long val2_ = Convert.ToInt64(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(ulong)) {
        ulong val1_ = Convert.ToUInt64(val1);
        ulong val2_ = Convert.ToUInt64(val2);
        return val1_ > val2_;
      }

      ExceptionGenerator.Run<InvalidCastException>("Тип [{0}] не корректный",
          type1.ToString());
      return false;
    }

    public static bool IsLess(object val1, object val2) {
      return IsGreater(val2, val1);
    }

    public static bool IsGreaterOrEqual(object val1, object val2) {
      Type type1 = val1.GetType();
      Type type2 = val2.GetType();

      if (type1 != type2) {
        ExceptionGenerator.Run<InvalidCastException>("Типы [{0} и {1}] данных должны быть одинаковы",
            type1.ToString(), type2.ToString());
      }

      if (type1 == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(int)) {
        int val1_ = Convert.ToInt32(val1);
        int val2_ = Convert.ToInt32(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(uint)) {
        uint val1_ = Convert.ToUInt32(val1);
        uint val2_ = Convert.ToUInt32(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(long)) {
        long val1_ = Convert.ToInt64(val1);
        long val2_ = Convert.ToInt64(val2);
        return val1_ > val2_;
      }
      if (type1 == typeof(ulong)) {
        ulong val1_ = Convert.ToUInt64(val1);
        ulong val2_ = Convert.ToUInt64(val2);
        return val1_ > val2_;
      }

      ExceptionGenerator.Run<InvalidCastException>("Тип [{0}] не корректный",
          type1.ToString());
      return false;
    }

    public static bool IsLessOrEqual(object val1, object val2) {
      return IsGreaterOrEqual(val2, val1);
    }

    #endregion

    #region Сложение, вычитание, умножение, деление чисел

    public static T Add<T>(object val1, object val2) {
      if (typeof(T) == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(byte)) {
        byte val1_ = Convert.ToByte(val1);
        byte val2_ = Convert.ToByte(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(sbyte)) {
        sbyte val1_ = Convert.ToSByte(val1);
        sbyte val2_ = Convert.ToSByte(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(short)) {
        short val1_ = Convert.ToInt16(val1);
        short val2_ = Convert.ToInt16(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(ushort)) {
        ushort val1_ = Convert.ToUInt16(val1);
        ushort val2_ = Convert.ToUInt16(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(int)) {
        int val1_ = Convert.ToInt32(val1);
        int val2_ = Convert.ToInt32(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(uint)) {
        uint val1_ = Convert.ToUInt32(val1);
        uint val2_ = Convert.ToUInt32(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(long)) {
        long val1_ = Convert.ToInt64(val1);
        long val2_ = Convert.ToInt64(val2);
        return (T)(object)(val1_ + val2_);
      }
      if (typeof(T) == typeof(ulong)) {
        ulong val1_ = Convert.ToUInt64(val1);
        ulong val2_ = Convert.ToUInt64(val2);
        return (T)(object)(val1_ + val2_);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return default(T);
    }

    public static T Add<T>(T val1, T val2) {
      return Add<T>((object)val1, (object)val2);
    }

    public static T Subtract<T>(object val1, object val2) {
      if (typeof(T) == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(byte)) {
        byte val1_ = Convert.ToByte(val1);
        byte val2_ = Convert.ToByte(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(sbyte)) {
        sbyte val1_ = Convert.ToSByte(val1);
        sbyte val2_ = Convert.ToSByte(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(short)) {
        short val1_ = Convert.ToInt16(val1);
        short val2_ = Convert.ToInt16(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(ushort)) {
        ushort val1_ = Convert.ToUInt16(val1);
        ushort val2_ = Convert.ToUInt16(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(int)) {
        int val1_ = Convert.ToInt32(val1);
        int val2_ = Convert.ToInt32(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(uint)) {
        uint val1_ = Convert.ToUInt32(val1);
        uint val2_ = Convert.ToUInt32(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(long)) {
        long val1_ = Convert.ToInt64(val1);
        long val2_ = Convert.ToInt64(val2);
        return (T)(object)(val1_ - val2_);
      }
      if (typeof(T) == typeof(ulong)) {
        ulong val1_ = Convert.ToUInt64(val1);
        ulong val2_ = Convert.ToUInt64(val2);
        return (T)(object)(val1_ - val2_);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return default(T);
    }

    public static T Subtract<T>(T val1, T val2) {
      return Subtract<T>((object)val1, (object)val2);
    }

    public static T Multiply<T>(object val1, object val2) {
      if (typeof(T) == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(byte)) {
        byte val1_ = Convert.ToByte(val1);
        byte val2_ = Convert.ToByte(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(sbyte)) {
        sbyte val1_ = Convert.ToSByte(val1);
        sbyte val2_ = Convert.ToSByte(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(short)) {
        short val1_ = Convert.ToInt16(val1);
        short val2_ = Convert.ToInt16(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(ushort)) {
        ushort val1_ = Convert.ToUInt16(val1);
        ushort val2_ = Convert.ToUInt16(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(int)) {
        int val1_ = Convert.ToInt32(val1);
        int val2_ = Convert.ToInt32(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(uint)) {
        uint val1_ = Convert.ToUInt32(val1);
        uint val2_ = Convert.ToUInt32(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(long)) {
        long val1_ = Convert.ToInt64(val1);
        long val2_ = Convert.ToInt64(val2);
        return (T)(object)(val1_ * val2_);
      }
      if (typeof(T) == typeof(ulong)) {
        ulong val1_ = Convert.ToUInt64(val1);
        ulong val2_ = Convert.ToUInt64(val2);
        return (T)(object)(val1_ * val2_);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return default(T);
    }

    public static T Multiply<T>(T val1, T val2) {
      return Multiply<T>((object)val1, (object)val2);
    }

    public static T Divide<T>(object val1, object val2) {
      if (typeof(T) == typeof(double)) {
        double val1_ = Convert.ToDouble(val1);
        double val2_ = Convert.ToDouble(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(float)) {
        float val1_ = Convert.ToSingle(val1);
        float val2_ = Convert.ToSingle(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(int)) {
        int val1_ = Convert.ToInt32(val1);
        int val2_ = Convert.ToInt32(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(byte)) {
        byte val1_ = Convert.ToByte(val1);
        byte val2_ = Convert.ToByte(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(sbyte)) {
        sbyte val1_ = Convert.ToSByte(val1);
        sbyte val2_ = Convert.ToSByte(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(short)) {
        short val1_ = Convert.ToInt16(val1);
        short val2_ = Convert.ToInt16(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(ushort)) {
        ushort val1_ = Convert.ToUInt16(val1);
        ushort val2_ = Convert.ToUInt16(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(int)) {
        int val1_ = Convert.ToInt32(val1);
        int val2_ = Convert.ToInt32(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(uint)) {
        uint val1_ = Convert.ToUInt32(val1);
        uint val2_ = Convert.ToUInt32(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(long)) {
        long val1_ = Convert.ToInt64(val1);
        long val2_ = Convert.ToInt64(val2);
        return (T)(object)(val1_ / val2_);
      }
      if (typeof(T) == typeof(ulong)) {
        ulong val1_ = Convert.ToUInt64(val1);
        ulong val2_ = Convert.ToUInt64(val2);
        return (T)(object)(val1_ / val2_);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return default(T);
    }

    public static T Divide<T>(T val1, T val2) {
      return Divide<T>((object)val1, (object)val2);
    }

    public static T Abs<T>(object val) {
      if (typeof(T) == typeof(double)) {
        double val_ = Convert.ToDouble(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(float)) {
        float val_ = Convert.ToSingle(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(byte)) {
        byte val_ = Convert.ToByte(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(sbyte)) {
        sbyte val_ = Convert.ToSByte(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(short)) {
        short val_ = Convert.ToInt16(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(ushort)) {
        ushort val_ = Convert.ToUInt16(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(int)) {
        int val_ = Convert.ToInt32(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(uint)) {
        uint val_ = Convert.ToUInt32(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(long)) {
        long val_ = Convert.ToInt64(val);
        return (T)(object)Math.Abs(val_);
      }
      if (typeof(T) == typeof(ulong)) {
        ulong val_ = Convert.ToUInt64(val);
        return (T)(object)Math.Abs((float)val_);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return default(T);
    }

    public static T Abs<T>(T val) {
      return Abs<T>((object)val);
    }

    #endregion

    #region Преобразование градусов в радианы и наоборот

    public static double ToRadian(double angle, int digits = -1) {
      if (digits == -1) {
        return angle * Math.PI / 180.0;
      }
      return Math.Round(angle * Math.PI / 180.0, digits);
    }

    public static float ToRadian(float angle, int digits = -1) {
      if (digits == -1) {
        return angle * (float)Math.PI / 180.0f;
      }
      return (float)Math.Round(angle * (float)Math.PI / 180.0f, digits);
    }

    public static double ToDegree(double angle, int digits = -1) {
      if (digits == -1) {
        return angle * 180.0f / Math.PI;
      }
      return Math.Round(angle * 180.0f / Math.PI, digits);
    }

    public static float ToDegree(float angle, int digits = -1) {
      if (digits == -1) {
        return angle * 180.0f / (float)Math.PI;
      }
      return (float)Math.Round(angle * 180.0f / (float)Math.PI, digits);
    }

    #endregion

    #region Интерполяция

    /// <summary>
    /// Равноускоренная
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double InterpExp(double value) {
      return (1.0 + Math.Sin((-Math.PI / 2.0 + value * Math.PI / 2.0)));
    }

    /// <summary>
    /// Равнозамедленная
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double InterpInvExp(double value) {
      return (Math.Sin(value * Math.PI / 2.0));
    }

    /// <summary>
    /// Плавная
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double InterpSmothed(double value) {
      return ((1.0 + Math.Sin(-Math.PI / 2.0 + value * Math.PI)) / 2.0);
    }

    #endregion

    #region Работа с округлением, получение числа знаков дробной части для округления, получение числа знаков целой части

    public static object Round<T>(object inValue, int digits = 0) {
      if (typeof(T) == typeof(float)) {
        return (float)Math.Round(Convert.ToSingle(inValue), digits);
      }

      if (typeof(T) == typeof(double)) {
        return Math.Round(Convert.ToDouble(inValue), digits);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return null;
    }

    public static object Ceiling<T>(T inValue) {
      if (typeof(T) == typeof(float)) {
        return (float)Math.Ceiling(Convert.ToDouble(inValue));
      }

      if (typeof(T) == typeof(double)) {
        return Math.Ceiling(Convert.ToDouble(inValue));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return null;
    }

    public static object Floor<T>(T inValue) {
      if (typeof(T) == typeof(float)) {
        return (float)Math.Floor(Convert.ToDouble(inValue));
      }

      if (typeof(T) == typeof(double)) {
        return Math.Floor(Convert.ToDouble(inValue));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return null;
    }

    /// <summary>
    /// Возвращает минимальное число знаков дробной части числа для округления, например, если число 0.068, то метод возвратить число 2
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetMinFractionDigitsNumber(double value) {
      if (value == 0.0) {
        return 0;
      }
      int digits = -(int)Math.Floor(Math.Log10(value));
      if (digits < 0) {
        digits = 0;
      }
      return digits;
    }

    /// <summary>
    /// Возвращает минимальное число знаков дробной части числа для округления, например, если число 0.068, то метод возвратить число 2
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetMinFractionDigitsNumber(float value) {
      if (value == 0.0f) {
        return 0;
      }
      int digits = -(int)Math.Floor(Math.Log10(value));
      if (digits < 0) {
        digits = 0;
      }
      return digits;
    }

    /// <summary>
    /// Возвращает минимальное число знаков дробной части числа для округления, например, если число 0.068, то метод возвратить число 2
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetMinFractionDigitsNumber<T>(object value) {
      var type = typeof(T);
      if (_integerTypes.Exists(it => it == type)) {
        return 0;
      }

      if (typeof(T) == typeof(double)) {
        return GetMinFractionDigitsNumber(Convert.ToDouble(value));
      }

      if (typeof(T) == typeof(float)) {
        return GetMinFractionDigitsNumber(Convert.ToSingle(value));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return 0;
    }

    /// <summary>
    /// Возвращает минимальное число знаков дробной части числа для округления, например, если число 0.068, то метод возвратить число 1
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetLessMinFractionDigitsNumber(double value) {
      if (value == 0.0) {
        return 0;
      }
      int digits = -(int)Math.Ceiling(Math.Log10(value));
      if (digits < 0) {
        digits = 0;
      }
      return digits;
    }

    /// <summary>
    /// Возвращает минимальное число знаков дробной части числа для округления, например, если число 0.068, то метод возвратить число 1
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetLessMinFractionDigitsNumber(float value) {
      if (value == 0.0f) {
        return 0;
      }
      int digits = -(int)Math.Ceiling(Math.Log10(value));
      if (digits < 0) {
        digits = 0;
      }
      return digits;
    }

    /// <summary>
    /// Возвращает минимальное число знаков дробной части числа для округления, например, если число 0.068, то метод возвратить число 1
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetLessMinFractionDigitsNumber<T>(object value) {
      var type = typeof(T);
      if (_integerTypes.Exists(it => it == type)) {
        return 0;
      }

      if (typeof(T) == typeof(double)) {
        return GetLessMinFractionDigitsNumber(Convert.ToDouble(value));
      }

      if (typeof(T) == typeof(float)) {
        return GetLessMinFractionDigitsNumber(Convert.ToSingle(value));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return 0;
    }

    /// <summary>
    /// Возвращает число знаков целой части числа например, если число 201, то метод возвратить число 3
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetIntegerDigitsNumber(int value) {
      if (value == 0) {
        return 0;
      }
      if (value == 1) {
        return 1;
      }
      double ddigits = Math.Log10(value);
      if (ddigits == (int)ddigits) {
        return (int)ddigits + 1;
      }
      int digits = (int)Math.Ceiling(ddigits);
      if (digits < 0) {
        digits = 0;
      }
      return digits;
    }

    /// <summary>
    /// Возвращает число знаков целой части числа например, если число 201, то метод возвратить число 3
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetIntegerDigitsNumber(double value) {
      if (value.IsEqual(0.0)) {
        return 0;
      }
      if (value.IsEqual(1.0)) {
        return 1;
      }
      double ddigits = Math.Log10(value);
      if (ddigits == (int)ddigits) {
        return (int)ddigits + 1;
      }
      int digits = (int)Math.Ceiling(ddigits);
      if (digits < 0) {
        digits = 0;
      }
      return digits;
    }

    /// <summary>
    /// Возвращает число знаков целой части числа например, если число 201, то метод возвратить число 3
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetIntegerDigitsNumber(float value) {
      if (value.IsEqual(0.0f)) {
        return 0;
      }
      if (value.IsEqual(1.0f)) {
        return 1;
      }
      double ddigits = Math.Log10(value);
      if (ddigits == (int)ddigits) {
        return (int)ddigits + 1;
      }
      int digits = (int)Math.Ceiling(ddigits);
      if (digits < 0) {
        digits = 0;
      }
      return digits;
    }

    /// <summary>
    /// Возвращает число знаков целой части числа например, если число 201, то метод возвратить число 3
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetIntegerDigitsNumber<T>(object value) {
      if (typeof(T) == typeof(int)) {
        return GetIntegerDigitsNumber(Convert.ToInt32(value));
      }

      if (typeof(T) == typeof(uint)) {
        return GetIntegerDigitsNumber(Convert.ToUInt32(value));
      }

      if (typeof(T) == typeof(double)) {
        return GetIntegerDigitsNumber(Convert.ToDouble(value));
      }

      if (typeof(T) == typeof(float)) {
        return GetIntegerDigitsNumber(Convert.ToSingle(value));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return 0;
    }

    #endregion

    #region Работа с диапазоном чисел и текущим значением

    public static bool OutOfRangeChecking(double inValue, double minValue, double maxValue) {
      if (inValue > maxValue || inValue < minValue) {
        ExceptionGenerator.Run<ArgumentOutOfRangeException>
        (String.Format("Значение '{0}' должно быть между '{1}' и '{2}'", inValue,
                       minValue, maxValue));
        return false;
      }
      return true;
    }

    public static bool OutOfRangeChecking(float inValue, float minValue, float maxValue) {
      if (inValue > maxValue || inValue < minValue) {
        ExceptionGenerator.Run<ArgumentOutOfRangeException>
        (String.Format("Значение '{0}' должно быть между '{1}' и '{2}'", inValue,
                       minValue, maxValue));
        return false;
      }
      return true;
    }

    public static bool OutOfRangeChecking(uint inValue, uint minValue, uint maxValue) {
      if (inValue > maxValue || inValue < minValue) {
        ExceptionGenerator.Run<ArgumentOutOfRangeException>
        (String.Format("Значение '{0}' должно быть между '{1}' и '{2}'", inValue,
                       minValue, maxValue));
        return false;
      }
      return true;
    }

    public static bool OutOfRangeChecking(int inValue, int minValue, int maxValue) {
      if (inValue > maxValue || inValue < minValue) {
        ExceptionGenerator.Run<ArgumentOutOfRangeException>
        (String.Format("Значение '{0}' должно быть между '{1}' и '{2}'", inValue,
                       minValue, maxValue));
        return false;
      }
      return true;
    }

    public static bool OutOfRangeChecking(long inValue, long minValue, long maxValue) {
      if (inValue > maxValue || inValue < minValue) {
        ExceptionGenerator.Run<ArgumentOutOfRangeException>
        (String.Format("Значение '{0}' должно быть между '{1}' и '{2}'", inValue,
                       minValue, maxValue));
        return false;
      }
      return true;
    }

    public static bool OutOfRangeChecking(ulong inValue, ulong minValue, ulong maxValue) {
      if (inValue > maxValue || inValue < minValue) {
        ExceptionGenerator.Run<ArgumentOutOfRangeException>
        (String.Format("Значение '{0}' должно быть между '{1}' и '{2}'", inValue,
                       minValue, maxValue));
        return false;
      }
      return true;
    }

    /// <summary>
    /// Увеличить диапазон чисел
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="precision">точность округления, если передано отрицательное число, то округления не будет</param>
    /// <param name="minValue">минимальное значение</param>
    /// <param name="maxValue">максимальное значение</param>
    public static void ScaleRange(double inValue, ref int precision, ref double minValue,
                                  ref double maxValue) {
      if (inValue > maxValue) {
        maxValue = inValue;
        if (precision > -1) {
          maxValue = Math.Round(maxValue, precision);
          if (inValue > maxValue) {
            maxValue += Math.Pow(10, precision);
          }
        }
      }
      if (inValue < minValue) {
        minValue = inValue;
        if (precision > -1) {
          int digits = GetMinFractionDigitsNumber(minValue);
          precision = digits > precision ? digits : precision;
          minValue = Math.Round(minValue, precision);
          if (inValue < minValue) {
            minValue -= Math.Pow(10, precision);
          }
        }
      }
    }

    public static void ScaleRange(float inValue, ref int precision, ref float minValue,
                                  ref float maxValue) {
      if (inValue > maxValue) {
        maxValue = inValue;
        if (precision > -1) {
          maxValue = (float)Math.Round(maxValue, precision);
          if (inValue > maxValue) {
            maxValue += (float)Math.Pow(10, precision);
          }
        }
      }
      if (inValue < minValue) {
        minValue = inValue;
        if (precision > -1) {
          int digits = GetMinFractionDigitsNumber(minValue);
          precision = digits > precision ? digits : precision;
          minValue = (float)Math.Round(minValue, precision);
          if (inValue < minValue) {
            minValue -= (float)Math.Pow(10, precision);
          }
        }
      }
    }

    public static void ScaleRange(int inValue, ref int minValue, ref int maxValue) {
      if (inValue > maxValue) {
        maxValue = inValue;
      }
      if (inValue < minValue) {
        minValue = inValue;
      }
    }

    public static void ScaleRange<T>(object inValue, ref int precision, ref object minValue,
                                     ref object maxValue) {
      if (typeof(T) == typeof(double)) {
        double value = Convert.ToDouble(inValue);
        double minValue_ = Convert.ToDouble(minValue);
        double maxValue_ = Convert.ToDouble(maxValue);
        ScaleRange(value, ref precision, ref minValue_, ref maxValue_);
        minValue = minValue_;
        maxValue = maxValue_;
      }
      if (typeof(T) == typeof(float)) {
        float value = Convert.ToSingle(inValue);
        float minValue_ = Convert.ToSingle(minValue);
        float maxValue_ = Convert.ToSingle(maxValue);
        ScaleRange(value, ref precision, ref minValue_, ref maxValue_);
        minValue = minValue_;
        maxValue = maxValue_;
      }

      if (typeof(T) == typeof(int)) {
        int value = Convert.ToInt32(inValue);
        int minValue_ = Convert.ToInt32(minValue);
        int maxValue_ = Convert.ToInt32(maxValue);
        ScaleRange(value, ref minValue_, ref maxValue_);
        minValue = minValue_;
        maxValue = maxValue_;
      }
    }

    public const int RANGE_NONE = 0;
    public const int RANGE_SCALING_ALLOWED = 1;
    public const int RANGE_VALUE_CYCLE = 2;

    public static double CorrectValueInRange(double inValue, double minValue, double maxValue,
        int correctingType) {
      if (correctingType == RANGE_VALUE_CYCLE) {
        while (inValue > maxValue) {
          inValue = minValue + (inValue - maxValue);
        }
        while (inValue < minValue) {
          inValue = maxValue + (inValue - minValue);
        }
        return inValue;
      }

      if (correctingType == RANGE_NONE) {
        if (inValue > maxValue) {
          return maxValue;
        }
        if (inValue < minValue) {
          return minValue;
        }
        return inValue;
      }

      if (correctingType == RANGE_SCALING_ALLOWED) {
        return inValue;
      }

      ExceptionGenerator.Run<ArgumentException>
      (String.Format("Значение '{0}' не корректно", correctingType)
      );
      return 0.0;
    }

    public static float CorrectValueInRange(float inValue, float minValue, float maxValue,
                                            int correctingType) {
      if (correctingType == RANGE_VALUE_CYCLE) {
        while (inValue > maxValue) {
          inValue = minValue + (inValue - maxValue);
        }
        while (inValue < minValue) {
          inValue = maxValue + (inValue - minValue);
        }
        return inValue;
      }

      if (correctingType == RANGE_NONE) {
        if (inValue > maxValue) {
          return maxValue;
        }
        if (inValue < minValue) {
          return minValue;
        }
        return inValue;
      }

      ExceptionGenerator.Run<ArgumentException>
      (String.Format("Значение '{0}' не корректно", correctingType)
      );
      return 0.0f;
    }

    public static int CorrectValueInRange(int inValue, int minValue, int maxValue, int correctingType) {
      if (correctingType == RANGE_VALUE_CYCLE) {
        while (inValue > maxValue) {
          inValue = minValue + (inValue - maxValue);
        }
        while (inValue < minValue) {
          inValue = maxValue + (inValue - minValue);
        }
        return inValue;
      }

      if (correctingType == RANGE_NONE) {
        if (inValue > maxValue) {
          return maxValue;
        }
        if (inValue < minValue) {
          return minValue;
        }
        return inValue;
      }

      ExceptionGenerator.Run<ArgumentException>
      (String.Format("Значение '{0}' не корректно", correctingType)
      );
      return 0;
    }

    public static object CorrectValueInRange<T>(object inValue, object minValue, object maxValue,
        int correctingType) {
      if (typeof(T) == typeof(double)) {
        double value = Convert.ToDouble(inValue);
        double minValue_ = Convert.ToDouble(minValue);
        double maxValue_ = Convert.ToDouble(maxValue);
        return CorrectValueInRange(value, minValue_, maxValue_, correctingType);
      }
      if (typeof(T) == typeof(float)) {
        float value = Convert.ToSingle(inValue);
        float minValue_ = Convert.ToSingle(minValue);
        float maxValue_ = Convert.ToSingle(maxValue);
        return CorrectValueInRange(value, minValue_, maxValue_, correctingType);
      }
      if (typeof(T) == typeof(int)) {
        int value = Convert.ToInt32(inValue);
        int minValue_ = Convert.ToInt32(minValue);
        int maxValue_ = Convert.ToInt32(maxValue);
        return CorrectValueInRange(value, minValue_, maxValue_, correctingType);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString())
                                                  );
      return null;
    }

    public static float MathClamp(float inValue, float minValue, float maxValue) {
      return inValue > maxValue ? maxValue : (inValue < minValue ? minValue : inValue);
    }

    public static double MathClamp(double inValue, double minValue, double maxValue) {
      return inValue > maxValue ? maxValue : (inValue < minValue ? minValue : inValue);
    }

    public static object MathClamp<T>(object inValue, object minValue, object maxValue) {
      if (typeof(T) == typeof(double)) {
        double value = Convert.ToDouble(inValue);
        double minValue_ = Convert.ToDouble(minValue);
        double maxValue_ = Convert.ToDouble(maxValue);
        return MathClamp(value, minValue_, maxValue_);
      }
      if (typeof(T) == typeof(float)) {
        float value = Convert.ToSingle(inValue);
        float minValue_ = Convert.ToSingle(minValue);
        float maxValue_ = Convert.ToSingle(maxValue);
        return MathClamp(value, minValue_, maxValue_);
      }
      if (typeof(T) == typeof(int)) {
        int value = Convert.ToInt32(inValue);
        int minValue_ = Convert.ToInt32(minValue);
        int maxValue_ = Convert.ToInt32(maxValue);
        return CorrectValueInRange(value, minValue_, maxValue_, RANGE_NONE);
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString())
                                                  );
      return null;
    }

    public static bool ValueInRange(int inValue, int value, int length) {
      return inValue >= value - length && inValue <= value + length;
    }

    #endregion

    #region Линейное преобразование и Обратное Линейное преобразование

    /// <summary>
    /// Линейное преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может возвратить метод</param>
    /// <param name="maxValue">максимальное значение, которое может возвратить метод</param>
    /// <param name="rangeValue">разница между максимальным и минимальным значением, которое может принимать входное значение</param>
    /// <returns></returns>
    public static double LinearTransformation(double inValue, double minValue, double maxValue,
        double rangeValue) {
      return minValue + (maxValue - minValue) * inValue / rangeValue;
    }

    /// <summary>
    /// Линейное преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может возвратить метод</param>
    /// <param name="maxValue">максимальное значение, которое может возвратить метод</param>
    /// <param name="rangeValue">разница между максимальным и минимальным значением, которое может принимать входное значение</param>
    /// <returns></returns>
    public static float LinearTransformation(float inValue, float minValue, float maxValue,
        float rangeValue) {
      return minValue + (maxValue - minValue) * inValue / rangeValue;
    }

    /// <summary>
    /// Линейное преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может возвратить метод</param>
    /// <param name="maxValue">максимальное значение, которое может возвратить метод</param>
    /// <param name="rangeValue">разница между максимальным и минимальным значением, которое может принимать входное значение</param>
    /// <returns></returns>
    public static int LinearTransformation(int inValue, int minValue, int maxValue, int rangeValue) {
      return minValue + (maxValue - minValue) * inValue / rangeValue;
    }

    /// <summary>
    /// Линейное преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может возвратить метод</param>
    /// <param name="maxValue">максимальное значение, которое может возвратить метод</param>
    /// <param name="rangeValue">разница между максимальным и минимальным значением, которое может принимать входное значение</param>
    /// <returns></returns>
    public static object LinearTransformation<T>(object inValue, object minValue, object maxValue,
        object rangeValue) {
      if (typeof(T) == typeof(int)) {
        return LinearTransformation(Convert.ToInt32(inValue), Convert.ToInt32(minValue),
                                    Convert.ToInt32(maxValue), Convert.ToInt32(rangeValue));
      }

      if (typeof(T) == typeof(double)) {
        return LinearTransformation(Convert.ToDouble(inValue), Convert.ToDouble(minValue),
                                    Convert.ToDouble(maxValue), Convert.ToDouble(rangeValue));
      }

      if (typeof(T) == typeof(float)) {
        return LinearTransformation(Convert.ToSingle(inValue), Convert.ToSingle(minValue),
                                    Convert.ToSingle(maxValue), Convert.ToSingle(rangeValue));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return null;
    }


    /// <summary>
    /// Обратное Линейное преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может принимать входное значение</param>
    /// <param name="maxValue">максимальное значение, которое может принимать входное значение</param>
    /// <param name="rangeValue">разница между максимальным и минимальным значением, которое может принимать возвращаемое значение</param>
    /// <returns></returns>
    public static double InverseLinearTransformation(double inValue, double minValue, double maxValue,
        double rangeValue) {
      OutOfRangeChecking(inValue, minValue, maxValue);
      return rangeValue * (inValue - minValue) / (maxValue - minValue);
    }

    /// <summary>
    /// Обратное Линейное преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может принимать входное значение</param>
    /// <param name="maxValue">максимальное значение, которое может принимать входное значение</param>
    /// <param name="rangeValue">разница между максимальным и минимальным значением, которое может принимать возвращаемое значение</param>
    /// <returns></returns>
    public static float InverseLinearTransformation(float inValue, float minValue, float maxValue,
        float rangeValue) {
      OutOfRangeChecking(inValue, minValue, maxValue);
      return rangeValue * (inValue - minValue) / (maxValue - minValue);
    }

    /// <summary>
    /// Обратное Линейное преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может принимать входное значение</param>
    /// <param name="maxValue">максимальное значение, которое может принимать входное значение</param>
    /// <param name="rangeValue">разница между максимальным и минимальным значением, которое может принимать возвращаемое значение</param>
    /// <returns></returns>
    public static int InverseLinearTransformation(int inValue, int minValue, int maxValue,
        int rangeValue) {
      OutOfRangeChecking(inValue, minValue, maxValue);
      return rangeValue * (inValue - minValue) / (maxValue - minValue);
    }

    /// <summary>
    /// Обратное Линейное преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может принимать входное значение</param>
    /// <param name="maxValue">максимальное значение, которое может принимать входное значение</param>
    /// <param name="rangeValue">разница между максимальным и минимальным значением, которое может принимать возвращаемое значение</param>
    /// <returns></returns>
    public static object InverseLinearTransformation<T>(object inValue, object minValue,
        object maxValue, object rangeValue) {
      if (typeof(T) == typeof(int)) {
        return InverseLinearTransformation(Convert.ToInt32(inValue), Convert.ToInt32(minValue),
                                           Convert.ToInt32(maxValue), Convert.ToInt32(rangeValue));
      }

      if (typeof(T) == typeof(double)) {
        return InverseLinearTransformation(Convert.ToDouble(inValue), Convert.ToDouble(minValue),
                                           Convert.ToDouble(maxValue), Convert.ToDouble(rangeValue));
      }

      if (typeof(T) == typeof(float)) {
        return InverseLinearTransformation(Convert.ToSingle(inValue), Convert.ToSingle(minValue),
                                           Convert.ToSingle(maxValue), Convert.ToSingle(rangeValue));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return null;
    }

    #endregion

    #region Логарифмическое преобразование и Обратное Логарифмическое преобразование

    /// <summary>
    /// получить базу для логарифмического преобразования
    /// </summary>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static double GetBaseLogarithmicTransformation(double inMinValue, double inMaxValue,
        double stepValue, double outMinValue, double outMaxValue) {
      double num = (inMaxValue - inMinValue) / stepValue + 1.0;
      return Math.Pow((outMaxValue - outMinValue + 1), 1.0 / (num - 1.0));
    }

    /// <summary>
    /// получить базу для логарифмического преобразования
    /// </summary>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static float GetBaseLogarithmicTransformation(float inMinValue, float inMaxValue,
        float stepValue, float outMinValue, float outMaxValue) {
      float num = (inMaxValue - inMinValue) / stepValue + 1.0f;
      return (float)Math.Pow((outMaxValue - outMinValue + 1), 1.0f / (num - 1.0f));
    }

    /// <summary>
    /// получить базу для логарифмического преобразования
    /// </summary>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static object GetBaseLogarithmicTransformation<T>(object inMinValue, object inMaxValue,
        object stepValue, object outMinValue, object outMaxValue) {
      if (typeof(T) == typeof(double)) {
        return GetBaseLogarithmicTransformation(Convert.ToDouble(inMinValue), Convert.ToDouble(inMaxValue),
                                                Convert.ToDouble(stepValue), Convert.ToDouble(outMinValue), Convert.ToDouble(outMaxValue));
      }

      if (typeof(T) == typeof(float)) {
        return GetBaseLogarithmicTransformation(Convert.ToSingle(inMinValue), Convert.ToSingle(inMaxValue),
                                                Convert.ToSingle(stepValue), Convert.ToSingle(outMinValue), Convert.ToDouble(outMaxValue));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return null;
    }


    /// <summary>
    /// Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может возвратить метод</param>
    /// <param name="baseValue">базу для логарифмического преобразования</param>
    /// <returns></returns>
    public static double LogarithmicTransformation(double inValue, double minValue, double baseValue) {
      return minValue + Math.Pow(baseValue, inValue) - 1.0;
    }

    /// <summary>
    /// Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может возвратить метод</param>
    /// <param name="baseValue">базу для логарифмического преобразования</param>
    /// <returns></returns>
    public static float LogarithmicTransformation(float inValue, float minValue, float baseValue) {
      return minValue + (float) Math.Pow(baseValue, inValue) - 1.0f;
    }

    /// <summary>
    /// Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, которое может возвратить метод</param>
    /// <param name="baseValue">базу для логарифмического преобразования</param>
    /// <returns></returns>
    public static object LogarithmicTransformation<T>(object inValue, object minValue,
        object baseValue) {
      if (typeof(T) == typeof(double)) {
        return LogarithmicTransformation(Convert.ToDouble(inValue), Convert.ToDouble(minValue),
                                         Convert.ToDouble(baseValue));
      }

      if (typeof(T) == typeof(float)) {
        return LogarithmicTransformation(Convert.ToSingle(inValue), Convert.ToSingle(minValue),
                                         Convert.ToSingle(baseValue));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return null;
    }


    /// <summary>
    /// Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static double LogarithmicTransformation(double inValue, double inMinValue, double inMaxValue,
        double stepValue, double outMinValue, double outMaxValue) {
      double baseValue = GetBaseLogarithmicTransformation(inMinValue, inMaxValue, stepValue, outMinValue,
                         outMaxValue);

      return LogarithmicTransformation(inValue, outMinValue, baseValue);
    }

    /// <summary>
    /// Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static float LogarithmicTransformation(float inValue, float inMinValue, float inMaxValue,
        float stepValue, float outMinValue, float outMaxValue) {
      float baseValue = GetBaseLogarithmicTransformation(inMinValue, inMaxValue, stepValue, outMinValue,
                        outMaxValue);

      return LogarithmicTransformation(inValue, outMinValue, baseValue);
    }

    /// <summary>
    /// Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static object LogarithmicTransformation<T>(object inValue, object inMinValue,
        object inMaxValue, object stepValue, object outMinValue, object outMaxValue) {
      object baseValue = GetBaseLogarithmicTransformation<T>(inMinValue, inMaxValue, stepValue,
                         outMinValue, outMaxValue);

      return LogarithmicTransformation<T>(inValue, outMinValue, baseValue);
    }

    /// <summary>
    /// Обратное Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, , которое может принимать входное значение</param>
    /// <param name="baseValue">базу для логарифмического преобразования</param>
    /// <returns></returns>
    public static double InverseLogarithmicTransformation(double inValue, double minValue,
        double baseValue) {
      return (Math.Log(inValue - minValue + 1) / Math.Log(baseValue));
    }

    /// <summary>
    /// Обратное Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, , которое может принимать входное значение</param>
    /// <param name="baseValue">базу для логарифмического преобразования</param>
    /// <returns></returns>
    public static float InverseLogarithmicTransformation(float inValue, float minValue,
        float baseValue) {
      return (float)(Math.Log(inValue - minValue + 1) / Math.Log(baseValue));
    }

    /// <summary>
    /// Обратное Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="minValue">минимальное значение, , которое может принимать входное значение</param>
    /// <param name="baseValue">базу для логарифмического преобразования</param>
    /// <returns></returns>
    public static object InverseLogarithmicTransformation<T>(object inValue, object minValue,
        object baseValue) {
      if (typeof(T) == typeof(double)) {
        return InverseLogarithmicTransformation(Convert.ToDouble(inValue), Convert.ToDouble(minValue),
                                                Convert.ToDouble(baseValue));
      }

      if (typeof(T) == typeof(float)) {
        return InverseLogarithmicTransformation(Convert.ToSingle(inValue), Convert.ToSingle(minValue),
                                                Convert.ToSingle(baseValue));
      }

      ExceptionGenerator.Run<InvalidCastException>(String.Format("Тип [{0}] не корректный",
          typeof(T).ToString()));
      return null;
    }

    /// <summary>
    /// Обратное Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static double InverseLogarithmicTransformation(double inValue, double inMinValue,
        double inMaxValue, double stepValue, double outMinValue, double outMaxValue) {
      OutOfRangeChecking(inValue, inMinValue, inMaxValue);
      double baseValue = GetBaseLogarithmicTransformation(outMinValue, outMaxValue, stepValue, inMinValue,
                         inMaxValue);

      return InverseLogarithmicTransformation(inValue, inMinValue, baseValue);
    }

    /// <summary>
    /// Обратное Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static float InverseLogarithmicTransformation(float inValue, float inMinValue,
        float inMaxValue, float stepValue, float outMinValue, float outMaxValue) {
      OutOfRangeChecking(inValue, inMinValue, inMaxValue);
      float baseValue = GetBaseLogarithmicTransformation(outMinValue, outMaxValue, stepValue, inMinValue,
                        inMaxValue);

      return InverseLogarithmicTransformation(inValue, inMinValue, baseValue);
    }

    /// <summary>
    /// Обратное Логарифмическое преобразование
    /// </summary>
    /// <param name="inValue">входное значение</param>
    /// <param name="inMinValue">минимальное входное значение</param>
    /// <param name="inMaxValue">максимальное входное значение</param>
    /// <param name="stepValue">шаг изменения входного значения</param>
    /// <param name="outMinValue">минимальное выходное значение</param>
    /// <param name="outMaxValue">максимальное выходное значение</param>
    /// <returns></returns>
    public static object InverseLogarithmicTransformation<T>(object inValue, object inMinValue,
        object inMaxValue, object stepValue, object outMinValue, object outMaxValue) {
      object baseValue = GetBaseLogarithmicTransformation<T>(outMinValue, outMaxValue, stepValue,
                         inMinValue, inMaxValue);

      return InverseLogarithmicTransformation<T>(inValue, inMinValue, baseValue);
    }

    #endregion

    /// <summary>
    /// Рассчитывает значение, зная предыдущее значение и значение перемещения колесика мыши
    /// </summary>
    /// <typeparam name="T">тип значения</typeparam>
    /// <param name="oldValue">старое значение</param>
    /// <param name="delta">значение перемещения колесика мыши</param>
    /// <returns>возвращаемое значение</returns>
    public static T GetValueByMouseWheel<T>(T oldValue, int delta) {
      int integerDigitsNumber = GetIntegerDigitsNumber<T>(oldValue);
      int firstDigit = Convert.ToInt32(Floor<T>(Divide<T>(oldValue, Math.Pow(10,
                                       integerDigitsNumber - 1))));

      var coef = firstDigit > 7 ? 0.002 : (firstDigit > 3 ? 0.001 : 0.0005);
      var shiftScale = Math.Round(delta * coef * Math.Pow(10, integerDigitsNumber - 1) / 1.2, 5);
      T val = (T)Round<T>((object)Add<T>(oldValue, shiftScale), 5);

      var integerDigitsNumber_ = GetIntegerDigitsNumber<T>(val);
      if (integerDigitsNumber_ < integerDigitsNumber && delta < 0) {
        coef = 0.002;
        shiftScale = Math.Round(delta * coef * Math.Pow(10, integerDigitsNumber_ - 1) / 1.2, 5);
        val = (T)Round<T>((object)Add<T>(oldValue, shiftScale), 5);
      }
      return val;
    }

    /// <summary>
    /// Рассчитывает значение, зная предыдущее значение и значение перемещения колесика мыши
    /// </summary>
    /// <typeparam name="T">тип значения</typeparam>
    /// <param name="oldValue">старое значение</param>
    /// <param name="delta">значение перемещения колесика мыши</param>
    /// <returns>возвращаемое целое значение</returns>
    public static T GetIntValueByMouseWheel<T>(T oldValue, int delta) {
      int integerDigitsNumber = GetIntegerDigitsNumber<T>(oldValue);
      int firstDigit = Convert.ToInt32(Floor<T>(Divide<T>(oldValue, Math.Pow(10,
                                       integerDigitsNumber - 1))));

      var coef = firstDigit > 7 ? 0.002 : (firstDigit > 3 ? 0.001 : 0.0005);
      var shiftScale = Math.Round(delta * coef * Math.Pow(10, integerDigitsNumber - 1) / 1.2, 5);

      var fractionOldValue = Subtract<T>(oldValue, Floor<T>(oldValue));
      double fractionOldValue_ = Convert.ToDouble(fractionOldValue);
      shiftScale = CorrectShiftScale(shiftScale, fractionOldValue_);

      T val = (T)Round<T>((object)Add<T>(oldValue, shiftScale));

      var integerDigitsNumber_ = GetIntegerDigitsNumber<T>(val);
      if (integerDigitsNumber_ < integerDigitsNumber && delta < 0) {
        coef = 0.002;
        shiftScale = Math.Round(delta * coef * Math.Pow(10, integerDigitsNumber_ - 1) / 1.2, 5);
        shiftScale = CorrectShiftScale(shiftScale, fractionOldValue_);
        val = (T)Round<T>((object)Add<T>(oldValue, shiftScale));
      }
      return val;
    }

    #region Побитовое сравнение чисел

    public static bool IsPartBinaryEqual(char number, char part) {
      return ((int)number & (int)part) == (int)part;
    }

    public static bool IsPartBinaryEqual(byte number, byte part) {
      return ((int)number & (int)part) == (int)part;
    }

    public static bool IsPartBinaryEqual(short number, short part) {
      return ((int)number & (int)part) == (int)part;
    }

    public static bool IsPartBinaryEqual(ushort number, ushort part) {
      return ((int)number & (int)part) == (int)part;
    }

    public static bool IsPartBinaryEqual(int number, int part) {
      return (number & part) == part;
    }

    public static bool IsPartBinaryEqual(long number, long part) {
      return (number & part) == part;
    }

    public static bool IsPartBinaryEqual(ulong number, ulong part) {
      return (number & part) == part;
    }

    public static char LogicSubtract(char number, char part) {
      return (char)((int)number & ~(int)part);
    }

    public static byte LogicSubtract(byte number, byte part) {
      return (byte)((int)number & ~(int)part); ;
    }

    public static short LogicSubtract(short number, short part) {
      return (short)((int)number & ~(int)part); ;
    }

    public static ushort LogicSubtract(ushort number, ushort part) {
      return (ushort)((int)number & ~(int)part); ;
    }

    public static int LogicSubtract(int number, int part) {
      return number & ~part;
    }

    public static uint LogicSubtract(uint number, uint part) {
      return number & ~part;
    }

    public static T LogicSubtract<T>(T number, T part) {
      int number_ = (int)Convert.ToDecimal(number);
      int part_ = (int)Convert.ToDecimal(part);
      var res = number_ & ~part_;
      return (T)(object)res;
    }

    #endregion

    public static int NeareastPow2(int value) {
      if (value == 0) {
        return 0;
      }

      value -= 1;
      int i = 0;
      int y;

      Action<int> func = ((n) => {
        y = value >> n;
        if (y > 0) {
          i += n;
          value = y;
        }
      });

      func(16);
      func(8);
      func(4);
      func(2);
      func(1);

      func = null;

      int powV = i + 1;

      return (int)Math.Pow(2, powV);
    }

    public static byte MathClamp(byte x, byte low, byte high) {
      return (byte)(((x) > (high)) ? (high) : (((x) < (low)) ? (low) : (x)));
    }

    public static Vector2[] Rotate2D(IEnumerable<Vector2> vects, Vector2 center, double angle,
                                     bool bDegree = false) {
      if (bDegree) {
        angle = ToRadian(angle);
      }
      var cos = Math.Cos(angle);
      var sin = Math.Sin(angle);

      Vector2[] resultVects = new Vector2[vects.Count()];

      int index = 0;
      foreach (var vec in vects) {
        //var vec_ = new Vector2(vec.x - center.x, vec.y - center.y);
        resultVects[index] = new Vector2((float)(vec.x * cos - vec.y * sin + center.x),
                                         (float)(vec.x * sin + vec.y * cos + center.y));
        index++;
      }
      return resultVects;
    }

    /*public static Vector2[] Rotate2D(IBox<double> box, Vector2 center, double angle,
                                     bool bDegree = false) {
      if (bDegree) {
        angle = ToRadian(angle);
      }
      var cos = (float)Math.Cos(angle);
      var sin = (float)Math.Sin(angle);
      var leftTop = new Vector2(box.Left - center.x, center.y - box.Top);
      var leftDown = new Vector2(box.Left - center.x, center.y - box.Bottom);
      var rightTop = new Vector2(box.Right - center.x, center.y - box.Top);
      var rightBottom = new Vector2(box.Right - center.x, center.y - box.Bottom);

      var leftTopRes = new Vector2();
      leftTopRes.x = leftTop.x * cos + leftTop.y * sin + center.x;
      leftTopRes.y = center.y + leftTop.x * sin - leftTop.y * cos;
      var leftDownRes = new Vector2();
      leftDownRes.x = leftDown.x * cos + leftDown.y * sin + center.x;
      leftDownRes.y = center.y + leftDown.x * sin - leftDown.y * cos;
      var rightTopRes = new Vector2();
      rightTopRes.x = rightTop.x * cos + rightTop.y * sin + center.x;
      rightTopRes.y = center.y + rightTop.x * sin - rightTop.y * cos;
      var rightBottomRes = new Vector2();
      rightBottomRes.x = rightBottom.x * cos + rightBottom.y * sin + center.x;
      rightBottomRes.y = center.y + rightBottom.x * sin - rightBottom.y * cos;

      return new Vector2[4] {leftTopRes, leftDownRes, rightTopRes, rightBottomRes};
      }*/

    private static List<Type> _integerTypes = new List<Type>() {
      typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(byte), typeof(sbyte), typeof(short),
             typeof(ushort)
    };

    private static double CorrectShiftScale(double shiftScale, double fractionOldValue) {
      if (!Math.Abs(fractionOldValue).IsLess(1.0)) {
        ExceptionGenerator.Run<ArgumentException>("Второй аргумент \"{0}\" по модулю должен быть 1.0",
            fractionOldValue);
      }

      if (shiftScale.IsEqual(0.0)) {
        return shiftScale;
      }
      if (shiftScale.IsLess(1.0) && shiftScale.IsGreater(0.0)) {
        return 1.0 - fractionOldValue;
      }
      if (shiftScale.IsGreater(-1.0) && shiftScale.IsLess(0.0)) {
        return fractionOldValue.IsEqual(0.0) ? -1.0 : -fractionOldValue;
      }

      shiftScale = Math.Round(shiftScale);
      if (fractionOldValue.IsGreater(0.0)) {
        if (Math.Sign(shiftScale) == 1) {
          shiftScale -= fractionOldValue;
        }
        else {
          shiftScale -= fractionOldValue - 1.0;
        }
      }
      return shiftScale;
    }
  }
}
