package nl.softable.hcetest;

import static androidx.core.content.PermissionChecker.PERMISSION_GRANTED;

import android.Manifest;
import android.app.Activity;
import android.nfc.NfcAdapter;
import android.nfc.NfcAdapter.ReaderCallback;
import android.nfc.Tag;
import android.nfc.tech.IsoDep;
import android.os.Bundle;
import android.widget.ListView;

import androidx.core.content.ContextCompat;

import nl.softable.hcetest.IsoDepTransceiver.OnMessageReceived;

public class MainActivity extends Activity implements OnMessageReceived, ReaderCallback {

    private NfcAdapter nfcAdapter;
    private ListView listView;
    private IsoDepAdapter isoDepAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        listView = findViewById(R.id.listView);
        isoDepAdapter = new IsoDepAdapter(getLayoutInflater());
        listView.setAdapter(isoDepAdapter);
        nfcAdapter = NfcAdapter.getDefaultAdapter(this);
    }

    @Override
    public void onResume() {
        super.onResume();
        //nfcAdapter.enableReaderMode(this, this, NfcAdapter.FLAG_READER_NFC_A | NfcAdapter.FLAG_READER_SKIP_NDEF_CHECK,                null);
    }

    @Override
    public void onPause() {
        super.onPause();
        //nfcAdapter.disableReaderMode(this);
    }

    @Override
    public void onTagDiscovered(Tag tag) {
        IsoDep isoDep = IsoDep.get(tag);
        IsoDepTransceiver transceiver = new IsoDepTransceiver(isoDep, this);
        Thread thread = new Thread(transceiver);
        thread.start();
    }

    @Override
    public void onMessage(final byte[] message) {
        runOnUiThread(() -> isoDepAdapter.addMessage(new String(message)));
    }

    @Override
    public void onError(Exception exception) {
        onMessage(exception.getMessage().getBytes());
    }

    @Override
    protected void onStart() {
        super.onStart();

        boolean hasNFC = ContextCompat.checkSelfPermission(this, Manifest.permission.NFC) == PERMISSION_GRANTED;
        if (!hasNFC){

        }

        boolean hasNFC_PREFERRED_PAYMENT_INFO = ContextCompat.checkSelfPermission(this, Manifest.permission.NFC_PREFERRED_PAYMENT_INFO) == PERMISSION_GRANTED;
        if (!hasNFC_PREFERRED_PAYMENT_INFO){

        }

        boolean hasNFC_TRANSACTION_EVENT = ContextCompat.checkSelfPermission(this, Manifest.permission.NFC_TRANSACTION_EVENT) == PERMISSION_GRANTED;
        if (!hasNFC_TRANSACTION_EVENT){

        }
    }
}
