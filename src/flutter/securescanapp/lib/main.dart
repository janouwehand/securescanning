import 'dart:convert';

import 'package:device_info_plus/device_info_plus.dart';
import 'package:flutter/material.dart';
import 'package:flutter_blue/flutter_blue.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        // This is the theme of your application.
        //
        // Try running your application with "flutter run". You'll see the
        // application has a blue toolbar. Then, without quitting the app, try
        // changing the primarySwatch below to Colors.green and then invoke
        // "hot reload" (press "r" in the console where you ran "flutter run",
        // or simply save your changes to "hot reload" in a Flutter IDE).
        // Notice that the counter didn't reset back to zero; the application
        // is not restarted.
        primarySwatch: Colors.blue,
      ),
      home: const MyHomePage(title: 'Flutter Demo Home Page'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key, required this.title});

  // This widget is the home page of your application. It is stateful, meaning
  // that it has a State object (defined below) that contains fields that affect
  // how it looks.

  // This class is the configuration for the state. It holds the values (in this
  // case the title) provided by the parent (in this case the App widget) and
  // used by the build method of the State. Fields in a Widget subclass are
  // always marked "final".

  final String title;

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  String text = 'Klik knop om te beginnen';

  Guid serviceId = Guid('bf80ab30-e076-46bc-aac8-7fefcc09976c');
  Guid characteristicId = Guid('e879628b-5832-45dd-9432-33a9717c8176');
  bool isScanning = false;

  void startScanning() async {
    setState(() {
      // This call to setState tells the Flutter framework that something has
      // changed in this State, which causes it to rerun the build method below
      // so that the display can reflect the updated values. If we changed
      // _counter without calling setState(), then the build method would not be
      // called again, and so nothing would appear to happen.
      text = 'Begonnen met scannen';
    });

    if (isScanning) {
      return;
    }

    DeviceInfoPlugin deviceInfo = DeviceInfoPlugin();
    AndroidDeviceInfo androidInfo = await deviceInfo.androidInfo;
    var name = androidInfo.host;

    FlutterBlue flutterBlue = FlutterBlue.instance;

    isScanning = true;
    flutterBlue.startScan(
        timeout: const Duration(seconds: 4), withServices: [serviceId]);

    flutterBlue.scanResults.listen((results) {
      for (ScanResult r in results) {
        flutterBlue.stopScan();
        isScanning = false;
        setState(() {
          text = '${r.device.name} found! rssi: ${r.rssi}';
        });
        onMFPFound(r);
        break;
      }
    });
  }

  void onMFPFound(ScanResult scanResult) async {
    if (scanResult.advertisementData.connectable) {
      setState(() {
        text = 'Connecting..........';
      });

      await scanResult.device.connect(autoConnect: false);

      setState(() {
        text = 'Connection established!';
      });

      List<BluetoothService> services =
          await scanResult.device.discoverServices();
      services.forEach((service) {
        if (serviceId == service.uuid) {
          onConnected(scanResult.device, service);
        }
      });
    }
  }

  void onConnected(BluetoothDevice device, BluetoothService service) async {
    setState(() {
      text = 'Connected to service: ${service.uuid.toString()}';
    });

    for (BluetoothCharacteristic c in service.characteristics) {
      if (c.uuid == characteristicId) {
        var phone = utf8.encode('JANOPHONE');
        c.write(phone);
      }
    }

    await device.disconnect();
  }

  @override
  Widget build(BuildContext context) {
    // This method is rerun every time setState is called, for instance as done
    // by the _incrementCounter method above.
    //
    // The Flutter framework has been optimized to make rerunning build methods
    // fast, so that you can just rebuild anything that needs updating rather
    // than having to individually change instances of widgets.
    return Scaffold(
      appBar: AppBar(
        // Here we take the value from the MyHomePage object that was created by
        // the App.build method, and use it to set our appbar title.
        title: Text(widget.title),
      ),
      body: Center(
        // Center is a layout widget. It takes a single child and positions it
        // in the middle of the parent.
        child: Column(
          // Column is also a layout widget. It takes a list of children and
          // arranges them vertically. By default, it sizes itself to fit its
          // children horizontally, and tries to be as tall as its parent.
          //
          // Invoke "debug painting" (press "p" in the console, choose the
          // "Toggle Debug Paint" action from the Flutter Inspector in Android
          // Studio, or the "Toggle Debug Paint" command in Visual Studio Code)
          // to see the wireframe for each widget.
          //
          // Column has various properties to control how it sizes itself and
          // how it positions its children. Here we use mainAxisAlignment to
          // center the children vertically; the main axis here is the vertical
          // axis because Columns are vertical (the cross axis would be
          // horizontal).
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              text,
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: startScanning,
        tooltip: 'Start scanning',
        child: const Icon(Icons.scanner),
      ), // This trailing comma makes auto-formatting nicer for build methods.
    );
  }
}
