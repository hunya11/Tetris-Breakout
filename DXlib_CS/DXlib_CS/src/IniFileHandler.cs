using System.Text;
using System.Runtime.InteropServices;


namespace DXlib_CS.src {

    /// <summary>
    /// iniファイルを読み込むためのクラス
    /// 参考：http://www.atmarkit.co.jp/fdotnet/dotnettips/039inifile/inifile.html
    /// </summary>
    static class IniFileHandler {
        [DllImport("KERNEL32.DLL")]
        public static extern uint
          GetPrivateProfileString(string lpAppName ,
          string lpKeyName , string lpDefault ,
          StringBuilder lpReturnedString , uint nSize ,
          string lpFileName);

        [DllImport("KERNEL32.DLL" ,
            EntryPoint = "GetPrivateProfileStringA")]
        public static extern uint
          GetPrivateProfileStringByByteArray(string lpAppName ,
          string lpKeyName , string lpDefault ,
          byte[] lpReturnedString , uint nSize ,
          string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint
          GetPrivateProfileInt(string lpAppName ,
          string lpKeyName , int nDefault , string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint WritePrivateProfileString(
          string lpAppName ,
          string lpKeyName ,
          string lpString ,
          string lpFileName);


        public static string GetIniValue(string section , string key,string filePath) {
            StringBuilder sb = new StringBuilder(256);
            GetPrivateProfileString(section , key , "err" , sb , (uint)sb.Capacity , filePath);
            return sb.ToString();
        }

        public static int GetSectionCount(string filePath) {
            byte [] ar = new byte[1024];
            uint resultSize
                = IniFileHandler.GetPrivateProfileStringByByteArray(
                null , null , "default" , ar , (uint)ar.Length , filePath);
            string result = Encoding.Default.GetString(ar, 0, (int)resultSize-1);
            string [] sections = result.Split('\0');
            return sections.Length;
        }

    }
}
