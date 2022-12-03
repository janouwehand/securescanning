package nl.ou.securescan.data

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import java.io.File

@Database(entities = [Document::class], version = 1)
abstract class DocumentDatabase : RoomDatabase() {

    abstract fun documentDao(): DocumentDao

    companion object {
        @Volatile
        private var INSTANCE: DocumentDatabase? = null

        fun getDatabase(context: Context): DocumentDatabase {
            val tempInstance = INSTANCE
            if (tempInstance != null) {
                return tempInstance
            }

            synchronized(this) {
                val instance = Room.databaseBuilder(
                    context.applicationContext,
                    DocumentDatabase::class.java,
                    "documentsdb"
                )
                    .fallbackToDestructiveMigration()
                    .build()
                INSTANCE = instance
                return instance
            }
        }

        fun deleteDatabaseFile(context: Context) {
            val databaseName = "documentsdb"
            val databases = File(context.applicationInfo.dataDir + "/databases")
            val db = File(databases, databaseName)
            if (db.delete()) println("Database deleted") else println("Failed to delete database")
            val journal = File(databases, "$databaseName-journal")
            if (journal.exists()) {
                if (journal.delete()) println("Database journal deleted") else println("Failed to delete database journal")
            }
        }
    }
}