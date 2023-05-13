package nl.ou.securescan.crypto.newcertificate

import android.security.keystore.KeyProperties
import android.util.Log
import org.bouncycastle.asn1.*
import org.bouncycastle.asn1.x500.X500Name
import org.bouncycastle.asn1.x509.*
import org.bouncycastle.cert.AttributeCertificateHolder
import java.math.BigInteger
import java.security.KeyPairGenerator
import java.security.Signature
import java.util.*
import javax.security.auth.x500.X500Principal

class GenerateAttributeCertificate {

    fun execute(): AttributeCertificate? {
        val holder = X500Name("CN=John Doe") //holder name
        val serialNumber = BigInteger.valueOf(123456) // serial number
        val issuer = X500Name("CN=AC Issuer") // issuer name
        val attrType = Extension.reasonCode // ASN1ObjectIdentifier("1.2.3.4") // attribute type
        val attrValue = DERBitString("Hello World".toByteArray()) // attribute value
        val attrs = ASN1EncodableVector()
        attrs.add(attrType)
        attrs.add(attrValue)
        val attributes = DERSequence(attrs)

        val h2 = AttributeCertificateHolder(holder)
        var hh = Holder(GeneralNames(GeneralName(holder)))

        val gen = V2AttributeCertificateInfoGenerator()
        gen.addAttribute("2.5.29.23", attrValue)
        gen.setHolder(hh)
        gen.setSerialNumber(ASN1Integer(BigInteger.valueOf(1)))
        gen.setStartDate(ASN1GeneralizedTime(Calendar.getInstance().time))
        gen.setEndDate(ASN1GeneralizedTime(Calendar.getInstance().time))
        gen.setIssuer(AttCertIssuer(GeneralNames(GeneralName(X500Name("CN=jan@sodtable.nl")))))
        gen.setIssuerUniqueID(DERBitString("01234567890123456789012345678912".toByteArray()))
        gen.setSignature(AlgorithmIdentifier(ASN1ObjectIdentifier("1.2.840.113549.1.1.11")))

        //gen.setSignature(AlgorithmIdentifier(ASN1ObjectIdentifier("2.16.840.1.101.3.4.2.1")))

        try {
            val info = gen.generateAttributeCertificateInfo()

            val keyPairGenerator = KeyPairGenerator.getInstance("RSA")
            val keyPair = keyPairGenerator.genKeyPair()

            val sig = Signature.getInstance("SHA256withRSA")
            sig.initSign(keyPair.private)
            sig.update(info.encoded)
            val signature = sig.sign()

            val signatureAlgorithm =
                AlgorithmIdentifier(ASN1ObjectIdentifier("1.2.840.113549.1.1.11"))
            val ac = AttributeCertificate(info, signatureAlgorithm, DERBitString(signature))
            return ac
        }
        catch (ex: Exception)
        {
            Log.e("SecureScan", ex.message.toString())
        }

        return null
    }

}