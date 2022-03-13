using UnityEngine;

public static class Extensions
{
    public static T LastValue<T>(this T[] array)
    {
        return array[array.Length - 1];
    }

    public static Vector3 Change(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x == null ? vector.x : (float)x,
                           y == null ? vector.y : (float)y,
                           z == null ? vector.z : (float)z);
    }
}