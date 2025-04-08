﻿using PygmyModManager.Classes;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PygmyModManager.Internals
{
    public class Installer
    {
        public static ReleaseInfo GetReleaseInfoFromMod(string modName)
        {
            foreach (ReleaseInfo mod in Main.Mods)
            {
                if (mod.Name == modName)
                    return mod;
            }

            return null;
        }

        public static byte[] DownloadFile(string url)
        {
            return new WebClient().DownloadData(url);
        }

        public static void InstallMods(ListView.CheckedListViewItemCollection items2Install, string InstallDir)
        {
            foreach (ListViewItem itemToInstall in items2Install)
            {
                ReleaseInfo modInfo = GetReleaseInfoFromMod(itemToInstall.Text);
                if (modInfo == null) continue;
                
                string downloadURL = "";
                byte[] content = null;

                if (modInfo.GitPath != "NONE") {
                    // there is somewhere to get repos
                    string download_this_thing = SourceAgent.Repo_API_Endpoint + modInfo.GitPath + "/releases";
                    var releaseJSONData = JSON.Parse(SourceAgent.GatherWebContent(download_this_thing)).AsArray;

                    downloadURL = releaseJSONData["assets"].AsArray[1]["browser_download_url"];
                } else {
                    downloadURL = modInfo.Link;
                }

                try
                {
                    content = DownloadFile(downloadURL);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }

                if (content == null) continue;

                string fileName = Path.GetFileName(downloadURL);

                if (Path.GetExtension(fileName).Equals(".dll"))
                {
                    string path = Path.Combine(InstallDir, @"BepInEx\plugins", fileName);

                    if (File.Exists(path))
                        File.Delete(path);

                    File.WriteAllBytes(path, content);
                    
                } else if (Path.GetExtension(fileName).Equals(".zip")) {
                    using (MemoryStream ms = new MemoryStream(content))
                    {
                        using (var unzip = new Unzip(ms))
                        {
                            unzip.ExtractToDirectory((modInfo.InstallLocation != null) ? Path.Combine(InstallDir, modInfo.InstallLocation) : InstallDir);
                        }
                    }
                }
            }
        }
    }
}
