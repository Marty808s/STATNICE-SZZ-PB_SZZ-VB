const express = require('express');
const mysql = require('mysql2') // jinak mi nešlo připojit (neprošel přístup pro ověření hesla..)
const cors = require('cors')

const app = express()
app.use(cors())

const isLOCAL = true

const rootDB = mysql.createConnection({
    host: isLOCAL ? "localhost" : "db",
    user: "root",
    password: "root",
    database: "skladovy_system"
})

rootDB.connect((error) => {
    if (error) {
        console.error("[MySQL] Connection error:", error.code, error.message)
        console.error("[MySQL] Full error:", error)
        return
    }
    console.log("[MySQL] Connected, thread id:", rootDB.threadId)
})


rootDB.on('error', (err) => {
    console.error("[MySQL] Connection error event:", err)
})


// tady logika pro získání db con stringu pro příslušnou roli
//....

app.get('/', (req,res) => {
    return res.json("From BE");
})

// Healthcheck endpoint to verify DB connectivity
app.get('/health/db', (req, res) => {
    rootDB.ping((err) => {
        if (err) {
            console.error("[MySQL] Ping failed:", err)
            return res.status(500).json({ ok: false, error: err.message })
        }
        return res.json({ ok: true })
    })
})

app.listen(8081, () => {
    console.log("Listening");
})

app.get('/api/users', (req,res) => {
    const sql = "SELECT * FROM users"
    rootDB.query(sql, (err, data) => {
        if (err) {
            console.error("[MySQL] /api/users query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json(data)
    } )
}) 