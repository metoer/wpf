using Hytera.EEMS.Common;
using Hytera.EEMS.Model;
using Hytera.EEMS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Fingerprint.Lib
{
    public class FingerViewModel
    {
        static FingerViewModel()
        {
            PoliceInfos = new ThreadSafeObservable<UserInfos>();
            //PoliceInfos.Add(new UserInfos() { UserID = "1", UserName = "张三", UserCode = "M1456432", OrgName = "南山派出所", UserType = "1", FingerNumber = 1, Fingers = new List<Finger>() { new Finger() { FingerID = "1", FingerName = "指纹1" } } });
            //PoliceInfos.Add(new UserInfos() { UserID = "2", UserName = "李四", UserCode = "M143432", OrgName = "南山派出所", UserType = "2", FingerNumber = 0 });
            //PoliceInfos.Add(new UserInfos() { UserID = "3", UserName = "王五", UserCode = "M53456432", OrgName = "南山派出所", UserType = "1", FingerNumber = 2, Fingers = new List<Finger>() { new Finger() { FingerID = "3", FingerName = "指纹3" }, new Finger() { FingerID = "5", FingerName = "指纹5" } } });
            //PoliceInfos.Add(new UserInfos() { UserID = "4", UserName = "黄六", UserCode = "M142432", OrgName = "南山派出所", UserType = "2", FingerNumber = 2, Fingers = new List<Finger>() { new Finger() { FingerID = "4", FingerName = "指纹4" }, new Finger() { FingerID = "6", FingerName = "指纹6" } } });
        }

        /// <summary>
        /// 警员信息列表
        /// </summary>
        public static ThreadSafeObservable<UserInfos> PoliceInfos
        {
            get;
            set;
        }

        /// <summary>
        /// 页数据
        /// </summary>
        public static int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有更多数据
        /// </summary>
        public static bool IsMoreData
        {
            get;
            set;
        }

        /// <summary>
        /// 搜索警员信息
        /// </summary>
        /// <param name="UserInfoList"></param>
        public static void SearchPoliceInfos(ThreadSafeList<UserInfos> UserInfoList)
        {
            PoliceInfos.Clear();
            foreach (var item in UserInfoList)
            {
                item.FingerNumber = item.Fingers.Count;
                PoliceInfos.AddItem(item);
            }
        }

        /// <summary>
        /// 添加警员信息到末尾
        /// </summary>
        /// <param name="UserInfoList"></param>
        public static void AddPoliceInfos(ThreadSafeList<UserInfos> UserInfoList)
        {
            foreach (var item in UserInfoList)
            {
                item.FingerNumber = item.Fingers.Count;
                PoliceInfos.AddItem(item);
            }
        }

        /// <summary>
        /// 编辑结果修改
        /// </summary>
        /// <param name="fingerResult"></param>
        public static void EditorFingerPrintByUserID(FingerResult fingerResult)
        {
            UserInfos userInfo = PoliceInfos.Find(p => p.UserID.Equals(fingerResult.UserID));
            if (userInfo == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(fingerResult.FingersEditor))
            {
                string[] fingers = fingerResult.FingersEditor.Split(',');
                foreach (var item in fingers)
                {
                    string[] fingerInfo = item.Split(':');
                    if (fingerInfo.Length != 2)
                    {
                        continue;
                    }

                    Finger finger = userInfo.Fingers.Find(p => p.FingerID.Equals(fingerInfo[0]));
                    if (finger != null)
                    {
                        finger.FingerName = fingerInfo[1];
                    }
                }
            }

            if (!string.IsNullOrEmpty(fingerResult.FingersDelete))
            {
                string[] fingerIds = fingerResult.FingersDelete.Split(',');
                foreach (var item in fingerIds)
                {
                    Finger finger = userInfo.Fingers.Find(p => p.FingerID.Equals(item));
                    if (finger != null)
                    {
                        userInfo.Fingers.Remove(finger);
                    }
                }

                userInfo.FingerNumber = userInfo.Fingers.Count;
            }
        }

        /// <summary>
        /// 指纹添加
        /// </summary>
        /// <param name="fingerResult"></param>
        public static void AddFingerPrintByUser(FingerResult fingerResult)
        {
            UserInfos userInfo = PoliceInfos.Find(p => p.UserID.Equals(fingerResult.UserID));
            if (userInfo == null)
            {
                return;
            }

            userInfo.Fingers.Add(new Finger() { FingerID = fingerResult.FingerID, FingerName = fingerResult.FingerName });
            userInfo.FingerNumber = userInfo.Fingers.Count;
        }
    }
}
