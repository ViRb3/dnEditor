using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using dnlib.DotNet;
using AssemblyHashAlgorithm = System.Configuration.Assemblies.AssemblyHashAlgorithm;

namespace dnEditor.Misc
{
    /*
     * FULL CREDITS OF THIS CLASS GO TO THE CODER OF SIMPLE ASSEMBLY EXPLORER.
     * I FULLY COPIED IT AND TAKE NO CREDIT FOR HIS WORK!
     */

    public class TokenUtils
    {
        public static byte[] GetPublicKeyTokenFromKeyFile(string keyFile)
        {
            byte[] pubKey = GetPublicKeyFromKeyFile(keyFile);
            return GetPublicKeyToken(pubKey, AssemblyHashAlgorithm.SHA1);
        }

        public static byte[] GetPublicKeyFromKeyFile(string keyFile)
        {
            using (var fs = new FileStream(keyFile, FileMode.Open, FileAccess.Read))
            {
                var sn = new StrongNameKeyPair(fs);
                return sn.PublicKey;
            }
        }

        public static byte[] GetPublicKeyToken(byte[] publicKey, AssemblyHashAlgorithm hashAlgo)
        {
            byte[] token = null;

            if (publicKey != null && publicKey.Length > 0)
            {
                HashAlgorithm ha = SHA1.Create();
                byte[] hash = ha.ComputeHash(publicKey);
                // we need the last 8 bytes in reverse order
                token = new byte[8];
                Array.Copy(hash, (hash.Length - 8), token, 0, 8);
                Array.Reverse(token, 0, 8);
            }
            return token;
        }

        public static byte[] GetPublicKeyToken(AssemblyName an)
        {
            return an.GetPublicKeyToken();
        }

        public static byte[] GetPublicKeyToken(string fileName)
        {
            AssemblyName an = AssemblyName.GetAssemblyName(fileName);
            return GetPublicKeyToken(an);
        }

        public static string GetPublicKeyTokenString(AssemblyName an)
        {
            return GetPublicKeyTokenString(GetPublicKeyToken(an));
        }

        public static string GetPublicKeyTokenString(byte[] token)
        {
            return BytesUtils.BytesToHexString(token);
        }

        public static string GetPublicKeyTokenString(string fileName)
        {
            return GetPublicKeyTokenString(GetPublicKeyToken(fileName));
        }

        public static string GetFullMetadataTokenString(MDToken mt)
        {
            return UintToHexString(mt.ToUInt32(), 8);
        }

        public static string GetMetadataTokenString(MDToken mt)
        {
            return UintToHexString(mt.Rid, 6);
        }

        public static string GetReferenceTokenString(byte[] token)
        {
            if (token != null && token.Length > 0)
            {
                return BytesUtils.BytesToHexString(token);
            }
            return "null";
        }

        public static string UintToHexString(uint ui, int digits)
        {
            return ui.ToString(String.Format("x0{0}", digits)).ToLower();
        }
    } //end of class
}