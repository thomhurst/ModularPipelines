using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.Engine;
using ModularPipelines.FileSystem;
using ModularPipelines.Secrets;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class BuildSystemContextTests
{
    [Test]
    [Arguments(BuildSystem.GitHubActions, "::warning file=src%3A%2C%25.cs,line=42::first%0Asecond%25;]'|")]
    [Arguments(BuildSystem.AzurePipelines, "##vso[task.logissue type=warning;sourcepath=src:,%AZP25.cs;linenumber=42;]first%0Asecond%AZP25%3B%5D'|")]
    [Arguments(BuildSystem.TeamCity, "##teamcity[message text='src:,%.cs(42): first|nsecond%;|]|'||' status='WARNING']")]
    public async Task AnnotationsEscapeMessageAndLocation(BuildSystem provider, string expected)
    {
        using var output = new StringWriter();
        var context = CreateContext(provider, output);

        await context.AnnotateAsync(BuildAnnotationLevel.Warning, "first\nsecond%;]'|", "src:,%.cs", 42);

        await Assert.That(output.ToString()).IsEqualTo(expected + Environment.NewLine);
    }

    [Test]
    [Arguments(BuildSystem.AzurePipelines, "##vso[build.updatebuildnumber]1%AZP25%3B%5D%0A'|")]
    [Arguments(BuildSystem.TeamCity, "##teamcity[buildNumber '1%;|]|n|'||']")]
    public async Task BuildNumbersUseProviderEscaping(BuildSystem provider, string expected)
    {
        using var output = new StringWriter();
        await CreateContext(provider, output).SetBuildNumberAsync("1%;]\n'|");
        await Assert.That(output.ToString()).IsEqualTo(expected + Environment.NewLine);
    }

    [Test]
    [Arguments(BuildSystem.GitHubActions, BuildAnnotationLevel.Notice, "::notice::message")]
    [Arguments(BuildSystem.GitHubActions, BuildAnnotationLevel.Error, "::error::message")]
    [Arguments(BuildSystem.AzurePipelines, BuildAnnotationLevel.Error, "##vso[task.logissue type=error;]message")]
    [Arguments(BuildSystem.TeamCity, BuildAnnotationLevel.Notice, "##teamcity[message text='message' status='NORMAL']")]
    [Arguments(BuildSystem.TeamCity, BuildAnnotationLevel.Error, "##teamcity[message text='message' status='ERROR']")]
    public async Task AnnotationsMapSeverity(BuildSystem provider, BuildAnnotationLevel level, string expected)
    {
        using var output = new StringWriter();
        await CreateContext(provider, output).AnnotateAsync(level, "message");
        await Assert.That(output.ToString()).IsEqualTo(expected + Environment.NewLine);
    }

    [Test]
    public async Task UnsupportedOperationsAndMissingGitHubFilesFailExplicitly()
    {
        using var output = new StringWriter();
        var github = CreateContext(BuildSystem.GitHubActions, output);
        await Assert.That(() => github.SetBuildNumberAsync("1")).Throws<NotSupportedException>();
        await Assert.That(() => github.SetOutputAsync("value", "1")).Throws<InvalidOperationException>();
        await Assert.That(() => github.AppendSummaryAsync("summary")).Throws<InvalidOperationException>();
        await Assert.That(() => CreateContext(BuildSystem.TeamCity, output).AppendSummaryAsync("summary"))
            .Throws<NotSupportedException>();
        await Assert.That(() => CreateContext(BuildSystem.AzurePipelines, output).AnnotateAsync(BuildAnnotationLevel.Notice, "notice"))
            .Throws<NotSupportedException>();
        await Assert.That(() => CreateContext(BuildSystem.GitLab, output).SetOutputAsync("value", "1"))
            .Throws<NotSupportedException>();
        var unknownCi = CreateContext(BuildSystem.Unknown, output, isCi: true);
        await Assert.That(() => unknownCi.SetBuildNumberAsync("1")).Throws<NotSupportedException>();
        await Assert.That(output.ToString()).IsEmpty();
    }

    [Test]
    [Arguments(BuildSystem.Unknown)]
    [Arguments(BuildSystem.GitHubActions)]
    [Arguments(BuildSystem.AzurePipelines)]
    [Arguments(BuildSystem.TeamCity)]
    public async Task CancelledOperationsHaveNoSideEffects(BuildSystem provider)
    {
        using var output = new StringWriter();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var context = CreateContext(provider, output);
        await Assert.That(() => context.SetOutputAsync("value", "1", cancellation.Token)).Throws<OperationCanceledException>();
        await Assert.That(() => context.AppendSummaryAsync("summary", cancellation.Token)).Throws<OperationCanceledException>();
        await Assert.That(() => context.AnnotateAsync(BuildAnnotationLevel.Error, "error", cancellationToken: cancellation.Token))
            .Throws<OperationCanceledException>();
        await Assert.That(() => context.SetBuildNumberAsync("1", cancellation.Token)).Throws<OperationCanceledException>();
        await Assert.That(output.ToString()).IsEmpty();
    }

    [Test]
    public async Task InvalidArgumentsCannotInjectCommands()
    {
        using var output = new StringWriter();
        var context = CreateContext(BuildSystem.GitHubActions, output);
        await Assert.That(() => context.SetOutputAsync("name\nEOF", "value")).Throws<ArgumentException>();
        await Assert.That(() => context.SetOutputAsync("name<<EOF", "value")).Throws<ArgumentException>();
        await Assert.That(() => context.AnnotateAsync(BuildAnnotationLevel.Error, "error", line: 1)).Throws<ArgumentException>();
        await Assert.That(() => context.AnnotateAsync(BuildAnnotationLevel.Error, "error", "file", 0)).Throws<ArgumentException>();
        await Assert.That(() => context.AnnotateAsync((BuildAnnotationLevel) 99, "error")).Throws<ArgumentOutOfRangeException>();
        await Assert.That(output.ToString()).IsEmpty();
    }

    [Test]
    [Arguments(BuildSystem.AzurePipelines)]
    [Arguments(BuildSystem.TeamCity)]
    public async Task CommandContentIsMaskedBeforeEscaping(BuildSystem provider)
    {
        using var output = new StringWriter();
        var obfuscator = CreateSecretObfuscator("secret\nvalue", "secret-name");
        var context = CreateContext(provider, output, obfuscator: obfuscator);

        await context.SetOutputAsync("result", "secret\nvalue");
        await context.AnnotateAsync(BuildAnnotationLevel.Error, "secret\nvalue", "secret\nvalue");
        await context.SetBuildNumberAsync("secret\nvalue");
        await Assert.That(output.ToString()).DoesNotContain("secret");
        await Assert.That(output.ToString()).Contains("***");
        await Assert.That(() => context.SetOutputAsync("secret\nvalue", "value")).Throws<ArgumentException>();
        await Assert.That(() => context.SetOutputAsync("secret-name", "value")).Throws<ArgumentException>();
    }

    [Test]
    public async Task FileContentIsMaskedAndAzureSummaryFileRemainsAvailable()
    {
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            var outputPath = Path.Combine(directory.FullName, "output");
            var variables = new Mock<IEnvironmentVariablesContext>();
            variables.Setup(x => x.Get("AGENT_TEMPDIRECTORY", It.IsAny<EnvironmentVariableTarget>())).Returns(directory.FullName);
            variables.Setup(x => x.Get("GITHUB_OUTPUT", It.IsAny<EnvironmentVariableTarget>())).Returns(outputPath);
            var obfuscator = CreateSecretObfuscator("private-token");
            using var output = new StringWriter();
            await CreateContext(BuildSystem.GitHubActions, output, variables.Object, obfuscator)
                .SetOutputAsync("value", "private-token");
            var azure = CreateContext(BuildSystem.AzurePipelines, output, variables.Object, obfuscator);
            await azure.AppendSummaryAsync("first private-token");
            await azure.AppendSummaryAsync("second private-token");

            await Assert.That(await File.ReadAllTextAsync(outputPath)).DoesNotContain("private-token");
            var paths = Directory.GetFiles(directory.FullName, "*.md");
            await Assert.That(paths.Length).IsEqualTo(2);
            foreach (var path in paths)
            {
                await Assert.That(await File.ReadAllTextAsync(path)).Contains("***");
                await Assert.That(output.ToString()).Contains($"##vso[task.uploadsummary]{path}");
            }
        }
        finally
        {
            directory.Delete(true);
        }
    }

    [Test]
    public async Task GitHubSummaryEnforcesCombinedUtf8LimitWithParallelAppends()
    {
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            var path = Path.Combine(directory.FullName, "summary");
            await File.WriteAllTextAsync(path, new string('x', (1024 * 1024) - 10));
            var variables = new Mock<IEnvironmentVariablesContext>();
            variables.Setup(x => x.Get("GITHUB_STEP_SUMMARY", It.IsAny<EnvironmentVariableTarget>())).Returns(path);
            using var output = new StringWriter();
            var context = CreateContext(BuildSystem.GitHubActions, output, variables.Object);
            var results = await Task.WhenAll(Enumerable.Range(0, 10).Select(async _ =>
            {
                try
                {
                    await context.AppendSummaryAsync("é"); // Two UTF-8 bytes plus newline.
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }));

            await Assert.That(results.Count(x => x)).IsEqualTo(3);
            await Assert.That(new FileInfo(path).Length).IsEqualTo((1024L * 1024) - 1);
        }
        finally
        {
            directory.Delete(true);
        }
    }

    [Test]
    [Arguments(BuildSystem.AzurePipelines, "##vso[task.setvariable variable=result;isOutput=true;]first%0Asecond%AZP25%3B%5D")]
    [Arguments(BuildSystem.TeamCity, "##teamcity[setParameter name='result' value='first|nsecond%;|]']")]
    public async Task OutputUsesProviderEscaping(BuildSystem provider, string expected)
    {
        using var output = new StringWriter();
        var context = CreateContext(provider, output);

        await context.SetOutputAsync("result", "first\nsecond%;]");

        await Assert.That(output.ToString()).IsEqualTo(expected + Environment.NewLine);
    }

    [Test]
    public async Task LocalOperationsDoNotWriteCommandsOrFiles()
    {
        using var output = new StringWriter();
        var context = CreateContext(BuildSystem.Unknown, output);

        await context.SetOutputAsync("result", "value");
        await context.AppendSummaryAsync("summary");
        await context.AnnotateAsync(BuildAnnotationLevel.Error, "error", "file.cs", 2);
        await context.SetBuildNumberAsync("1.2.3");

        await Assert.That(output.ToString()).IsEmpty();
    }

    [Test]
    public async Task GitHubOutputPreservesMultilineValuesAndSerializesParallelWrites()
    {
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            var path = Path.Combine(directory.FullName, "output");
            var variables = new Mock<IEnvironmentVariablesContext>();
            variables.Setup(x => x.Get("GITHUB_OUTPUT", It.IsAny<EnvironmentVariableTarget>())).Returns(path);
            using var output = new StringWriter();
            var context = CreateContext(BuildSystem.GitHubActions, output, variables.Object);
            await Task.WhenAll(Enumerable.Range(0, 20)
                .Select(i => context.SetOutputAsync($"value{i}", "first\nEOF\nsecond\r\nlast")));

            await Assert.That(await File.ReadAllTextAsync(path)).Contains("first\nEOF\nsecond\r\nlast");
            var lines = (await File.ReadAllLinesAsync(path)).Where(x => x.Length > 0).ToArray();
            await Assert.That(lines.Length).IsEqualTo(120);
            for (var i = 0; i < lines.Length; i += 6)
            {
                var delimiter = lines[i].Split("<<")[1];
                await Assert.That(lines[i + 1]).IsEqualTo("first");
                await Assert.That(lines[i + 2]).IsEqualTo("EOF");
                await Assert.That(lines[i + 3]).IsEqualTo("second");
                await Assert.That(lines[i + 4]).IsEqualTo("last");
                await Assert.That(lines[i + 5]).IsEqualTo(delimiter);
            }
        }
        finally
        {
            directory.Delete(true);
        }
    }

    private static ISecretObfuscator CreateSecretObfuscator(params string[] secrets)
    {
        var provider = new Mock<ISecretProvider>();
        provider.Setup(x => x.GetSnapshot()).Returns(new SecretSnapshot(0, secrets));
        return new SecretObfuscator(provider.Object, Microsoft.Extensions.Options.Options.Create(
            new SecretMaskingOptions { MaskValue = "***" }));
    }

    private static BuildSystemContext CreateContext(
        BuildSystem provider,
        StringWriter output,
        IEnvironmentVariablesContext? variables = null,
        ISecretObfuscator? obfuscator = null,
        bool? isCi = null)
    {
        var detector = new Mock<IBuildSystemDetector>();
        detector.SetupGet(x => x.Current).Returns(provider);
        detector.SetupGet(x => x.IsCI).Returns(isCi ?? provider != BuildSystem.Unknown);
        if (obfuscator is null)
        {
            var mock = new Mock<ISecretObfuscator>();
            mock.Setup(x => x.Obfuscate(It.IsAny<string>(), null)).Returns((string text, object? _) => text);
            obfuscator = mock.Object;
        }

        var files = SystemFileSystemProvider.Instance;
        return new BuildSystemContext(detector.Object, variables ?? Mock.Of<IEnvironmentVariablesContext>(),
            new BuildSystemCommandWriter(output), new BuildSystemFormatterProvider(detector.Object), obfuscator,
            new BuildSystemFileWriter(files, obfuscator), files);
    }
}
