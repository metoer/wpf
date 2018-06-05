using Hytera.EEMS.Common;
using Hytera.EEMS.Manage.BLL;
using Hytera.EEMS.Manage.Enums;
using Hytera.EEMS.Manage.Lib;
using Hytera.EEMS.Model;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hytera.EEMS.Manage
{
    public partial class ModelResponsible
    {
        #region 主界面数据更新

        #region 执法记录信息列表
        /// <summary>
        /// 解析执法记录信息列表
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeMediaListDetail(string value)
        {
            MediaList mediaList = XmlUnityConvert.XmlDeserialize<MediaList>(value);
            if (mediaList != null && mediaList.MediaInfoList != null)
            {
                foreach (var item in mediaList.MediaInfoList)
                {

                    MediaInfo mediaInfo = ManageViewModel.MediaList.Find(p => p.RecordID.Equals(item.RecordID));
                    if (mediaInfo == null)
                    {
                        // 添加
                        ManageViewModel.MediaList.AddItem(item);
                        item.SequenceNum = ManageViewModel.MediaList.Count;
                        UpdateDetailCount(QueryType.MediaLog);
                    }
                    else
                    {
                        // 更新
                        item.ValueCloneToObject(mediaInfo);
                    }
                }
            }
        }
        /// <summary>
        /// 解析执法记录总数
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeAllMediaListCount(string value)
        {
            MediaLogsCount mediaLogsCount = XmlUnityConvert.XmlDeserialize<MediaLogsCount>(value);
            if (mediaLogsCount != null)
            {
                UpdateCount(QueryType.MediaLog, FileType.All, mediaLogsCount.AllCount);
                UpdateCount(QueryType.MediaLog, FileType.Picture, mediaLogsCount.PicCount);
                UpdateCount(QueryType.MediaLog, FileType.Video, mediaLogsCount.VideoCount);
                UpdateCount(QueryType.MediaLog, FileType.Voice, mediaLogsCount.VoiceCount);
            }
        }
        /// <summary>
        /// 清除执法记录信息列表
        /// </summary>
        public void ClearMediaList()
        {
            ManageViewModel.MediaList.Clear();
        }

        private void AnalyzeMediaLogUpload(string value)
        {
            MediaList mediaList = XmlUnityConvert.XmlDeserialize<MediaList>(value);
            if (mediaList != null && mediaList.MediaInfoList != null)
            {
                foreach (var item in mediaList.MediaInfoList)
                {

                    MediaInfo mediaInfo = ManageViewModel.MediaList.Find(p => p.RecordID.Equals(item.RecordID));
                    if (mediaInfo != null)
                    {
                        mediaInfo.UpLoadState = item.UpLoadState;
                    }
                }
            }
        }
        private void AnalyzeMediaLogEditor(string value)
        {
            MediaLogEditorResult mer = XmlUnityConvert.XmlDeserialize<MediaLogEditorResult>(value);
            if (mer != null && mer.ResultCode == "0")
            {

                MediaInfo mediaInfo = ManageViewModel.MediaList.Find(p => p.RecordID.Equals(mer.MediaLogID));
                if (mediaInfo != null)
                {
                    mediaInfo.UserImp = mediaInfo.UpdateUserImp;
                    mediaInfo.UserTag = mediaInfo.UpdateMark;
                }

                MediaInfo mediaVoiceInfo = ManageViewModel.VoicePlayMediaList.Find(p => p.RecordID.Equals(mer.MediaLogID));
                if (mediaVoiceInfo != null)
                {
                    mediaVoiceInfo.UserImp = mediaVoiceInfo.UpdateUserImp;
                    mediaVoiceInfo.UserTag = mediaVoiceInfo.UpdateMark;
                }
                MediaInfo mediaPicInfo = ManageViewModel.PicturePlayMediaList.Find(p => p.RecordID.Equals(mer.MediaLogID));
                if (mediaPicInfo != null)
                {
                    mediaPicInfo.UserImp = mediaPicInfo.UpdateUserImp;
                    mediaPicInfo.UserTag = mediaPicInfo.UpdateMark;
                }
                MediaInfo mediaVideoInfo = ManageViewModel.VideoPlayMediaList.Find(p => p.RecordID.Equals(mer.MediaLogID));
                if (mediaVideoInfo != null)
                {
                    mediaVideoInfo.UserImp = mediaVideoInfo.UpdateUserImp;
                    mediaVideoInfo.UserTag = mediaVideoInfo.UpdateMark;
                }

                ResultWindow resultWindow = CheckResultMsg(MsgType.MediaLogEditorRespond);
                if (resultWindow != null)
                {
                    resultWindow.SuccessCloseWindow();
                }
            }
            else
            {
                NewMessageBox.Show(Application.Current.FindResource("SearchManagerSaveTimeOut").ToString());
            }
        }
        #endregion

        #region 执法仪操作日志
        /// <summary>
        /// 解析执法仪操作日志
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeCameraLogsDetail(string value)
        {
            CameraLogs cameraLogs = XmlUnityConvert.XmlDeserialize<CameraLogs>(value);
            if (cameraLogs != null && cameraLogs.CameraLogInfoList != null)
            {
                foreach (var item in cameraLogs.CameraLogInfoList)
                {

                    CameraLogInfo mediaInfo = ManageViewModel.CameraLogs.Find(p => p.LogID.Equals(item.LogID));
                    if (mediaInfo == null)
                    {
                        // 添加
                        ManageViewModel.CameraLogs.AddItem(item);
                        item.SequenceNum = ManageViewModel.CameraLogs.Count;
                        UpdateDetailCount(QueryType.CameraOperateLog);
                    }
                    else
                    {
                        // 更新
                        item.ValueCloneToObject(mediaInfo);
                    }
                }
            }
        }
        /// <summary>
        /// 解析执法仪日志总数
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeCameraLogsCount(string value)
        {
            CameraLogsCount cameraLogsCount = XmlUnityConvert.XmlDeserialize<CameraLogsCount>(value);
            if (cameraLogsCount != null)
            {
                UpdateCount(QueryType.CameraOperateLog, FileType.All, cameraLogsCount.DataCount);
            }
        }

        private void AnalyzeCameraLogUpload(string value)
        {
            CameraLogs cameraLogs = XmlUnityConvert.XmlDeserialize<CameraLogs>(value);
            if (cameraLogs != null && cameraLogs.CameraLogInfoList != null)
            {
                foreach (var item in cameraLogs.CameraLogInfoList)
                {

                    CameraLogInfo cameraInfo = ManageViewModel.CameraLogs.Find(p => p.LogID.Equals(item.LogID));
                    if (cameraInfo != null)
                    {
                        cameraInfo.UpLoadState = item.UpLoadState;
                    }
                }
            }
        }
        /// <summary>
        /// 清除执法仪操作日志
        /// </summary>
        public void ClearCameraLogs()
        {
            ManageViewModel.CameraLogs.Clear();
        }
        #endregion

        #region 采集站日志
        /// <summary>
        /// 解析采集站日志
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeCollectLogsDetail(string value)
        {
            CollectLogs collectLogs = XmlUnityConvert.XmlDeserialize<CollectLogs>(value);
            
            if (collectLogs != null && collectLogs.CollectLogList != null)
            {
                foreach (var item in collectLogs.CollectLogList)
                {

                    CollectLogInfo collectInfo = ManageViewModel.CollectLogs.Find(p => p.LogID.Equals(item.LogID));
                    if (collectInfo == null)
                    {
                        // 添加
                        ManageViewModel.CollectLogs.AddItem(item);
                        item.SequenceNum = ManageViewModel.CollectLogs.Count;
                        UpdateDetailCount(QueryType.CollectOperateLog);
                    }
                    else
                    {
                        // 更新
                        item.ValueCloneToObject(collectInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 解析采集站日志总数
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeCollectLogsCount(string value)
        {
            CollectLogsCount collectLogsCount = XmlUnityConvert.XmlDeserialize<CollectLogsCount>(value);
            if (collectLogsCount != null)
            {
                UpdateCount(QueryType.CollectOperateLog, FileType.All, collectLogsCount.DataCount);
            }
        }

        private void AnalyzeCollectLogUpload(string value)
        {
            CollectLogs collectLogs = XmlUnityConvert.XmlDeserialize<CollectLogs>(value);

            if (collectLogs != null && collectLogs.CollectLogList != null)
            {
                foreach (var item in collectLogs.CollectLogList)
                {

                    CollectLogInfo collectInfo = ManageViewModel.CollectLogs.Find(p => p.LogID.Equals(item.LogID));
                    if (collectInfo != null)
                    {
                        collectInfo.UpLoadState = item.UpLoadState;
                    }
                }
            }
        }
        /// <summary>
        /// 清除采集站日志
        /// </summary>
        public void ClearCollectLogs()
        {
            ManageViewModel.CollectLogs.Clear();
        }

        #endregion

        #region 告警日志
        /// <summary>
        /// 解析告警
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeAlarmLogsDetail(string value)
        {
            AlarmLogs collectLogs = XmlUnityConvert.XmlDeserialize<AlarmLogs>(value);
            if (collectLogs != null && collectLogs.AlarmInfoList != null)
            {
                foreach (var item in collectLogs.AlarmInfoList)
                {

                    AlarmInfo alarmInfo = ManageViewModel.AlarmLogs.Find(p => p.AlarmID.Equals(item.AlarmID));
                    if (alarmInfo == null)
                    {
                        // 添加
                        ManageViewModel.AlarmLogs.AddItem(item);
                        item.SequenceNum = ManageViewModel.AlarmLogs.Count;
                        UpdateDetailCount(QueryType.HisAlarm);
                    }
                    else
                    {
                        // 更新
                        item.ValueCloneToObject(alarmInfo);
                    }
                }
            }
        }
        /// <summary>
        /// 解析告警总数
        /// </summary>
        /// <param name="value"></param>
        private void AnalyzeAlarmLogsCount(string value)
        {
            AlarmLogsCount alarmLogsCount = XmlUnityConvert.XmlDeserialize<AlarmLogsCount>(value);
            if (alarmLogsCount != null )
            {
                UpdateCount(QueryType.HisAlarm, FileType.All, alarmLogsCount.DataCount);
            }
        }
        /// <summary>
        /// 清除告警
        /// </summary>
        public void ClearAlarmLogs()
        {
            ManageViewModel.AlarmLogs.Clear();
        }

        #endregion

        #endregion

        #region 播放界面数据更新

        /// <summary>
        /// 视频播放列表
        /// </summary>
        /// <param name="item"></param>
        public void AnalyzeVideoPlayMediaList(MediaInfo item)
        {


            MediaInfo mediaInfo = ManageViewModel.VideoPlayMediaList.Find(p => p.RecordID.Equals(item.RecordID));
            if (mediaInfo == null)
            {
                // 添加
                ManageViewModel.VideoPlayMediaList.AddItem(item);
            }
            else
            {
                // 更新
                item.ValueCloneToObject(mediaInfo);
            }
        }

        /// <summary>
        /// 清除视频播放列表
        /// </summary>
        public void ClearVideoPlayMediaList()
        {
            ManageViewModel.VideoPlayMediaList.Clear();
        }

        /// <summary>
        /// 音频播放列表
        /// </summary>
        /// <param name="item"></param>
        public void AnalyzeVoicePlayMediaList(MediaInfo item)
        {


            MediaInfo mediaInfo = ManageViewModel.VoicePlayMediaList.Find(p => p.RecordID.Equals(item.RecordID));
            if (mediaInfo == null)
            {
                // 添加
                ManageViewModel.VoicePlayMediaList.AddItem(item);
            }
            else
            {
                // 更新
                item.ValueCloneToObject(mediaInfo);
            }
        }

        /// <summary>
        /// 清除音频播放列表
        /// </summary>
        public void ClearVoicePlayMediaList()
        {
            ManageViewModel.VoicePlayMediaList.Clear();
        }

        /// <summary>
        /// 图片播放列表
        /// </summary>
        /// <param name="item"></param>
        public void AnalyzePicturePlayMediaList(MediaInfo item)
        {


            MediaInfo mediaInfo = ManageViewModel.PicturePlayMediaList.Find(p => p.RecordID.Equals(item.RecordID));
            if (mediaInfo == null)
            {
                // 添加
                ManageViewModel.PicturePlayMediaList.AddItem(item);
            }
            else
            {
                // 更新
                item.ValueCloneToObject(mediaInfo);
            }
        }

        /// <summary>
        /// 清除图片播放列表
        /// </summary>
        public void ClearPicturePlayMediaList()
        {
            ManageViewModel.PicturePlayMediaList.Clear();
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public bool AnalyzeHisPlayMediaList(MediaInfo item)
        {

            MediaInfo mediaInfo = ManageViewModel.HisPlayMediaList.Find(p => p.RecordID.Equals(item.RecordID));
            if (mediaInfo == null)
            {
                // 添加
                ManageViewModel.HisPlayMediaList.AddItem(item);
                UpdateDetailCount(QueryType.PlayList);
                return true;
            }
            else
            {
                // 更新
                item.ValueCloneToObject(mediaInfo);
                return false;
            }
        }
        
        public void RemoveHisPlayListByItem(MediaInfo item)
        {
            ManageViewModel.HisPlayMediaList.Remove(item);
            UpdateDetailCount(QueryType.PlayList);
        }
        /// <summary>
        /// 清除图片播放列表
        /// </summary>
        public void ClearHisPlayMediaList()
        {
            ManageViewModel.HisPlayMediaList.Clear();
            UpdateDetailCount(QueryType.PlayList);
        }
    }
}
