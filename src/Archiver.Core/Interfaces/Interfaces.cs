using Archiver.Core.Models;

namespace Archiver.Core.Interfaces;

// Це чернетка інтерфейсів між модулями (Зріз 0). Команда погоджує їх до початку реалізації.
// Кожен інтерфейс належить одній зоні відповідальності.

/// <summary>Зона «Аналіз файлу і оптимізація». Визначає тип файлу і стратегію.</summary>
public interface IFileAnalyzer
{
    FileAnalysis Analyze(byte[] data);
}

/// <summary>Зона «Стиснення і розбір потоку». Жадібний або лінивий розбір на елементи LZ77.</summary>
public interface IStreamParser
{
    IReadOnlyList<LzToken> Parse(byte[] data);
}

/// <summary>Зона «Стиснення і розбір потоку». Гаффман над елементами LZ77, плюс режим без стиснення.</summary>
public interface IBlockCompressor
{
    CompressedBlock Compress(byte[] raw, FileAnalysis plan);
    byte[] Decompress(CompressedBlock block);
}

/// <summary>Зона «Захист і пакування в томи». Рюкзак 0/1: які блоки потрапляють у том.</summary>
public interface IVolumePacker
{
    /// <summary>Повертає томи; кожен том є множиною блоків. Вага блока = CodedLength + 15.</summary>
    IReadOnlyList<IReadOnlyList<PackItem>> Pack(IReadOnlyList<PackItem> items, int volumeCapacity);
}

/// <summary>Зона «Захист і пакування в томи». Encrypt-then-MAC: AES-256-CTR, потім HMAC-SHA256.</summary>
public interface IArchiveCrypto
{
    byte[] Seal(byte[] plaintext, int volumeIndex);

    /// <summary>Перевіряє HMAC до розшифрування; при невдачі кидає ArchiveFormatException.</summary>
    byte[] Open(byte[] sealedData, int volumeIndex);
}

/// <summary>Зона «Інтеграція». Наскрізний тракт створення і розпакування.</summary>
public interface IArchiveWriter
{
    void Create(IReadOnlyList<string> inputFiles, string outputPrefix, ArchiveOptions options);
}

public interface IArchiveReader
{
    void Extract(IReadOnlyList<string> volumePaths, string outputDir, string password);
}
