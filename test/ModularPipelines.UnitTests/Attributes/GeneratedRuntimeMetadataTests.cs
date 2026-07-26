using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
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

    [SecretValue("token", "password")]
    public IReadOnlyList<KeyValue>? Properties { get; init; }
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

[CliTool("control-character-test")]
internal sealed record ControlCharacterMetadataOptions : CommandLineToolOptions
{
    [CliOption("--value\0", ShortForm = "-\b", CustomSeparator = "\f\v")]
    public string? Value { get; init; }
}

internal class GeneratedSecretBase
{
    [SecretValue]
    public string? BaseSecret { get; init; }
}

internal sealed class GeneratedDerivedSecret : GeneratedSecretBase
{
    [SecretValue]
    public string? DerivedSecret { get; init; }
}

internal class GeneratedOverrideSecretBase
{
    [SecretValue]
    public virtual string? InheritedSecret { get; init; }
}

internal sealed class GeneratedOverrideSecretDerived : GeneratedOverrideSecretBase
{
    public override string? InheritedSecret { get; init; }

    [SecretValue]
    public string? OwnSecret { get; init; }
}

[CliGlobalOptions]
internal record GeneratedOverrideCommandBase : CommandLineToolOptions
{
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; init; }
}

internal sealed record GeneratedOverrideCommandDerived : GeneratedOverrideCommandBase
{
    public override bool? Verbose { get; init; }
}

[CliGlobalOptions]
internal record ReflectionOverrideCommandBase : CommandLineToolOptions
{
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; init; }
}

internal sealed record ReflectionOverrideCommandDerived : ReflectionOverrideCommandBase
{
    public override bool? Verbose { get; init; }

    [CliOption("--hidden")]
    private string Hidden => "hidden";
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
        await Assert.That((bool) flag.Getter(options)!).IsTrue();
        await Assert.That(flag.Attribute.GetEffectiveName()).IsEqualTo("-v");

        var option = model.OfType<OptionPart>().Single();
        await Assert.That(option.Getter(options)).IsEqualTo(options.Output);
        await Assert.That(option.Attribute.GetSeparator()).IsEqualTo("=");
        await Assert.That(option.Attribute.AllowMultiple).IsTrue();
    }

    [Test]
    public async Task CommandMetadata_PreservesAttributesFromOverriddenProperties()
    {
        var found = GeneratedCommandMetadata.TryGet(typeof(GeneratedOverrideCommandDerived), out var model);
        var options = new GeneratedOverrideCommandDerived { Verbose = true };

        await Assert.That(found).IsTrue();
        var flag = model.OfType<FlagPart>().Single();
        await Assert.That(flag.PropertyName).IsEqualTo(nameof(GeneratedOverrideCommandDerived.Verbose));
        await Assert.That(flag.Attribute.Name).IsEqualTo("--verbose");
        await Assert.That((bool) flag.Getter(options)!).IsTrue();
        await Assert.That(flag.IsGlobalOption).IsTrue();
    }

    [Test]
    public async Task ReflectionMetadata_PreservesGlobalMetadataFromOverriddenProperties()
    {
        var model = new CommandModelProvider().GetCommandModel(typeof(ReflectionOverrideCommandDerived));

        var flag = model.OfType<FlagPart>().Single();
        await Assert.That(flag.IsGlobalOption).IsTrue();
    }

    [Test]
    public async Task SecretMetadata_RegistersDirectGetter()
    {
        var options = new GeneratedMetadataOptions { Token = "generated-secret" };

        var found = GeneratedSecretMetadata.TryGetAccessors(typeof(GeneratedMetadataOptions), out var accessors);

        await Assert.That(found).IsTrue();
        await Assert.That(accessors).Count().IsEqualTo(2);
        var tokenAccessor = accessors.Single(x => x.PropertyName == nameof(GeneratedMetadataOptions.Token));
        await Assert.That(tokenAccessor.Getter(options)).IsEqualTo("generated-secret");

        var propertiesAccessor = accessors.Single(x => x.PropertyName == nameof(GeneratedMetadataOptions.Properties));
        await Assert.That(propertiesAccessor.SecretValueKeys).IsEquivalentTo(["token", "password"]);
    }

    [Test]
    public async Task SecretPropertyAccessor_PreservesTwoArgumentConstructor()
    {
        var constructor = typeof(SecretPropertyAccessor).GetConstructor(
        [
            typeof(string),
            typeof(Func<object, object?>),
        ]);

        await Assert.That(constructor).IsNotNull();
    }

    [Test]
    public async Task SecretPropertyAccessor_PreservesTwoValueDeconstruct()
    {
        Func<object, object?> expectedGetter = value => value;
        var accessor = new SecretPropertyAccessor("Token", expectedGetter, ["token"]);
        var deconstruct = typeof(SecretPropertyAccessor).GetMethod(
            nameof(SecretPropertyAccessor.Deconstruct),
        [
            typeof(string).MakeByRefType(),
            typeof(Func<object, object?>).MakeByRefType(),
        ]);

        await Assert.That(deconstruct).IsNotNull();

        accessor.Deconstruct(out var propertyName, out var getter);

        await Assert.That(propertyName).IsEqualTo("Token");
        await Assert.That(getter).IsEqualTo(expectedGetter);
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

    [Test]
    public async Task CommandMetadata_EscapesControlCharactersInAttributeValues()
    {
        var found = GeneratedCommandMetadata.TryGet(typeof(ControlCharacterMetadataOptions), out var model);
        var option = model.OfType<OptionPart>().Single();

        await Assert.That(found).IsTrue();
        await Assert.That(option.Attribute.Name).IsEqualTo("--value\0");
        await Assert.That(option.Attribute.ShortForm).IsEqualTo("-\b");
        await Assert.That(option.Attribute.CustomSeparator).IsEqualTo("\f\v");
    }

    [Test]
    public async Task SecretMetadata_FlattensInheritedProperties()
    {
        var found = GeneratedSecretMetadata.TryGetAccessors(typeof(GeneratedDerivedSecret), out var accessors);

        await Assert.That(found).IsTrue();
        await Assert.That(accessors.Select(x => x.PropertyName))
            .IsEquivalentTo([nameof(GeneratedSecretBase.BaseSecret), nameof(GeneratedDerivedSecret.DerivedSecret)]);
    }

    [Test]
    public async Task SecretMetadata_PreservesAttributesFromOverriddenProperties()
    {
        var found = GeneratedSecretMetadata.TryGetAccessors(typeof(GeneratedOverrideSecretDerived), out var accessors);
        var value = new GeneratedOverrideSecretDerived
        {
            InheritedSecret = "inherited-secret",
            OwnSecret = "own-secret",
        };

        await Assert.That(found).IsTrue();
        await Assert.That(accessors.Select(x => x.PropertyName))
            .IsEquivalentTo([nameof(GeneratedOverrideSecretDerived.InheritedSecret), nameof(GeneratedOverrideSecretDerived.OwnSecret)]);
        await Assert.That(accessors.Select(x => x.Getter(value)?.ToString()))
            .IsEquivalentTo(["inherited-secret", "own-secret"]);
    }

    [Test]
    public async Task MissingExactSecretMetadata_UsesReflectionFallback()
    {
        var found = GeneratedSecretMetadata.TryGetAccessors(typeof(UngeneratedDerivedSecret), out _);

        await Assert.That(found).IsFalse();
    }

    [Test]
    public async Task DuplicateCommandMetadataRegistration_Throws()
    {
        GeneratedCommandMetadata.Register(typeof(DuplicateCommandMetadataType), []);

        await Assert.That(() => GeneratedCommandMetadata.Register(typeof(DuplicateCommandMetadataType), []))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task DuplicateSecretMetadataRegistration_Throws()
    {
        GeneratedSecretMetadata.Register(typeof(DuplicateSecretMetadataType), []);

        await Assert.That(() => GeneratedSecretMetadata.Register(typeof(DuplicateSecretMetadataType), []))
            .Throws<InvalidOperationException>();
    }

    private sealed class UngeneratedDerivedSecret : GeneratedSecretBase
    {
        [SecretValue]
        public string? DerivedSecret { get; init; }
    }

    private sealed class DuplicateCommandMetadataType
    {
    }

    private sealed class DuplicateSecretMetadataType
    {
    }
}
