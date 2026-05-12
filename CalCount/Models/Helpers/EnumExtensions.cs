using System;
using System.ComponentModel;
using System.Reflection;

namespace CalCount.Models.Helpers;

public static class EnumExtensions
{
    // Returns the DescriptionAttribute value if present, otherwise the enum's name
    public static string GetDescription(this Enum value)
    {
        if (value == null) return string.Empty;
        var fi = value.GetType().GetField(value.ToString());
        if (fi == null) return value.ToString();
        var attr = fi.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }

    // Try parse a string into an enum value (case-insensitive) with a fallback default
    public static T ToEnum<T>(this string value, T defaultValue = default) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value)) return defaultValue;
        if (Enum.TryParse<T>(value, true, out var result)) return result;
        return defaultValue;
    }
}
