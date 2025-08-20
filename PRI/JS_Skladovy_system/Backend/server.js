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

app.get('/api/objednavky', (req,res) => {
    const sql = 'SELECT o.id, c.name, c.surname, c.email, o.total_price, o.currency, o.state ' +
    'FROM `order` AS o ' +
    'JOIN customer AS c ON o.customer_id = c.id';
    //if (!userDB) {
    //    return res.status(401).json({ ok: false, error: 'Unauthorized' })
    //}
    rootDB.execute(sql, (err, results) => {
        if (err) {
            console.error("[MySQL] /api/objednavky query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json(results)


    })
})

app.get('/api/objednavky/:id', (req,res) => {
    const orderId = req.params.id;
    
    // Získání detailu objednávky s zákazníkem
    const orderSql = 'SELECT o.id, o.currency, o.total_price, o.state, c.name, c.surname, c.email ' +
                     'FROM `order` AS o ' +
                     'JOIN customer AS c ON o.customer_id = c.id ' +
                     'WHERE o.id = ?';
    
    // Získání produktů v objednávce
    const productsSql = 'SELECT op.id, op.unit_price, op.quantity, op.line_total, ' +
                        'p.id as product_id, p.name as product_name, p.piece_unit_type ' +
                        'FROM order_product AS op ' +
                        'JOIN product AS p ON op.product_id = p.id ' +
                        'WHERE op.order_id = ?';
    
    rootDB.execute(orderSql, [orderId], (err, orderResults) => {
        if (err) {
            console.error("[MySQL] /api/objednavky/:id order query error:", err)
            return res.status(500).json({ error: err.message })
        }
        
        if (!Array.isArray(orderResults) || orderResults.length === 0) {
            return res.status(404).json({ error: 'Objednávka nenalezena' })
        }
        
        const order = orderResults[0];
        
        rootDB.execute(productsSql, [orderId], (err, productResults) => {
            if (err) {
                console.error("[MySQL] /api/objednavky/:id products query error:", err)
                return res.status(500).json({ error: err.message })
            }
            
            const response = {
                order: order,
                products: productResults || []
            };
            
            return res.json(response);
        });
    });
})