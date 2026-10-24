namespace ModularPipelines.Testing.UnitTests;

public class InMemoryFileSystemProviderOpenArgumentTests
{
    [Test]
    [MatrixDataSource]
    public Task InvalidModeDoesNotChangeFile(
        [Matrix(-1, 0, 7, 999)] int mode,
        [Matrix(FileAccess.Read, FileAccess.Write, FileAccess.ReadWrite)] FileAccess access,
        [Matrix(false, true)] bool exists) =>
        AssertInvalidOpenPreservesFile((FileMode) mode, access, exists, "mode");

    [Test]
    [MatrixDataSource]
    public Task InvalidAccessDoesNotChangeFile(
        [Matrix(FileMode.CreateNew, FileMode.Create, FileMode.Open, FileMode.OpenOrCreate, FileMode.Truncate, FileMode.Append)] FileMode mode,
        [Matrix(-1, 0, 4, 999)] int access,
        [Matrix(false, true)] bool exists) =>
        AssertInvalidOpenPreservesFile(mode, (FileAccess) access, exists, "access");

    [Test]
    [Arguments(FileMode.Append, FileAccess.Read)]
    [Arguments(FileMode.Append, FileAccess.ReadWrite)]
    [Arguments(FileMode.Create, FileAccess.Read)]
    [Arguments(FileMode.CreateNew, FileAccess.Read)]
    [Arguments(FileMode.Truncate, FileAccess.Read)]
    public async Task InvalidCombinationStillRejectsNamedEnumValues(FileMode mode, FileAccess access)
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "invalid-combination.txt");
        await provider.WriteAllTextAsync(path, "original");

        await Assert.That(() => provider.Open(path, mode, access)).Throws<ArgumentException>();
        await Assert.That(await provider.ReadAllTextAsync(path)).IsEqualTo("original");
    }

    private static async Task AssertInvalidOpenPreservesFile(
        FileMode mode,
        FileAccess access,
        bool exists,
        string parameterName)
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "invalid-open.txt");
        var timestamp = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        if (exists)
        {
            await provider.WriteAllTextAsync(path, "original");
            provider.SetAttributes(path, FileAttributes.Hidden);
            provider.SetCreationTimeUtc(path, timestamp);
            provider.SetLastWriteTimeUtc(path, timestamp.AddDays(1));
            provider.SetLastAccessTimeUtc(path, timestamp.AddDays(2));
        }

        var exception = await Assert.That(() =>
        {
            using var stream = provider.Open(path, mode, access);
        }).Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo(parameterName);
        await Assert.That(provider.FileExists(path)).IsEqualTo(exists);
        if (exists)
        {
            await Assert.That(provider.GetAttributes(path)).IsEqualTo(FileAttributes.Hidden);
            await Assert.That(provider.GetCreationTimeUtc(path)).IsEqualTo(timestamp);
            await Assert.That(provider.GetLastWriteTimeUtc(path)).IsEqualTo(timestamp.AddDays(1));
            await Assert.That(provider.GetLastAccessTimeUtc(path)).IsEqualTo(timestamp.AddDays(2));
            await Assert.That(await provider.ReadAllTextAsync(path)).IsEqualTo("original");
        }
        else
        {
            await Assert.That(() => provider.GetAttributes(path)).Throws<FileNotFoundException>();
        }

        // An exclusive open also verifies that the rejected call did not reserve a handle.
        using var retry = provider.Open(path, exists ? FileMode.Open : FileMode.CreateNew, FileAccess.ReadWrite);
        await Assert.That(retry.CanRead).IsTrue();
        await Assert.That(retry.CanWrite).IsTrue();
        await Assert.That(retry.Length).IsEqualTo(exists ? 8 : 0);
    }
}
