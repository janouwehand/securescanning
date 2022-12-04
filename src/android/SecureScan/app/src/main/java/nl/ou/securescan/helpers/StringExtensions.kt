package nl.ou.securescan.helpers

import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter


fun String.toZonedDateTime(): ZonedDateTime = ZonedDateTime.parse(this)

fun ZonedDateTime.toNeatDateString(): String {
    val formatter = DateTimeFormatter.ofPattern("EEEE, yyyy MMM d, HH:mm")
    return this.format(formatter)
}