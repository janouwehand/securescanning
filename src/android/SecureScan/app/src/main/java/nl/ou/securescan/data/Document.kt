package nl.ou.securescan.data

import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "document")
data class Document(
    @PrimaryKey(autoGenerate = true) val id: Int?,
    @ColumnInfo(name = "name") val name: String?,
    @ColumnInfo(name = "scannedOn") val scannedOn: String?,
    @ColumnInfo(name = "document_hash") val documentHash: ByteArray?,
    @ColumnInfo(name = "document_password") val documentPassword: ByteArray?,
)
