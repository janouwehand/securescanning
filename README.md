# Intro

The Secure Scan architecture consists of three parts:
1. The Multi-Function Printer (MFP) mock, that 'digitizes' a PDF, applies encryption to it, and submits it to an SMTP server. It communicates with the smartphone (application) for exchanging the public key.
2. The Mail User Agent (MUA) add-in (for Outlook) that can communicatie with the smartphone over Bluetooth to obtain the encryption key for the document that is attached to the email.
3. The smartphone app itself, which utilizes Host-Based Card Emulation (HCE) for communicating with the MFP's NFC reader and Bluetooth for the MUA.

Secure Scanning Project

<p float="left">
  <img src="https://github.com/janouwehand/securescanning/assets/64165589/c4579d47-3350-4638-821a-c25efee70f69" width="300" />
  <img src="https://github.com/janouwehand/securescanning/assets/64165589/3198962c-59c8-479c-810d-4869846f9bfb" width="300" /> 
</p>
