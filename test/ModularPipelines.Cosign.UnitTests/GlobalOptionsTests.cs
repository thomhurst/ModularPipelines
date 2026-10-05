using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Cosign.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Cosign.UnitTests;

public class GlobalOptionsTests : TestBase
{
    [Test]
    public async Task Globals_Precede_Command_And_Local_Options()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new CosignVerifyOptions(["registry.example/app:v1"])
        {
            OutputFile = "verification log.txt",
            Timeout = "2m30s",
            Verbose = true,
            Key = "cosign.pub",
        });

        await AssertArguments(command.Arguments,
            ["--output-file=verification log.txt", "--timeout=2m30s", "--verbose", "verify", "registry.example/app:v1", "--key=cosign.pub"]);
    }

    [Test]
    public async Task Globals_Precede_Nested_Command()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new CosignBundleInspectOptions("bundle.json") { Timeout = "45s" });
        await AssertArguments(command.Arguments, ["--timeout=45s", "bundle", "inspect", "bundle.json"]);
    }

    [Test]
    [Arguments(null)]
    [Arguments(false)]
    public async Task Unset_Globals_And_False_Verbose_Are_Omitted(bool? verbose)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new CosignVerifyOptions(["image"]) { Verbose = verbose });
        await AssertArguments(command.Arguments, ["verify", "image"]);
    }

    [Test]
    public async Task Every_Command_Inherits_Exactly_One_Copy_Of_Each_Global()
    {
        var commands = typeof(CosignOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(CosignOptions))).ToArray();
        await Assert.That(commands).IsNotEmpty();
        foreach (var command in commands)
        {
            foreach (var name in new[] { "OutputFile", "Timeout", "Verbose" })
            {
                var properties = command.GetProperties().Where(property => property.Name == name).ToArray();
                await Assert.That(properties).Count().IsEqualTo(1);
                await Assert.That(properties.Single().DeclaringType).IsEqualTo(typeof(CosignOptions));
            }
        }
    }

    [Test]
    public async Task Inherited_Metadata_Preserves_Cli_Aliases()
    {
        var timeout = typeof(CosignOptions).GetProperty("Timeout")!.GetCustomAttribute<CliOptionAttribute>();
        var verbose = typeof(CosignOptions).GetProperty("Verbose")!.GetCustomAttribute<CliFlagAttribute>();
        await Assert.That(timeout!.ShortForm).IsEqualTo("-t");
        await Assert.That(verbose!.ShortForm).IsEqualTo("-d");
    }

    [Test]
    public async Task Local_Identity_Token_Remains_Masked_With_Global_Options()
    {
        var obfuscator = await GetService<ISecretObfuscator>();
        var options = new CosignSignOptions(["image"])
        {
            Timeout = "45s",
            IdentityToken = "private-identity-token",
            Key = "public-key-path",
        };

        var output = obfuscator.Obfuscate("45s private-identity-token public-key-path", options);
        await Assert.That(output).IsEqualTo($"45s {new SecretMaskingOptions().MaskValue} public-key-path");
    }
}
