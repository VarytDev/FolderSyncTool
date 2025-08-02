using FluentAssertions;
using FolderSyncTool.App.Common.Utilities;
using System.Text;

namespace FolderSyncTool.UnitTests.Utilities
{
    public class FileHashingUtilityTests
    {
        /// <summary>
        /// Test cases took from: https://www.rfc-editor.org/rfc/rfc1321#appendix-A.5
        /// </summary>

        [Theory]
        [InlineData("", "d41d8cd98f00b204e9800998ecf8427e")]
        [InlineData("a", "0cc175b9c0f1b6a831c399e269772661")]
        [InlineData("abc", "900150983cd24fb0d6963f7d28e17f72")]
        [InlineData("message digest", "f96b697d7cb7938d525a2f31aaf161d0")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "c3fcd3d76192e4007dfb496cca67e13b")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "d174ab98d277d9f5a5611c2c9f419d9f")]
        [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890", "57edf4a22be3c955ac49da2e2107b67a")]
        public void CheckMD5_ShouldReturnValidChecksum(string value, string hash)
        {
            //Arrange
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));

            //Act
            string hashToCompare = FileHashingUtility.CheckMD5(stream);

            //Assert
            hash.Should().Be(hashToCompare);
        }
    }
}
