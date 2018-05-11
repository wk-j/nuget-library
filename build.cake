#addin "wk.ProjectParser"

using ProjectParser;

var name = "NuGetLibrary";
var info = Parser.Parse($"src/{name}/{name}.csproj");

Task("Create-NuGet").Does(() => {
    DotNetCorePack($"src/{name}/{name}.csproj", new DotNetCorePackSettings {
        OutputDirectory = "publish"
    });
});

Task("Publish-Nuget")
    .IsDependentOn("Create-NuGet")
    .Does(() => {
        var nupkg = new DirectoryInfo("publish").GetFiles("*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "http://192.168.0.109:7777/nuget",
            ApiKey = "beecircle"
        });
    });

var target = Argument("target", "default");
RunTarget(target);