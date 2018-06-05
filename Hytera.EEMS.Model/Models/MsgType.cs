
namespace Hytera.EEMS.Model
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 设置查看
        /// </summary>
        SetLook,

        /// <summary>
        /// 设置修改
        /// </summary>
        SetUpdate,

        /// <summary>
        /// 指纹添加
        /// </summary>
        FingerprintAdd,

        /// <summary>
        /// 指纹删除
        /// </summary>
        FingerprintDel,

        /// <summary>
        /// 账号验证
        /// </summary>
        AccountValidate,

        /// <summary>
        /// 账号验证结果
        /// </summary>
        AccountValidateResult,

        /// <summary>
        /// 指纹验证
        /// </summary>
        FingerprintValidate,

        /// <summary>
        /// 停止指纹验证
        /// </summary>
        FingerprintStopValidate,

        /// <summary>
        /// 电子证据编辑请求
        /// </summary>
        MediaLogEditorRequest,

        /// <summary>
        /// 电子证据编辑返回结果
        /// </summary>
        MediaLogEditorRespond,

        /// <summary>
        /// 电子证据上传请求
        /// </summary>
        MediaLogUploadRequest,

        /// <summary>
        /// 电子证据上传返回结果
        /// </summary>
        MediaLogUploadRespond,

        /// <summary>
        /// 执法记录条件查询总数请求
        /// </summary>
        MediaLogCountRequest,

        /// <summary>
        /// 执法记录条件查询内容
        /// </summary>
        MediaLogRequest,

        /// <summary>
        /// 执法记录仪信息查询结果
        /// </summary>
        MediaLogInfosResult,

        /// <summary>
        /// 执法记录总数
        /// </summary>
        MediaLogCountRespond,

        /// <summary>
        /// 执法记录结果内容
        /// </summary>
        MediaLogRespond,

        /// <summary>
        /// 请求执法记录仪信息列表
        /// </summary>
        DeviceInfosRequest,

        /// <summary>
        /// 返回执法记录仪信息列表
        /// </summary>
        DeviceInfosRespond,

        /// <summary>
        /// 执法记录仪状态更新
        /// </summary>
        DeviceUpdateState,

        /// <summary>
        /// 手动采集
        /// </summary>
        DeviceHandCollect,

        /// <summary>
        /// 手动采集结果
        /// </summary>
        DeviceHandCollectResult,

        /// <summary>
        /// 恢复采集
        /// </summary>
        DeviceResumeCollect,

        /// <summary>
        /// 恢复采集结果
        /// </summary>
        DeviceResumeCollectResult,

        /// <summary>
        /// 停止采集结果
        /// </summary>
        DeviceStopCollectResult,

        /// <summary>
        /// 执法仪配对
        /// </summary>
        DevicePair,

        /// <summary>
        /// 执法记录仪配对结果
        /// </summary>
        DevicePairResult,

        /// <summary>
        /// 执法记录仪取消配对
        /// </summary>
        DeviceCancelPair,

        /// <summary>
        /// 执法记录仪取消配对结果
        /// </summary>
        DeviceCancelPairResult,

        /// <summary>
        /// 执法记录仪移除
        /// </summary>
        DeviceInfosRemove,

        /// <summary>
        /// 本机设置信息
        /// </summary>
        PcSetInfos,

        /// <summary>
        /// 请求采集站状态
        /// </summary>
        PcStateRequest,

        /// <summary>
        /// 采集站状态
        /// </summary>
        PcState,

        /// <summary>
        /// 警员信息列表
        /// </summary>
        PoliceInfosRequest,

        /// <summary>
        /// 返回警员信息列表
        /// </summary>
        PoliceInfosRespond,

        /// <summary>
        /// 执法记录仪注册
        /// </summary>
        DeviceRegister,

        /// <summary>
        /// 执法记录仪注册结果
        /// </summary>
        DeviceRegisterResult,

        /// <summary>
        /// 采集站日志总数查询
        /// </summary>
        LogCountRequest,

        /// <summary>
        /// 采集站日志内容查询
        /// </summary>
        LogContentRequest,

        /// <summary>
        /// 采集站日志
        /// </summary>
        CollectLogRecord,

        /// <summary>
        /// 采集站日志数量
        /// </summary>
        CollecLogCountRespond,

        /// <summary>
        /// 执法记录日志数量
        /// </summary>
        CameraLogCountRespond,

        /// <summary>
        /// 采集站日志内容
        /// </summary>
        CollecLogRespond,

        /// <summary>
        /// 执法记录操作日志内容
        /// </summary>
        CameraLogRespond,

        /// <summary>
        /// 执法仪日志上传请求
        /// </summary>
        CameraLogUploadRequest,

        /// <summary>
        /// 执法仪日志上传结果
        /// </summary>
        CameraLogUploadRespond,

        /// <summary>
        /// 采集站日志上传请求
        /// </summary>
        CollectLogUploadRequest,

        /// <summary>
        /// 采集站日志上传结果
        /// </summary>
        CollectLogUploadRespond,

        /// <summary>
        /// 告警信息数量查询条件
        /// </summary>
        AlarmCountRequest,

        /// <summary>
        /// 告警信息内容查询条件
        /// </summary>
        AlarmLogRequest,

        /// <summary>
        /// 告警信息数量
        /// </summary>
        AlarmCountRespond,

        /// <summary>
        /// 告警日志内容
        /// </summary>
        AlarmLogRespond,

        /// <summary>
        ///  数据同步请求
        /// </summary>
        DataRefreshRequest,

        /// <summary>
        /// 数据同步请求响应
        /// </summary>
        DataRefreshRespond,

        /// <summary>
        /// 同步数据更新
        /// </summary>
        DataRefreshUpdate,

        /// <summary>
        /// 心跳包
        /// </summary>
        Heartbeat,

        /// <summary>
        /// 操作日志内容
        /// </summary>
        StationLogContent,

        /// <summary>
        /// 高级查询权限
        /// </summary>
        AccountLimits,

        /// <summary>
        /// 高级查询权限结果
        /// </summary>
        AccountLimitsResult,

        /// <summary>
        /// 解密
        /// </summary>
        DecipherFile,

        /// <summary>
        ///  License信息请求
        /// </summary>
        LicenseRequest,

        /// <summary>
        ///  License信息返回
        /// </summary>
        LicenseRespond,

        /// <summary>
        /// 请求采集端口信息
        /// </summary>
        PortInfosRequest,

        /// <summary>
        /// 返回采集端口信息
        /// </summary>
        PortInfosRespond,

        /// <summary>
        /// 设置优先端口请求
        /// </summary>
        SetFirstPortRequest,

        /// <summary>
        /// 设置优先端口返回信息 
        /// </summary>
        SetFirstPortRespond,

        /// <summary>
        /// 执法记录类型
        /// </summary>
        DeviceTypesRequest,

        /// <summary>
        /// 执法记录类型
        /// </summary>
        DeviceTypesRespond,

        /// <summary>
        /// 用户指纹信息列表请求
        /// </summary>
        FingerInfosRequest,

        /// <summary>
        /// 用户指纹信息列表响应
        /// </summary>
        FingerInfosRespond,

        /// <summary>
        /// 指纹采集
        /// </summary>
        FingerStartRequest,

        /// <summary>
        /// 指纹采集结果
        /// </summary>
        FingerEndResult,

        /// <summary>
        /// 指纹采集保存请求
        /// </summary>
        FingerSaveRequest,

        /// <summary>
        /// 指纹采集保存响应
        /// </summary>
        FingerSaveRespond,

        /// <summary>
        /// 指纹采集停止
        /// </summary>
        FingerStopRequest,

        /// <summary>
        /// 指纹编辑请求
        /// </summary>
        FingerEditorRequest,

        /// <summary>
        /// 指纹编辑响应
        /// </summary>
        FingerEditorRespond
    }
}
