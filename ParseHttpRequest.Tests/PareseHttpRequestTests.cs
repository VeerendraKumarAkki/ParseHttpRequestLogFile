using System;
using System.Configuration;
using System.IO;
using Xunit;

namespace ParseHttpRequest.Tests
{
    public class PareseHttpRequestTests
    {
        [Fact]
        public void ReadFileContent_CheckForArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ParseHttpRequests.ReadFile(null));
        }

        [Fact]
        public void ReadFileContent_FileNotFoundException()
        {
            string filePath = Environment.CurrentDirectory + ConfigurationManager.AppSettings["TestFile"];
            Assert.Throws<FileNotFoundException>(() => ParseHttpRequests.ReadFile(filePath));
        }

        [Fact]
        public void ReadFileContent_DirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(() => ParseHttpRequests.ReadFile(@"ToProcess\Test"));
        }

        [Fact]
        public void ReadFileContent_FileContentExists()
        {
            string filePath = Environment.CurrentDirectory + ConfigurationManager.AppSettings["LogFile"];
            Assert.NotNull(ParseHttpRequests.ReadFile(filePath));
        }

        [Fact]
        public void ExtractIpAddress_Test()
        {
            var URL = "550.112.00.11 - admin [11/Jul/2018:17:33:01 +0200] \"GET / asset.css HTTP / 1.1\" 200 3574 \" - \" \"Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 536.6(KHTML, like Gecko) Chrome / 20.0.1092.0 Safari / 536.6\"";
            string expected = "550.112.00.11";
            string actual = ParseHttpRequests.ExtractIpAddress(URL);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractUrl_Test()
        {
            var URL = "550.112.00.11 - admin [11/Jul/2018:17:33:01 +0200] \"GET /asset.css HTTP / 1.1\" 200 3574 \" - \" \"Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 536.6(KHTML, like Gecko) Chrome / 20.0.1092.0 Safari / 536.6\"";
            string expected = "/asset.css";
            ConfigurationManager.AppSettings["CharsAfterGet"] = "4";
            string actual = ParseHttpRequests.ExtractUrl(URL);
            Assert.Equal(expected, actual);
        }

    }
}
