using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Core;
using Windows.System.Threading;
using Windows.Management.Deployment;
using Windows.UI.Popups;
using Windows.Data.Json;

namespace AlipaySdk
{
    public sealed class AlixPay
    {
        #region interface
        public delegate void AlipayCallback(string val);
        private static AlipayCallback mCallback = null;
        private static AlipayCallback mProtCallback = null;
        public string ProtocolName
        {
            get;
            set;
        }

        /*
        参数：  orderInfo是string类型的订单信息；
                Callback是应用接收支付结果的异步回调委托；
                Protocol是前述步骤获取到的protocol name，在此处传入；
        返回值：无
        备注：  orderInfo参数传入的值如果和上次调用时相同的话，就可以实现被中断的支付场景恢复的功能；如果不相同，就被认为是一次新的订单。
               如果有支付结果，一定可以从callback中获取到；如果没有获取或者没有等到支付结果，您可以传入相同的orderInfo以恢复上次支付场景；
        建议：  建议用户调用时保存orderInfo，便于场景恢复。
         */
        public static void AlixpayAsync(string orderInfo, AlipayCallback callback,string protocol)
        {
            AlixPay curInstance = AlixPay.getInstance();
            curInstance.ProtocolName = protocol;
                
            //启动alipay
            curInstance.payStart(orderInfo, null, callback);

            return ;
        }

        /*
        参数：  protocolname是调用者应用（外部商户应用）的protocolname，如何获取protocolname请参考章节3.1.1.2
                Callback是应用接收支付结果的异步回调委托；
        返回值：无。
        功能：  如果快捷支付存在，就会调起快捷支付提示用户：支付模块正常，5秒后返回（用户也可以直接点击“返回”）。 5秒（或者点击“返回”）会自动返回到调用程序。
        备注：  如果callback中接收到这样的信息：“resultStatus={7100};memo={当前系统已经安装移动快捷支付应用。}”表明已经安装好移动快捷支付应用。
         */
        public async static void TestFastPayExist(string protocolname, AlipayCallback callback)
        {
            string uristr = "alifastpaywin8:?protocol=";
            mProtCallback = callback;
            uristr += protocolname; //protocolname是用于通知消息的。protocolname是商户传进来的。
            var uri = new Uri(uristr);//创建uri。
            var options = new Windows.System.LauncherOptions();
            options.DisplayApplicationPicker = false;            //为true时，强制显示选择列表。
            options.PreferredApplicationDisplayName = "移动快捷支付";                        //没有找到应用时，会弹出displayName
            options.PreferredApplicationPackageFamilyName = "424203F4.324177B885247_8asrdwhv739xj";  //这是移动快捷支付的packageFamilyName            
            await Windows.System.Launcher.LaunchUriAsync(uri, options);
            return ;
        }

        #endregion

        #region platform_interface
        private static readonly string mCnstChkHeader = "stamp";
        private static readonly string mCnstChkHeaderData = "Alipay";
        private static readonly string mCnstChkBody = "value";        

        private static bool _CheckData(JsonObject obj)
        {
            bool ret = false;
            if (false == obj.ContainsKey(mCnstChkHeader)
                || mCnstChkHeaderData != obj[mCnstChkHeader].GetString()
                || false == obj.ContainsKey(mCnstChkBody))
            {
                ret = false;
            }
            else
            {
                ret = true;
            }

            return ret;
        }

        private static string _GetData(JsonObject obj)
        {
            string ret = null;

            if (true == obj.ContainsKey(mCnstChkBody))
            {
                ret = obj[mCnstChkBody].GetString();
            }
            return ret;
        }

        public static void AlipayProcessProtRsp(IActivatedEventArgs args)
        {
            AlixPay curInstance = AlixPay.getInstance();
            ProtocolActivatedEventArgs te = args as ProtocolActivatedEventArgs;
            string data = te.Uri.OriginalString;
            string excape = ":?";
            int index = data.IndexOf(excape);
            data = data.Substring(index + excape.Length);

            if (data.Equals("launch=success"))
            {
                curInstance.onExitEx("resultStatus={7100};memo={当前系统已经安装移动快捷支付应用。}");
                return;
            }
            JsonObject obj = JsonObject.Parse(data);
            if (false == _CheckData(obj))
            {
                return;
            }
            string txtString = _GetData(obj);
            if (null != args)
            {
                curInstance.onExit(txtString);
            }
            else
            {
                //error callback
                curInstance.onExit("resultStatus={7000};memo={未知的订单状态，请同步服务器。}");
            }
            return;
        }
        #endregion

        #region implement
        private static AlixPay mInstance = null;

        private string mCurOrder = null;
        private string mCtiCode = null;

        private AlixPay()
        {

        }

        private static AlixPay getInstance()
        {
            if (null == mInstance)
            {
                mInstance = new AlixPay();
            }
            return mInstance;
        }

        private void onExit(string val)
        {
            if (null != mCallback)
            {
                mCallback(val);
            }
            mCallback = null;
            mProtCallback = null;
            mInstance = null;
            sdkDeinit();
        }

        private void onExitEx(String val)
        {
            if (null != mProtCallback)
            {
                mProtCallback(val);
            }
            mCallback = null;
            mProtCallback = null;
            mInstance = null;
            sdkDeinit();
        }

        private static void onDeferredReqHdlr(DataProviderRequest request)
        {
            DataProviderDeferral deferral = request.GetDeferral();

            // Make sure to always call Complete when done with the deferral.
            try
            {

            }
            finally
            {
                deferral.Complete();
            }
        }

        private void onDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.SetDataProvider("pay",
                    new DataProviderHandler(onDeferredReqHdlr));

            string dataPackageFormat = "pay";
            DataPackage requestData = e.Request.Data;
            requestData.Properties.Title = "支付宝";

            // The description is optional.
            string dataPackageDescription = "发送给支付宝的付款订单";
            if (dataPackageDescription != null)
            {
                requestData.Properties.Description = dataPackageDescription;
            }

            PackageId packageId = Package.Current.Id;
            string name = this.ProtocolName;

            JsonObject obj = new JsonObject();
            obj.SetNamedValue("protname", JsonValue.CreateStringValue(name));
            obj.SetNamedValue("cticode", JsonValue.CreateStringValue(mCtiCode));
            obj.SetNamedValue("order", JsonValue.CreateStringValue(mCurOrder));
            string data = obj.Stringify();
            requestData.SetData(dataPackageFormat, data);
        }

        private void sdkInit()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += onDataRequested;
        }

        private void sdkDeinit()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested -= onDataRequested;
        }
        
        private void payTrigger()
        {
            DataTransferManager.ShowShareUI();
        }

        private const String mCtiDefine = "out_trade_no=\"";
        private string getCtiCode()
        {
            String ret = null;
            if (true == mCurOrder.Contains(mCtiDefine))
            {
                int begin = mCurOrder.IndexOf(mCtiDefine);
                int end = mCurOrder.Substring(begin + mCtiDefine.Length).IndexOf("\"");
                ret = mCurOrder.Substring(begin + mCtiDefine.Length, end);
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                DateTime date = DateTime.Now;
                string format = "MMddHHmmss";
                sb.Append(date.ToString(format));

                Random random = new Random(int.Parse(sb.ToString()));
                sb.Append(random.Next());
                ret = sb.ToString().Substring(0, 15);
            }

            return ret;
        }

        private void payStart(string order,string ctiCode,AlipayCallback callback)
        {
            mCurOrder = order;
            mCallback = callback;
            if (null == ctiCode)
            {
                mCtiCode = getCtiCode();
            }
            sdkDeinit();
            sdkInit();
            payTrigger();
        }
        #endregion
    }
}