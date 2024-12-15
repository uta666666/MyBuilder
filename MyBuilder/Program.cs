// See https://aka.ms/new-console-template for more information

using MyBuilder;
using System.CommandLine;

var rootCommand = new RootCommand("ビルドするためのコマンドラインツール");
var buildArgument = new Argument<string>("project", "プロジェクトファイルを指定します");

var configOption = new Option<string>(["--configuration", "-c"], "ビルド構成を指定します");
configOption.SetDefaultValue("Release");
var platformOption = new Option<string>(["--platform", "-p"], "プラットフォームを指定します");
platformOption.SetDefaultValue("Any CPU");
var outputOption = new Option<string>(["--output", "-o"], "出力先フォルダを指定します")
{
    IsRequired = true
};
var updateVersionOption = new Option<int>(["--updateversion", "-u"], "指定されたバージョンを加算します（1:majour, 2:minor, 3:build, 4:revision）");
updateVersionOption.AddCompletions("1", "2", "3", "4");
updateVersionOption.SetDefaultValue("3");
var newVersionOption = new Option<string>(["--newversion", "-v"], "バージョンを変更します");

rootCommand.AddOption(outputOption);
rootCommand.AddArgument(buildArgument);
rootCommand.AddOption(configOption);
rootCommand.AddOption(platformOption);
rootCommand.AddOption(updateVersionOption);
rootCommand.AddOption(newVersionOption);


rootCommand.SetHandler((project, configuration, platform, output, newVersion, updateVersion) =>
{
    if (!string.IsNullOrWhiteSpace(newVersion))
    {
        VersionChanger.ChangeVersions(project, newVersion: newVersion);
    }
    else if (updateVersion > 0)
    {
        VersionChanger.ChangeVersions(project, updateVersion: updateVersion);
    }
    ApplicationBuilder.Build(project, configuration, platform, output);
},
buildArgument,
configOption,
platformOption,
outputOption,
newVersionOption,
updateVersionOption);

await rootCommand.InvokeAsync(args);
