using System;
using System.Collections.Generic;
using System.Linq;
public static class StringExtension
{
    public static string ToRomanNumber(this int num)
    {
        if (num <= 0) return "";
        var result = string.Empty;
        var map = new Dictionary<string, int>
        {
            { "M", 1000 },
            { "CM", 900 },
            { "D", 500 },
            { "CD", 400 },
            { "C", 100 },
            { "XC", 90 },
            { "L", 50 },
            { "XL", 40 },
            { "X", 10 },
            { "IX", 9 },
            { "V", 5 },
            { "IV", 4 },
            { "I", 1 }
        };
        foreach (var pair in map)
        {
            result += string.Join(string.Empty, Enumerable.Repeat(pair.Key, num / pair.Value));
            num %= pair.Value;
        }

        return result;
    }

   
    public static string ToAbbreviatedString(this int num)
    {
        return Abbreviate(num);
    }

    public static string ToAbbreviatedString(this long num)
    {
        return Abbreviate(num);
    }


    public static string ToAbbreviatedString(this double num)
    {
        return Abbreviate((long)num);
    }

    public static string Abbreviate(long value)
    {
        string signification = value < 0 ? "-" : "";
        long number = Math.Abs(value);
        if (number == 0)
            return "0";

        int mag = (int)(Math.Floor(Math.Log10(number)) / 3); // Truncates to 6, divides to 2
        double divisor = Math.Pow(10, mag * 3);

        decimal shortNumber = number / (decimal)divisor;
        decimal noFraction = shortNumber - (shortNumber % 1);
        decimal fraction = (shortNumber % 1) * 100;
        decimal minFraction = (fraction - (fraction % 1)) / 100;
        shortNumber = noFraction + minFraction;

        string[] suffixes = { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff" };
        string suffix = suffixes[mag];

        return $"{signification}{(float)shortNumber}{suffix}";
    }

  
}