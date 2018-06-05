
namespace Hytera.EEMS.AppLib
{
    /// <summary>
    /// 权限配置
    /// </summary>
    public class PermissionConfig
    {
        #region 主页面
        /// <summary>
        /// 指纹查看
        /// </summary>
        public static readonly string Fingerprint = "011";

        /// <summary>
        /// 指纹编辑
        /// </summary>
        public static readonly string FingerprintEditor = "011001";

        /// <summary>
        /// 采集指纹
        /// </summary>
        public static readonly string FingerprintCollect = "011002";

        /// <summary>
        /// 系统设置查看
        /// </summary>
        public static readonly string SystemSetLook = "007";

        /// <summary>
        /// 系统设置修改
        /// </summary>
        public static readonly string SystemSetUpdate = "007001";
        
        /// <summary>
        /// 查询回放模块
        /// </summary>
        public static readonly string DataSearchMoudle = "010";

        /// <summary>
        /// 系统关闭
        /// </summary>
        public static readonly string AppClose = "012";
        #endregion

        #region 采集模块
        /// <summary>
        /// 开始采集、恢复采集
        /// </summary>
        public static readonly string StartCollect = "009001";

        /// <summary>
        /// 暂停采集
        /// </summary>
        public static readonly string SuspendCollect = "009002";

        /// <summary>
        /// 设备注册
        /// </summary>
        public static readonly string DeviceRegister = "009004";

        /// <summary>
        /// 设备配对、编辑配对
        /// </summary>
        public static readonly string DeviceMatch = "009005";
        #endregion
       
        #region 查询回放模块
        /// <summary>
        /// 编辑
        /// </summary>
        public static readonly string DataSearchModuleEdit = "010001";
        /// <summary>
        /// 上传
        /// </summary>
        public static readonly string DataSearchModuleUpload = "010002";
        #endregion
    }
}
