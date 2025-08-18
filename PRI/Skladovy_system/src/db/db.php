<?php

//$db_conn = isUser() ? getRoleConnection(role: $_SESSION['role']) : getRootConnection();
$db_conn = getRootConnection();

if ($db_conn -> connect_errno) {
    error_log("Failed to connect to MySQL: " . $conn -> connect_error);
    exit();
} else {
    echo "| Připojení je ok |";
}

function getRootConnection(): mysqli {
  return new mysqli('db','root','root','skladovy_system', 3306);
}

// tohle v případě, že to řeším i přes db - NENÍ MŮJ PŘÍPAD
function getRoleConnection(string $role): mysqli {
  if ($role === 'admin') return new mysqli('db','admin_user','admin_pass','skladovy_system', 3306);
  return new mysqli('db','worker_user','worker_pass','skladovy_system', 3306);
}

function dbQuery($sql, $params = []) {
    global $db_conn;
    $stmt = $db_conn->prepare($sql);

    //proběhla příprava úspěšně? existuje tabulka?...atd..
    if (!$stmt) {
        error_log("Chyba při přípravě dotazu: " . $db_conn->error);
        return false;
    }

    // pokud nezadám parametry ani dotaz - dochází k navazáí parametrů na dotaz
    if ($params && $stmt->bind_param(str_repeat("s", count($params)), ...$params) === false) {
        error_log("Chyba při vázání parametrů: " . $stmt->error);
        return false;
    }

    // pokud se nevykoná dotaz
    if ($stmt->execute() === false) {
        error_log("Chyba při vykonávání dotazu: " . $stmt->error);
        return false;
    }

    // získání výsledků
    $result = $stmt->get_result();
    if ($result) {
        return $result->fetch_all(MYSQLI_ASSOC); 
    } else {
        return []; 
    }
}