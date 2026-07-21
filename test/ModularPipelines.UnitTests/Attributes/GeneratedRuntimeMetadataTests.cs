using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Attributes;

[CliTool("metadata-test")]
internal sealed record GeneratedMetadataOptions : CommandLineToolOptions
{
    [CliArgument(0, Placement = ArgumentPlacement.BeforeOptions, Name = "<FILE>")]
    public string? File { get; init; }

    [CliFlag("--verbose", ShortForm = "-v", PreferShortForm = true)]
    public bool? Verbose { get; init; }

    [CliOption("--output", Format = OptionFormat.EqualsSeparated, AllowMultiple = true)]
    public string[]? Output { get; init; }

    [SecretValue]
    public string? Token { get; init; }
}

internal sealed record IncompleteGeneratedMetadataOptions : CommandLineToolOptions
{
    [CliOption("--visible")]
    public string Visible => "visible";

    [CliOption("--hidden")]
    private string Hidden => "hidden";

    [SecretValue]
    public string VisibleSecret => "visible-secret";

    [SecretValue]
    private string HiddenSecret => "hidden-secret";
}

public class GeneratedRuntimeMetadataTests
{
    [Test]
    public async Task CommandMetadata_RegistersAttributeValuesAndDirectGetters()
    {
        var options = new GeneratedMetadataOptions
        {
            File = "pipeline.yml",
            Verbose = true,
            Output = ["json", "yaml"],
        };

        var found = GeneratedCommandMetadata.TryGet(typeof(GeneratedMetadataOptions), out var model);

        await Assert.That(found).IsTrue();
        await Assert.That(model).Count().IsEqualTo(3);

        var argument = model.OfType<ArgumentPart>().Single();
        await Assert.That(argument.PropertyName).IsEqualTo(nameof(GeneratedMetadataOptions.File));
        await Assert.That(argument.Getter(options)).IsEqualTo("pipeline.yml");
        await Assert.That(argument.Attribute.Position).IsEqualTo(0);
        await Assert.That(argument.Attribute.Placement).IsEqualTo(ArgumentPlacement.BeforeOptions);
        await Assert.That(argument.Attribute.Name).IsEqualTo("<FILE>");

        var flag = model.OfType<FlagPart>().Single();
        await Assert.That(flag.Getter(options)).IsEqualTo(true);
        await Assert.That(flag.Attribute.GetEffectiveName()).IsEqualTo("-v");

        var option = model.OfType<OptionPart>().Single();
        await Assert.That(option.Getter(options)).IsEqualTo(options.Output);
        await Assert.That(option.Attribute.GetSeparator()).IsEqualTo("=");
        await Assert.That(option.Attribute.AllowMultiple).IsTrue();
    }

    [Test]
    public async Task SecretMetadata_RegistersDirectGetter()
    {
        var options = new GeneratedMetadataOptions { Token = "generated-secret" };

        var found = GeneratedSecretMetadata.TryGetAccessors(typeof(GeneratedMetadataOptions), out var accessors);

        await Assert.That(found).IsTrue();
        await Assert.That(accessors).Count().IsEqualTo(1);
        await Assert.That(accessors[0].PropertyName).IsEqualTo(nameof(GeneratedMetadataOptions.Token));
        await Assert.That(accessors[0].Getter(options)).IsEqualTo("generated-secret");
    }

    [Test]
    public async Task InaccessibleProperties_MarkGeneratedMetadataIncomplete()
    {
        var commandMetadataFound = GeneratedCommandMetadata.TryGet(
            typeof(IncompleteGeneratedMetadataOptions),
            out _);
        var secretMetadataFound = GeneratedSecretMetadata.TryGetAccessors(
            typeof(IncompleteGeneratedMetadataOptions),
            out _);

        await Assert.That(commandMetadataFound).IsFalse();
        await Assert.That(secretMetadataFound).IsFalse();
    }

    [Test]
    public async Task IncompleteCommandMetadata_UsesReflectionFallback()
    {
        var model = new CommandModelProvider().GetCommandModel(typeof(IncompleteGeneratedMetadataOptions));

        await Assert.That(model.Select(part => part.PropertyName))
            .IsEquivalentTo(["Visible", "Hidden"]);
    }
}
