namespace SyScreenshoter;

class Primitive
{
    public static readonly Primitive UNDO_DELIMITER = new Primitive { Kind = PrimitiveKind.UndoDelimiter };

    public PrimitiveKind Kind;
    public Point Pt0;
    public Point Pt1;
    public string? Text;
}