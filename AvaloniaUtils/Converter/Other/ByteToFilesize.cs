namespace MSHC.Avalonia.Converter;

/// <summary>
/// Converts a byte count to a human-readable file size string.
/// </summary>
public class ByteToFilesize : OneWayConverter<long, string>
{
    protected override string Convert(long byteCount, object? parameter) => Convert(byteCount);

    public static string Convert(long byteCount)
    {
        string[] suf = ["byte", "KB", "MB", "GB", "TB", "PB", "EB"];
        if (byteCount == 0) return "0 " + suf[0];
        var bytes = System.Math.Abs(byteCount);
        var place = System.Convert.ToInt32(System.Math.Floor(System.Math.Log(bytes, 1024)));
        var num = System.Math.Round(bytes / System.Math.Pow(1024, place), 1);
        return (System.Math.Sign(byteCount) * num) + " " + suf[place];
    }
}
