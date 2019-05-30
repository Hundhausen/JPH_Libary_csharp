#region Header

//----------------------------------------------------------------------
// 
// Project is available at https://github.com/Hundhausen
// This Project is licensed under the GNU General Public License v3.0
//
// Date: 2019-05-26
// User: Hundhausen
//
//----------------------------------------------------------------------

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


namespace JPH_Library.Updater
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for Updater
    public class Updater
    {
        #region var



        #endregion

        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        /// TODO Edit XML Comment Template for Get
        public static bool NewVersionAvaible(string userIn, string repoIn, List<int> versionIn, bool preReleases, out List<GithubRelease> releasesOut)
        {
            if (!AskApiForReleases(userIn, repoIn, out string content))
            {
                releasesOut = new List<GithubRelease>();
                return false;
            }
            releasesOut = JsonConvert.DeserializeObject<List<GithubRelease>>(content);

            List<int> lstVersionNumbers = new List<int>();

            Regex regex = new Regex(@"\d+");

            foreach (GithubRelease release in releasesOut)
            {
                if (release.PreRelease)
                {
                    if (!preReleases) break;
                }

                foreach (Match match in regex.Matches(release.Tag))
                {
                    lstVersionNumbers.Add(Convert.ToInt32(match.Value));
                }
                if (versionIn.Count < lstVersionNumbers.Count) break;

                for (int ii = 0; ii < lstVersionNumbers.Count; ii++)
                {
                    if (versionIn[ii] > lstVersionNumbers[ii]) return true;
                }
                return true;
            }
            return false;

        }

        /// <summary>Asks the Github API of all releases </summary>
        /// <param name="userIn">The User of the Repo</param>
        /// <param name="repoIn">The Repo</param>
        /// <param name="responseOut">Response of the Api</param>
        /// <returns>If the connection was successful</returns>
        private static bool AskApiForReleases(string userIn, string repoIn, out string responseOut)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Uri uri = new Uri($"https://api.github.com/repos/{userIn}/{repoIn}/releases");
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);

                request.Method = "GET";
                request.UserAgent = "Hundhausen/JPH_Library";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                responseOut = string.Empty;

                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            responseOut = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                responseOut = string.Empty;
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// A Github Release
    /// </summary>
    public class GithubRelease
    { 
        /// <summary>URL of the Release</summary>
        [JsonProperty(PropertyName = "html_url")]
        public string Url { get; set; }

        /// <summary>Tag of the Release</summary>
        [JsonProperty(PropertyName = "tag_name")]
        public string Tag { get; set; }

        /// <summary> If the Release is marked as Draft </summary>
        [JsonProperty(PropertyName = "draft")]
        public bool Draft { get; set; }

        /// <summary> If the Release is marked as PreRelease </summary>
        [JsonProperty(PropertyName = "prerelease")]
        public bool PreRelease { get; set; }

        /// <summary>Date of when the Release was Published at</summary>
        [JsonProperty(PropertyName = "published_at")]
        public DateTime PublishedAt { get; set; }

        /// <summary>Dowload URLs of the Assets</summary>
        [JsonProperty(PropertyName = "assets")]
        public List<GithubAssets> Assets { get; set; }

        /// <summary>Text (Changelog etc.) of the Release</summary>
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
    }

    /// <summary>
    /// The Assets of a Github Release
    /// </summary>
    public class GithubAssets
    {
        /// <summary>(File)name of the Asset</summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>Type of the Asset</summary>
        [JsonProperty(PropertyName = "content_type")]
        public string ContentType { get; set; }

        /// <summary>Size of the Asset</summary>
        [JsonProperty(PropertyName = "size")]
        public ulong Size { get; set; }

        /// <summary>Download URL of the Asset</summary>
        [JsonProperty(PropertyName = "browser_download_url")]
        public string DownloadUrl { get; set; }
    }
}
//----------------------------------------------------------------------
// Project is available at https://github.com/Hundhausen
//----------------------------------------------------------------------