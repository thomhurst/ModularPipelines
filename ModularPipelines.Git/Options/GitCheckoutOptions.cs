namespace ModularPipelines.Git.Options;

public record GitCheckoutOptions(string BranchName) : GitArgumentOptions(new [] { "checkout", BranchName });