<?php
require_once 'prolog.php';

// https://www.w3schools.com/Php/func_mysqli_connect.asp
// předělat do dvou connection stringů, přidat vytvoření dvou účtů do dockerfilu
function getConnectionAdmin() : mysqli {
    $mysqli = new mysqli("db","root","root","skladovy_system");
    return $mysqli;
} 

function getConnectionWorker() : mysqli {
    $mysqli = new mysqli("db","root","root","skladovy_system");
    return $mysqli;
} 

