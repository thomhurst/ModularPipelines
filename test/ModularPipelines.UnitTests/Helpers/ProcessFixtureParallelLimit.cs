namespace ModularPipelines.UnitTests.Helpers;

// Each test can start several .NET hosts. Bound their overlap so startup remains
// observable within the fixture deadlines on small CI runners.
public sealed class ProcessFixtureParallelLimit : TUnit.Core.Interfaces.IParallelLimit
{
    public int Limit => 2;
}
