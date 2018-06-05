using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Model.AppLib
{
    public class EEMSMediaAPI
    {
        /**
	@param [in]         无
	@param [out]        无
	@return             成功返回0，失败返回-1
	@note               此方法在调用解码函数之前调用，
	*/
        [DllImport("EEMSMediaDecipher", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int DecipherInit(string pszKey, int iKeySize, int iCipherBuffSize);

        /** 
	@param [in]         无
	@param [out]        无
	@return             成功返回0，失败返回错误结果码
	@note               解密视频BUFF
	*/
        [DllImport("EEMSMediaDecipher", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int DecipherVideo(string pszData, int iDataSize);

        /**
        @param [in]         无
        @param [out]        无
        @return             成功返回0，失败返回错误结果码
        @note               解密音频BUFF
        */
        [DllImport("EEMSMediaDecipher", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int DecipherAudio(string pszData, int iDataSize);


        /**
        @param [in]         无
        @param [out]        无
        @return             成功返回0，失败返回错误结果码
        @note               解密图片BUFF
        */
        [DllImport("EEMSMediaDecipher", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int DecipherPicture(string pszData, int iDataSize);

        /**
        @param [in]         无
        @param [out]        无
        @return             成功返回0，失败返回-1
        @note               此方法在最后调用，
        */
        [DllImport("EEMSMediaDecipher", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void DecipherUninit();
        

        [DllImport("EEMSMediaDecipherEx", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int DecipherInitEx();
        /** 
        @param [in]         无
        @param [out]        无
        @return             成功返回0，失败返回错误结果码
        @note               解密视频BUFF
        */
        [DllImport("EEMSMediaDecipherEx", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int DecipherVideoEx(string pszFilePath, string pszOutFilePath);

        /**
        @param [in]         无
        @param [out]        无
        @return             成功返回0，失败返回错误结果码
        @note               解密音频BUFF
        */
        [DllImport("EEMSMediaDecipherEx", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int DecipherAudioEx(string pszFilePath, string pszOutFilePath);


        /**
        @param [in]         无
        @param [out]        无
        @return             成功返回0，失败返回错误结果码
        @note               解密图片BUFF
        */
        [DllImport("EEMSMediaDecipherEx", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int DecipherPictureEx(string pszFilePath, string pszOutFilePath);
        
    }
}
