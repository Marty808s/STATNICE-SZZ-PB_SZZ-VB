const express = require('express');
const mysql = require('mysql2') // jinak mi nešlo připojit (neprošel přístup pro ověření hesla..)
const cors = require('cors')

const app = express()
app.use(cors())
app.use(express.json())

const isLOCAL = true

const rootDB = mysql.createConnection({
    host: isLOCAL ? "localhost" : "db",
    user: "root",
    password: "root",
    database: "skladovy_system",
    charset: 'utf8mb4'
})

// tohle bude používat následně každá role - svůj string
let userDB = null;

// role je i password - v mém případě
const getUserConn = (role) => {
    const conn =  mysql.createConnection({
        host: isLOCAL ? "localhost" : "db",
        user: role,
        password: role, //tohle kdyžtak změnit
        database: "skladovy_system",
        charset: 'utf8mb4'
    });
    console.log("STRING CONN PRO ROLI", conn);
    return conn;
}

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
    const sql = 'SELECT id, username, role FROM users'
    rootDB.query(sql, (err, results) => {
        if (err) {
            console.error("[MySQL] /api/users query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json(results)
    })
}) 

app.post('/api/login', (req,res) => {
    const { username, password } = req.body || {}
    if (typeof username !== 'string' || typeof password !== 'string') {
        return res.status(400).json({ error: 'Invalid payload' })
    }

    //tady by mělo být hashování a porovnání s db hashem...

    const sql = 'SELECT id, username, role FROM users WHERE username = ? AND password_hash = ?'
    rootDB.execute(sql, [username, password], (err, results) => {
        if (err) {
            console.error("[MySQL] /api/login query error:", err)
            return res.status(500).json({ error: err.message })
        }
        if (!Array.isArray(results) || results.length === 0) {
            return res.status(401).json({ ok: false, error: 'Unauthorized' })
        }
        userDB = getUserConn(results[0].role)
        return res.json(results[0])
    })
}) 

app.get('/api/produkty', (req,res) => {
    const sql = 'SELECT * FROM product';
    //if (!userDB) {
    //    return res.status(401).json({ ok: false, error: 'Unauthorized' })
    //}
    rootDB.execute(sql, (err, results) => {
        if (err) {
            console.error("[MySQL] /api/produkty query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json(results)


    })
})