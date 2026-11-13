namespace Archiver.Core.Models;

/// <summary>Сім груп корпусу (розділ 6 умови).</summary>
public enum FileKind
{
    NaturalText,
    SourceCode,
    StructuredData,
    Raster,
    Executable,
    AlreadyCompressed,
    Synthetic
}

/// <summary>Результат аналізу файлу: тип і стратегія стиснення.</summary>
public readonly record struct FileAnalysis(FileKind Kind, bool Compress, int BlockSize);

/// <summary>Елемент потоку LZ77: літерал або посилання «назад на Distance, взяти Length байтів».</summary>
public abstract record LzToken;
public sealed record Literal(byte Value) : LzToken;
public sealed record Match(int Distance, int Length) : LzToken;

/// <summary>Стиснений блок. IsRaw = true означає NO_COMPRESSION (coded_len = raw_len).</summary>
public sealed record CompressedBlock(bool IsRaw, int RawLength, byte[] Payload);

/// <summary>Одиниця пакування в том: блок файла з цінністю для рюкзака. Вага = CodedLength + 15 байтів.</summary>
public sealed record PackItem(int FileIndex, int BlockIndex, int CodedLength, int Value);

/// <summary>Параметри створення архіву.</summary>
public sealed record ArchiveOptions(string Password, int MaxVolumeBytes, int BlockSize);

/// <summary>Коди відмов розархіватора (повний список беремо з FORMAT_SPEC_P2.md та expected_failures.json).</summary>
public enum ErrorCode
{
    Header,
    Hmac,
    Payload,
    Volume,
    Config
}

public sealed class ArchiveFormatException : Exception
{
    public ErrorCode Code { get; }

    public ArchiveFormatException(ErrorCode code, string message) : base(message)
    {
        Code = code;
    }
}
