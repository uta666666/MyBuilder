// See https://aka.ms/new-console-template for more information

using MyBuilder;
using System.CommandLine;

//コマンド
var rootCommand = new RootCommand("ビルドするためのコマンドラインツール");

//引数
var projectArgument = new Argument<string>("project", "プロジェクトファイルを指定します");
var outputArgument = new Argument<string>("output", "出力先フォルダを指定します");

rootCommand.AddArgument(projectArgument);
rootCommand.AddArgument(outputArgument);


//オプション
var sdkOpotion = new Option<bool>(["--usesdk", "-sdk"], "dotnetSDK形式のプロジェクトファイル");
sdkOpotion.SetDefaultValue(false);

sdkOpotion.Arity = ArgumentArity.Zero;
var configOption = new Option<string>(["--configuration", "-c"], "ビルド構成を指定します");
configOption.SetDefaultValue("Release");

var platformOption = new Option<string>(["--platform", "-p"], "プラットフォームを指定します");
platformOption.SetDefaultValue("Any CPU");

var updateVersionOption = new Option<int>(["--updateversion", "-u"], "指定したバージョンを加算します（1:majour, 2:minor, 3:build, 4:revision）");
updateVersionOption.AddCompletions("1", "2", "3", "4");
updateVersionOption.AddValidator(result =>
{
    var value = result.GetValueOrDefault<int?>();
    if (value < 1 || value > 4)
    {
        result.ErrorMessage = "updateversion オプションは 1 から 4 の間で指定してください。";
    }
});

var newVersionOption = new Option<string>(["--newversion", "-n"], "指定したバージョンに変更します");
newVersionOption.AddValidator(result =>
{
    var value = result.GetValueOrDefault<string>();
    if (string.IsNullOrWhiteSpace(value))
    {
        result.ErrorMessage = "newversion オプションにはバージョンを指定してください。（例：1.0.0.0）";
        return;
    }
    if (value.Split('.').Length != 4)
    {
        result.ErrorMessage = "newversion オプションにはバージョンを指定してください。（例：1.0.0.0）";
        return;
    }
});

rootCommand.AddOption(sdkOpotion);
rootCommand.AddOption(configOption);
rootCommand.AddOption(platformOption);
rootCommand.AddOption(updateVersionOption);
rootCommand.AddOption(newVersionOption);

//ハンドラ
rootCommand.SetHandler((project, output, configuration, platform, newVersion, updateVersion, useSdk) =>
{
    if (!string.IsNullOrWhiteSpace(newVersion))
    {
        //バージョンが指定されていればそれを優先する。
        VersionChanger.ChangeVersions(project, useSdk, newVersion: newVersion);
    }
    else if (updateVersion > 0)
    {
        //バージョンが指定されていない、かつ、updateversionが指定されている場合はバージョンアップ
        VersionChanger.ChangeVersions(project, useSdk, updateVersion: updateVersion);
    }
    ApplicationBuilder.Build(project, configuration, platform, output);
},
projectArgument,
outputArgument,
configOption,
platformOption,
newVersionOption,
updateVersionOption,
sdkOpotion);

//実行
await rootCommand.InvokeAsync(args);
