#define _ALIPAY_ONLINE_
//#define __ALIPAY_TEST__
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using AlipaySdk;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Animation;
using Windows.ApplicationModel;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsRTDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private string getOutTradeNo()
        {
            StringBuilder sb = new StringBuilder();

            DateTime date = DateTime.Now;
            string format = "MMddHHmmss";
            sb.Append(date.ToString(format));

            Random random = new Random(int.Parse(sb.ToString()));
            sb.Append(random.Next());

            return sb.ToString().Substring(0, 15);
        }
#if _ALIPAY_ONLINE_
        public static readonly string PRIVATE = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAISiSBlMFdjvrMxGboN3/mZ7oRJUQqPVKjPwam4O2DBOO3u3K0VS43ci52uqIvT+sKg48HEeYNufLhmqumyd0UP+n3IRmFxraZmjwKBuvqwjNqkZiuTVGvYDfz3uvjfur2HdOh8vW4ht61NU5+iSb1nRBpOzedjVhEkkCDWlYOZvAgMBAAECgYB34caRNvg4UMo7CR4yrm6/atc3nflt4+p1b8SDHd/CKbQSKolt63G16VKLOgjGsL40DuMlG3QojkQ45twB+NN/9tTP+DFsJ6N+r45U7pvq8FIrV+6ZYgexpnDBPP3aoXv8ne97wsPD9rZVVIvSm8P2m5ZDs/unAhcwsa93FrGwYQJBAMzphED+vrpBftlbeBq9J7lw3X/y5DXMdsIEF/1HC3mAgt1UCE/72HyBiXxEp/shUkOQycfwCTDi8HCQuk2XoLsCQQCls54ivahNME5Kwy2hkWxMDUcSBbu25f/2xkK8Ryt+F32C2IMLJV3+Mg5BQJmr7yHQKoIDvhulQqechsb7BZ/dAkAucYb+TD7ibFHZ5fd06AaG62Poyh6bavpHwzHEwEODiHMgwxkXN9e7cIi+17jTHJxOoBR78pXCtM0WVldDmuhdAkA8Yo+vY2RQ9NijQCuB5KgNsw0CUVqOFZVJDglF6b28zryrkVF4H174gq9VMkCOOrAc11DYIlCa0gaY8TjUQ8F1AkEAuCXyfsxQf9ttar2lCsGzNve8OP+n2vfp0TZDeCt02TZf7O04hC5CNp7JQXJ/wnkzJTmQpPSAjKOojhJpADlpOA==";
#else
#if __ALIPAY_TEST__
        public static readonly string PRIVATE = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBALaeckPlqpa5FWi1yxVAT3t1ezzojnCqzHQ5xI0r2XW7mr7YeXl+RVuVB8zxFboHrpdnlZp3sanS1Jj2xohlvGOMWHaZs3Ye3KVr+gaMm/A8qUv2B0euzd8U1AvZLMOFfBC17uXw9/x8gHLCRn5ud9tkgLIbRuolAAjMllCh7p/hAgMBAAECgYAPt0MOJM2xeuwkvsBja81rSoj4jOr1Nz2xIuePXp6wSxzeH7MUiZFeeEzbjkPYZqDX1spBKNvZSZOSNmqPgnHb2aT3A1HIeJzwuct03Uby83tZV+Zi2b+Pdv0kB1JPNrU8A2DfgbNBm1+iy3nabO2X9/GyochjszgC/uYSDnVuIQJBAPyzvQkna5OIeGX7Q4nGrn6sgoaD6UUWIknhbQN8qN+tt8T2xlenJYrH56T9WKLfOGu8EJFfupLWknFemL+zYq8CQQC5AJCE5BuAjXz3pMTLGj1GPB0rQj7QvAo0Cyz5tlf4CFK9jCVKwZ65BITM0zqMaZJhRWwAgaBWcRQABqdWdApvAkEAtZeX9VcNmDROiMJ58y0CQedH2NA8NjhEpaDHzOStGifk0jafq2ditAsZbFfedRRBoDHCGiWXlmN5UtyumbuX4wJACyftjxXyUp41mvlkpJrAdyvI1oL4Jr4wH1NNMwG77EkUNDnvRcLHP4D2QSO7tBvpp9P330/xy0SmYBoGnUACIQJBAIVqqXilDKr/R7ZxXHG4l4OG1yPdmqaQl+6amncEZAKdBnCBGX8mplda5BbpWN5p8YAmaHDBSleHvmUktG1LPcs=";
#else
        public static readonly string PRIVATE = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBALaeckPlqpa5FWi1yxVAT3t1ezzojnCqzHQ5xI0r2XW7mr7YeXl+RVuVB8zxFboHrpdnlZp3sanS1Jj2xohlvGOMWHaZs3Ye3KVr+gaMm/A8qUv2B0euzd8U1AvZLMOFfBC17uXw9/x8gHLCRn5ud9tkgLIbRuolAAjMllCh7p/hAgMBAAECgYAPt0MOJM2xeuwkvsBja81rSoj4jOr1Nz2xIuePXp6wSxzeH7MUiZFeeEzbjkPYZqDX1spBKNvZSZOSNmqPgnHb2aT3A1HIeJzwuct03Uby83tZV+Zi2b+Pdv0kB1JPNrU8A2DfgbNBm1+iy3nabO2X9/GyochjszgC/uYSDnVuIQJBAPyzvQkna5OIeGX7Q4nGrn6sgoaD6UUWIknhbQN8qN+tt8T2xlenJYrH56T9WKLfOGu8EJFfupLWknFemL+zYq8CQQC5AJCE5BuAjXz3pMTLGj1GPB0rQj7QvAo0Cyz5tlf4CFK9jCVKwZ65BITM0zqMaZJhRWwAgaBWcRQABqdWdApvAkEAtZeX9VcNmDROiMJ58y0CQedH2NA8NjhEpaDHzOStGifk0jafq2ditAsZbFfedRRBoDHCGiWXlmN5UtyumbuX4wJACyftjxXyUp41mvlkpJrAdyvI1oL4Jr4wH1NNMwG77EkUNDnvRcLHP4D2QSO7tBvpp9P330/xy0SmYBoGnUACIQJBAIVqqXilDKr/R7ZxXHG4l4OG1yPdmqaQl+6amncEZAKdBnCBGX8mplda5BbpWN5p8YAmaHDBSleHvmUktG1LPcs=";
#endif
#endif
        public static readonly string PUBLIC = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC2nnJD5aqWuRVotcsVQE97dXs86I5wqsx0OcSNK9l1u5q+2Hl5fkVblQfM8RW6B66XZ5Wad7Gp0tSY9saIZbxjjFh2mbN2Htyla/oGjJvwPKlL9gdHrs3fFNQL2SzDhXwQte7l8Pf8fIBywkZ+bnfbZICyG0bqJQAIzJZQoe6f4QIDAQAB";

        private string rsaEncoding(string content, string privateKey)
        {
            string ret = "";
            if (null == content
                || null == privateKey)
            {
                return ret;
            }
            try
            {
                AsymmetricKeyAlgorithmProvider Algorithm = AsymmetricKeyAlgorithmProvider.OpenAlgorithm("RSASIGN_PKCS1_SHA1");
                IBuffer orderBuf = CryptographicBuffer.ConvertStringToBinary(content, BinaryStringEncoding.Utf8);
                IBuffer privBuf = CryptographicBuffer.DecodeFromBase64String(privateKey);
                CryptographicKey privKey = Algorithm.ImportKeyPair(privBuf, CryptographicPrivateKeyBlobType.Pkcs8RawPrivateKeyInfo);

                HashAlgorithmProvider sha1Algorithm = HashAlgorithmProvider.OpenAlgorithm("SHA1");
                IBuffer digest = sha1Algorithm.HashData(orderBuf);
                //string sha1value = CryptographicBuffer.EncodeToHexString(digest);
                ///return md5value;
                byte[] sha1 = new byte[20];
                CryptographicBuffer.CopyToByteArray(digest, out sha1);

                IBuffer Signature = CryptographicEngine.Sign(privKey, orderBuf);

                byte[] outByte;
                CryptographicBuffer.CopyToByteArray(Signature, out outByte);

                byte[] total = new byte[outByte.Length + 38];
                total[0] = 0;
                total[1] = 5;
                for (int i = 0; i < outByte.Length; i++)
                {
                    total[2 + i] = outByte[i];
                }
                total[2 + outByte.Length] = 0;

                //byte[] rsa_sha1 = { "\x30\x21\x30\x09\x06\x05\x2B\x0E\x03\x02\x1A\x05\x00\x04\x14" };

                total[3 + outByte.Length + 0] = 0x30;
                total[3 + outByte.Length + 1] = 0x21;
                total[3 + outByte.Length + 2] = 0x30;
                total[3 + outByte.Length + 3] = 0x09;
                total[3 + outByte.Length + 4] = 0x06;
                total[3 + outByte.Length + 5] = 0x05;
                total[3 + outByte.Length + 6] = 0x2B;
                total[3 + outByte.Length + 7] = 0x0E;
                total[3 + outByte.Length + 8] = 0x03;
                total[3 + outByte.Length + 9] = 0x02;
                total[3 + outByte.Length + 10] = 0x1A;
                total[3 + outByte.Length + 11] = 0x05;
                total[3 + outByte.Length + 12] = 0x00;
                total[3 + outByte.Length + 13] = 0x04;
                total[3 + outByte.Length + 14] = 0x14;

                for (int i = 0; i < sha1.Length; i++)
                {
                    total[18 + outByte.Length + i] = sha1[i];
                }

                IBuffer sha1Signed = CryptographicBuffer.CreateFromByteArray(total);

                ret = CryptographicBuffer.EncodeToBase64String(Signature);
            }
            catch
            {

            }
            return ret;
        }

        private string getOrderInfo(string partner,string money)
        {
            StringBuilder sb = new StringBuilder();
            string order = null;
            string subject = "付款信息";
            string body = "LOVE TIME";
#if _ALIPAY_ONLINE_
            sb.Append("partner=\"").Append("2088201564809153");
            sb.Append("\"&seller=\"").Append("2088201564809153");
#else
            sb.Append("partner=\"").Append(partner);
            sb.Append("\"&seller=\"").Append(partner);
#endif
            sb.Append("\"&out_trade_no=\"").Append(getOutTradeNo());
            sb.Append("\"&subject=\"").Append(subject);
            sb.Append("\"&body=\"").Append(body);
            sb.Append("\"&total_fee=\"").Append(money);
            sb.Append("\"&notify_url=\"").Append(Uri.EscapeDataString("http://notify.java.jpxx.org/index.jsp"));
            sb.Append("\"");

            order = sb.ToString();
            string rsaValue = this.rsaEncoding(order,PRIVATE);

            sb.Append("&sign=\"");

            sb.Append(Uri.EscapeDataString(rsaValue));
            sb.Append("\"&sign_type=\"RSA\"");

            return sb.ToString();
        }

        private void callbackFunc(string val)
        {
            txtbox2.Text = val;
            defaultPay.Visibility = Visibility.Visible;
            rePay.Visibility = Visibility.Collapsed;
        }

        private void PackageChanged(object sender, TextChangedEventArgs e)
        {

            if (this.packagebox.Text.Length > 0)
            {
                string temp = "ali";
                temp += this.packagebox.Text.GetHashCode().ToString();
                this.protocolbox.Text = temp;
            }
        }

        private String mOrder = null;
        //新订单支付
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string money = "0.01";
#if __ALIPAY_TEST__
            string order = getOrderInfo("2088102002691058", money);
#else
            string order = getOrderInfo("2088102000947391",money);
#endif
            mOrder = order;
            AlixPay.AlixpayAsync(mOrder, callbackFunc, "ali-1367898968");
            defaultPay.Visibility = Visibility.Collapsed;
            rePay.Visibility = Visibility.Visible;
        }

        //订单重新支付
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AlixPay.AlixpayAsync(mOrder, callbackFunc, "ali-1367898968");
        }
        
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            AlixPay.TestFastPayExist("ali-1367898968", (val) =>
                {
                    txtbox2.Text = val;
                });
        }
    }
}
