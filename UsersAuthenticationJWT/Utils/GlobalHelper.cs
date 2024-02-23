namespace UsersAuthenticationJWT.Utils
{
    public static class GlobalHelper
    {
        /// <summary>
        /// Convert a secure string to a normal string
        /// </summary>
        /// <param name="secureStr"></param>
        /// <returns></returns>
        public static string ToPlainString(this System.Security.SecureString secureStr)
        {
            string plainStr = new System.Net.NetworkCredential(
                string.Empty,
                secureStr).Password;
            return plainStr;
        }


        /// <summary>
        /// Convert a string to a secure string
        /// </summary>
        /// <param name="plainStr"></param>
        /// <returns></returns>
        public static System.Security.SecureString ToSecureString(this string plainStr)
        {
            var secStr = new System.Security.SecureString();
            secStr.Clear();

            foreach (char c in plainStr.ToCharArray())
            {
                secStr.AppendChar(c);
            }

            return secStr;
        }
    }
}
