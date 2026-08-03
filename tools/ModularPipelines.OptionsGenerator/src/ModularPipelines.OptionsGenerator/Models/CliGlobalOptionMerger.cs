namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Combines options discovered from CLI help with supplemental options that help cannot expose.
/// </summary>
public static class CliGlobalOptionMerger
{
    /// <summary>
    /// Merges global options deterministically and rejects ambiguous CLI or C# representations.
    /// </summary>
    public static IReadOnlyList<CliOptionDefinition> Merge(
        IEnumerable<CliOptionDefinition> scrapedOptions,
        IEnumerable<CliOptionDefinition> supplementalOptions)
    {
        ArgumentNullException.ThrowIfNull(scrapedOptions);
        ArgumentNullException.ThrowIfNull(supplementalOptions);

        var optionsBySwitch = new Dictionary<string, CliOptionDefinition>(StringComparer.OrdinalIgnoreCase);
        var switchByProperty = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var primarySwitchByAlias = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var option in scrapedOptions.Concat(supplementalOptions))
        {
            ValidateOption(option);

            if (optionsBySwitch.TryGetValue(option.SwitchName, out var existing))
            {
                if (!HasSameShape(existing, option))
                {
                    throw new InvalidOperationException(
                        $"Global option '{option.SwitchName}' has conflicting scraped and supplemental definitions.");
                }

                optionsBySwitch[existing.SwitchName] = MergeDocumentation(existing, option);
                continue;
            }

            if (switchByProperty.TryGetValue(option.PropertyName, out var existingSwitch))
            {
                throw new InvalidOperationException(
                    $"Global options '{existingSwitch}' and '{option.SwitchName}' both generate property '{option.PropertyName}'.");
            }

            RegisterAlias(option.SwitchName, option.SwitchName, primarySwitchByAlias);
            if (option.ShortForm is not null)
            {
                RegisterAlias(option.ShortForm, option.SwitchName, primarySwitchByAlias);
            }

            optionsBySwitch.Add(option.SwitchName, option);
            switchByProperty.Add(option.PropertyName, option.SwitchName);
        }

        return optionsBySwitch.Values
            .OrderBy(option => option.SwitchName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(option => option.SwitchName, StringComparer.Ordinal)
            .ToList();
    }

    private static void ValidateOption(CliOptionDefinition option)
    {
        if (string.IsNullOrWhiteSpace(option.SwitchName))
        {
            throw new InvalidOperationException("A supplemental global option has an empty switch name.");
        }

        if (string.IsNullOrWhiteSpace(option.PropertyName))
        {
            throw new InvalidOperationException(
                $"Global option '{option.SwitchName}' has an empty generated property name.");
        }
    }

    private static void RegisterAlias(
        string alias,
        string primarySwitch,
        IDictionary<string, string> primarySwitchByAlias)
    {
        if (primarySwitchByAlias.TryGetValue(alias, out var existingSwitch))
        {
            throw new InvalidOperationException(
                $"Global option alias '{alias}' is shared by '{existingSwitch}' and '{primarySwitch}'.");
        }

        primarySwitchByAlias.Add(alias, primarySwitch);
    }

    private static CliOptionDefinition MergeDocumentation(
        CliOptionDefinition scraped,
        CliOptionDefinition supplemental)
    {
        return scraped with
        {
            Description = scraped.Description ?? supplemental.Description,
            DocumentationUrl = supplemental.DocumentationUrl ?? scraped.DocumentationUrl,
            Availability = supplemental.Availability ?? scraped.Availability,
        };
    }

    private static bool HasSameShape(CliOptionDefinition left, CliOptionDefinition right)
    {
        return left.SwitchName.Equals(right.SwitchName, StringComparison.OrdinalIgnoreCase)
               && StringEquals(left.ShortForm, right.ShortForm)
               && left.PreferShortForm == right.PreferShortForm
               && left.PropertyName.Equals(right.PropertyName, StringComparison.Ordinal)
               && left.CSharpType.Equals(right.CSharpType, StringComparison.Ordinal)
               && left.IsFlag == right.IsFlag
               && left.ValueArity == right.ValueArity
               && left.Phase == right.Phase
               && left.IsRequired == right.IsRequired
               && left.AcceptsMultipleValues == right.AcceptsMultipleValues
               && left.GroupValues == right.GroupValues
               && left.IsCollection == right.IsCollection
               && left.IsKeyValue == right.IsKeyValue
               && left.IsNumeric == right.IsNumeric
               && left.IsSecret == right.IsSecret
               && left.SecretValueKeys.SequenceEqual(right.SecretValueKeys, StringComparer.OrdinalIgnoreCase)
               && left.ValueSeparator.Equals(right.ValueSeparator, StringComparison.Ordinal)
               && EnumDefinitionsEqual(left.EnumDefinition, right.EnumDefinition)
               && Equals(left.ValidationConstraints, right.ValidationConstraints);
    }

    private static bool EnumDefinitionsEqual(CliEnumDefinition? left, CliEnumDefinition? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        return left is not null
               && right is not null
               && left.EnumName.Equals(right.EnumName, StringComparison.Ordinal)
               && string.Equals(left.Description, right.Description, StringComparison.Ordinal)
               && left.Values.SequenceEqual(right.Values);
    }

    private static bool StringEquals(string? left, string? right)
    {
        return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }
}
