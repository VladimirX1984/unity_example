using System;
using System.Security.Cryptography;
using System.Text;

using EG.Misc;

namespace EG.Kernel {
/// <summary>
/// Генератор идентификаторов для объектов
/// </summary>
public class IDGenerator : IBaseObject {
  public static TIdType Get<TIdType>(IBaseIDObject<TIdType> obj) {
    Check<TIdType>();
    Type type = typeof(TIdType);
    TIdType id = default(TIdType);
    if (!type.TryConvert<TIdType>(obj.GetHashCode(), ref id)) {
      ExceptionGenerator.Run<InvalidCastException>("Невозможно преобразовать код хеш-значения в значение тип идентификатора \"{0}\"",
          type);
    }
    return id;
  }

  public static TIdType GetMD5<TIdType>(string str) {
    var buffer = Encoding.UTF8.GetBytes(str);
    return GetMD5<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetMD5<TIdType>(byte[] buffer) {
    return GetMD5<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetMD5<TIdType>(byte[] buffer, int offset, int count) {
    Check<TIdType>();
    var myMD5 = MD5.Create();
    var hashValue = myMD5.ComputeHash(buffer, offset, count);
    return GetHashAlgorithm<TIdType>(hashValue);
  }

  public static string GetMD5(string str) {
    var buffer = Encoding.UTF8.GetBytes(str);
    var myMD5 = MD5.Create();
    return Convert.ToBase64String(myMD5.ComputeHash(buffer));
  }

  public static string GetMD5(string str, string prefix, string postfix = "") {
    var sEncoded = GetMD5(str);
    return AddPrefixAndPostfix(prefix, sEncoded, postfix);
  }

  public static TIdType GetSHA1<TIdType>(string str) {
    var buffer = Encoding.UTF8.GetBytes(str);
    return GetSHA1<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetSHA1<TIdType>(byte[] buffer) {
    return GetSHA1<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetSHA1<TIdType>(byte[] buffer, int offset, int count) {
    Check<TIdType>();
    SHA1 mySHA1 = SHA1Managed.Create();
    var hashValue = mySHA1.ComputeHash(buffer, offset, count);
    return GetHashAlgorithm<TIdType>(hashValue);
  }

  public static TIdType GetSHA256<TIdType>(string str) {
    var buffer = Encoding.UTF8.GetBytes(str);
    return GetSHA256<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetSHA256<TIdType>(byte[] buffer) {
    return GetSHA256<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetSHA256<TIdType>(byte[] buffer, int offset, int count) {
    Check<TIdType>();
    SHA256 mySHA256 = SHA256Managed.Create();
    var hashValue = mySHA256.ComputeHash(buffer, offset, count);
    return GetHashAlgorithm<TIdType>(hashValue);
  }

  public static TIdType GetSHA384<TIdType>(string str) {
    var buffer = Encoding.UTF8.GetBytes(str);
    return GetSHA384<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetSHA384<TIdType>(byte[] buffer) {
    return GetSHA384<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetSHA384<TIdType>(byte[] buffer, int offset, int count) {
    Check<TIdType>();
    SHA384 mySHA384 = SHA384Managed.Create();
    var hashValue = mySHA384.ComputeHash(buffer, offset, count);
    return GetHashAlgorithm<TIdType>(hashValue);
  }

  public static TIdType GetSHA512<TIdType>(string str) {
    var buffer = Encoding.UTF8.GetBytes(str);
    return GetSHA512<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetSHA512<TIdType>(byte[] buffer) {
    return GetSHA512<TIdType>(buffer, 0, buffer.Length);
  }

  public static TIdType GetSHA512<TIdType>(byte[] buffer, int offset, int count) {
    Check<TIdType>();
    SHA512 mySHA512 = SHA512Managed.Create();
    var hashValue = mySHA512.ComputeHash(buffer, offset, count);
    return GetHashAlgorithm<TIdType>(hashValue);
  }

  private static void Check<TIdType>() {
    Type type = typeof(TIdType);
    if (type != typeof(string) && (!type.IsPrimitive || !type.IsValueType)) {
      ExceptionGenerator.Run<InvalidCastException>("Тип идентификатора \"{0}\" должен быть примитивным типом или типом значений или строковым",
          type);
    }
  }

  private static TIdType GetHashAlgorithm<TIdType>(byte[] hashValue) {
    Type type = typeof(TIdType);
    double value = 0;
    for (int i = 0; i < hashValue.Length; ++i) {
      value += hashValue[i] * Math.Pow(256, hashValue.Length - 1 - i);
    }
    TIdType id = default(TIdType);
    if (!type.TryConvert<TIdType>(value, ref id)) {
      ExceptionGenerator.Run<InvalidCastException>("Невозможно преобразовать крипт-значение в значение тип идентификатора \"{0}\"",
          type);
    }
    return id;
  }

  private static string AddPrefixAndPostfix(string prefix, string id, string postfix) {
    if (prefix == null) {
      prefix = String.Empty;
    }
    if (postfix == null) {
      postfix = String.Empty;
    }
    return String.Format("{0}{1}{2}", prefix, id, postfix);
  }
}
}
