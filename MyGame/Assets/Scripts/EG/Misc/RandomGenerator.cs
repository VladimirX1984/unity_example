﻿using System;

namespace EG.Misc {
public sealed class RandomGenerator : Singleton<Random> {
  // Summary:
  //     Returns a nonnegative random number.
  //
  // Returns:
  //     A 32-bit signed integer greater than or equal to zero and less than System.Int32.MaxValue.
  public static int Next() {
    return GetInstance().Next();
  }

  //
  // Summary:
  //     Returns a nonnegative random number less than the specified maximum.
  //
  // Parameters:
  //   maxValue:
  //     The exclusive upper bound of the random number to be generated. maxValue
  //     must be greater than or equal to zero.
  //
  // Returns:
  //     A 32-bit signed integer greater than or equal to zero, and less than maxValue;
  //     that is, the range of return values ordinarily includes zero but not maxValue.
  //     However, if maxValue equals zero, maxValue is returned.
  //
  // Exceptions:
  //   System.ArgumentOutOfRangeException:
  //     maxValue is less than zero.
  public static int Next(int maxValue) {
    return GetInstance().Next(maxValue);
  }

  //
  // Summary:
  //     Returns a random number within a specified range.
  //
  // Parameters:
  //   minValue:
  //     The inclusive lower bound of the random number returned.
  //
  //   maxValue:
  //     The exclusive upper bound of the random number returned. maxValue must be
  //     greater than or equal to minValue.
  //
  // Returns:
  //     A 32-bit signed integer greater than or equal to minValue and less than maxValue;
  //     that is, the range of return values includes minValue but not maxValue. If
  //     minValue equals maxValue, minValue is returned.
  //
  // Exceptions:
  //   System.ArgumentOutOfRangeException:
  //     minValue is greater than maxValue.
  public static int Next(int minValue, int maxValue) {
    return GetInstance().Next(minValue, maxValue);
  }

  //
  // Summary:
  //     Fills the elements of a specified array of bytes with random numbers.
  //
  // Parameters:
  //   buffer:
  //     An array of bytes to contain random numbers.
  //
  // Exceptions:
  //   System.ArgumentNullException:
  //     buffer is null.
  public static void NextBytes(byte[] buffer) {
    GetInstance().NextBytes(buffer);
  }

  //
  // Summary:
  //     Returns a random number between 0.0 and 1.0.
  //
  // Returns:
  //     A double-precision floating point number greater than or equal to 0.0, and
  //     less than 1.0.
  public static double NextDouble() {
    return GetInstance().NextDouble();
  }

  public static int RandDelta(int x) {
    return RandMinMax(-x, x);
  }

  public static int RandMinMax(int min, int max) {
    return min + (int)Math.Round(((RandomGenerator.Next() % 10000) * (max - min)) / 10000.0);
  }

  public static float RandDelta(float x) {
    return RandMinMax(-x, x);
  }

  public static float RandMinMax(float min, float max) {
    return min + (float)((RandomGenerator.Next() % 10000) * (max - min)) / 10000.0f;
  }

  public static double RandDelta(double x) {
    return RandMinMax(-x, x);
  }

  public static double RandMinMax(double min, double max) {
    return min + ((RandomGenerator.Next() % 10000) * (max - min)) / 10000.0;
  }

  private RandomGenerator() {

  }
}
}
