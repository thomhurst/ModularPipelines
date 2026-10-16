using ModularPipelines.Build.Helpers;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Services;
using ModularPipelines.FileSystem;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.DotNet.UnitTests;

public class NugetUploadHelperTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Retry_Explicitly_Includes_Existing_Symbols(bool hasSymbols)
    {
        var directory = Directory.CreateTempSubdirectory("nuget upload ");
        try
        {
            var packagePath = Path.Combine(directory.FullName, "Example.1.0.0.nupkg");
            var symbolsPath = Path.ChangeExtension(packagePath, ".snupkg");
            await File.WriteAllTextAsync(packagePath, "package");
            if (hasSymbols)
            {
                await File.WriteAllTextAsync(symbolsPath, "symbols");
            }

            using var cancellation = new CancellationTokenSource();
            DotNetNuGetPushOptions? capturedOptions = null;
            var duplicateResult = CommandResult.Ok("Package already exists; skipping duplicate.");
            var nuget = new Mock<IDotNetNuGet>(MockBehavior.Strict);
            nuget.Setup(x => x.PushAsync(It.IsAny<DotNetNuGetPushOptions>(), null, cancellation.Token))
                .Callback<DotNetNuGetPushOptions, CommandExecutionOptions?, CancellationToken>(
                    (options, _, _) => capturedOptions = options)
                .ReturnsAsync(duplicateResult);
            var dotnet = Mock.Of<IDotNet>(x => x.NuGet == nuget.Object);
            var tools = Mock.Of<IToolsContext>(x => x.Get<IDotNet>() == dotnet);
            var context = Mock.Of<IModuleContext>(x => x.Tools == tools);

            var results = await NugetUploadHelper.UploadPackagesAsync(
                context, [new FilePath(packagePath)], "https://api.nuget.org/v3/index.json", "test-key", cancellation.Token);

            var options = capturedOptions!;
            string[] expectedPaths = hasSymbols ? [packagePath, symbolsPath] : [packagePath];
            await Assert.That(options.PackagePaths.SequenceEqual(expectedPaths)).IsTrue();
            await Assert.That(options.SkipDuplicate).IsTrue();
            await Assert.That(options.NoSymbols).IsNotEqualTo(true);
            await Assert.That(options.Source).IsEqualTo("https://api.nuget.org/v3/index.json");
            await Assert.That(options.ApiKey).IsEqualTo("test-key");
            await Assert.That(results.Single()).IsEqualTo(duplicateResult);
            nuget.Verify(x => x.PushAsync(It.IsAny<DotNetNuGetPushOptions>(), null, cancellation.Token), Times.Once);
            nuget.VerifyNoOtherCalls();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
