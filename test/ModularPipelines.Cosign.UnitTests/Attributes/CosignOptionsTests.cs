using ModularPipelines.Secrets;
using ModularPipelines.Attributes;
using ModularPipelines.Cosign.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Cosign.UnitTests.Attributes;

public class CosignOptionsTests
{
    [Test]
    public async Task Sign_Renders_Images_Repeated_Annotations_And_Flags()
    {
        var arguments = BuildArguments(new CosignSignOptions(["registry.example/app:v1", "registry.example/app:v2"])
        {
            Annotations = ["team=platform", "environment=production"],
            Slot = "card-authentication",
            Upload = true,
        });

        await AssertArguments(arguments,
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
            UseSigningConfig = false,
        });

        await AssertArguments(arguments,
        [
            "registry.example/app:v1",
            "--upload=false",
            "--use-signing-config=false",
        ]);
    }

    [Test]
    public async Task Verify_Renders_Custom_Predicate_Uri_And_Explicit_False()
    {
        var arguments = BuildArguments(new CosignVerifyAttestationOptions(["registry.example/app:v1"])
        {
            Type = "https://example.com/predicates/release/v1",
            CheckClaims = false,
        });

        await AssertArguments(arguments,
        [
            "registry.example/app:v1",
            "--check-claims=false",
            "--type=https://example.com/predicates/release/v1",
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

        await AssertArguments(arguments,
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

        await Assert.That(registryToken!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(password!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }
}
