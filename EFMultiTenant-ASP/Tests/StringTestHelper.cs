using System.IO;

namespace EFMultiTenantTest
{
    public class StringTestHelper
    {
        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }
    }
}