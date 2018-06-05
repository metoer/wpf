// **********************************************************************
//
// Copyright (c) 2003-2015 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************
//
// Ice version 3.6.1
//
// <auto-generated>
//
// Generated from file `Log_Plugin_RPC.ice'
//
// Warning: do not edit this file.
//
// </auto-generated>
//


using _System = global::System;
using _Microsoft = global::Microsoft;

#pragma warning disable 1591

namespace IceCompactId
{
}

namespace IRPC
{
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724")]
#if !SILVERLIGHT
    [_System.Serializable]
#endif
    public partial class LogInitInfo : _System.ICloneable
    {
        #region Slice data members

        private string strServerID__prop;
        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public virtual string strServerID
        {
            get
            {
                return strServerID__prop;
            }
            set
            {
                strServerID__prop = value;
            }
        }

        private string strIp__prop;
        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public virtual string strIp
        {
            get
            {
                return strIp__prop;
            }
            set
            {
                strIp__prop = value;
            }
        }

        private string strPluginName__prop;
        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public virtual string strPluginName
        {
            get
            {
                return strPluginName__prop;
            }
            set
            {
                strPluginName__prop = value;
            }
        }

        #endregion

        #region Constructors

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public LogInitInfo()
        {
            strServerID = "";
            strIp = "";
            strPluginName = "";
        }

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public LogInitInfo(string strServerID, string strIp, string strPluginName)
        {
            this.strServerID__prop = strServerID;
            this.strIp__prop = strIp;
            this.strPluginName__prop = strPluginName;
        }

        #endregion

        #region ICloneable members

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region Object members

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public override int GetHashCode()
        {
            int h__ = 5381;
            IceInternal.HashUtil.hashAdd(ref h__, "::IRPC::LogInitInfo");
            IceInternal.HashUtil.hashAdd(ref h__, strServerID);
            IceInternal.HashUtil.hashAdd(ref h__, strIp);
            IceInternal.HashUtil.hashAdd(ref h__, strPluginName);
            return h__;
        }

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public override bool Equals(object other__)
        {
            if(object.ReferenceEquals(this, other__))
            {
                return true;
            }
            if(other__ == null)
            {
                return false;
            }
            if(GetType() != other__.GetType())
            {
                return false;
            }
            LogInitInfo o__ = (LogInitInfo)other__;
            if(strServerID == null)
            {
                if(o__.strServerID != null)
                {
                    return false;
                }
            }
            else
            {
                if(!strServerID.Equals(o__.strServerID))
                {
                    return false;
                }
            }
            if(strIp == null)
            {
                if(o__.strIp != null)
                {
                    return false;
                }
            }
            else
            {
                if(!strIp.Equals(o__.strIp))
                {
                    return false;
                }
            }
            if(strPluginName == null)
            {
                if(o__.strPluginName != null)
                {
                    return false;
                }
            }
            else
            {
                if(!strPluginName.Equals(o__.strPluginName))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Comparison members

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public static bool operator==(LogInitInfo lhs__, LogInitInfo rhs__)
        {
            return Equals(lhs__, rhs__);
        }

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public static bool operator!=(LogInitInfo lhs__, LogInitInfo rhs__)
        {
            return !Equals(lhs__, rhs__);
        }

        #endregion

        #region Marshalling support

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public void write__(IceInternal.BasicStream os__)
        {
            os__.writeString(strServerID__prop);
            os__.writeString(strIp__prop);
            os__.writeString(strPluginName__prop);
        }

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public void read__(IceInternal.BasicStream is__)
        {
            strServerID__prop = is__.readString();
            strIp__prop = is__.readString();
            strPluginName__prop = is__.readString();
        }
        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public static void write__(IceInternal.BasicStream os__, LogInitInfo v__)
        {
            if(v__ == null)
            {
                nullMarshalValue__.write__(os__);
            }
            else
            {
                v__.write__(os__);
            }
        }

        [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
        public static LogInitInfo read__(IceInternal.BasicStream is__, LogInitInfo v__)
        {
            if(v__ == null)
            {
                v__ = new LogInitInfo();
            }
            v__.read__(is__);
            return v__;
        }
        
        private static readonly LogInitInfo nullMarshalValue__ = new LogInitInfo();

        #endregion
    }

    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public abstract class strLogPluginRPCId
    {
        public const string value = "ID_LogPluginRPC";
    }

    [_System.Runtime.InteropServices.ComVisible(false)]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722")]
    [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724")]
    public partial interface LogServer_RPC : Ice.Object, LogServer_RPCOperations_, LogServer_RPCOperationsNC_
    {
    }
}

namespace IRPC
{
    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public delegate void Callback_LogServer_RPC_WriteLogInit(bool ret__);

    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public delegate void Callback_LogServer_RPC_SetWriteLogFlag();

    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public delegate void Callback_LogServer_RPC_WriteLog();
}

namespace IRPC
{
    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public interface LogServer_RPCPrx : Ice.ObjectPrx
    {
        bool WriteLogInit(IRPC.LogInitInfo InitInfo);
        bool WriteLogInit(IRPC.LogInitInfo InitInfo, _System.Collections.Generic.Dictionary<string, string> context__);

        Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLogInit> begin_WriteLogInit(IRPC.LogInitInfo InitInfo);
        Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLogInit> begin_WriteLogInit(IRPC.LogInitInfo InitInfo, _System.Collections.Generic.Dictionary<string, string> ctx__);

        Ice.AsyncResult begin_WriteLogInit(IRPC.LogInitInfo InitInfo, Ice.AsyncCallback cb__, object cookie__);
        Ice.AsyncResult begin_WriteLogInit(IRPC.LogInitInfo InitInfo, _System.Collections.Generic.Dictionary<string, string> ctx__, Ice.AsyncCallback cb__, object cookie__);

        bool end_WriteLogInit(Ice.AsyncResult r__);

        void SetWriteLogFlag(bool bWrite);
        void SetWriteLogFlag(bool bWrite, _System.Collections.Generic.Dictionary<string, string> context__);

        Ice.AsyncResult<IRPC.Callback_LogServer_RPC_SetWriteLogFlag> begin_SetWriteLogFlag(bool bWrite);
        Ice.AsyncResult<IRPC.Callback_LogServer_RPC_SetWriteLogFlag> begin_SetWriteLogFlag(bool bWrite, _System.Collections.Generic.Dictionary<string, string> ctx__);

        Ice.AsyncResult begin_SetWriteLogFlag(bool bWrite, Ice.AsyncCallback cb__, object cookie__);
        Ice.AsyncResult begin_SetWriteLogFlag(bool bWrite, _System.Collections.Generic.Dictionary<string, string> ctx__, Ice.AsyncCallback cb__, object cookie__);

        void end_SetWriteLogFlag(Ice.AsyncResult r__);

        void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel);
        void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, _System.Collections.Generic.Dictionary<string, string> context__);

        Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLog> begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel);
        Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLog> begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, _System.Collections.Generic.Dictionary<string, string> ctx__);

        Ice.AsyncResult begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, Ice.AsyncCallback cb__, object cookie__);
        Ice.AsyncResult begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, _System.Collections.Generic.Dictionary<string, string> ctx__, Ice.AsyncCallback cb__, object cookie__);

        void end_WriteLog(Ice.AsyncResult r__);
    }
}

namespace IRPC
{
    /// <summary>
    /// Interface Definition
    /// </summary>
    
    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public interface LogServer_RPCOperations_
    {
        bool WriteLogInit(IRPC.LogInitInfo InitInfo, Ice.Current current__);

        void SetWriteLogFlag(bool bWrite, Ice.Current current__);

        void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, Ice.Current current__);
    }

    /// <summary>
    /// Interface Definition
    /// </summary>
    
    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public interface LogServer_RPCOperationsNC_
    {
        bool WriteLogInit(IRPC.LogInitInfo InitInfo);

        void SetWriteLogFlag(bool bWrite);

        void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel);
    }
}

namespace IRPC
{
    /// <summary>
    /// Interface Definition
    /// </summary>
    
    [_System.Runtime.InteropServices.ComVisible(false)]
    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public sealed class LogServer_RPCPrxHelper : Ice.ObjectPrxHelperBase, LogServer_RPCPrx
    {
        #region Synchronous operations

        public void SetWriteLogFlag(bool bWrite)
        {
            this.SetWriteLogFlag(bWrite, null, false);
        }

        public void SetWriteLogFlag(bool bWrite, _System.Collections.Generic.Dictionary<string, string> context__)
        {
            this.SetWriteLogFlag(bWrite, context__, true);
        }

        private void SetWriteLogFlag(bool bWrite, _System.Collections.Generic.Dictionary<string, string> context__, bool explicitCtx__)
        {
            end_SetWriteLogFlag(begin_SetWriteLogFlag(bWrite, context__, explicitCtx__, true, null, null));
        }

        public void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel)
        {
            this.WriteLog(strServerID, strPluginName, strLog, LogLevel, null, false);
        }

        public void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, _System.Collections.Generic.Dictionary<string, string> context__)
        {
            this.WriteLog(strServerID, strPluginName, strLog, LogLevel, context__, true);
        }

        private void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, _System.Collections.Generic.Dictionary<string, string> context__, bool explicitCtx__)
        {
            end_WriteLog(begin_WriteLog(strServerID, strPluginName, strLog, LogLevel, context__, explicitCtx__, true, null, null));
        }

        public bool WriteLogInit(IRPC.LogInitInfo InitInfo)
        {
            return this.WriteLogInit(InitInfo, null, false);
        }

        public bool WriteLogInit(IRPC.LogInitInfo InitInfo, _System.Collections.Generic.Dictionary<string, string> context__)
        {
            return this.WriteLogInit(InitInfo, context__, true);
        }

        private bool WriteLogInit(IRPC.LogInitInfo InitInfo, _System.Collections.Generic.Dictionary<string, string> context__, bool explicitCtx__)
        {
            checkTwowayOnly__(__WriteLogInit_name);
            return end_WriteLogInit(begin_WriteLogInit(InitInfo, context__, explicitCtx__, true, null, null));
        }

        #endregion

        #region Asynchronous operations

        public Ice.AsyncResult<IRPC.Callback_LogServer_RPC_SetWriteLogFlag> begin_SetWriteLogFlag(bool bWrite)
        {
            return begin_SetWriteLogFlag(bWrite, null, false, false, null, null);
        }

        public Ice.AsyncResult<IRPC.Callback_LogServer_RPC_SetWriteLogFlag> begin_SetWriteLogFlag(bool bWrite, _System.Collections.Generic.Dictionary<string, string> ctx__)
        {
            return begin_SetWriteLogFlag(bWrite, ctx__, true, false, null, null);
        }

        public Ice.AsyncResult begin_SetWriteLogFlag(bool bWrite, Ice.AsyncCallback cb__, object cookie__)
        {
            return begin_SetWriteLogFlag(bWrite, null, false, false, cb__, cookie__);
        }

        public Ice.AsyncResult begin_SetWriteLogFlag(bool bWrite, _System.Collections.Generic.Dictionary<string, string> ctx__, Ice.AsyncCallback cb__, object cookie__)
        {
            return begin_SetWriteLogFlag(bWrite, ctx__, true, false, cb__, cookie__);
        }

        private const string __SetWriteLogFlag_name = "SetWriteLogFlag";

        public void end_SetWriteLogFlag(Ice.AsyncResult r__)
        {
            end__(r__, __SetWriteLogFlag_name);
        }

        private Ice.AsyncResult<IRPC.Callback_LogServer_RPC_SetWriteLogFlag> begin_SetWriteLogFlag(bool bWrite, _System.Collections.Generic.Dictionary<string, string> ctx__, bool explicitContext__, bool synchronous__, Ice.AsyncCallback cb__, object cookie__)
        {
            IceInternal.OnewayOutgoingAsync<IRPC.Callback_LogServer_RPC_SetWriteLogFlag> result__ = getOnewayOutgoingAsync<IRPC.Callback_LogServer_RPC_SetWriteLogFlag>(__SetWriteLogFlag_name, SetWriteLogFlag_completed__, cookie__);
            if(cb__ != null)
            {
                result__.whenCompletedWithAsyncCallback(cb__);
            }
            try
            {
                result__.prepare(__SetWriteLogFlag_name, Ice.OperationMode.Normal, ctx__, explicitContext__, synchronous__);
                IceInternal.BasicStream os__ = result__.startWriteParams(Ice.FormatType.DefaultFormat);
                os__.writeBool(bWrite);
                result__.endWriteParams();
                result__.invoke();
            }
            catch(Ice.Exception ex__)
            {
                result__.abort(ex__);
            }
            return result__;
        }

        private void SetWriteLogFlag_completed__(IRPC.Callback_LogServer_RPC_SetWriteLogFlag cb__)
        {
            if(cb__ != null)
            {
                cb__();
            }
        }

        public Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLog> begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel)
        {
            return begin_WriteLog(strServerID, strPluginName, strLog, LogLevel, null, false, false, null, null);
        }

        public Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLog> begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, _System.Collections.Generic.Dictionary<string, string> ctx__)
        {
            return begin_WriteLog(strServerID, strPluginName, strLog, LogLevel, ctx__, true, false, null, null);
        }

        public Ice.AsyncResult begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, Ice.AsyncCallback cb__, object cookie__)
        {
            return begin_WriteLog(strServerID, strPluginName, strLog, LogLevel, null, false, false, cb__, cookie__);
        }

        public Ice.AsyncResult begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, _System.Collections.Generic.Dictionary<string, string> ctx__, Ice.AsyncCallback cb__, object cookie__)
        {
            return begin_WriteLog(strServerID, strPluginName, strLog, LogLevel, ctx__, true, false, cb__, cookie__);
        }

        private const string __WriteLog_name = "WriteLog";

        public void end_WriteLog(Ice.AsyncResult r__)
        {
            end__(r__, __WriteLog_name);
        }

        private Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLog> begin_WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, _System.Collections.Generic.Dictionary<string, string> ctx__, bool explicitContext__, bool synchronous__, Ice.AsyncCallback cb__, object cookie__)
        {
            IceInternal.OnewayOutgoingAsync<IRPC.Callback_LogServer_RPC_WriteLog> result__ = getOnewayOutgoingAsync<IRPC.Callback_LogServer_RPC_WriteLog>(__WriteLog_name, WriteLog_completed__, cookie__);
            if(cb__ != null)
            {
                result__.whenCompletedWithAsyncCallback(cb__);
            }
            try
            {
                result__.prepare(__WriteLog_name, Ice.OperationMode.Normal, ctx__, explicitContext__, synchronous__);
                IceInternal.BasicStream os__ = result__.startWriteParams(Ice.FormatType.DefaultFormat);
                os__.writeString(strServerID);
                os__.writeString(strPluginName);
                os__.writeString(strLog);
                os__.writeInt(LogLevel);
                result__.endWriteParams();
                result__.invoke();
            }
            catch(Ice.Exception ex__)
            {
                result__.abort(ex__);
            }
            return result__;
        }

        private void WriteLog_completed__(IRPC.Callback_LogServer_RPC_WriteLog cb__)
        {
            if(cb__ != null)
            {
                cb__();
            }
        }

        public Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLogInit> begin_WriteLogInit(IRPC.LogInitInfo InitInfo)
        {
            return begin_WriteLogInit(InitInfo, null, false, false, null, null);
        }

        public Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLogInit> begin_WriteLogInit(IRPC.LogInitInfo InitInfo, _System.Collections.Generic.Dictionary<string, string> ctx__)
        {
            return begin_WriteLogInit(InitInfo, ctx__, true, false, null, null);
        }

        public Ice.AsyncResult begin_WriteLogInit(IRPC.LogInitInfo InitInfo, Ice.AsyncCallback cb__, object cookie__)
        {
            return begin_WriteLogInit(InitInfo, null, false, false, cb__, cookie__);
        }

        public Ice.AsyncResult begin_WriteLogInit(IRPC.LogInitInfo InitInfo, _System.Collections.Generic.Dictionary<string, string> ctx__, Ice.AsyncCallback cb__, object cookie__)
        {
            return begin_WriteLogInit(InitInfo, ctx__, true, false, cb__, cookie__);
        }

        private const string __WriteLogInit_name = "WriteLogInit";

        public bool end_WriteLogInit(Ice.AsyncResult r__)
        {
            IceInternal.OutgoingAsync outAsync__ = IceInternal.OutgoingAsync.check(r__, this, __WriteLogInit_name);
            try
            {
                if(!outAsync__.wait())
                {
                    try
                    {
                        outAsync__.throwUserException();
                    }
                    catch(Ice.UserException ex__)
                    {
                        throw new Ice.UnknownUserException(ex__.ice_name(), ex__);
                    }
                }
                bool ret__;
                IceInternal.BasicStream is__ = outAsync__.startReadParams();
                ret__ = is__.readBool();
                outAsync__.endReadParams();
                return ret__;
            }
            finally
            {
                outAsync__.cacheMessageBuffers();
            }
        }

        private Ice.AsyncResult<IRPC.Callback_LogServer_RPC_WriteLogInit> begin_WriteLogInit(IRPC.LogInitInfo InitInfo, _System.Collections.Generic.Dictionary<string, string> ctx__, bool explicitContext__, bool synchronous__, Ice.AsyncCallback cb__, object cookie__)
        {
            checkAsyncTwowayOnly__(__WriteLogInit_name);
            IceInternal.TwowayOutgoingAsync<IRPC.Callback_LogServer_RPC_WriteLogInit> result__ =  getTwowayOutgoingAsync<IRPC.Callback_LogServer_RPC_WriteLogInit>(__WriteLogInit_name, WriteLogInit_completed__, cookie__);
            if(cb__ != null)
            {
                result__.whenCompletedWithAsyncCallback(cb__);
            }
            try
            {
                result__.prepare(__WriteLogInit_name, Ice.OperationMode.Normal, ctx__, explicitContext__, synchronous__);
                IceInternal.BasicStream os__ = result__.startWriteParams(Ice.FormatType.DefaultFormat);
                IRPC.LogInitInfo.write__(os__, InitInfo);
                result__.endWriteParams();
                result__.invoke();
            }
            catch(Ice.Exception ex__)
            {
                result__.abort(ex__);
            }
            return result__;
        }

        private void WriteLogInit_completed__(Ice.AsyncResult r__, IRPC.Callback_LogServer_RPC_WriteLogInit cb__, Ice.ExceptionCallback excb__)
        {
            bool ret__;
            try
            {
                ret__ = end_WriteLogInit(r__);
            }
            catch(Ice.Exception ex__)
            {
                if(excb__ != null)
                {
                    excb__(ex__);
                }
                return;
            }
            if(cb__ != null)
            {
                cb__(ret__);
            }
        }

        #endregion

        #region Checked and unchecked cast operations

        public static LogServer_RPCPrx checkedCast(Ice.ObjectPrx b)
        {
            if(b == null)
            {
                return null;
            }
            LogServer_RPCPrx r = b as LogServer_RPCPrx;
            if((r == null) && b.ice_isA(ice_staticId()))
            {
                LogServer_RPCPrxHelper h = new LogServer_RPCPrxHelper();
                h.copyFrom__(b);
                r = h;
            }
            return r;
        }

        public static LogServer_RPCPrx checkedCast(Ice.ObjectPrx b, _System.Collections.Generic.Dictionary<string, string> ctx)
        {
            if(b == null)
            {
                return null;
            }
            LogServer_RPCPrx r = b as LogServer_RPCPrx;
            if((r == null) && b.ice_isA(ice_staticId(), ctx))
            {
                LogServer_RPCPrxHelper h = new LogServer_RPCPrxHelper();
                h.copyFrom__(b);
                r = h;
            }
            return r;
        }

        public static LogServer_RPCPrx checkedCast(Ice.ObjectPrx b, string f)
        {
            if(b == null)
            {
                return null;
            }
            Ice.ObjectPrx bb = b.ice_facet(f);
            try
            {
                if(bb.ice_isA(ice_staticId()))
                {
                    LogServer_RPCPrxHelper h = new LogServer_RPCPrxHelper();
                    h.copyFrom__(bb);
                    return h;
                }
            }
            catch(Ice.FacetNotExistException)
            {
            }
            return null;
        }

        public static LogServer_RPCPrx checkedCast(Ice.ObjectPrx b, string f, _System.Collections.Generic.Dictionary<string, string> ctx)
        {
            if(b == null)
            {
                return null;
            }
            Ice.ObjectPrx bb = b.ice_facet(f);
            try
            {
                if(bb.ice_isA(ice_staticId(), ctx))
                {
                    LogServer_RPCPrxHelper h = new LogServer_RPCPrxHelper();
                    h.copyFrom__(bb);
                    return h;
                }
            }
            catch(Ice.FacetNotExistException)
            {
            }
            return null;
        }

        public static LogServer_RPCPrx uncheckedCast(Ice.ObjectPrx b)
        {
            if(b == null)
            {
                return null;
            }
            LogServer_RPCPrx r = b as LogServer_RPCPrx;
            if(r == null)
            {
                LogServer_RPCPrxHelper h = new LogServer_RPCPrxHelper();
                h.copyFrom__(b);
                r = h;
            }
            return r;
        }

        public static LogServer_RPCPrx uncheckedCast(Ice.ObjectPrx b, string f)
        {
            if(b == null)
            {
                return null;
            }
            Ice.ObjectPrx bb = b.ice_facet(f);
            LogServer_RPCPrxHelper h = new LogServer_RPCPrxHelper();
            h.copyFrom__(bb);
            return h;
        }

        public static readonly string[] ids__ =
        {
            "::IRPC::LogServer_RPC",
            "::Ice::Object"
        };

        public static string ice_staticId()
        {
            return ids__[0];
        }

        #endregion

        #region Marshaling support

        public static void write__(IceInternal.BasicStream os__, LogServer_RPCPrx v__)
        {
            os__.writeProxy(v__);
        }

        public static LogServer_RPCPrx read__(IceInternal.BasicStream is__)
        {
            Ice.ObjectPrx proxy = is__.readProxy();
            if(proxy != null)
            {
                LogServer_RPCPrxHelper result = new LogServer_RPCPrxHelper();
                result.copyFrom__(proxy);
                return result;
            }
            return null;
        }

        #endregion
    }
}

namespace IRPC
{
    [_System.Runtime.InteropServices.ComVisible(false)]
    [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.1")]
    public abstract class LogServer_RPCDisp_ : Ice.ObjectImpl, LogServer_RPC
    {
        #region Slice operations

        public bool WriteLogInit(IRPC.LogInitInfo InitInfo)
        {
            return WriteLogInit(InitInfo, Ice.ObjectImpl.defaultCurrent);
        }

        public abstract bool WriteLogInit(IRPC.LogInitInfo InitInfo, Ice.Current current__);

        public void SetWriteLogFlag(bool bWrite)
        {
            SetWriteLogFlag(bWrite, Ice.ObjectImpl.defaultCurrent);
        }

        public abstract void SetWriteLogFlag(bool bWrite, Ice.Current current__);

        public void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel)
        {
            WriteLog(strServerID, strPluginName, strLog, LogLevel, Ice.ObjectImpl.defaultCurrent);
        }

        public abstract void WriteLog(string strServerID, string strPluginName, string strLog, int LogLevel, Ice.Current current__);

        #endregion

        #region Slice type-related members

        public static new readonly string[] ids__ = 
        {
            "::IRPC::LogServer_RPC",
            "::Ice::Object"
        };

        public override bool ice_isA(string s)
        {
            return _System.Array.BinarySearch(ids__, s, IceUtilInternal.StringUtil.OrdinalStringComparer) >= 0;
        }

        public override bool ice_isA(string s, Ice.Current current__)
        {
            return _System.Array.BinarySearch(ids__, s, IceUtilInternal.StringUtil.OrdinalStringComparer) >= 0;
        }

        public override string[] ice_ids()
        {
            return ids__;
        }

        public override string[] ice_ids(Ice.Current current__)
        {
            return ids__;
        }

        public override string ice_id()
        {
            return ids__[0];
        }

        public override string ice_id(Ice.Current current__)
        {
            return ids__[0];
        }

        public static new string ice_staticId()
        {
            return ids__[0];
        }

        #endregion

        #region Operation dispatch

        [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
        public static Ice.DispatchStatus WriteLogInit___(LogServer_RPC obj__, IceInternal.Incoming inS__, Ice.Current current__)
        {
            Ice.ObjectImpl.checkMode__(Ice.OperationMode.Normal, current__.mode);
            IceInternal.BasicStream is__ = inS__.startReadParams();
            IRPC.LogInitInfo InitInfo;
            InitInfo = null;
            InitInfo = IRPC.LogInitInfo.read__(is__, InitInfo);
            inS__.endReadParams();
            bool ret__ = obj__.WriteLogInit(InitInfo, current__);
            IceInternal.BasicStream os__ = inS__.startWriteParams__(Ice.FormatType.DefaultFormat);
            os__.writeBool(ret__);
            inS__.endWriteParams__(true);
            return Ice.DispatchStatus.DispatchOK;
        }

        [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
        public static Ice.DispatchStatus SetWriteLogFlag___(LogServer_RPC obj__, IceInternal.Incoming inS__, Ice.Current current__)
        {
            Ice.ObjectImpl.checkMode__(Ice.OperationMode.Normal, current__.mode);
            IceInternal.BasicStream is__ = inS__.startReadParams();
            bool bWrite;
            bWrite = is__.readBool();
            inS__.endReadParams();
            obj__.SetWriteLogFlag(bWrite, current__);
            inS__.writeEmptyParams__();
            return Ice.DispatchStatus.DispatchOK;
        }

        [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
        public static Ice.DispatchStatus WriteLog___(LogServer_RPC obj__, IceInternal.Incoming inS__, Ice.Current current__)
        {
            Ice.ObjectImpl.checkMode__(Ice.OperationMode.Normal, current__.mode);
            IceInternal.BasicStream is__ = inS__.startReadParams();
            string strServerID;
            string strPluginName;
            string strLog;
            int LogLevel;
            strServerID = is__.readString();
            strPluginName = is__.readString();
            strLog = is__.readString();
            LogLevel = is__.readInt();
            inS__.endReadParams();
            obj__.WriteLog(strServerID, strPluginName, strLog, LogLevel, current__);
            inS__.writeEmptyParams__();
            return Ice.DispatchStatus.DispatchOK;
        }

        private static string[] all__ =
        {
            "SetWriteLogFlag",
            "WriteLog",
            "WriteLogInit",
            "ice_id",
            "ice_ids",
            "ice_isA",
            "ice_ping"
        };

        public override Ice.DispatchStatus dispatch__(IceInternal.Incoming inS__, Ice.Current current__)
        {
            int pos = _System.Array.BinarySearch(all__, current__.operation, IceUtilInternal.StringUtil.OrdinalStringComparer);
            if(pos < 0)
            {
                throw new Ice.OperationNotExistException(current__.id, current__.facet, current__.operation);
            }

            switch(pos)
            {
                case 0:
                {
                    return SetWriteLogFlag___(this, inS__, current__);
                }
                case 1:
                {
                    return WriteLog___(this, inS__, current__);
                }
                case 2:
                {
                    return WriteLogInit___(this, inS__, current__);
                }
                case 3:
                {
                    return Ice.ObjectImpl.ice_id___(this, inS__, current__);
                }
                case 4:
                {
                    return Ice.ObjectImpl.ice_ids___(this, inS__, current__);
                }
                case 5:
                {
                    return Ice.ObjectImpl.ice_isA___(this, inS__, current__);
                }
                case 6:
                {
                    return Ice.ObjectImpl.ice_ping___(this, inS__, current__);
                }
            }

            _System.Diagnostics.Debug.Assert(false);
            throw new Ice.OperationNotExistException(current__.id, current__.facet, current__.operation);
        }

        #endregion

        #region Marshaling support

        protected override void writeImpl__(IceInternal.BasicStream os__)
        {
            os__.startWriteSlice(ice_staticId(), -1, true);
            os__.endWriteSlice();
        }

        protected override void readImpl__(IceInternal.BasicStream is__)
        {
            is__.startReadSlice();
            is__.endReadSlice();
        }

        #endregion
    }
}
