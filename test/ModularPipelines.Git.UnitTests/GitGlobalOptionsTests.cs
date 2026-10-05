using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Git.UnitTests;

public class GitGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Values_With_Spaces_Keep_Argument_Boundaries_And_Are_Registered_As_Secrets()
    {
        var options = new GitStatusOptions
        {
            ChangeDirectories = ["parent directory", "child directory"],
            Configuration = [("http.extraHeader", "Authorization: Bearer test-secret")],
        };
        var builder = await GetService<ICommandLineBuilder>();
        await Assert.That(builder.Build(options).Arguments.SequenceEqual([
            "-C", "parent directory", "-C", "child directory",
            "-c", "http.extraHeader=Authorization: Bearer test-secret", "status",
        ])).IsTrue();
        var secrets = await GetService<ISecretProvider>();
        await Assert.That(secrets.GetSecretsInObject(options))
            .Contains("Authorization: Bearer test-secret");
    }

    [Test]
    public async Task Repeated_Global_Values_Precede_Command_And_Local_Options()
    {
        var command = await RenderCommand(new GitStatusOptions
        {
            ChangeDirectories = ["parent", "child"],
            Configuration = [("custom.value", "first"), ("custom.value", "second")],
            Short = true,
        });
        await Assert.That(command).IsEqualTo("git -C parent -C child -c custom.value=first -c custom.value=second status --short");
    }

    [Test]
    public async Task Root_And_Command_Local_Git_Directory_Switches_Have_Distinct_Scopes()
    {
        var command = await RenderCommand(new GitRevParseOptions
        {
            GitDirectory = "repository.git",
            ConfigEnv = ["custom.value=CONFIG_VALUE", "custom.other=OTHER_VALUE"],
            GitDir = true,
        });
        await Assert.That(command).Contains("--git-dir=repository.git");
        await Assert.That(command).Contains("--config-env=custom.value=CONFIG_VALUE --config-env=custom.other=OTHER_VALUE");
        await Assert.That(command).EndsWith("rev-parse --git-dir");
    }

    [Test]
    public async Task Root_Version_Remains_A_Control_Action()
    {
        await Assert.That(await RenderCommand(new GitBaseOptions { Version = true, NoPager = true }))
            .IsEqualTo("git --no-pager --version");
        await Assert.That(typeof(GitOptions).GetProperty("Version")).IsNull();
        await Assert.That(typeof(GitOptions).GetProperty("HtmlPath")).IsNull();
        await Assert.That(typeof(GitOptions).GetProperty("Configuration")!.GetCustomAttribute<SecretValueAttribute>()).IsNotNull();
        await Assert.That(typeof(GitOptions).GetCustomAttribute<CliGlobalOptionsAttribute>()).IsNotNull();
    }

    [Test]
    public async Task Shared_Options_Are_Inherited_Once()
    {
        var shared = typeof(GitStatusOptions).GetProperties()
            .Where(property => property.Name == nameof(GitOptions.NoOptionalLocks)).ToArray();
        await Assert.That(shared).HasSingleItem();
        await Assert.That(shared[0].DeclaringType).IsEqualTo(typeof(GitOptions));
        await Assert.That(await RenderCommand(new GitStatusOptions { NoOptionalLocks = true, NoAdvice = true }))
            .IsEqualTo("git --no-optional-locks --no-advice status");
    }
}
