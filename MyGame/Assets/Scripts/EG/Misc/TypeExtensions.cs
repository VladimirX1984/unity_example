using System;

using EG.Kernel;

namespace EG.Misc {
public static class TypeExtensions {
  public static T ToTypeValue<T>(this Type convertedType, object value) {
    T convertedValue = default(T);
    if (convertedType.TryConvert(value, ref convertedValue)) {
      return convertedValue;
    }
    return default(T);
  }

  public static object ToTypeValue(this Type convertedType, object value) {
    object convertedValue = null;
    if (convertedType.TryConvert(value, ref convertedValue)) {
      return convertedValue;
    }
    return null;
  }

  public static bool TryConvert<T>(this Type convertedType, object value, ref T convertedValue) {
    object convertedValue_ = convertedValue;
    if (convertedType.TryConvert(value, ref convertedValue_)) {
      convertedValue = (T)convertedValue_;
      return true;
    }
    return false;
  }

  /// <summary>
  /// Если значение строка, то преобразует строку в примитивный тип данных, и наоборот, если нужен тип строка, а данные представляют примитивный тип
  /// </summary>
  /// <param name="convertedType">тип данных, в который данные будут конвертироваться</param>
  /// <param name="value">новое значение</param>
  /// <returns></returns>
  public static bool TryConvert(this Type convertedType, object value, ref object convertedValue) {
    if (value == null) {
      if (convertedType.IsPrimitive || convertedType.IsValueType) {
        ExceptionGenerator.Run<InvalidCastException>("Примитивный тип (или тип значений) \"{0}\" не может принимать значение null",
            convertedType, convertedType);
      }
      convertedValue = value;
      return true;
    }

    Type valType = value.GetType();

    if (convertedType == valType) {
      convertedValue = value;
      return true;
    }

    if (!convertedType.IsPrimitive) {
      if (convertedType == typeof(string)) {
        if (!valType.IsPrimitive) {
          if (valType != convertedType) {
            string sMsg =
              "Пытаемся преобразовать данные объекта типа \"{0}\" в строку. ";
            sMsg += "Если метод \"ToString\" у объекта неопределен, то мы получим неверные данные";
            DebugLogger.WriteWarning(sMsg, valType);
          }
          convertedValue = value.ToString();
          return true;
        }
        convertedValue = value.ToString();
        return true;
      }

      if (valType.IsPrimitive) {
        ExceptionGenerator.Run<InvalidCastException>(
          "Невозможно преобразовать из примитивного типа \"{0}\" в не примитивный тип \"{1}\"",
          valType, convertedType);
      }

      if (!convertedType.IsAssignableFrom(valType)) {
        if (value is IConvertableObject) {
          var newValue = (value as IConvertableObject).ToObject();
          if (newValue == null) {
            ExceptionGenerator.Run<InvalidCastException>(
              "Невозможно преобразовать из объекта конвертируемого типа \"{0}\" в значение типа \"{1}\". {2}",
              valType, convertedType,
              "Метод \"ToObject\" объекта \"value\" возвратил \"null\"");
          }
          if (!convertedType.IsAssignableFrom(newValue.GetType())) {
            ExceptionGenerator.Run<InvalidCastException>(
              "Невозможно преобразовать из объекта конвертируемого типа \"{0}\" в значение типа \"{1}\"",
              valType, convertedType);
          }
          convertedValue = newValue;
        }
        ExceptionGenerator.Run<InvalidCastException>("Невозможно преобразовать из типа \"{0}\" в тип \"{1}\"",
            valType, convertedType);
      }

      convertedValue = value;
      return true;
    }

    if (valType.IsPrimitive) {
      if (convertedType == typeof(IntPtr)) {
        long val = (long)Convert.ChangeType(value, typeof(long));
        convertedValue = new IntPtr(val);
        return true;
      }
      if (convertedType == typeof(UIntPtr)) {
        ulong val = (ulong)Convert.ChangeType(value, typeof(ulong));
        convertedValue = new UIntPtr(val);
        return true;
      }
      convertedValue = Convert.ChangeType(value, convertedType);
      return true;
    }

    if (valType != typeof(string)) {
      if (convertedType == typeof(IntPtr)) {
        long val = (long)Convert.ChangeType(value, typeof(long));
        convertedValue = new IntPtr(val);
        return true;
      }
      if (convertedType == typeof(UIntPtr)) {
        ulong val = (ulong)Convert.ChangeType(value, typeof(ulong));
        convertedValue = new UIntPtr(val);
        return true;
      }
      convertedValue = Convert.ChangeType(value, convertedType);
      return true;
    }

    var str = (string)value;

    if (convertedType == typeof(byte)) {
      byte outValue;
      if (!byte.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(char)) {
      char outValue;
      if (!char.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(sbyte)) {
      sbyte outValue;
      if (!sbyte.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(short)) {
      short outValue;
      if (!short.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(ushort)) {
      ushort outValue;
      if (!ushort.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(int)) {
      int outValue;
      if (!int.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(uint)) {
      uint outValue;
      if (!uint.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(long)) {
      long outValue;
      if (!long.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(ulong)) {
      ulong outValue;
      if (!ulong.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(float)) {
      float outValue;
      if (!float.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(double)) {
      double outValue;
      if (!double.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(bool)) {
      bool outValue;
      if (!bool.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = outValue;
      return true;
    }

    if (convertedType == typeof(IntPtr)) {
      long outValue;
      if (!long.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = new IntPtr(outValue);
      return true;
    }

    if (convertedType == typeof(UIntPtr)) {
      ulong outValue;
      if (!ulong.TryParse(str, out outValue)) {
        return false;
      }
      convertedValue = new UIntPtr(outValue);
      return true;
    }

    return false;
  }
}
}
