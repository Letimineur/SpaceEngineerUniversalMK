using System;

namespace UniversalMK
{
	public static class FloatExtensionsReimpl
	{
		// reimplementation of IsEqual and IsZero from FloatExtensions as apparently it's prohibited
		public static bool IsEqualFloat(this float f, float other, float epsilon = 0.0001f)
		{
			if (float.IsNaN(f) || float.IsNaN(other))
			{
				return false;
			}
			return f == other || (f - other).IsZeroFloat(epsilon);
		}

		public static bool IsZeroFloat(this float f, float epsilon = 0.0001f)
		{
			if (float.IsNaN(f))
			{
				return false;
			}
			return Math.Abs(f) < epsilon;
		}
	}
}