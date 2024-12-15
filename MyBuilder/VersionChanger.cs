using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBuilder
{
    public class VersionChanger
    {
        /// <summary>
        /// バージョンを変更する
        /// </summary>
        public static void ChangeVersions(string projectPath, int updateVersion = 0, string? newVersion = null)
        {
            foreach (var assemblyInfoPath in GetAssemblyInfoPaths(projectPath))
            {
                if (string.IsNullOrWhiteSpace(newVersion) && updateVersion > 0)
                {
                    var version = GetVersion(assemblyInfoPath);
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
                UpdateVersion(assemblyInfoPath, newVersion);
            }
        }

        private static IEnumerable<string> GetAssemblyInfoPaths(string projectPath)
        {
            return Directory.GetFiles(Path.GetDirectoryName(projectPath), "assemblyinfo.cs", SearchOption.AllDirectories);
        }

        private static string GetVersion(string assemblyInfoPath)
        {
            if (assemblyInfoPath == null)
            {
                throw new Exception("AssemblyInfo.csが見つかりません。");
            }
            var lines = File.ReadAllLines(assemblyInfoPath);
            var versionLine = lines.FirstOrDefault(x => x.Contains("AssemblyVersion", StringComparison.OrdinalIgnoreCase));
            if (versionLine == null)
            {
                throw new Exception("AssemblyVersionが見つかりません。");
            }
            var version = versionLine.Split('"')[1];
            return version;
        }

        private static void UpdateVersion(string assemblyInfoPath, string newVersion)
        {
            var lines = File.ReadAllLines(assemblyInfoPath);
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
            File.WriteAllLines(assemblyInfoPath, newLines);
        }
    }
}
