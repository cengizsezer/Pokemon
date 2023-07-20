using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ColorExtentions
{
    public static Color SetAlpha(this Color color, float a)
    {
        color.a = a;
        return color;
    }

    public static Color SetColor(this Color color, Color color2)
    {
        color2.a = color.a;
        return color2;
    }

    public static Color Transparent(this Color color)
    {
        color.a = 0;
        return color;
    }

    public static Color Opaque(this Color color)
    {
        color.a = 1;
        return color;
    }

    public static void MakeTransparent(this Graphic g)
    {
        Color c = g.color;
        c = c.Transparent();
        g.color = c;
    }

    public static void MakeOpaque(this Graphic g)
    {
        Color c = g.color;
        c = c.Opaque();
        g.color = c;
    }

    public static void SetAlpha(this Graphic g, float a)
    {
        Color c = g.color;
        c.a = a;
        g.color = c;
    }

    public static void SetColor(this Graphic g, Color c, bool changeAlpha = true)
    {
        if (!changeAlpha)
            c.a = g.color.a;

        g.color = c;
    }

    public static Color GetColor(this MeshFilter meshFilter)
    {
        var mesh = meshFilter.mesh;

        if (mesh != null)
        {
            var colors = mesh.colors;

            if (colors.Length <= 0)
            {
                return Color.white;
            }

            return colors[0];
        }

        return Color.white;

    }

    public static void SetColor(this MeshFilter meshFilter, Color color)
    {
        if (meshFilter != null)
        {
            var mesh = meshFilter.mesh;

            if (mesh != null)
            {
                var colors = mesh.colors;
                if (colors.Length <= 0)
                {
                    colors = new Color[mesh.vertices.Length];
                }
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = color;
                }
                mesh.colors = colors;
            }
        }
    }
}
