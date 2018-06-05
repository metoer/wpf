using Hytera.EEMS.Common;
using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Log;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.AppLib;
using Hytera.EEMS.Resources;
using Hytera.EEMS.Resources.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hytera.EEMS.Manage.BLL
{
    public class SearchManager
    {
        public UserInfos CurrentUserInfo;

        private static object lockthis = new object();

        private static SearchManager _SearchManager = null;

        public static SearchManager GetInstance()
        {
            lock (lockthis)
            {
                if (_SearchManager == null)
                {
                    _SearchManager = new SearchManager();
                }
            }
            return _SearchManager;
        }
        /// <summary>
        /// 告警查询条件
        /// </summary>
        public AlarmLogsSearch AlarmLogsSearch ;
        
        /// <summary>
        /// 电子证据查询条件
        /// </summary>
        public MediaLogsSerach MediaLogsSerach;
        /// <summary>
        /// 采集站日志查询条件
        /// </summary>
        public CollectLogsSerach CollectLogsSerach;
        /// <summary>
        /// 执法仪日志查询条件
        /// </summary>
        public CameraLogsSerach CameraLogsSerach;

        private SearchManager()
        {
            AlarmLogsSearch = new AlarmLogsSearch();
            AlarmLogsSearch.PageCount = AppConfigInfos.AppStateInfos.SearchPageCount;
            AlarmLogsSearch.IsAdvanced = false;
            MediaLogsSerach = new MediaLogsSerach();
            MediaLogsSerach.PageCount = AppConfigInfos.AppStateInfos.SearchPageCount;
            MediaLogsSerach.IsAdvanced = false;
            CollectLogsSerach = new CollectLogsSerach();
            CollectLogsSerach.PageCount = AppConfigInfos.AppStateInfos.SearchPageCount;
            CollectLogsSerach.IsAdvanced = false;
            CameraLogsSerach = new CameraLogsSerach();
            CameraLogsSerach.PageCount = AppConfigInfos.AppStateInfos.SearchPageCount;
            CameraLogsSerach.IsAdvanced = false;
            CurrentUserInfo = new UserInfos();
        }

        /// <summary>
        /// 电子证据总数
        /// </summary>
        /// <param name="mls"></param>
        public void SearchMediaLogCount(MediaLogsSerach mls)
        {
            string UserGuid = string.Empty;
            string OrgID = string.Empty;
            if (AppConfigInfos.CurrentUserInfos.UserType == "1")
            {
                if(mls.IsAdvanced)
                {
                    UserGuid = mls.UserGuid;
                    OrgID = string.IsNullOrEmpty(mls.OrgID) ? AppConfigInfos.LimitsUserInfos.OrgIDCodeStr : mls.OrgID;
                }
                else
                {
                    OrgID = AppConfigInfos.LimitsUserInfos.OrgIDCodeStr;
                }
            }
            else
            {
                UserGuid = AppConfigInfos.CurrentUserInfos.UserGuid;
                OrgID= AppConfigInfos.CurrentUserInfos.OrgID;
            }

            Conditions cons = new Conditions();
            cons.AddItem("UserGuid", UserGuid);
            cons.AddItem("OrgID", OrgID);
            cons.AddItem("FileName", mls.IsAdvanced ? mls.FileName : "");
            cons.AddItem("DeviceID", mls.IsAdvanced ? mls.DeviceID : "");
            cons.AddItem("FileType", "0");
            cons.AddItem("UserImp", mls.IsAdvanced ? mls.UserImp : "0");
            cons.AddItem("UserTag", mls.IsAdvanced ? mls.UserTag : "");
            cons.AddItem("UpLoadState", mls.IsAdvanced ? mls.UploadState : "");
            cons.AddItem("SearchTime", mls.SearchTime);
            cons.AddItem("CollectStartTime", mls.CollectStartTime);
            cons.AddItem("CollectEndTime", mls.CollectEndTime);

            ModelResponsible.Instance.SendMessage(MsgType.MediaLogCountRequest, cons);
        }
        /// <summary>
        /// 电子证据明细
        /// </summary>
        /// <param name="mls"></param>
        public void SearchMediaLogDetail(MediaLogsSerach mls,bool showWindow=true)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }
            string UserGuid = string.Empty;
            string OrgID = string.Empty;
            if (AppConfigInfos.CurrentUserInfos.UserType == "1")
            {
                if (mls.IsAdvanced)
                {
                    UserGuid = mls.UserGuid;
                    OrgID = string.IsNullOrEmpty(mls.OrgID) ? AppConfigInfos.LimitsUserInfos.OrgIDCodeStr : mls.OrgID;
                }
                else
                {
                    OrgID = AppConfigInfos.LimitsUserInfos.OrgIDCodeStr;
                }
            }
            else
            {
                UserGuid = AppConfigInfos.CurrentUserInfos.UserGuid;
                OrgID = AppConfigInfos.CurrentUserInfos.OrgID;
            }

            Conditions cons = new Conditions();
            cons.AddItem("UserGuid", UserGuid);
            cons.AddItem("OrgID", OrgID);
            cons.AddItem("FileName", mls.IsAdvanced ? mls.FileName : "");
            cons.AddItem("DeviceID", mls.IsAdvanced ? mls.DeviceID : "");
            cons.AddItem("FileType", mls.FileType);
            cons.AddItem("UserImp", mls.IsAdvanced ? mls.UserImp : "0");
            cons.AddItem("UserTag", mls.IsAdvanced ? mls.UserTag : "");
            cons.AddItem("UpLoadState", mls.IsAdvanced ? mls.UploadState : ""); 
            cons.AddItem("SearchTime", mls.SearchTime);
            cons.AddItem("CollectStartTime", mls.CollectStartTime);
            cons.AddItem("CollectEndTime", mls.CollectEndTime);
            cons.AddItem("PageIndex", mls.PageIndex.ToString());
            cons.AddItem("PageCount", mls.PageCount.ToString());
            if (showWindow)
            {
                ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(null, MsgType.MediaLogRequest, MsgType.MediaLogRespond, cons, Application.Current.FindResource("SearchManagerSearch").ToString(), AppConfigInfos.AppStateInfos.SearchTimeOut);
                MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
                if (msgBoxResult == MessageBoxResult.Cancel)
                {
                    NewMessageBox.Show(Application.Current.FindResource("SearchManagerSearchFail").ToString());
                }
            }
            else
                ModelResponsible.Instance.SendMessage(MsgType.MediaLogRequest, cons);
        }
        /// <summary>
        /// 告警总数
        /// </summary>
        /// <param name="als"></param>
        public void SearchAlarmLogCount(AlarmLogsSearch als)
        {
            if (DateTime.Compare(Convert.ToDateTime(als.AlarmtEndTime), DateTime.Now) > 0)
                SearchManager.GetInstance().AlarmLogsSearch.SearchTime = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            else
                SearchManager.GetInstance().AlarmLogsSearch.SearchTime = als.AlarmtEndTime;

            Conditions cons = new Conditions();
            cons.AddItem("AlarmLevel", als.IsAdvanced ? als.AlarmLevel : "");
            cons.AddItem("AlarmCode", als.IsAdvanced ? als.AlarmCode : "");
            cons.AddItem("AlarmModule", (als.IsAdvanced && !string.IsNullOrEmpty(als.AlarmModule) )? als.AlarmModule : "16,17,18,21,23,25,28");
            cons.AddItem("AlarmStatus", als.IsAdvanced ? als.AlarmStatus : "");
            cons.AddItem("AlarmIp", EEMSConfigHelper.GetValueByCommomConfig("config/CommonConfig/local_machine_ip", "127.0.0.1"));
            cons.AddItem("AlarmStartTime", als.AlarmStartTime);
            cons.AddItem("AlarmEndTime", als.SearchTime);

            ModelResponsible.Instance.SendMessage(MsgType.AlarmCountRequest, cons);
        }
        /// <summary>
        /// 告警明细
        /// </summary>
        /// <param name="als"></param>
        public void SearchAlarmLogDetail(AlarmLogsSearch als, bool showWindow = true)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }
            Conditions cons = new Conditions();
            cons.AddItem("AlarmLevel", als.IsAdvanced ? als.AlarmLevel : "");
            cons.AddItem("AlarmCode", als.IsAdvanced ? als.AlarmCode : "");
            cons.AddItem("AlarmModule", (als.IsAdvanced && !string.IsNullOrEmpty(als.AlarmModule)) ? als.AlarmModule : "16,17,18,19,21,23,25,28");
            cons.AddItem("AlarmStatus", als.IsAdvanced ? als.AlarmStatus : "");
            cons.AddItem("AlarmIp", EEMSConfigHelper.GetValueByCommomConfig("config/CommonConfig/local_machine_ip", "127.0.0.1"));
            cons.AddItem("AlarmStartTime", als.AlarmStartTime);
            cons.AddItem("AlarmEndTime", als.SearchTime);
            cons.AddItem("PageIndex", als.PageIndex.ToString());
            cons.AddItem("PageCount", als.PageCount.ToString());
            if (showWindow)
            {
                ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(null, MsgType.AlarmLogRequest, MsgType.AlarmLogRespond, cons, Application.Current.FindResource("SearchManagerSearch").ToString(), AppConfigInfos.AppStateInfos.SearchTimeOut);
                MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
                if (msgBoxResult == MessageBoxResult.Cancel)
                {
                    NewMessageBox.Show(Application.Current.FindResource("SearchManagerSearchFail").ToString());
                }
            }
            else
                ModelResponsible.Instance.SendMessage(MsgType.AlarmLogRequest, cons);
        }
        /// <summary>
        /// 采集站日志总数
        /// </summary>
        /// <param name="cls"></param>
        public void SearchCollectLogCount(CollectLogsSerach cls)
        {
            string UserGuid = string.Empty;
            string OrgID = string.Empty;
            if (AppConfigInfos.CurrentUserInfos.UserType == "1")
            {
                if (cls.IsAdvanced)
                {
                    UserGuid = cls.UserGuid;
                    OrgID = string.IsNullOrEmpty(cls.OrgID) ? AppConfigInfos.LimitsUserInfos.OrgIDCodeStr : cls.OrgID;
                }
                else
                {
                    OrgID = AppConfigInfos.LimitsUserInfos.OrgIDCodeStr;
                }
            }
            else
            {
                UserGuid = AppConfigInfos.CurrentUserInfos.UserGuid;
                OrgID = AppConfigInfos.CurrentUserInfos.OrgID;
            }

            if (DateTime.Compare(Convert.ToDateTime(cls.CollectEndTime), DateTime.Now) > 0)
                SearchManager.GetInstance().CollectLogsSerach.SearchTime = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            else
                SearchManager.GetInstance().CollectLogsSerach.SearchTime = cls.CollectEndTime;

            Conditions cons = new Conditions();
            cons.AddItem("UserGuid", UserGuid);
            cons.AddItem("OrgID", OrgID);
            cons.AddItem("DeviceID",cls.IsAdvanced ? cls.DeviceID :"");
            cons.AddItem("LogType", cls.LogType);
            cons.AddItem("UpLoadState", cls.IsAdvanced ? cls.UploadState : "");
            cons.AddItem("CollectStartTime", cls.CollectStartTime);
            cons.AddItem("CollectEndTime", cls.SearchTime);
            ModelResponsible.Instance.SendMessage(MsgType.LogCountRequest, cons);
        }
        /// <summary>
        /// 采集站日志明细
        /// </summary>
        /// <param name="cls"></param>
        public void SearchCollectLogDetail(CollectLogsSerach cls, bool showWindow = true)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }
            string UserGuid = string.Empty;
            string OrgID = string.Empty;
            if (AppConfigInfos.CurrentUserInfos.UserType == "1")
            {
                if (cls.IsAdvanced)
                {
                    UserGuid = cls.UserGuid;
                    OrgID = string.IsNullOrEmpty(cls.OrgID) ? AppConfigInfos.LimitsUserInfos.OrgIDCodeStr : cls.OrgID;
                }
                else
                {
                    OrgID = AppConfigInfos.LimitsUserInfos.OrgIDCodeStr;
                }
            }
            else
            {
                UserGuid = AppConfigInfos.CurrentUserInfos.UserGuid;
                OrgID = AppConfigInfos.CurrentUserInfos.OrgID;
            }
            Conditions cons = new Conditions();
            cons.AddItem("UserGuid", UserGuid);
            cons.AddItem("OrgID", OrgID);
            cons.AddItem("DeviceID", cls.IsAdvanced ? cls.DeviceID : "");
            cons.AddItem("LogType", cls.LogType);
            cons.AddItem("UpLoadState", cls.IsAdvanced ? cls.UploadState : "");
            cons.AddItem("CollectStartTime", cls.CollectStartTime);
            cons.AddItem("CollectEndTime", cls.SearchTime);
            cons.AddItem("PageIndex", cls.PageIndex.ToString());
            cons.AddItem("PageCount", cls.PageCount.ToString());
            if (showWindow)
            {
                ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(null, MsgType.LogContentRequest, MsgType.CollecLogRespond, cons, Application.Current.FindResource("SearchManagerSearch").ToString(), AppConfigInfos.AppStateInfos.SearchTimeOut);
                MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
                if (msgBoxResult == MessageBoxResult.Cancel)
                {
                    NewMessageBox.Show(Application.Current.FindResource("SearchManagerSearchFail").ToString());
                }
            }
            else
                ModelResponsible.Instance.SendMessage(MsgType.LogContentRequest, cons);
        }
        /// <summary>
        /// 执法仪日志总数
        /// </summary>
        /// <param name="cls"></param>
        public void SearchCameraLogCount(CameraLogsSerach cls)
        {
            string UserGuid = string.Empty;
            string OrgID = string.Empty;
            if (AppConfigInfos.CurrentUserInfos.UserType == "1")
            {
                if (cls.IsAdvanced)
                {
                    UserGuid = cls.UserGuid;
                    OrgID = string.IsNullOrEmpty(cls.OrgID) ? AppConfigInfos.LimitsUserInfos.OrgIDCodeStr : cls.OrgID;
                }
                else
                {
                    OrgID = AppConfigInfos.LimitsUserInfos.OrgIDCodeStr;
                }
            }
            else
            {
                UserGuid = AppConfigInfos.CurrentUserInfos.UserGuid;
                OrgID = AppConfigInfos.CurrentUserInfos.OrgID;
            }


            if (DateTime.Compare(Convert.ToDateTime(cls.CollectEndTime), DateTime.Now) > 0)
                SearchManager.GetInstance().CameraLogsSerach.SearchTime = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            else
                SearchManager.GetInstance().CameraLogsSerach.SearchTime = cls.CollectEndTime;

            Conditions cons = new Conditions();
            cons.AddItem("UserGuid", UserGuid);
            cons.AddItem("OrgID", OrgID);
            cons.AddItem("DeviceID", cls.IsAdvanced ? cls.DeviceID : "");
            cons.AddItem("LogType", cls.LogType);
            cons.AddItem("UpLoadState", cls.IsAdvanced ? cls.UploadState : "");
            cons.AddItem("CollectStartTime", cls.CollectStartTime);
            cons.AddItem("CollectEndTime", cls.SearchTime);

            ModelResponsible.Instance.SendMessage(MsgType.LogCountRequest, cons);
        }
        /// <summary>
        /// 执法仪日志明细
        /// </summary>
        /// <param name="cls"></param>
        public void SearchCameraLogDetail(CameraLogsSerach cls, bool showWindow = true)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }
            string UserGuid = string.Empty;
            string OrgID = string.Empty;
            if (AppConfigInfos.CurrentUserInfos.UserType == "1")
            {
                if (cls.IsAdvanced)
                {
                    UserGuid = cls.UserGuid;
                    OrgID = string.IsNullOrEmpty(cls.OrgID) ? AppConfigInfos.LimitsUserInfos.OrgIDCodeStr : cls.OrgID;
                }
                else
                {
                    OrgID = AppConfigInfos.LimitsUserInfos.OrgIDCodeStr;
                }
            }
            else
            {
                UserGuid = AppConfigInfos.CurrentUserInfos.UserGuid;
                OrgID = AppConfigInfos.CurrentUserInfos.OrgID;
            }
            Conditions cons = new Conditions();
            cons.AddItem("UserGuid", UserGuid);
            cons.AddItem("OrgID", OrgID);
            cons.AddItem("DeviceID", cls.IsAdvanced ? cls.DeviceID : "");
            cons.AddItem("LogType", cls.LogType);
            cons.AddItem("UpLoadState", cls.IsAdvanced ? cls.UploadState : "");
            cons.AddItem("CollectStartTime", cls.CollectStartTime);
            cons.AddItem("CollectEndTime", cls.SearchTime);
            cons.AddItem("PageIndex", cls.PageIndex.ToString());
            cons.AddItem("PageCount", cls.PageCount.ToString());
            if (showWindow)
            {
                ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(null, MsgType.LogContentRequest, MsgType.CameraLogRespond, cons, Application.Current.FindResource("SearchManagerSearch").ToString(), AppConfigInfos.AppStateInfos.SearchTimeOut);
                MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
                if (msgBoxResult == MessageBoxResult.Cancel)
                {
                    NewMessageBox.Show(Application.Current.FindResource("SearchManagerSearchFail").ToString());
                }
            }
            else
                ModelResponsible.Instance.SendMessage(MsgType.LogContentRequest, cons);
        }
        /// <summary>
        /// 保存编辑后的电子证据信息
        /// </summary>
        /// <param name="cls"></param>
        public void SaveMediaInfo(MediaInfo cls)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }
            Conditions cons = new Conditions();
            cons.AddItem("MediaLogID", cls.RecordID);
            cons.AddItem("UserImp", cls.UpdateUserImp);
            cons.AddItem("UserID", cls.UserID);
            cons.AddItem("UserName", cls.UserName);
            cons.AddItem("OrgName", cls.OrgName);
            cons.AddItem("DateTime", cls.RecordTime);
            cons.AddItem("Mark", cls.UpdateMark);
            ResultWindow resultWindow = WindowsHelper.ShowDialogWindow<ResultWindow>(null, MsgType.MediaLogEditorRequest, MsgType.MediaLogEditorRespond, cons, Application.Current.FindResource("SearchManagerSave").ToString());
            MessageBoxResult msgBoxResult = resultWindow.MessageBoxResult;
            if (msgBoxResult == MessageBoxResult.Cancel)
            {
                NewMessageBox.Show(Application.Current.FindResource("SearchManagerSaveTimeOut").ToString());
            }
        }

        /// <summary>
        /// 上传电子证据信息
        /// </summary>
        /// <param name="mediaLogIDs"></param>
        public void UploadMediaInfo(string mediaLogIDs)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }
            Conditions cons = new Conditions();
            cons.AddItem("MediaLogIDs", mediaLogIDs);
            ModelResponsible.Instance.SendMessage(MsgType.MediaLogUploadRequest, cons);
        }

        /// <summary>
        /// 执法仪日志上传
        /// </summary>
        /// <param name="logIDs"></param>
        public void UploadCameraLog(string logIDs)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }
            Conditions cons = new Conditions();
            cons.AddItem("LogIDs", logIDs);
            ModelResponsible.Instance.SendMessage(MsgType.CameraLogUploadRequest, cons);
        }

        /// <summary>
        /// 采集站日志上传
        /// </summary>
        /// <param name="logIDs"></param>
        public void UploadCollectLog(string logIDs)
        {
            if (!AppHelper.CheckAppState(ModelResponsible.Instance.ParentWindow))
            {
                return;
            }
            Conditions cons = new Conditions();
            cons.AddItem("LogIDs", logIDs);
            ModelResponsible.Instance.SendMessage(MsgType.CollectLogUploadRequest, cons);
        }

        /// <summary>
        /// 根据用户信息是否清楚缓存数据
        /// </summary>
        public void UpdateSearchData()
        {
            if (!AppConfigInfos.LimitsUserInfos.UserCode.Equals(CurrentUserInfo.UserCode))
            {
                CurrentUserInfo = AppConfigInfos.LimitsUserInfos;
                ModelResponsible.Instance.ClearPicturePlayMediaList();
                ModelResponsible.Instance.ClearVideoPlayMediaList();
                ModelResponsible.Instance.ClearVoicePlayMediaList();
                ModelResponsible.Instance.ClearHisPlayMediaList();
                ModelResponsible.Instance.ClearAlarmLogs();
                ModelResponsible.Instance.ClearCameraLogs();
                ModelResponsible.Instance.ClearCollectLogs();
                ModelResponsible.Instance.ClearMediaList();
                LogHelper.Instance.WirteLog(string.Format("SearchManager: UserCode:{0} UpdateSearchData  ClearAll", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
                
                SearchManager.GetInstance().MediaLogsSerach.PageIndex = 1;
                SearchManager.GetInstance().MediaLogsSerach.IsAdvanced = false;
                SearchManager.GetInstance().MediaLogsSerach.SearchTime = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                SearchManager.GetInstance().SearchMediaLogCount(SearchManager.GetInstance().MediaLogsSerach);
                SearchManager.GetInstance().SearchMediaLogDetail(SearchManager.GetInstance().MediaLogsSerach,false);
                LogHelper.Instance.WirteLog(string.Format("SearchManager: UserCode:{0} UpdateSearchData  MediaLogsSerach", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);

                SearchManager.GetInstance().CollectLogsSerach.PageIndex = 1;
                SearchManager.GetInstance().CollectLogsSerach.LogType = "1";
                SearchManager.GetInstance().CollectLogsSerach.IsAdvanced = false;
                SearchManager.GetInstance().SearchCollectLogCount(SearchManager.GetInstance().CollectLogsSerach);
                SearchManager.GetInstance().SearchCollectLogDetail(SearchManager.GetInstance().CollectLogsSerach,false);
                LogHelper.Instance.WirteLog(string.Format("SearchManager: UserCode:{0} UpdateSearchData  CollectLogsSerach", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);

                SearchManager.GetInstance().CameraLogsSerach.PageIndex = 1;
                SearchManager.GetInstance().CameraLogsSerach.LogType = "2";
                SearchManager.GetInstance().CameraLogsSerach.IsAdvanced = false;
                SearchManager.GetInstance().SearchCameraLogCount(SearchManager.GetInstance().CameraLogsSerach);
                SearchManager.GetInstance().SearchCameraLogDetail(SearchManager.GetInstance().CameraLogsSerach,false);
                LogHelper.Instance.WirteLog(string.Format("SearchManager: UserCode:{0} UpdateSearchData  CameraLogsSerach", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);

                SearchManager.GetInstance().AlarmLogsSearch.PageIndex = 1;
                SearchManager.GetInstance().AlarmLogsSearch.IsAdvanced = false;
                SearchManager.GetInstance().SearchAlarmLogCount(SearchManager.GetInstance().AlarmLogsSearch);
                SearchManager.GetInstance().SearchAlarmLogDetail(SearchManager.GetInstance().AlarmLogsSearch,false);
                LogHelper.Instance.WirteLog(string.Format("SearchManager: UserCode:{0} UpdateSearchData  AlarmLogsSearch", AppConfigInfos.CurrentUserInfos.UserCode), LogLevel.LogDebug);
                
            }
        }

        public void SendOperationLog(string operateCode, MediaInfo mi = null)
        {
            Conditions cons = null;
            if (mi!=null)
            {
                cons = new Conditions();
                cons.AddItem("OperatedID", mi.RecordID);
                cons.AddItem("OperatedName", mi.FileName);
                cons.AddItem("OperatedCode", mi.DeviceID);
                cons.AddItem("OperatedOrgID", mi.OrgID);
                cons.AddItem("OperatedOrgName", mi.OrgName);
            }
            ModelResponsible.Instance.SendOperationLog(operateCode, cons);
        }

        /// <summary>
        /// 防盗链密码
        /// </summary>
        string password = "lianghuilonglong";

        /// <summary>
        /// 防盗链秘钥
        /// </summary>
        string iv = "1234567812345678";

        public string DecipherFile(MediaInfo mi)
        {
            string filepath = mi.FilePath + mi.FileName;

            if (mi.EncryType == "1")
            {
                Dictionary<string, string> conditions = new Dictionary<string, string>();
                conditions.Add("file_path", mi.FilePath);
                conditions.Add("file_name", mi.FileName);
                conditions.Add("timer", DateTime.Now.Ticks.ToString());
                conditions.Add("media_type", "1");
                conditions.Add("upload_process", "1");

                filepath = "http://127.0.0.1:8080/eems-streamserver/mediaPlay/" + GetMediaUrl(conditions, password, iv);
            }

            LogHelper.Instance.WirteLog(string.Format("SearchManager: UserCode:{0} DecipherFile EncryType:{2}  filepath:{1}", AppConfigInfos.CurrentUserInfos.UserCode, filepath,mi.EncryType), LogLevel.LogDebug);

            return filepath;
        }

        /// <summary>
        /// 生成VLC媒体播放地址
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public string GetMediaUrl(Dictionary<string, string> conditions, string password, string iv)
        {
            conditions = (from item in conditions orderby item.Key select item).ToDictionary(p => p.Key, v => v.Value);

            string paras = string.Empty;

            foreach (string key in conditions.Keys)
            {
                paras += "&" + key + "=" + conditions[key];
            }

            if (!string.IsNullOrEmpty(paras))
            {
                paras = paras.Substring(1);
            }

            string md5Sign = MD5Helper.MD5Encrypt(paras).ToUpper();

            conditions.Add("sign", md5Sign);

            string json = JsonUnityConvert.SerializeObject(conditions);

            return AESHelper.CBCEncrypt(json, password, iv).Replace("+", "-").Replace("/", "_");
        }
    }
}
