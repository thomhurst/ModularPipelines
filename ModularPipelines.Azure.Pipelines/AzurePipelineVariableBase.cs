namespace ModularPipelines.Azure.Pipelines;

public abstract class AzurePipelineVariableBase
{
    protected abstract string Prefix { get; }

    protected string? Get( string variableName ) => Environment.GetEnvironmentVariable( ToEnvironmentVariableName( variableName ) );

    private string ToEnvironmentVariableName( string variableName ) => Prefix?.ToUpperInvariant() + "_" + variableName.ToUpperInvariant().Replace( '.', '_' );
}
