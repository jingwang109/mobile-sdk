package com.chinagame.demo;

import android.app.ListActivity;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.View;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;
import cn.cmgame.billing.api.GameInterface;

import com.chinagame.billing.GameInfoBean;
import com.chinagame.billing.TouchPay;
import com.chinagame.billing.TouchPay.ExitCallBack;
import com.chinagame.billing.TouchPay.PayCallBack;

/**
 * This is just a demo activity for game developer to integrate CMGC's SDK.
 * @author AFWang
 *
 */
public class MainActivity extends ListActivity {
	static final String[] BUTTONS = new String[] {
		"00.Activate game",
	    "01.Game Community",
	    "02.Game List", 
	    "03.Options",
	    "04.About",
	    "05.Exit"
	    };

	/** Called when the activity is first created. */
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
    	
		// Initialize SDK instance for game billing.
		TouchPay.initSdk(this, new GameInfoBean("86000504", "90234616120120921431100", "测试DEMO", "捕鱼达人", "400-800-900"));
		
		Toast.makeText(MainActivity.this, "music is enabled ? "+ GameInterface.isMusicEnabled(), Toast.LENGTH_SHORT).show();
		
		setListAdapter(new ArrayAdapter<String>(this, R.layout.game_item, BUTTONS));
		ListView lv = getListView();
		lv.setOnItemClickListener(new OnItemClickListener() {
			public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			    if (0 == position) {
			    	TouchPay.pay(MainActivity.this, "购买120金币", "2.0元", "001", "130201102727", "YYYY", new PayCallBack() {
						@Override
						public void onPayResult(int arg0, String arg1) {
							if(arg0==TouchPay.SUCCESS){
								Toast.makeText(MainActivity.this, "pay ok...", Toast.LENGTH_SHORT).show();
							} else if(arg0==TouchPay.FAILED){
								Toast.makeText(MainActivity.this, "pay failed...", Toast.LENGTH_SHORT).show();
								exitGame();
							} else {
								Toast.makeText(MainActivity.this, "pay canceled...", Toast.LENGTH_SHORT).show();
								exitGame();
							}
						}
					});
				}  else {
					Toast.makeText(MainActivity.this, "Oh...no! This is just a demo.", Toast.LENGTH_SHORT).show();
				}
			}

		});
	}
	
	/**
	 * Exit game.
	 */
	private void exitGame(){	
		Toast.makeText(MainActivity.this, "exiting game..", Toast.LENGTH_SHORT).show();
		TouchPay.exit(MainActivity.this, new ExitCallBack() {
			@Override
			public void onConfirmExit() {
				MainActivity.this.finish();
			}
			
			@Override
			public void onCancelExit() {
				Toast.makeText(MainActivity.this, "do nothiong for exiting game..", Toast.LENGTH_SHORT).show();
			}
		});
	}
	
	@Override
	public void onResume(){
		super.onResume();
	}
    
    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if(keyCode==KeyEvent.KEYCODE_BACK){
        	exitGame();
            return true;
        }
        return super.onKeyDown(keyCode, event);
    }
}
