using System;
using dnlib.DotNet;

namespace dnEditor.Misc
{
    /*
     * FULL CREDITS OF THIS CLASS GO TO THE CODER OF SIMPLE ASSEMBLY EXPLORER.
     * I FULLY COPIED IT AND TAKE NO CREDIT FOR HIS WORK!
     */

    public static class TokenUtils
    {
        public static string FullMetadataTokenString(this MDToken mdToken)
        {
            return UintToHexString(mdToken.ToUInt32(), 8);
        }

        public static string MetadataTokenString(this MDToken mdToken)
        {
            return UintToHexString(mdToken.Rid, 6);
        }

        public static string UintToHexString(uint input, int digits)
        {
            return input.ToString(String.Format("x0{0}", digits)).ToLower();
        }

    }
}
