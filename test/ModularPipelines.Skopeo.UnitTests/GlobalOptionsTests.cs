using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Secrets;
using ModularPipelines.Skopeo.Options;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Skopeo.UnitTests;

public class GlobalOptionsTests : TestBase
{
    [Test]
    public async Task Persistent_Settings_Precede_Command_And_Preserve_Dotted_Switch()
    {
        var options = new SkopeoInspectOptions("docker://example/image")
        {
            CommandTimeout = "0s",
            Debug = "false",
            RegistriesD = "registries",
            TlsVerify = "false",
        };
        await AssertArguments(BuildArguments(options),
            ["--command-timeout=0s", "--debug=false", "--registries.d=registries", "docker://example/image", "--tls-verify=false"]);
        await Assert.That(await RenderCommand(options))
            .IsEqualTo("skopeo --command-timeout=0s --debug=false --registries.d=registries inspect docker://example/image --tls-verify=false");
    }

    [Test]
    public async Task Copy_Keeps_Source_And_Destination_Tls_Settings_Local()
    {
        var options = new SkopeoCopyOptions("docker://source/image", "dir:output")
        {
            Debug = CliOptionValue.Bare,
            SrcTlsVerify = "false",
            DestTlsVerify = "true",
        };
        var arguments = BuildArguments(options);
        await Assert.That(arguments).Contains("--debug");
        await Assert.That(arguments).Contains("--src-tls-verify=false");
        await Assert.That(arguments).Contains("--dest-tls-verify=true");
        await Assert.That(typeof(SkopeoOptions).GetProperty("TlsVerify")).IsNull();
        await Assert.That(typeof(SkopeoOptions).GetProperty("SrcTlsVerify")).IsNull();
        await Assert.That(typeof(SkopeoLoginOptions).GetProperty(nameof(SkopeoLoginOptions.Verbose))!
            .GetCustomAttribute<CliFlagAttribute>()!.ShortForm).IsEqualTo("-v");
    }

    [Test]
    public async Task Credential_Pairs_Are_Masked_And_Authfile_Paths_Are_Preserved()
    {
        var options = new SkopeoInspectOptions("docker://example/image")
        {
            Creds = "alice:private-password",
            Authfile = "public-auth-path",
        };
        var obfuscator = await GetService<ISecretObfuscator>();
        var output = obfuscator.Obfuscate("alice:private-password public-auth-path", options);
        await Assert.That(output).DoesNotContain("private-password");
        await Assert.That(output).Contains("public-auth-path");
        foreach (var property in new[] { "SrcCreds", "DestCreds" })
        {
            await Assert.That(typeof(SkopeoCopyOptions).GetProperty(property)!
                .IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        }
    }

    [Test]
    public async Task Commands_Inherit_Nine_Public_Settings_Without_Promoting_Local_Flags()
    {
        await Assert.That(typeof(SkopeoOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Count().IsEqualTo(9);
        await Assert.That(typeof(SkopeoInspectOptions).GetProperty(nameof(SkopeoOptions.RegistriesD))!.DeclaringType)
            .IsEqualTo(typeof(SkopeoOptions));
        await Assert.That(typeof(SkopeoOptions).GetProperty("Creds")).IsNull();
        await Assert.That(typeof(SkopeoOptions).GetProperty("Authfile")).IsNull();
    }
}
