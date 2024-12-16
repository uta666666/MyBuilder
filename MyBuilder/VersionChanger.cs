namespace MyBuilder
{
    public class VersionChanger
    {
        /// <summary>
        /// バージョンを変更する
        /// </summary>
        public static void ChangeVersions(string projectPath, bool sdkBuild, int updateVersion = 0, string? newVersion = null)
        {
            foreach (var appVersion in GetFilePaths(projectPath, sdkBuild))
            {
                if (string.IsNullOrWhiteSpace(newVersion) && updateVersion > 0)
                {
                    var version = appVersion.GetVersion();
                    if (updateVersion == 1)
                    {
                        newVersion = VersionIncreaser.IncreaseMajorVersion(version);
                    }
                    else if (updateVersion == 2)
                    {
                        newVersion = VersionIncreaser.IncreaseMinorVersion(version);
                    }
                    else if (updateVersion == 3)
                    {
                        newVersion = VersionIncreaser.IncreaseBuildNumber(version);
                    }
                    else if (updateVersion == 4)
                    {
                        newVersion = VersionIncreaser.IncreaseRivisionNumber(version);
                    }
                    else
                    {
                        newVersion = version;
                    }
                }

                if (newVersion == null)
                {
                    throw new ArgumentException("versionが指定されませんでした");
                }

                appVersion.UpdateVersion(newVersion);
                Console.WriteLine($"Assembly Version を変更しました。（{newVersion}）");
            }
        }

        private static IEnumerable<IAppVersion> GetFilePaths(string projectPath, bool sdkBuild)
        {
            return sdkBuild ? AppVersionDotNetSdk.GetFiles(projectPath) : AppVersionDotnetFramework.GetFiles(projectPath);
        }
    }
}
