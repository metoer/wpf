using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Model
{
   public class DeviceRegisterInfo
    {
       /// <summary>
       /// 注册结果
       /// </summary>
       public int ResultCode
       {
           get;
           set;
       }

       /// <summary>
       /// 执法记录仪ID
       /// </summary>
       public string DeviceID
       {
           get;
           set;
       }

       /// <summary>
       /// 执法记录仪Code
       /// </summary>
       public string DeviceCode
       {
           get;
           set;
       }

       /// <summary>
       /// 注册用户
       /// </summary>
       public string UserID
       {
           get;
           set;
       }
    }
}
