package nl.ou.securescan.data

import androidx.room.Dao
import androidx.room.Delete
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query

@Dao
interface DocumentDao {
    @Query("select * from document")
    suspend fun getAll(): List<Document>

    @Query("select * from document where id = :id")
    suspend fun getById(id: Int): Document

    @Query("update document set name = :name where id = :id")
    suspend fun updateName(id: Int, name: String)

    @Query("select * from document where name like :name limit 1")
    suspend fun getByName(name: String): Document

    @Insert(onConflict = OnConflictStrategy.IGNORE)
    suspend fun insert(document: Document)

    @Delete
    suspend fun delete(document: Document)

    @Query("delete from document")
    suspend fun deleteAll()

    @Query("select max(id) from document")
    suspend fun getLastDocumentId(): Int
}