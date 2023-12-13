# Intro

The Secure Scan architecture consists of three parts:
1. The Multi-Function Printer (MFP) mock, that 'digitizes' a PDF, applies encryption to it, and submits it to an SMTP server. It communicates with the smartphone (application) for exchanging the public key.
2. The Mail User Agent (MUA) add-in (for Outlook) that can communicatie with the smartphone over Bluetooth to obtain the encryption key for the document that is attached to the email.
3. The smartphone app itself, which utilizes Host-Based Card Emulation (HCE) for communicating with the MFP's NFC reader and Bluetooth for the MUA.

# Impression

<p float="left">
  <img src="https://github.com/janouwehand/securescanning/assets/64165589/c4579d47-3350-4638-821a-c25efee70f69" width="300" />
  <img src="https://github.com/janouwehand/securescanning/assets/64165589/3198962c-59c8-479c-810d-4869846f9bfb" width="300" /> 
</p>

MFP mock:

![image](https://github.com/janouwehand/securescanning/assets/64165589/dbf1793f-797a-40ee-bd0a-d0e99569b2f6)

Click enroll+scan. 
This refers to the combined process of authenticating the MFP and digitizing the document.
While the smartphone communicates with the MFP over NFC, authentication is performed out-of-bound, using a QR code.

![image](https://github.com/janouwehand/securescanning/assets/64165589/48a492c5-8e54-48ce-bb3f-5bdb26a2ac15)



Click enroll+scan within the app and then scan the QR code. 
The QR code shares a symmetric key which is used for subsequent authenticated and encrypted communication.

<p float="left">
  <img src="https://github.com/janouwehand/securescanning/assets/64165589/4284217a-d50d-4591-a1a7-66f40d71ed68" width="300" />
  <img src="https://github.com/janouwehand/securescanning/assets/64165589/e06fed5c-c9ea-4446-a3cb-7712fabb4ffe" width="300" /> 
</p>

Hold the smartphone to the MFP's NFC reader.
The user's X.509 certificate is sent to the MFP (CN=user's email address).

![image](https://github.com/janouwehand/securescanning/assets/64165589/1b3b3e64-f8a7-4483-b2a3-ec4529efeada)

User clicks 'start' after which the document starts digitizing the pages of the document.

![image](https://github.com/janouwehand/securescanning/assets/64165589/2f16be40-a43c-470f-b206-cb2fa7afdd01)

Digitization is done.
Hold the smartphone to the NFC reader again to retrieve the key.
Note that in this prototype the document's key is securely stored on the smartphone, and, that the key is not stored within the email, in encrypted form, akin S/MIME and OpenPGP.
Future versions of this prototype will align with the OpenPGP standard, which obviates the need for this step.

![image](https://github.com/janouwehand/securescanning/assets/64165589/8d204938-0245-40b5-80de-845cbc8089c8)

The MFP submitted to email with secure document to the email server.
The email:

![image](https://github.com/janouwehand/securescanning/assets/64165589/c9dcaa33-1d98-4f3f-88c0-7237b378fbc0)

Click 'document info' to retrieve info on the document:

![image](https://github.com/janouwehand/securescanning/assets/64165589/48b6ace7-9c83-4989-9a44-78c2ccdb024a)

Click 'Read secure document' to initiate the secure reading process.
This process queries Bluetooth to find an active (and paired) smartphone offering the Secure Scan Bluetooth service, which it, then, requests for the document's encryption key.

![image](https://github.com/janouwehand/securescanning/assets/64165589/f665e415-7f51-4a89-bf1c-4d80aea2fb39)

The encrpytion key is only returned after explicit consent.
To this end the smartphone shows the user an 'access request' notification.

<p float="left">
  <img src="https://github.com/janouwehand/securescanning/assets/64165589/4c53505b-185e-44a6-bc5b-96f71736dc8c" width="300" />
  <img src="https://github.com/janouwehand/securescanning/assets/64165589/61964427-1ffc-4277-9f0d-f9a31fc80227" width="300" /> 
</p>

After consent is given, the MUA add-in shows the document within the embedded secure document viewer.

![image](https://github.com/janouwehand/securescanning/assets/64165589/50c272de-ae53-451c-a2da-1a0342c8e73a)


