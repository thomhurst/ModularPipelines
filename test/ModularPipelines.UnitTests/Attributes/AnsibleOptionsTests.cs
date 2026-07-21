using ModularPipelines.Ansible.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Attributes;

public class AnsibleOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    [Test]
    public async Task Execute_Renders_Required_Pattern_Repeatable_Options_And_Flags()
    {
        var arguments = BuildArguments(new AnsibleExecuteOptions("webservers")
        {
            Background = 30,
            Check = true,
            ExtraVars = ["environment=staging", "@vars.yml"],
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

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
