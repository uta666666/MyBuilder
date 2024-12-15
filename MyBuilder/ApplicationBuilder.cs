using System.Configuration;
using System.Diagnostics;

namespace MyBuilder
{
    public class ApplicationBuilder
    {
        public static void Build(string projectPath, string configuration, string platform, string outputDir)
        {
            Console.WriteLine($"ビルドを開始します。");
            Console.WriteLine($"ビルド対象: {projectPath}");
            Console.WriteLine($"ビルド構成: {configuration}");
            Console.WriteLine($"プラットフォーム: {platform}");
            Console.WriteLine($"出力先: {outputDir}");

            var p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = GetMsBuildPath(),
                Arguments = $"\"{projectPath}\" /p:Configuration={configuration} /p:Platform=\"{platform}\" /p:OutputPath=\"{outputDir}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            p.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();

            if (p.ExitCode == 0)
            {
                Console.WriteLine("ビルドが完了しました。");
            }
            else
            {
                Console.WriteLine("ビルドに失敗しました。");
            }
        }

        private static string GetMsBuildPath()
        {
            var msBuildPath = ConfigurationManager.AppSettings["MSBuildPath"];
            //var msBuildPath = ToolLocationHelper.GetPathToDotNetFrameworkFile("MSBuild.exe", TargetDotNetFrameworkVersion.Version40);
            if (string.IsNullOrEmpty(msBuildPath))
            {
                throw new Exception("MSBuild.exeが見つかりません。");
            }
            return msBuildPath;
        }
    }
}
