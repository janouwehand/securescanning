package nl.softable.hcetest;

import android.nfc.cardemulation.HostApduService;
import android.os.Bundle;
import android.util.Log;

import java.lang.reflect.Array;
import java.nio.charset.Charset;

public class MyHostApduService extends HostApduService {

	private int messageCounter = 0;
	private static Object lock = new Object();

	@Override
	public byte[] processCommandApdu(byte[] apdu, Bundle extras) {
		if (selectAidApdu(apdu)) {
			Log.i("HCEDEMO", "Application selected");
			return getWelcomeMessage();
		}
		else {
			Log.i("HCEDEMO", "Received: " + bytesToHex(apdu));
			return getNextMessage();
		}
	}

	public static String bytesToHex(byte[] bytes) {
		final char[] hexArray = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
		char[] hexChars = new char[bytes.length * 2];
		int v;
		for ( int j = 0; j < bytes.length; j++ ) {
			v = bytes[j] & 0xFF;
			hexChars[j * 2] = hexArray[v >>> 4];
			hexChars[j * 2 + 1] = hexArray[v & 0x0F];
		}
		return new String(hexChars);
	}

	private byte[] getWelcomeMessage() {
		synchronized(lock) {
		messageCounter=0;
		Log.i("HCEDEMO", "Current counter: " + messageCounter);
			return ("Hello " + messageCounter++ + " welcome Home  ").getBytes(Charset.forName("UTF8"));
		}
	}

	private byte[] getNextMessage() {
		synchronized(lock) {
			Log.i("HCEDEMO", "Current counter: " + messageCounter);
			return ("A new message from android: " + (messageCounter++) + "    ").getBytes();
		}
	}

	private boolean selectAidApdu(byte[] apdu) {
		return apdu.length >= 2 && apdu[0] == (byte)0 && apdu[1] == (byte)0xa4;
	}

	@Override
	public void onDeactivated(int reason) {
		Log.i("HCEDEMO", "Deactivated: " + reason);
	}
}