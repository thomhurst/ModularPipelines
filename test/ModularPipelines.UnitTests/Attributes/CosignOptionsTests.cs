using ModularPipelines.Attributes;
using ModularPipelines.Cosign.Enums;
using ModularPipelines.Cosign.Options;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Attributes;

public class CosignOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    [Test]
    public async Task Sign_Renders_Images_Repeated_Annotations_Enum_And_Flags()
    {
        var arguments = BuildArguments(new CosignSignOptions(["registry.example/app:v1", "registry.example/app:v2"])
        {
            Annotations = ["team=platform", "environment=production"],
            Slot = CosignSignSlot.CardAuthentication,
            Upload = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "registry.example/app:v1",
            "registry.example/app:v2",
            "--annotations=team=platform",
            "--annotations=environment=production",
            "--slot=card-authentication",
            "--upload=true",
        ]);
    }

    [Test]
    public async Task Sign_Renders_Explicit_False_For_Default_True_Options()
    {
        var arguments = BuildArguments(new CosignSignOptions(["registry.example/app:v1"])
        {
            Upload = false,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "registry.example/app:v1",
            "--upload=false",
        ]);
    }

    [Test]
    public async Task Login_Renders_Optional_Server_And_Credentials()
    {
        var arguments = BuildArguments(new CosignLoginOptions
        {
            Server = "registry.example",
            Password = "password-value",
            Username = "pipeline-user",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "registry.example",
            "--password=password-value",
            "--username=pipeline-user",
        ]);
    }

    [Test]
    public async Task Credential_Options_Are_Marked_As_Secrets()
    {
        var registryToken = typeof(CosignSignOptions).GetProperty(nameof(CosignSignOptions.RegistryToken));
        var password = typeof(CosignLoginOptions).GetProperty(nameof(CosignLoginOptions.Password));
        var oldManagementKey = typeof(CosignPivToolSetManagementKeyOptions)
            .GetProperty(nameof(CosignPivToolSetManagementKeyOptions.OldKey));

        await Assert.That(registryToken!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(password!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(oldManagementKey!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
