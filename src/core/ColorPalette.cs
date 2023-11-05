using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class ColorPalette : Resource
    {
        [Export]
        public Color Color01 = new Color(1.0f, 0.0f, 1.0f, 1.0f); // Pink

        [Export]
        public Color Color02 = new Color(0.0f, 1.0f, 1.0f, 1.0f); // Cyan

        [Export]
        public Color Color03 = new Color(1.0f, 0.5f, 0.0f, 1.0f); // Orange

        [Export]
        public Color Color04 = new Color(0.0f, 1.0f, 0.0f, 1.0f); // Green

        [Export]
        public Color Color05 = new Color(1.0f, 1.0f, 0.0f, 1.0f); // Yellow

        [Export]
        public Color Color06 = new Color(0.0f, 0.0f, 1.0f, 1.0f); // Blue

        [Export]
        public Color Color07 = new Color(1.0f, 0.0f, 0.0f, 1.0f); // Red

        [Export]
        public Color Color14 = new Color(0.5f, 0.5f, 0.5f, 1.0f); // Gray

        [Export]
        public Color Color15 = new Color(1.0f, 1.0f, 1.0f, 1.0f); // White

        [Export]
        public Color Color16 = new Color(0.0f, 0.0f, 0.0f, 1.0f); // Black

        public Color GetColor(ColorPaletteChoices color)
        {
            switch (color)
            {
                case ColorPaletteChoices.Default:
                    return Color01;
                case ColorPaletteChoices.Color01:
                    return Color01;
                case ColorPaletteChoices.Color02:
                    return Color02;
                case ColorPaletteChoices.Color03:
                    return Color03;
                case ColorPaletteChoices.Color04:
                    return Color04;
                case ColorPaletteChoices.Color05:
                    return Color05;
                case ColorPaletteChoices.Color06:
                    return Color06;
                case ColorPaletteChoices.Color07:
                    return Color07;
                case ColorPaletteChoices.Color14:
                    return Color14;
                case ColorPaletteChoices.Color15:
                    return Color15;
                case ColorPaletteChoices.Color16:
                    return Color16;
                default:
                    return Color01;
            }
        }
    }

    public enum ColorPaletteChoices
    {
        Default,
        Color01,
        Color02,
        Color03,
        Color04,
        Color05,
        Color06,
        Color07,

        Color14,
        Color15,
        Color16,
    }
}
