using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

public static class GitHub {
    public static string UserAgent;
    static readonly WebClient client = new();
    static readonly Regex branchesRegex = new("name\":\"([^\"]*)", RegexOptions.Compiled);
    static readonly Regex fileRegex = new("content\":\"([^\"]*)", RegexOptions.Compiled);
    static readonly Regex filesRegex = new("path\":\"([^\"]*).*?url\":\"([^\"]*)", RegexOptions.Compiled);

    static MatchCollection Get(string url, Regex regex) {
        client.Headers["User-Agent"] = UserAgent ?? throw new("User-Agent not set.");
        var json = client.DownloadString(url);
        return regex.Matches(json);
    }

    public static byte[] File(string url) => Get(url, fileRegex).Select(x => Convert.FromBase64String(x.Groups[1].Value.Replace("\\n", ""))).FirstOrDefault();

    public static string Branch(string owner, string repo) => Branches(owner, repo).First();

    public static IEnumerable<string> Branches(string owner, string repo) =>
        Get($"https://api.github.com/repos/{owner}/{repo}/branches", branchesRegex).Select(x => x.Groups[1].Value);

    public static IEnumerable<(string path, string url)> Files(string owner, string repo) =>
        Files(owner, repo, Branch(owner, repo));

    public static IEnumerable<(string path, string url)> Files(string owner, string repo, string version) =>
        Get($"https://api.github.com/repos/{owner}/{repo}/git/trees/{version}?recursive=1", filesRegex)
            .Select(x => (x.Groups[1].Value, x.Groups[2].Value));

    public static byte[] Zip(string owner, string repo) => Zip(owner, repo, Branch(owner, repo));

    public static byte[] Zip(string owner, string repo, string version) =>
        client.DownloadData($"https://codeload.github.com/{owner}/{repo}/zip/{version}");
}