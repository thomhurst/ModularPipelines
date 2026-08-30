using ModularPipelines.Secrets;
using ModularPipelines.Ansible.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Ansible.UnitTests.Attributes;

public class AnsibleOptionsTests : TestBase
{
    [Test]
    public async Task Execute_Renders_Required_Pattern_Repeatable_Options_And_Flags()
    {
        var arguments = BuildArguments(new AnsibleExecuteOptions("webservers")
        {
            Background = 30,
            Check = true,
            ExtraVars = ["environment=staging", "@vars.yml"],
            FlushCache = true,
            Inventory = ["hosts.ini", "dynamic.yml"],
            ModuleName = "ping",
            Verbose = 3,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "webservers",
            "--background", "30",
            "--check",
            "--extra-vars", "environment=staging",
            "--extra-vars", "@vars.yml",
            "--flush-cache",
            "--inventory", "hosts.ini",
            "--inventory", "dynamic.yml",
            "--module-name", "ping",
            "--verbose", "--verbose", "--verbose",
        ]);
    }

    [Test]
    public async Task Credential_Options_Are_Marked_As_Secrets()
    {
        var secretProperties = new[]
        {
            nameof(AnsibleExecuteOptions.BecomePasswordFile),
            nameof(AnsibleExecuteOptions.ConnectionPasswordFile),
            nameof(AnsibleExecuteOptions.PrivateKey),
            nameof(AnsibleExecuteOptions.VaultPasswordFile),
        };

        foreach (var propertyName in secretProperties)
        {
            var property = typeof(AnsibleExecuteOptions).GetProperty(propertyName);
            await Assert.That(property!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        }
    }

    [Test]
    public async Task Execute_Rejects_Verbosity_Outside_Declared_Range()
    {
        var builder = await GetService<ICommandLineBuilder>();

        await Assert.That(() => builder.Build(new AnsibleExecuteOptions("webservers")
        {
            Verbose = 7,
        }))
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("AnsibleExecuteOptions.Verbose");
    }
}
