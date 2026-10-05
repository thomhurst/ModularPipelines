namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>The installed argparse action's value contract, independent of help prose.</summary>
internal sealed record AzArgumentMetadata
{
    public required string Nargs { get; init; }

    public required bool IsInteger { get; init; }

    public required bool IsRepeated { get; init; }

    public bool IsFlag => Nargs == "0";

    public bool IsOptional => Nargs is "?" or "*";

    public bool GroupValues => Nargs is "+" or "*" || (int.TryParse(Nargs, out var count) && count > 1);

    public bool IsCollection => GroupValues || IsRepeated;

    public void Validate()
    {
        if (Nargs is not ("?" or "*" or "+") && (!int.TryParse(Nargs, out var count) || count < 0))
        {
            throw new InvalidOperationException($"Unsupported Azure CLI argument arity '{Nargs}'.");
        }
    }
}
