using System.Xml.Linq;

namespace MyBuilder
{
    /// <summary>
    /// アセンブリバージョンを取得・更新する（dotnetsdk用）
    /// </summary>
    public class AppVersionDotNetSdk : IAppVersion
    {
        /// <summary>
        /// ファイルの一覧を列挙する
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public static IEnumerable<AppVersionDotNetSdk> GetFiles(string projectPath)
        {
            foreach (var dir in Directory.GetFiles(Path.GetDirectoryName(projectPath), "*.csproj", SearchOption.AllDirectories))
            {
                yield return new AppVersionDotNetSdk(dir);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath"></param>
        public AppVersionDotNetSdk(string filePath)
        {
            _filepath = filePath;
        }

        private string _filepath;

        /// <summary>
        /// アセンブリバージョンを取得する
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            // csprojからバージョンを取得する
            XDocument csproj = XDocument.Load(_filepath);
            XNamespace ns = csproj.Root.GetDefaultNamespace();
            var versionElement = csproj.Descendants(ns + "AssemblyVersion").FirstOrDefault();
            if (versionElement == null)
            {
                return "0.0.0.0";
            }
            return versionElement.Value;
        }

        /// <summary>
        /// アセンブリバージョンを更新する
        /// </summary>
        /// <param name="newVersion"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateVersion(string newVersion)
        {
            // csprojのAssemblyVersionを更新する
            XDocument csproj = XDocument.Load(_filepath);
            XNamespace ns = csproj.Root.GetDefaultNamespace();
            var versionElement = csproj.Descendants(ns + "AssemblyVersion").FirstOrDefault();
            if (versionElement == null)
            {
                // AssemblyVersionが見つからない場合は追加する
                var propertyGroup = csproj.Descendants(ns + "PropertyGroup").FirstOrDefault();
                if (propertyGroup == null)
                {
                    throw new Exception("PropertyGroupが見つかりません。");
                }
                versionElement = new XElement(ns + "AssemblyVersion", newVersion);
                propertyGroup.Add(versionElement);
            }
            else
            {
                versionElement.Value = newVersion;
            }
            csproj.Save(_filepath);
        }
    }
}
