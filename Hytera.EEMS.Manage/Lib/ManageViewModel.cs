using Hytera.EEMS.Common;
using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Manage.Lib
{
    /// <summary>
    /// 数据管理模块模型对象层
    /// </summary>
    public class ManageViewModel
    {
        static ManageViewModel()
        {
            MediaList = new ThreadSafeObservable<MediaInfo>();
            CameraLogs = new ThreadSafeObservable<CameraLogInfo>();
            CollectLogs = new ThreadSafeObservable<CollectLogInfo>();
            AlarmLogs = new ThreadSafeObservable<AlarmInfo>();

            VideoPlayMediaList = new ThreadSafeObservable<MediaInfo>();
            VoicePlayMediaList = new ThreadSafeObservable<MediaInfo>();
            PicturePlayMediaList = new ThreadSafeObservable<MediaInfo>();
            HisPlayMediaList = new ThreadSafeObservable<MediaInfo>();
        }

        #region 主界面数据源
        /// <summary>
        /// 执法记录信息列表
        /// </summary>
        public static ThreadSafeObservable<MediaInfo> MediaList
        {
            get;
            set;
        }

        /// <summary>
        /// 执法仪操作日志
        /// </summary>
        public static ThreadSafeObservable<CameraLogInfo> CameraLogs
        {
            get;
            set;
        }

        /// <summary>
        /// 采集站操作日志
        /// </summary>
        public static ThreadSafeObservable<CollectLogInfo> CollectLogs
        {
            get;
            set;
        }

        public static ThreadSafeObservable<AlarmInfo> AlarmLogs
        {
            get;
            set;
        }

        #endregion

        #region 播放界面数据源
        /// <summary>
        /// 视频播放列表
        /// </summary>
        public static ThreadSafeObservable<MediaInfo> VideoPlayMediaList
        {
            get;
            set;
        }

        /// <summary>
        /// 音频播放列表
        /// </summary>
        public static ThreadSafeObservable<MediaInfo> VoicePlayMediaList
        {
            get;
            set;
        }

        /// <summary>
        /// 图片播放列表
        /// </summary>
        public static ThreadSafeObservable<MediaInfo> PicturePlayMediaList
        {
            get;
            set;
        }
        
        #endregion
        /// <summary>
        /// 历史播放列表
        /// </summary>
        public static ThreadSafeObservable<MediaInfo> HisPlayMediaList
        {
            get;
            set;
        }
        
    }
}
