namespace TsGenCli
{
    public record MsBuildOutputRecord(TargetResultsRecord TargetResults);
    public record TargetResultsRecord(BuildResultsRecord Build);
    public record BuildResultsRecord(string Result, List<BuildItemRecord> Items);
    public record BuildItemRecord(
        string Identity,
        string TargetFrameworkIdentifier,
        string TargetPlatformMoniker,
        string CopyUpToDateMarker,
        string TargetPlatformIdentifier,
        string TargetFrameworkVersion,
        string ReferenceAssembly,
        string FullPath,
        string RootDir,
        string Filename,
        string Extension,
        string RelativeDir,
        string Directory,
        string RecursiveDir,
        string ModifiedTime,
        string CreatedTime,
        string AccessedTime,
        string DefiningProjectFullPath,
        string DefiningProjectDirectory,
        string DefiningProjectName,
        string DefiningProjectExtension);
}
