using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Helper {

    public static T GetObjectFromDirection<T>(Vector2 from, Vector2 to, T up, T right, T down, T left)
    {
        float angle = Vector2.SignedAngle(new Vector2(-0.5f, 0.5f).normalized, (to - from).normalized);
        if (angle < 0) angle += 360;
        angle = 360 - angle;
        if (angle >= 0 && angle <= 90)
        {
            return up;
        }
        else if (angle > 90 && angle <= 180)
        {
            return right;
        }
        else if (angle > 180 && angle <= 270)
        {
            return down;
        }
        else
        {
            return left;
        }
    }
    public static T GetObjectFromDirection<T>(Vector2 direction, T up, T right, T down, T left)
    {
        float angle = Vector2.SignedAngle(new Vector2(-0.5f, 0.5f).normalized, direction.normalized);
        if (angle < 0) angle += 360;
        angle = 360 - angle;
        if (angle >= 0 && angle <= 90)
        {
            return up;
        }
        else if (angle > 90 && angle <= 180)
        {
            return right;
        }
        else if (angle > 180 && angle <= 270)
        {
            return down;
        }
        else
        {
            return left;
        }
    }

    //public static string goldenrod
    //{
    //    get
    //    {
    //        return "#ffd700";
    //    }
    //}

    public static int NextIndexOf(this string str, int startingIndex, char character)
    {
        return startingIndex + str.Substring(startingIndex).IndexOf(character);
    }

    public static bool StartsWith(this string str, char @char)
    {
        if (str.Length == 0) return false;
        return str[0] == @char;
    }

    /*public static Flag StartCoroutineWithFlag(this MonoBehaviour mono, IEnumerator coroutine)
    {
        Flag flag = new Flag();
        mono.StartCoroutine(FlagCoroutine(coroutine, flag));
        return flag;
    }

    static IEnumerator FlagCoroutine(IEnumerator enumerator, Flag flag)
    {
        while (enumerator.MoveNext()) yield return null;
        flag.value = true;
    }*/

    public static bool ToBool(this int i)
    {
        return (i == 1); 
    }

    public static int ToInt(this bool b)
    {
        if (b) return 1;
        return 0;
    }

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static T RandomElement<T>(this List<T> e)
    {
        return e[Random.Range(0, e.Count)];
    }

    public static T[] GetComponents<T>(this GameObject[] gameObjects) where T : Component
    {
        return gameObjects.Select(item => item.GetComponent<T>()).ToArray();
    }

    public static void IfHasComponent<TSource, T>(this TSource source, System.Action<T> action) where T : Component where TSource : Component 
    {
        var c = source.GetComponent<T>();
        if (c != null)
        {
            action.Invoke(c);
        }
    }

    public static void IfHasComponent<T>(this GameObject source, System.Action<T> action) where T : Component
    {
        var c = source.GetComponent<T>();
        if (c != null)
        {
            action.Invoke(c);
        }
    }

    public static void IfDefined<T>(this T t, System.Action<T> action)
    {
        if (t != null && action != null) action.Invoke(t);
    }

    public static Matrix4x4 Lerp(Matrix4x4 from, Matrix4x4 to, float percentage)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
        {
            ret[i] = Mathf.Lerp(from[i], to[i], percentage);
        }
        return ret;
    }

    public static Vector2 RandomPointOnUnitCircle()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

    }
}
