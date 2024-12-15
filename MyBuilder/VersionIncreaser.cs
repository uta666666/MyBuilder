using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBuilder
{
    public class VersionIncreaser
    {
        public static string IncreaseRivisionNumber(string version)
        {
            var versionParts = version.Split('.');
            if (versionParts.Length < 4)
            {
                return version;
            }

            if (int.TryParse(versionParts[3], out var revisionNumber))
            {
                versionParts[3] = (++revisionNumber).ToString();
            }

            var versioon = string.Join(".", versionParts);
            Console.WriteLine($"Assembly Version を変更します。（{version}）");
            return version;
        }

        public static string IncreaseBuildNumber(string version)
        {
            var versionParts = version.Split('.');
            if (versionParts.Length < 3)
            {
                return version;
            }

            if (int.TryParse(versionParts[2], out var buildNumber))
            {
                versionParts[2] = (++buildNumber).ToString();
            }

            if (versionParts.Length >= 3)
            {
                versionParts[3] = "0";
            }

            var versioon = string.Join(".", versionParts);
            Console.WriteLine($"Assembly Version を変更します。（{version}）");
            return version;
        }

        public static string IncreaseMinorVersion(string version)
        {
            var versionParts = version.Split('.');
            if (versionParts.Length < 2)
            {
                return version;
            }

            if (int.TryParse(versionParts[1], out var minorVersion))
            {
                versionParts[1] = (++minorVersion).ToString();
            }

            if (versionParts.Length >= 3)
            {
                versionParts[2] = "0";
            }

            if (versionParts.Length >= 4)
            {
                versionParts[3] = "0";
            }

            var versioon = string.Join(".", versionParts);
            Console.WriteLine($"Assembly Version を変更します。（{version}）");
            return version;
        }

        public static string IncreaseMajorVersion(string version)
        {
            var versionParts = version.Split('.');
            if (versionParts.Length < 1)
            {
                return version;
            }

            if (int.TryParse(versionParts[0], out var majorVersion))
            {
                versionParts[0] = (++majorVersion).ToString();
            }

            if (versionParts.Length >=2)
            {
                versionParts[1] = "0";
            }

            if (versionParts.Length >= 3)
            {
                versionParts[2] = "0";
            }

            if (versionParts.Length >= 4)
            {
                versionParts[3] = "0";
            }

            var versioon = string.Join(".", versionParts);
            Console.WriteLine($"Assembly Version を変更します。（{version}）");
            return version;
        }
    }
}
