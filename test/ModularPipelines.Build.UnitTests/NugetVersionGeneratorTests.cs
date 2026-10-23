using Microsoft.Extensions.Options;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Settings;
using ModularPipelines.Git;
using ModularPipelines.Git.Models;
using Moq;

namespace ModularPipelines.Build.UnitTests;

public class NugetVersionGeneratorTests
{
    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public async Task Publishing_Requires_An_Expected_Version(string? expectedVersion)
    {
        var (module, context) = CreateModule(true, false, expectedVersion, "4.0.0");

        await Assert.That(async () => { await module.Execute(context); })
            .Throws<InvalidOperationException>().And.HasMessageContaining("expected-version");
    }

    [Test]
    [Arguments(false, "4.0.1", "4.0.0")]
    [Arguments(false, "4.0.0", "4.0.1")]
    [Arguments(false, "4.0.0", "v4.0.0")]
    [Arguments(true, "4.0.0", "4.0.0")]
    public async Task Publishing_Rejects_An_Unexpected_Version(bool isAlpha, string computedVersion, string expectedVersion)
    {
        var (module, context) = CreateModule(true, isAlpha, expectedVersion, computedVersion);

        await Assert.That(async () => { await module.Execute(context); })
            .Throws<InvalidOperationException>().And.HasMessageContaining("does not match expected version");
    }

    [Test]
    [Arguments(false, "4.0.0", "4.0.0")]
    [Arguments(true, "4.0.0-alpha0002", "4.0.0-alpha0002")]
    public async Task Publishing_Returns_The_Matching_Version(bool isAlpha, string expectedVersion, string packageVersion)
    {
        var (module, context) = CreateModule(true, isAlpha, expectedVersion, "4.0.0");

        await Assert.That(await module.Execute(context)).IsEqualTo(packageVersion);
    }

    [Test]
    [Arguments(false, null, "4.0.0")]
    [Arguments(true, null, "4.0.0-alpha0002")]
    [Arguments(false, "different-version", "4.0.0")]
    public async Task Validation_And_Tagging_Do_Not_Require_An_Expected_Version(bool isAlpha, string? expectedVersion, string packageVersion)
    {
        var (module, context) = CreateModule(false, isAlpha, expectedVersion, "4.0.0");

        await Assert.That(await module.Execute(context)).IsEqualTo(packageVersion);
    }

    private static (ExposedVersionModule Module, IModuleContext Context) CreateModule(
        bool shouldPublish, bool isAlpha, string? expectedVersion, string computedVersion)
    {
        var git = new Mock<IGit> { DefaultValue = DefaultValue.Mock };
        git.Setup(x => x.Versioning.GetVersioningInformationAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GitVersionInformation
            {
                FullSemVer = computedVersion,
                CommitsSinceVersionSource = 2,
            });
        var context = new Mock<IModuleContext> { DefaultValue = DefaultValue.Mock };
        context.Setup(x => x.Tools.Get<IGit>()).Returns(git.Object);
        var module = new ExposedVersionModule(Microsoft.Extensions.Options.Options.Create(new PublishSettings
        {
            ShouldPublish = shouldPublish,
            IsAlpha = isAlpha,
            ExpectedVersion = expectedVersion,
        }));
        return (module, context.Object);
    }

    // Exercise the module body independently of its CI capability requirement.
    private class ExposedVersionModule(IOptions<PublishSettings> settings) : NugetVersionGeneratorModule(settings)
    {
        public Task<string> Execute(IModuleContext context) => base.ExecuteAsync(context, CancellationToken.None);
    }
}
