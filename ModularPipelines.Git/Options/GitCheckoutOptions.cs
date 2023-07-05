namespace ModularPipelines.Git.Options;

public record GitCheckoutOptions( string BranchName ) : GitCommandOptions( new[] { "checkout", BranchName } );
