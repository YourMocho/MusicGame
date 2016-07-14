using UnityEngine;
using System.Collections;
using System;

public static class ExtensionMethods
{
    public static void DestroyChildren(this MonoBehaviour monoBehaviour)
    {
        int childCount = monoBehaviour.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            MonoBehaviour.DestroyImmediate(monoBehaviour.transform.GetChild(i).gameObject);
        }
    }

    public static float ConvertToPositiveAngle(this object anything, float angle)
    {
        while (angle < 0)
        {
            angle += 360;
        }

        while (angle >= 360)
        {
            angle -= 360;
        }

        return angle;
    }

    public static float GetDifferenceBetweenAngles(this object anything, float angle1, float angle2)
    {
        float result = angle1 - angle2;

        if (result < -180)
        {
            result += 360;
        }

        if (result > 180)
        {
            result -= 360;
        }

        return Mathf.Abs(result);
    }

    public static string NumberToWords(this object anything, int number)
    {
        string result = "";

        if (number > 0)
        {
            string[] unitsMap = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

            result += unitsMap[number];
        }

        return result;
    }

    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static string GetString(this Transform transform)
    {
        string result = "";
        result += transform.position.GetString() + ";";
        result += transform.rotation.GetString() + ";";
        result += transform.localScale.GetString();

        return result;
    }

    public static string GetString(this Vector3 vector)
    {
        string result = "";
        result += vector.x + ",";
        result += vector.y + ",";
        result += vector.z;

        return result;
    }

    public static string GetString(this Quaternion quaternion)
    {
        string result = "";
        result += quaternion.x + ",";
        result += quaternion.y + ",";
        result += quaternion.z + ",";
        result += quaternion.w;

        return result;
    }

    public static Vector3 Vector3FromString(this object anything, string vectorString)
    {
        float[] numbers = new float[3];

        string[] values = vectorString.Split(',');

        if (numbers.Length != values.Length)
        {
            return Vector3.zero;
        }

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = float.Parse(values[i]);
        }

        return new Vector3(numbers[0], numbers[1], numbers[2]);
    }

    public static Quaternion QuaternionFromString(this object anything, string quaternionString)
    {
        float[] numbers = new float[4];

        string[] values = quaternionString.Split(',');

        if (numbers.Length != values.Length)
        {
            return Quaternion.identity;
        }

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = float.Parse(values[i]);
        }

        return new Quaternion(numbers[0], numbers[1], numbers[2], numbers[3]);
    }
}