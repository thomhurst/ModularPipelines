namespace ModularPipelines.SourceGenerator.UnitTests;

internal static class SnapshotVerifier
{
    public static async Task VerifyAsync(string snapshotName, string actual)
    {
        var snapshotPath = Path.Combine(
            AppContext.BaseDirectory,
            "Snapshots",
            $"{snapshotName}.verified.txt");
        var expected = await File.ReadAllTextAsync(snapshotPath);

        await Assert.That(Normalize(actual)).IsEqualTo(Normalize(expected));
    }

    private static string Normalize(string value)
    {
        return value.ReplaceLineEndings("\n").TrimEnd();
    }
}
