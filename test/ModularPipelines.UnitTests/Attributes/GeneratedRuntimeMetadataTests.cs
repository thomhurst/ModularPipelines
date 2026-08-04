using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Metadata;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.VisualBasic.TestFixtures;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace ModularPipelines.UnitTests.Attributes;

[CliTool("metadata-test")]
internal sealed record GeneratedMetadataOptions : CommandLineToolOptions
{
    [CliArgument(
        0,
        Phase = CommandLinePhase.EarlyOperand,
        PrependOptionTerminator = true,
        Required = true)]
    public string? File { get; init; }

    [CliFlag("--verbose", ShortForm = "-v", PreferShortForm = true)]
    public bool? Verbose { get; init; }

    [CliOption(
        "--output",
        Format = OptionFormat.EqualsSeparated,
        ValueArity = CliOptionValueArity.Optional,
        GroupValues = true,
        Phase = CommandLinePhase.Terminal)]
    public string[]? Output { get; init; }

    [SecretValue]
    public string? Token { get; init; }

    [SecretValue("token", "password")]
    public IReadOnlyList<KeyValue>? Properties { get; init; }
}

[CliTool("control-character-test")]
internal sealed record ControlCharacterMetadataOptions : CommandLineToolOptions
{
    [CliOption("--value\0", ShortForm = "-\b", Format = OptionFormat.ColonSeparated)]
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

public class GeneratedRuntimeMetadataTests
{
    [Test]
    public async Task SecretValueAttribute_IsSealed()
    {
        await Assert.That(typeof(SecretValueAttribute).IsSealed).IsTrue();
    }

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
        await Assert.That(argument.Attribute.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
        await Assert.That(argument.Attribute.PrependOptionTerminator).IsTrue();
        await Assert.That(argument.Attribute.Required).IsTrue();

        var flag = model.OfType<FlagPart>().Single();
        await Assert.That((bool) flag.Getter(options)!).IsTrue();
        await Assert.That(flag.Attribute.Name).IsEqualTo("--verbose");
        await Assert.That(flag.Attribute.ShortForm).IsEqualTo("-v");
        await Assert.That(flag.Attribute.PreferShortForm).IsTrue();
        await Assert.That(flag.Attribute.Phase).IsEqualTo(CommandLinePhase.Normal);
        var option = model.OfType<OptionPart>().Single();
        await Assert.That(option.Getter(options)).IsEqualTo(options.Output);
        await Assert.That(option.Attribute.Format).IsEqualTo(OptionFormat.EqualsSeparated);
        await Assert.That(option.Attribute.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(option.Attribute.GroupValues).IsTrue();
        await Assert.That(option.Attribute.Phase).IsEqualTo(CommandLinePhase.Terminal);
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
    public async Task CommandModelProvider_UsesGeneratedOverrideMetadata()
    {
        var model = new CommandModelProvider().GetCommandModel(typeof(GeneratedOverrideCommandDerived));

        var flag = model.OfType<FlagPart>().Single();
        await Assert.That(flag.IsGlobalOption).IsTrue();
    }

    [Test]
    public async Task CommandModelProvider_UsesReflectionForVisualBasicOptions()
    {
        var options = new VisualBasicCommandOptions();

        var model = new CommandModelProvider().GetCommandModel(options.GetType());

        var option = model.OfType<OptionPart>().Single();
        await Assert.That(option.Attribute.Name).IsEqualTo("--value");
        await Assert.That(option.Getter(options)).IsEqualTo("visual-basic-value");
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
    public async Task MissingCommandMetadata_ThrowsActionableException()
    {
        var optionsType = CreateDynamicType("MissingCommandMetadata");
        GeneratedCommandMetadata.RegisterAssembly(optionsType.Assembly);
        GeneratedCommandMetadata.RegisterCoveredTypeNames(optionsType.Assembly, [optionsType.FullName!]);

        var exception = await Assert.That(() => new CommandModelProvider().GetCommandModel(optionsType))
            .Throws<MissingCommandMetadataException>();

        await Assert.That(exception!.OptionsType).IsEqualTo(optionsType);
        await Assert.That(exception.Message).Contains("ModularPipelines.SourceGenerator");
        await Assert.That(exception.Message).Contains("RuntimeMetadataRegistry");
    }

    [Test]
    public async Task ProcessedAssembly_UsesReflectionForGeneratorEmittedOptions()
    {
        var optionsType = typeof(VisualBasicCommandOptions);
        GeneratedCommandMetadata.RegisterAssembly(optionsType.Assembly);

        var model = new CommandModelProvider().GetCommandModel(optionsType);

        var option = model.OfType<OptionPart>().Single();
        await Assert.That(option.Attribute.Name).IsEqualTo("--value");
    }

    [Test]
    public async Task CommandMetadata_TracksProcessedAssembliesIndependently()
    {
        var type = CreateDynamicType("CommandAssemblyRegistration");

        GeneratedCommandMetadata.RegisterAssembly(type.Assembly);

        using (Assert.Multiple())
        {
            await Assert.That(GeneratedCommandMetadata.IsAssemblyProcessed(type.Assembly)).IsTrue();
            await Assert.That(GeneratedSecretMetadata.IsAssemblyProcessed(type.Assembly)).IsFalse();
        }
    }

    [Test]
    public async Task OtherGeneratorsCanContributeAotSafeRuntimeMetadata()
    {
        var type = CreateDynamicType("ContributedRuntimeMetadata");
        var commandModel = new List<PropertyCommandLinePart>
        {
            new OptionPart("Value", static _ => "value", new CliOptionAttribute("--value")),
        };
        var secretAccessors = new List<SecretPropertyAccessor>
        {
            new("Token", static _ => "secret"),
        };

        RuntimeMetadataRegistry.RegisterCommandOptions(type, commandModel);
        RuntimeMetadataRegistry.RegisterSecrets(type, secretAccessors);

        using (Assert.Multiple())
        {
            await Assert.That(GeneratedCommandMetadata.TryGet(type, out var registeredCommandModel)).IsTrue();
            await Assert.That(registeredCommandModel).IsSameReferenceAs(commandModel);
            await Assert.That(GeneratedSecretMetadata.TryGetAccessors(type, out var registeredSecretAccessors)).IsTrue();
            await Assert.That(registeredSecretAccessors).IsSameReferenceAs(secretAccessors);
        }
    }

    [Test]
    public async Task CommandMetadata_EscapesControlCharactersInAttributeValues()
    {
        var found = GeneratedCommandMetadata.TryGet(typeof(ControlCharacterMetadataOptions), out var model);
        var option = model.OfType<OptionPart>().Single();

        await Assert.That(found).IsTrue();
        await Assert.That(option.Attribute.Name).IsEqualTo("--value\0");
        await Assert.That(option.Attribute.ShortForm).IsEqualTo("-\b");
        await Assert.That(option.Attribute.Format).IsEqualTo(OptionFormat.ColonSeparated);
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
        await Assert.That(accessors.Select(x => x.Getter(value)?.ToString()!))
            .IsEquivalentTo(["inherited-secret", "own-secret"]);
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

    [Test]
    public async Task DuplicateExternalCommandMetadataRegistration_PreservesFirstModel()
    {
        var type = CreateDynamicType("DuplicateExternalCommand");
        var firstModel = new List<PropertyCommandLinePart>();

        var consumerAssembly = typeof(GeneratedRuntimeMetadataTests).Assembly;
        GeneratedCommandMetadata.RegisterExternal(consumerAssembly, type, firstModel);
        GeneratedCommandMetadata.RegisterExternal(
            consumerAssembly,
            type,
            new List<PropertyCommandLinePart>());

        var found = GeneratedCommandMetadata.TryGet(type, out var registeredModel);
        using (Assert.Multiple())
        {
            await Assert.That(found).IsTrue();
            await Assert.That(registeredModel).IsSameReferenceAs(firstModel);
        }
    }

    [Test]
    public async Task DuplicateExternalSecretMetadataRegistration_PreservesFirstModel()
    {
        var type = CreateDynamicType("DuplicateExternalSecret");
        var firstModel = new List<SecretPropertyAccessor>();

        var consumerAssembly = typeof(GeneratedRuntimeMetadataTests).Assembly;
        GeneratedSecretMetadata.RegisterExternal(consumerAssembly, type, firstModel);
        GeneratedSecretMetadata.RegisterExternal(
            consumerAssembly,
            type,
            new List<SecretPropertyAccessor>());

        var found = GeneratedSecretMetadata.TryGetAccessors(type, out var registeredModel);
        using (Assert.Multiple())
        {
            await Assert.That(found).IsTrue();
            await Assert.That(registeredModel).IsSameReferenceAs(firstModel);
        }
    }

    [Test]
    public async Task LegacyRegistrationOverloads_PreserveCompleteness()
    {
        var completeCommandType = CreateDynamicType("CompleteCommand");
        var incompleteCommandType = CreateDynamicType("IncompleteCommand");
        var completeSecretType = CreateDynamicType("CompleteSecret");
        var incompleteSecretType = CreateDynamicType("IncompleteSecret");

        GeneratedCommandMetadata.Register(completeCommandType, [], isComplete: true);
        GeneratedCommandMetadata.Register(incompleteCommandType, [], isComplete: false);
        GeneratedSecretMetadata.Register(completeSecretType, [], isComplete: true);
        GeneratedSecretMetadata.Register(incompleteSecretType, [], isComplete: false);

        using (Assert.Multiple())
        {
            await Assert.That(GeneratedCommandMetadata.TryGet(completeCommandType, out _)).IsTrue();
            await Assert.That(GeneratedCommandMetadata.TryGet(incompleteCommandType, out _)).IsFalse();
            await Assert.That(GeneratedSecretMetadata.TryGetAccessors(completeSecretType, out _)).IsTrue();
            await Assert.That(GeneratedSecretMetadata.TryGetAccessors(incompleteSecretType, out _)).IsFalse();
        }
    }

    [Test]
    public async Task GeneratedMetadata_DoesNotRootCollectibleAssemblies()
    {
        var (assemblyReference, typeReference) = RegisterCollectibleMetadata();

        for (var attempt = 0; attempt < 10 && (assemblyReference.IsAlive || typeReference.IsAlive); attempt++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        using (Assert.Multiple())
        {
            await Assert.That(assemblyReference.IsAlive).IsFalse();
            await Assert.That(typeReference.IsAlive).IsFalse();
        }
    }

    [Test]
    public async Task ExternalMetadata_DoesNotRootCollectibleConsumerAssembly()
    {
        var (assemblyReference, typeReference) = RegisterCollectibleExternalMetadata();

        for (var attempt = 0; attempt < 10 && (assemblyReference.IsAlive || typeReference.IsAlive); attempt++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        using (Assert.Multiple())
        {
            await Assert.That(assemblyReference.IsAlive).IsFalse();
            await Assert.That(typeReference.IsAlive).IsFalse();
        }
    }

    private static Type CreateDynamicType(string name)
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName($"{name}_{Guid.NewGuid():N}"),
            AssemblyBuilderAccess.Run);

        return assembly.DefineDynamicModule("Main")
            .DefineType(name, TypeAttributes.Public)
            .CreateType()!;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference Assembly, WeakReference Type) RegisterCollectibleMetadata()
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName($"CollectibleMetadata_{Guid.NewGuid():N}"),
            AssemblyBuilderAccess.RunAndCollect);
        var type = assembly.DefineDynamicModule("Main")
            .DefineType("CollectibleOptions", TypeAttributes.Public)
            .CreateType()!;

        GeneratedCommandMetadata.RegisterAssembly(assembly);
        GeneratedCommandMetadata.Register(type, []);
        GeneratedSecretMetadata.RegisterAssembly(assembly);
        GeneratedSecretMetadata.Register(type, []);

        return (new WeakReference(assembly), new WeakReference(type));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference Assembly, WeakReference Type) RegisterCollectibleExternalMetadata()
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName($"ExternalMetadataConsumer_{Guid.NewGuid():N}"),
            AssemblyBuilderAccess.RunAndCollect);
        var typeBuilder = assembly.DefineDynamicModule("Main")
            .DefineType("ExternalMetadataConsumer", TypeAttributes.Public);
        var getterMethod = typeBuilder.DefineMethod(
            "GetValue",
            MethodAttributes.Public | MethodAttributes.Static,
            typeof(object),
            [typeof(object)]);
        var il = getterMethod.GetILGenerator();
        il.Emit(OpCodes.Ldnull);
        il.Emit(OpCodes.Ret);
        var consumerType = typeBuilder.CreateType()!;
        var getter = (Func<object, object?>) Delegate.CreateDelegate(
            typeof(Func<object, object?>),
            consumerType.GetMethod("GetValue")!);
        var sharedOptionsType = typeof(ExternalConsumerSharedMetadataType);

        GeneratedCommandMetadata.RegisterExternal(
            assembly,
            sharedOptionsType,
            [new FlagPart("Value", getter, new CliFlagAttribute("--value"))]);
        GeneratedSecretMetadata.RegisterExternal(
            assembly,
            sharedOptionsType,
            [new SecretPropertyAccessor("Value", getter)]);
        if (!GeneratedCommandMetadata.TryGet(sharedOptionsType, out _)
            || !GeneratedSecretMetadata.TryGetAccessors(sharedOptionsType, out _))
        {
            throw new InvalidOperationException("External metadata registration was not visible.");
        }

        return (new WeakReference(assembly), new WeakReference(consumerType));
    }

    private sealed class DuplicateCommandMetadataType
    {
    }

    private sealed class DuplicateSecretMetadataType
    {
    }

    private sealed class ExternalConsumerSharedMetadataType
    {
    }
}
