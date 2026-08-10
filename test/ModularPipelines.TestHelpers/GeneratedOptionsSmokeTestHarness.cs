using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Smoke-tests command-line rendering for every options record in an integration assembly.
/// </summary>
public static class GeneratedOptionsSmokeTestHarness
{
    /// <summary>
    /// Validates every concrete <see cref="CommandLineToolOptions"/> type in an assembly.
    /// </summary>
    /// <param name="assembly">The integration assembly to validate.</param>
    /// <returns>The number of option types and attributed properties tested.</returns>
    public static GeneratedOptionsSmokeTestResult ValidateAssembly(Assembly assembly)
    {
        RuntimeHelpers.RunModuleConstructor(assembly.ManifestModule.ModuleHandle);

        var results = assembly.GetTypes()
            .Where(IsOptionsType)
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .Select(ValidateOptionsType)
            .ToList();

        return new GeneratedOptionsSmokeTestResult(
            results.Count,
            results.Sum(result => result.PropertiesTested));
    }

    /// <summary>
    /// Validates every command-line property on one options type.
    /// </summary>
    /// <param name="optionsType">The options type to validate.</param>
    /// <returns>The number of option types and attributed properties tested.</returns>
    public static GeneratedOptionsSmokeTestResult ValidateOptionsType(Type optionsType)
    {
        if (!typeof(CommandLineToolOptions).IsAssignableFrom(optionsType))
        {
            throw new ArgumentException(
                $"{optionsType.FullName} does not derive from {nameof(CommandLineToolOptions)}.",
                nameof(optionsType));
        }

        if (!GeneratedCommandMetadata.TryGet(optionsType, out _))
        {
            throw new InvalidOperationException(
                $"Source-generated command metadata is missing for {optionsType.FullName}.");
        }

        var model = new CommandModelProvider().GetCommandModel(optionsType);

        if (optionsType.IsAbstract)
        {
            return new GeneratedOptionsSmokeTestResult(1, 0);
        }

        var builder = new CommandArgumentBuilder();

        var propertiesTested = 0;
        foreach (var part in model)
        {
            if (ValidatePart(optionsType, model, builder, part))
            {
                propertiesTested++;
            }
        }

        return new GeneratedOptionsSmokeTestResult(1, propertiesTested);
    }

    private static bool IsOptionsType(Type type) =>
        !type.ContainsGenericParameters
        && typeof(CommandLineToolOptions).IsAssignableFrom(type);

    private static bool ValidatePart(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model,
        CommandArgumentBuilder builder,
        PropertyCommandLinePart part)
    {
        try
        {
            var property = GetProperty(optionsType, part.PropertyName);
            if (!CanAssign(property))
            {
                return false;
            }

            var sample = CreateSample(property.PropertyType);
            var options = RuntimeHelpers.GetUninitializedObject(optionsType);

            InitializeRequiredArguments(optionsType, model, options);
            SetValueIfAssignable(options, property, sample);

            var actual = builder.BuildArguments(model, options);
            var expected = GetExpectedArguments(model, options);

            if (!actual.SequenceEqual(expected, StringComparer.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Expected [{string.Join(", ", expected)}], " +
                    $"but rendered [{string.Join(", ", actual)}].");
            }

            return true;
        }
        catch (Exception exception) when (exception is not GeneratedOptionsSmokeTestException)
        {
            throw new GeneratedOptionsSmokeTestException(
                optionsType,
                part.PropertyName,
                exception);
        }
    }

    private static void InitializeRequiredArguments(
        Type optionsType,
        IEnumerable<PropertyCommandLinePart> model,
        object options)
    {
        foreach (var argument in model
                     .OfType<ArgumentPart>()
                     .Where(argument => argument.Attribute.Required))
        {
            var property = GetProperty(optionsType, argument.PropertyName);
            SetValueIfAssignable(options, property, CreateSample(property.PropertyType));
        }
    }

    private static PropertyInfo GetProperty(Type optionsType, string propertyName) =>
        optionsType
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .First(candidate => candidate.Name == propertyName);

    private static IReadOnlyList<string> GetExpectedArguments(
        IReadOnlyList<PropertyCommandLinePart> model,
        object options)
    {
        var arguments = model.OfType<ArgumentPart>().ToList();
        var switches = model.Where(part => part is FlagPart or OptionPart).ToList();
        var expected = new List<string>();

        foreach (var phase in Enum.GetValues<CommandLinePhase>())
        {
            var phaseArguments = arguments.Where(argument => argument.Phase == phase);
            var phaseSwitches = switches.Where(part => part.Phase == phase);

            expected.AddRange(RenderSwitches(phaseSwitches, options));
            expected.AddRange(RenderArguments(phaseArguments, options));
        }

        return expected;
    }

    private static IEnumerable<string> RenderArguments(
        IEnumerable<ArgumentPart> arguments,
        object options) =>
        arguments
            .OrderBy(argument => argument.Attribute.Position)
            .SelectMany(argument => GetExpectedArguments(argument, argument.Getter(options)));

    private static IEnumerable<string> RenderSwitches(
        IEnumerable<PropertyCommandLinePart> switches,
        object options) =>
        switches.SelectMany(part => GetExpectedArguments(part, part.Getter(options)));

    private static IReadOnlyList<string> GetExpectedArguments(
        PropertyCommandLinePart part,
        object? value)
    {
        if (value is null)
        {
            return [];
        }

        return part switch
        {
            ArgumentPart argument => GetExpectedArgument(argument.Attribute, value),
            FlagPart flag => GetExpectedFlag(flag.Attribute, value),
            OptionPart option => GetExpectedOption(option, value),
            _ => throw new ArgumentOutOfRangeException(nameof(part)),
        };
    }

    private static List<string> GetExpectedFlag(
        CliFlagAttribute attribute,
        object value)
    {
        return value switch
        {
            true => [GetEffectiveName(attribute)],
            int count when count > 0 => [.. Enumerable.Repeat(GetEffectiveName(attribute), count)],
            _ => [],
        };
    }

    private static IReadOnlyList<string> GetExpectedArgument(
        CliArgumentAttribute attribute,
        object sample)
    {
        var values = GetValues(sample);

        var requiresOptionTerminator = attribute.PrependOptionTerminator
            || (attribute.PrependOptionTerminatorIfValueStartsWithDash
                && values.Any(static value => value.StartsWith('-')));
        return requiresOptionTerminator
            ? ["--", .. values]
            : values;
    }

    private static List<string> GetExpectedOption(OptionPart option, object value)
    {
        var optionName = GetEffectiveName(option.Attribute);

        if (value is CliValuePair pair)
        {
            return [optionName, pair.First!, pair.Second!];
        }

        if (value is IEnumerable<CliValuePair> pairs)
        {
            return [.. pairs.SelectMany(pairValue => new[] { optionName, pairValue.First!, pairValue.Second! })];
        }

        var separator = GetSeparator(option.Attribute);

        if (value is CliOptionValue optionValue)
        {
            return RenderOptionalValue(optionName, separator, optionValue).ToList();
        }

        if (option.Attribute.ValueArity == CliOptionValueArity.Optional
            && value is IEnumerable<CliOptionValue> optionValues)
        {
            var optionalValues = optionValues.OfType<CliOptionValue>().ToList();
            if (option.Attribute.GroupValues && optionalValues.Count > 0)
            {
                return
                [
                    optionName,
                    .. optionalValues
                        .Where(static item => !item.IsBare)
                        .Select(static item => item.Value!),
                ];
            }

            return optionalValues
                .SelectMany(item => RenderOptionalValue(optionName, separator, item))
                .ToList();
        }

        var values = GetValues(value);
        if (option.Attribute.GroupValues && values.Count > 0)
        {
            return separator == " "
                ? [optionName, .. values]
                : [$"{optionName}{separator}{values[0]}", .. values.Skip(1)];
        }

        return values
            .SelectMany(renderedValue => RenderOptionValue(optionName, separator, renderedValue))
            .ToList();
    }

    private static string GetEffectiveName(CliFlagAttribute attribute) =>
        attribute.PreferShortForm && !string.IsNullOrEmpty(attribute.ShortForm)
            ? attribute.ShortForm
            : attribute.Name;

    private static string GetEffectiveName(CliOptionAttribute attribute) =>
        attribute.PreferShortForm && !string.IsNullOrEmpty(attribute.ShortForm)
            ? attribute.ShortForm
            : attribute.Name;

    private static string GetSeparator(CliOptionAttribute attribute)
    {
        return attribute.Format switch
        {
            OptionFormat.SpaceSeparated => " ",
            OptionFormat.EqualsSeparated => "=",
            OptionFormat.ColonSeparated => ":",
            OptionFormat.NoSeparator => string.Empty,
            _ => " ",
        };
    }

    private static IReadOnlyList<string> RenderOptionalValue(
        string optionName,
        string separator,
        CliOptionValue optionValue) =>
        optionValue.IsBare
            ? [optionName]
            : RenderOptionValue(optionName, separator, optionValue.Value!);

    private static IReadOnlyList<string> RenderOptionValue(
        string optionName,
        string separator,
        string value) =>
        separator == " "
            ? [optionName, value]
            : [$"{optionName}{separator}{value}"];

    private static object CreateSample(Type propertyType)
    {
        var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (TryCreateKnownSample(type) is { } knownSample)
        {
            return knownSample;
        }

        if (type.IsEnum)
        {
            return CreateEnumSample(type);
        }

        if (IsNumeric(type))
        {
            return Convert.ChangeType(1, type, CultureInfo.InvariantCulture);
        }

        if (TryCreateEnumerableSample(type) is { } enumerableSample)
        {
            return enumerableSample;
        }

        return CreateConstructedSample(type);
    }

    private static object? TryCreateKnownSample(Type type) =>
        type == typeof(string) ? "smoke-value"
        : type == typeof(bool) ? true
        : type == typeof(CliValuePair) ? new CliValuePair("smoke-first", "smoke-second")
        : type == typeof(CliOptionValue) ? (CliOptionValue) "smoke-value"
        : type == typeof(KeyValue) ? new KeyValue("smoke-key", "smoke-value")
        : type == typeof(Uri) ? new Uri("https://example.invalid/smoke")
        : null;

    private static object CreateEnumSample(Type type) =>
        Enum.GetValues(type).GetValue(0)
        ?? throw new InvalidOperationException($"{type.FullName} has no values.");

    private static object? TryCreateEnumerableSample(Type type)
    {
        if (TryGetEnumerableElementType(type) is not { } elementType)
        {
            return null;
        }

        var element = CreateSample(elementType);
        var array = Array.CreateInstance(elementType, 1);
        array.SetValue(element, 0);

        if (type.IsAssignableFrom(array.GetType()))
        {
            return array;
        }

        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList) Activator.CreateInstance(listType)!;
        list.Add(element);

        return type.IsAssignableFrom(listType) ? list : null;
    }

    private static object CreateConstructedSample(Type type)
    {
        if (type.GetConstructor([typeof(string)]) is { } stringConstructor)
        {
            return stringConstructor.Invoke(["smoke-value"]);
        }

        if (Activator.CreateInstance(type) is { } instance)
        {
            return instance;
        }

        throw new InvalidOperationException($"No representative value is defined for {type.FullName}.");
    }

    private static Type? TryGetEnumerableElementType(Type type)
    {
        if (type.IsArray)
        {
            return type.GetElementType();
        }

        return type
            .GetInterfaces()
            .Append(type)
            .FirstOrDefault(candidate =>
                candidate.IsGenericType
                && candidate.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            ?.GetGenericArguments()[0];
    }

    private static bool IsNumeric(Type type) =>
        type == typeof(byte)
        || type == typeof(short)
        || type == typeof(ushort)
        || type == typeof(int)
        || type == typeof(uint)
        || type == typeof(long)
        || type == typeof(ulong)
        || type == typeof(float)
        || type == typeof(double)
        || type == typeof(decimal);

    private static void SetValueIfAssignable(object target, PropertyInfo property, object value)
    {
        if (property.SetMethod is not null)
        {
            property.SetValue(target, value);
            return;
        }

        var backingField = GetBackingField(property);

        if (backingField is null)
        {
            throw new InvalidOperationException(
                $"{property.DeclaringType?.FullName}.{property.Name} cannot be assigned a representative value.");
        }

        backingField.SetValue(target, value);
    }

    private static bool CanAssign(PropertyInfo property) =>
        property.SetMethod is not null
        || GetBackingField(property) is not null;

    private static FieldInfo? GetBackingField(PropertyInfo property) =>
        property.DeclaringType?.GetField(
            $"<{property.Name}>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);

    private static IReadOnlyList<string> GetValues(object value) =>
        value switch
        {
            string stringValue => [stringValue],
            IReadOnlyList<KeyValue> keyValues => [.. keyValues.Select(item => item.ToString())],
            IEnumerable enumerable when value is not IEnumerable<char> => GetEnumerableValues(enumerable),
            bool boolValue => [boolValue.ToString().ToLowerInvariant()],
            Uri uri => [uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString()],
            IFormattable formattable when !value.GetType().IsEnum =>
                [formattable.ToString(null, CultureInfo.InvariantCulture)],
            _ => GetEnumOrDefaultValue(value),
        };

    private static List<string> GetEnumerableValues(IEnumerable enumerable) =>
        [.. enumerable
            .Cast<object>()
            .SelectMany(GetValues)];

    private static IReadOnlyList<string> GetEnumOrDefaultValue(object value)
    {
        var enumValue = value.GetType()
            .GetField(value.ToString()!)?
            .GetCustomAttribute<EnumValueAttribute>()?
            .Value;

        return [enumValue ?? value.ToString()!];
    }
}

/// <summary>
/// Counts the command-line options covered by a smoke-test run.
/// </summary>
/// <param name="OptionsTypesTested">The number of concrete options types tested.</param>
/// <param name="PropertiesTested">The number of attributed command-line properties tested.</param>
public sealed record GeneratedOptionsSmokeTestResult(
    int OptionsTypesTested,
    int PropertiesTested);

/// <summary>
/// Identifies the options type and property that failed command-line rendering.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GeneratedOptionsSmokeTestException"/> class.
/// </remarks>
/// <param name="optionsType">The options type under test.</param>
/// <param name="propertyName">The property under test.</param>
/// <param name="innerException">The rendering failure.</param>
public sealed class GeneratedOptionsSmokeTestException(
    Type optionsType,
    string propertyName,
    Exception innerException) : Exception($"{optionsType.FullName}.{propertyName} failed generated-options smoke testing.", innerException)
{
    /// <summary>
    /// Gets the options type under test.
    /// </summary>
    public Type OptionsType { get; } = optionsType;

    /// <summary>
    /// Gets the property under test.
    /// </summary>
    public string PropertyName { get; } = propertyName;
}
