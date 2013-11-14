package com.wandoujia.wdpaydemo;

import java.math.BigDecimal;
import java.net.URLEncoder;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.wandoujia.login.AccountHelper;
import com.wandoujia.paydef.LoginCallBack;
import com.wandoujia.paydef.MSG;
import com.wandoujia.paydef.PayCallBack;
import com.wandoujia.paydef.User;
import com.wandoujia.paydef.WandouAccount;
import com.wandoujia.paydef.WandouOrder;
import com.wandoujia.paydef.WandouPay;
import com.wandoujia.paysdk.PayConfig;
import com.wandoujia.paysdkimpl.WandouPayImpl;

public class PayDemo extends Activity {
    private static final String TAG = "PayDemo";
    private final static String checkTokenUrl = "https://pay.wandoujia.com/api/uid/check";
    Activity act = this;
    Bundle savedIns;
    long start_time = 0;
    static String show_msg = null;
    static User cur_user;
    
    // 需要配置的部分- 开始
    // 开发者appkey_id
    final Long appkey_id = 100000000L;
    // 开发者 安全秘钥
    final String secretkey = "99b4efb45d49338573a00be7a1431511";
    // 初始化支付实例
    WandouPay wandoupay = new WandouPayImpl();
    // 初始化账户实例
    WandouAccount account = new AccountHelper();

    static Context appContext;

    WandouOrder order;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        Log.e(TAG, "start onCreate~~~" + android.os.Build.VERSION.RELEASE);
        super.onCreate(savedInstanceState);
        PayConfig.init(this, appkey_id, secretkey);
        appContext = getApplicationContext();
        gameLayout();
    }

    public void gameLayout() {
        setContentView(R.layout.activity_main);

        TextView text = (TextView) findViewById(R.id.orderId);
        if (order != null)
            text.setText(order + "");
        if (show_msg != null)
            text.setText(show_msg);

        // 付款
        Button submit = (Button) findViewById(R.id.submit);
        submit.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View arg0) {
                BigDecimal d = new BigDecimal(textString(R.id.money));
                d = d.multiply(new BigDecimal("100"));
                // 三个参数分别是 游戏名(String)，商品(String)，价格(Long)单位是分
                order = new WandouOrder(textString(R.id.subject), textString(R.id.desc), Long.valueOf(d.longValue()));
                order.out_trade_no = "gameZone|gameOrderID|uid";
                Log.w("Pay", "pay!");
                wandoupay.pay(act, order, new PayCallBack() {

                    @Override
                    public void onSuccess(User user, WandouOrder order) {
                        Log.w("DemoPay", "onSuccess:" + order + " status:" + order.status(order.TRADE_SUCCESS));
                        show_msg = user.getNick() + " 支付成功！" + order;
                        TextView text = (TextView) findViewById(R.id.orderId);
                        text.setText(show_msg);
                        cur_user = user;
                    }

                    @Override
                    public void onError(User user, WandouOrder order) {
                        Log.w("DemoPay", "onError:" + order);
                        gameLayout();// 回到游戏页面
                        TextView text = (TextView) findViewById(R.id.orderId);
                        show_msg = user.getNick() + "支付失败：" + order;
                        text.setText(show_msg);// 显示订单信息，包含提示信息
                    }
                });
            }

        });
        // 登录
        Button logintest = (Button) findViewById(R.id.logintest);
        logintest.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub

                Log.w(TAG, "doLogin!");
                account.doLogin(act, new LoginCallBack() {

                    @Override
                    public void onError(int code, String info) {
                        setText(R.id.account, "Demo中登陆失败:" + MSG.trans(info));
                        show_msg = MSG.trans(info);
                        Log.e(TAG, info);
                    }

                    @Override
                    public void onSuccess(User user, int type) {
                        Log.w("login", "success:+" + user);
                        Log.w("checkTokenUrl",
                                checkTokenUrl + "?uid=" + user.getUid() + "&token="
                                        + URLEncoder.encode(user.getToken()));
                        setText(R.id.account, user.toString());
                        cur_user = user;
                        //请在游戏创建角色时调用，不要每次登录都使用
                        account.createRole(appContext, user, "gameZone", "roleName");
                    }
                });
            }
        });
        // setPath
        Button button = (Button) findViewById(R.id.setPath);
        button.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View arg0) {
                /*
                 * com.wandoujia.paysdkimpl.Config.serverPath =
                 * textString(R.id.bashPath);
                 * setToast(com.wandoujia.paysdkimpl.Config.serverPath);
                 */
                // String a=null;
                // TestMail.send(LogEvent.sdk_value+"-crash log", "test",
                // "aidi@wandoujia.com",
                // "aidi@wandoujia.com,zhangchangyi@wandoujia.com");
                // a.length();
            }
        });
        // confirmLogin
        logintest = (Button) findViewById(R.id.confirmlogin);
        logintest.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub

                Log.w(TAG, "doLogin!");
                account.doLogin(act, true, new LoginCallBack() {

                    @Override
                    public void onError(int code, String info) {
                        setText(R.id.account, "Demo中登陆失败:" + MSG.trans(info));
                        show_msg = MSG.trans(info);
                        Log.e(TAG, info);
                    }

                    @Override
                    public void onSuccess(User user, int type) {
                        Log.w("login", "success:+" + user);
                        Log.w("checkTokenUrl",
                                checkTokenUrl + "?uid=" + user.getUid() + "&token="
                                        + URLEncoder.encode(user.getToken()));
                        setText(R.id.account, user.toString());
                        cur_user = user;

                        account.createRole(appContext, user, "gameZone", "roleName");
                    }
                });
            }
        });

        logintest = (Button) findViewById(R.id.logouttest);

        logintest.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                WandouAccount account = new AccountHelper();
                account.doLogout(act, null);
                setText(R.id.account, "");
            }
        });

        findViewById(R.id.exit).setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub
                System.exit(0);
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return true;
    }

    public String textString(int id) {
        return ((TextView) findViewById(id)).getText().toString();
    }

    public void setText(int id, String str) {
        ((TextView) findViewById(id)).setText(str);
    }

    private Toast toast = null;

    private void setToast(String str) {
        if (str == null) {
            if (toast != null)
                toast.cancel();
            return;
        }
        toast = Toast.makeText(getApplicationContext(), str, Toast.LENGTH_SHORT);
        toast.show();
    }

    @Override
    protected void onStart() {
        super.onStart();
        Log.e(TAG, "start onStart~~~");
    }

    @Override
    protected void onRestart() {
        super.onRestart();
        Log.e(TAG, "start onRestart~~~");
    }

    @Override
    protected void onResume() {
        super.onResume();
        Log.e(TAG, "start onResume~~~");
    }

    @Override
    protected void onPause() {
        super.onPause();
        Log.e(TAG, "start onPause~~~");
    }

    @Override
    protected void onStop() {
        super.onStop();
        Log.e(TAG, "start onStop~~~");
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        Log.e(TAG, "start onDestroy~~~");
    }
}
