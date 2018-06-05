using Hytera.EEMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Dispatcher
{
    public delegate void DelegateAppSelfMessageNotic(object sender, SelfMessageEventArgs e);

    internal delegate void InitEventHandler(DataResponsible DataResponsible);

    internal delegate void MessageNoticeEventHandler(DataResponsible DataResponsible, MessageEventArgs e);

    internal delegate void UnknownMessageNoticeEventHandler(DataResponsible DataResponsible, UnknownMessageEventArgs e);

    public delegate void ShowViewNoticeHandler(object sender);
}
