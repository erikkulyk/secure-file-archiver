using Archiver.Core.Models;
using Xunit;

namespace Archiver.Tests;

public class SmokeTests
{
    [Fact]
    public void FileKind_HasSevenCorpusGroups()
    {
        // Корпус викладача складається з семи груп (розділ 6 умови).
        Assert.Equal(7, Enum.GetValues<FileKind>().Length);
    }

    [Fact]
    public void ArchiveFormatException_KeepsErrorCode()
    {
        var ex = new ArchiveFormatException(ErrorCode.Payload, "bad name");
        Assert.Equal(ErrorCode.Payload, ex.Code);
    }
}
