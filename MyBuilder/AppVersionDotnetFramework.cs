namespace MyBuilder
{
    /// <summary>
    /// アセンブリバージョンを取得・更新する（.netFramework用）
    /// </summary>
    public class AppVersionDotnetFramework : IAppVersion
    {
        /// <summary>
        /// ファイル一覧を列挙する
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public static IEnumerable<AppVersionDotnetFramework> GetFiles(string projectPath)
        {
            foreach (var dir in Directory.GetFiles(Path.GetDirectoryName(projectPath), "assemblyinfo.cs", SearchOption.AllDirectories))
            {
                yield return new AppVersionDotnetFramework(dir);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath"></param>
        public AppVersionDotnetFramework(string filePath)
        {
            _filePath = filePath;
        }

        private string _filePath;

        /// <summary>
        /// アセンブリバージョンを取得する
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetVersion()
        {
            if (_filePath == null)
            {
                throw new Exception("AssemblyInfo.csが見つかりません。");
            }
            var lines = File.ReadAllLines(_filePath);
            var versionLine = lines.FirstOrDefault(x => x.Contains("AssemblyVersion", StringComparison.OrdinalIgnoreCase));
            if (versionLine == null)
            {
                throw new Exception("AssemblyVersionが見つかりません。");
            }
            var version = versionLine.Split('"')[1];
            return version;
        }

        /// <summary>
        /// アセンブリバージョンを更新する
        /// </summary>
        /// <param name="newVersion"></param>
        public void UpdateVersion(string newVersion)
        {
            var lines = File.ReadAllLines(_filePath);
            var newLines = new List<string>();

            foreach (var line in lines)
            {
                if (line.Contains("AssemblyVersion", StringComparison.OrdinalIgnoreCase))
                {
                    newLines.Add($"[assembly: AssemblyVersion(\"{newVersion}\")]");
                }
                else if (line.Contains("AssemblyFileVersion", StringComparison.OrdinalIgnoreCase))
                {
                    newLines.Add($"[assembly: AssemblyFileVersion(\"{newVersion}\")]");
                }
                else
                {
                    newLines.Add(line);
                }
            }
            File.WriteAllLines(_filePath, newLines);
        }
    }
}
