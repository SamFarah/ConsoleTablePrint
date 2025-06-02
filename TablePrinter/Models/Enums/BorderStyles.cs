namespace TablePrinter.Models.Enums;

/// <summary>
/// Defines the available border styles for table rendering.
/// Each style corresponds to a specific set of Unicode or ASCII characters
/// used to draw the table borders and corners.
/// </summary>
public enum BorderStyles
{
    /// <summary>
    /// A single line border using standard box-drawing characters.
    /// Uses the characters: ┌ ┐ └ ┘ ─ │ ┬ ┴ ┤ ├ ┼
    /// </summary>
    SingleLine,

    /// <summary>
    /// A bold single line border with heavier weight box-drawing characters.
    /// Uses the characters: ┏ ┓ ┗ ┛ ━ ┃ ┳ ┻ ┫ ┣ ╋
    /// <remark> This style requires <see cref=" System.Text.Encoding.UTF8"/> encoding that will be set automatically.</remark>
    /// </summary>
    SingleBoldLine,

    /// <summary>
    /// A double line border style using double-line box-drawing characters.
    /// Uses the characters: ╔ ╗ ╚ ╝ ═ ║ ╦ ╩ ╣ ╠ ╬
    /// </summary>
    DoubleLine,

    /// <summary>
    /// A mixed style with double lines on vertical edges and single lines on horizontal edges.
    /// Uses the characters: ╓ ╖ ╙ ╜ ─ ║ ╥ ╨ ╢ ╟ ╫
    /// </summary>
    DoubleToSingleLine,

    /// <summary>
    /// A mixed style with single lines on vertical edges and double lines on horizontal edges.
    /// Uses the characters: ╒ ╕ ╘ ╛ ═ │ ╤ ╧ ╡ ╞ ╪
    /// </summary>
    SingleToDoubleLine,

    /// <summary>
    /// A single dashed line border style.
    /// Uses the characters: ┌ ┐ └ ┘ ╌ ╎ ┬ ┴ ┤ ├ ┼
    /// <remark> This style requires <see cref=" System.Text.Encoding.UTF8"/> encoding that will be set automatically.</remark>
    /// </summary>
    SingleDashedLine,

    /// <summary>
    /// A bold single dashed line border style.
    /// Uses the characters: ┏ ┓ ┗ ┛ ╍ ╏ ┳ ┻ ┫ ┣ ╋
    /// <remark> This style requires <see cref=" System.Text.Encoding.UTF8"/> encoding that will be set automatically.</remark>
    /// </summary>
    SingleDashedBoldLine,

    /// <summary>
    /// A single curved line border style using rounded corners.
    /// Uses the characters: ╭ ╮ ╰ ╯ ─ │ ┬ ┴ ┤ ├ ┼
    /// <remark> This style requires <see cref=" System.Text.Encoding.UTF8"/> encoding that will be set automatically.</remark>
    /// </summary>
    SingleCurvedLine,

    /// <summary>
    /// A simple ASCII border style using only basic characters.
    /// Uses the characters: - │
    /// </summary>
    GoodOldAscii,

    /// <summary>
    /// An improved ASCII border style using plus, dash, and pipe characters.
    /// Uses the characters: + - │
    /// </summary>
    ImprovedAscii
}
