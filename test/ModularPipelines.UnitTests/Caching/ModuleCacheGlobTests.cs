using ModularPipelines.Caching;

namespace ModularPipelines.UnitTests.Caching;

public class ModuleCacheGlobTests
{
    [Test]
    [Arguments("*.cs", "App.cs", true)]
    [Arguments("*.cs", "src/App.cs", false)]
    [Arguments("**/*.cs", "App.cs", true)]
    [Arguments("**/*.cs", "src/nested/App.cs", true)]
    [Arguments("src/**/*.cs", "src/App.cs", true)]
    [Arguments("src/**/*.cs", "src/nested/App.cs", true)]
    [Arguments("src/**/*.cs", "other/App.cs", false)]
    [Arguments("src/**/App.cs", "src/nested/OtherApp.cs", false)]
    [Arguments("src/**/App.cs", "src/nested/App.cs", true)]
    [Arguments("src/**/App.cs", "src/App.cs", true)]
    [Arguments("src/**/App.cs", "src/xApp.cs", false)]
    [Arguments("src/**", "src/nested/App.cs", true)]
    [Arguments("**.cs", "src/nested/App.cs", true)]
    [Arguments("App?.cs", "App1.cs", true)]
    [Arguments("App?.cs", "App.cs", false)]
    [Arguments("App?.cs", "App12.cs", false)]
    [Arguments("src?App.cs", "src/App.cs", false)]
    [Arguments("file[1](a)+$.?", "file[1](a)+$.x", true)]
    [Arguments("file[1](a)+$.?", "file1a.x", false)]
    [Arguments("**/*.cs", "App.cs.bak", false)]
    [Arguments("**/*.cs", "OtherApp.cs", true)]
    [Arguments("**/**/App.cs", "App.cs", true)]
    [Arguments("**/**/App.cs", "src/nested/App.cs", true)]
    [Arguments("a**/b?.cs", "ab1.cs", true)]
    [Arguments("a**/b?.cs", "a/nested/b1.cs", true)]
    [Arguments("***.cs", "src/App.cs", true)]
    public async Task ArtifactScopeUsesWholePathGlobSemantics(string pattern, string relativePath, bool expected)
    {
        var root = Path.GetTempPath();
        var matches = ModuleCacheFileResolver.IsWithinDeclaredArtifactScope(
            root,
            Path.Combine(root, relativePath),
            [pattern]);

        await Assert.That(matches).IsEqualTo(expected);
    }

    [Test]
    public async Task AmbiguousWildcardsRejectNonMatchingPathWithoutBacktrackingTimeout()
    {
        var root = Path.GetTempPath();
        var pattern = string.Concat(Enumerable.Repeat("*a", 24)) + "*b*";
        var relativePath = new string('a', 64) + "c";

        var matches = ModuleCacheFileResolver.IsWithinDeclaredArtifactScope(
            root,
            Path.Combine(root, relativePath),
            [pattern]);

        await Assert.That(matches).IsFalse();
    }
    [Test]
    [Arguments(false, false)]
    [Arguments(true, true)]
    public async Task MatchingUsesConfiguredPathCaseComparison(bool ignoreCase, bool expected)
    {
        var glob = new ModuleCacheGlob("SRC/**/ÄPP?.CS", ignoreCase);

        await Assert.That(glob.IsMatch("src/nested/äpp1.cs")).IsEqualTo(expected);
    }

    [Test]
    public async Task CaseInsensitiveLiteralsHandleSupplementaryUnicodeCharacters()
    {
        var glob = new ModuleCacheGlob("**/\U00010400.txt", ignoreCase: true);

        await Assert.That(glob.IsMatch("nested/\U00010428.txt")).IsTrue();
    }

    [Test]
    [Arguments("App.cs", true)]
    [Arguments("App.txt", false)]
    public async Task RecursiveGlobHandlesLongPaths(string fileName, bool expected)
    {
        var path = string.Join('/', Enumerable.Repeat("segment", 80)) + "/" + fileName;
        var glob = new ModuleCacheGlob("**/App.cs", ignoreCase: false);

        await Assert.That(glob.IsMatch(path)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("**", "", true)]
    [Arguments("**/", "", true)]
    [Arguments("**/", "nested/", true)]
    [Arguments("**/", "nested", false)]
    [Arguments("?", "", false)]
    [Arguments("*", "", true)]
    [Arguments("long?", "x", false)]
    [Arguments("*.cs", "App.cs\n", false)]
    [Arguments("**/*.cs", "nested\n/App.cs", true)]
    public async Task GlobMatchesCompletePaths(string pattern, string path, bool expected)
    {
        var glob = new ModuleCacheGlob(pattern, ignoreCase: false);

        await Assert.That(glob.IsMatch(path)).IsEqualTo(expected);
    }

    [Test]
    public async Task FileExpansionRejectsAmbiguousNonMatchAndKeepsMatchingFiles()
    {
        var root = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-wildcards-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);

        try
        {
            var prefix = new string('a', 64);
            var matchingFile = Path.Combine(root, prefix + "b");
            await System.IO.File.WriteAllTextAsync(matchingFile, "match");
            await System.IO.File.WriteAllTextAsync(Path.Combine(root, prefix + "c"), "non-match");
            var pattern = string.Concat(Enumerable.Repeat("*a", 24)) + "*b*";

            var files = ModuleCacheFileResolver.ResolveFiles(root, [pattern], maximumFiles: 1);

            await Assert.That(files).IsEquivalentTo(new[] { matchingFile });
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
