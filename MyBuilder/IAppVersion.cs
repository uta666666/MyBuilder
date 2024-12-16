namespace MyBuilder
{
    public interface IAppVersion
    {
        /// <summary>
        /// アセンブリバージョンを取得する
        /// </summary>
        /// <returns></returns>
        string GetVersion();

        /// <summary>
        /// アセンブリバージョンを更新する
        /// </summary>
        /// <param name="newVersion"></param>
        void UpdateVersion(string newVersion);
    }
}
