using ModularPipelines.UnitTests.Attributes;
using TUnit.Core.Exceptions;

namespace ModularPipelines.UnitTests.FileSystem;

public class SymbolicLinkTestHelperTests
{
    [Test]
    public async Task Executes_Link_Creation_When_Available()
    {
        var created = false;

        SymbolicLinkTestHelper.CreateOrSkip(() => created = true);

        await Assert.That(created).IsTrue();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Skips_Only_When_Windows_Symbolic_Link_Privilege_Is_Missing()
    {
        var exception = new IOException("Missing privilege", unchecked((int) 0x80070522));

        await Assert.That(() => SymbolicLinkTestHelper.CreateOrSkip(() => throw exception))
            .Throws<SkipTestException>().And.HasMessageContaining("ERROR_PRIVILEGE_NOT_HELD");
    }

    [Test]
    [Arguments(unchecked((int) 0x80070005))] // Access denied
    [Arguments(unchecked((int) 0x800700B7))] // Link already exists
    [Arguments(unchecked((int) 0x80004005))] // Unspecified failure
    public async Task Does_Not_Skip_Unrelated_Io_Failures(int errorCode)
    {
        var exception = new IOException("Link creation failed", errorCode);

        await Assert.That(() => SymbolicLinkTestHelper.CreateOrSkip(() => throw exception))
            .Throws<IOException>();
    }

    [Test]
    public async Task Does_Not_Skip_Access_Denied()
    {
        await Assert.That(() => SymbolicLinkTestHelper.CreateOrSkip(() => throw new UnauthorizedAccessException()))
            .Throws<UnauthorizedAccessException>();
    }
}
