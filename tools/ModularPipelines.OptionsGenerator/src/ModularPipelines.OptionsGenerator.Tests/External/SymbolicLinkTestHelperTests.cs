using TUnit.Core.Exceptions;

namespace ModularPipelines.OptionsGenerator.Tests.External;

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
    public async Task Missing_Privilege_Skips_Only_On_Windows_And_Preserves_Cleanup()
    {
        var workspace = Path.Combine(Path.GetTempPath(), $"OptionsGenerator-symlink-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workspace);
        var exception = new IOException("Missing privilege", unchecked((int) 0x80070522));

        void CreateLinkWithCleanup()
        {
            try
            {
                SymbolicLinkTestHelper.CreateOrSkip(() => throw exception);
            }
            finally
            {
                Directory.Delete(workspace, recursive: true);
            }
        }

        if (OperatingSystem.IsWindows())
        {
            await Assert.That(CreateLinkWithCleanup)
                .Throws<SkipTestException>().And.HasMessageContaining("ERROR_PRIVILEGE_NOT_HELD");
        }
        else
        {
            await Assert.That(CreateLinkWithCleanup).Throws<IOException>();
        }

        await Assert.That(Directory.Exists(workspace)).IsFalse();
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
