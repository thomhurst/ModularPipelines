namespace ModularPipelines;

public static class BuildSystemValues
{
    public static class TeamCity
    {
        public static string StartBlock(string name) => $"##teamcity[blockOpened name='{name}']";

        public static string EndBlock(string name) => $@"##teamcity[blockClosed name='{name}']";
    }

    public static class AzurePipelines
    {
        public static string StartBlock(string name) => $"##[group]{name}";

        public static string EndBlock => @"##[endgroup]";
    }
    
    public static class GitHub
    {
        public static string StartBlock(string name) => $"::group::{name}";

        public static string EndBlock => @"::endgroup::";
    }
}