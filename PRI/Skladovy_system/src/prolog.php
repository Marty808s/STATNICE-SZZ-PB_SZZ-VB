<?php
define('INC', __DIR__ . '/includes');
define('DB', __DIR__ . '/db');
define('PHP', __DIR__ . '/php');
define('AST', __DIR__ . '/assets');

session_start();

require_once DB . '/db.php';



// pomocné funkce
function requireLogin(): void
{
    if (!isUser()) {
        header('Location: /login.php');
        exit;
    }
}

function isUser() : bool {
    return isset($_SESSION['jmeno']) && isset($_SESSION['role']);
}

function setUser(string $jmeno = '', string $role ='') {
    if (isset($jmeno) && $jmeno !== '' && isset($role) && $role !== '') {
        $_SESSION['jmeno'] = $jmeno;
        $_SESSION['role'] = $role;
    }
    else {
        unset($_SESSION['jmeno']);
        unset($_SESSION['role']);
    }
}

function getName(): string
{
    return $_SESSION['jmeno'];
}


function isSkladnik(): bool
{
    if (isset($_SESSION['role'])) {
        return $_SESSION['role'] == "skladnik";
    } 
    else {
        return false;
    }
}

function isAdmin(): bool
{
    if (isset($_SESSION['role'])) {
        return $_SESSION['role'] == "admin";
    } 
    else {
        return false;
    }
}

function logout(): bool
{
    if (isUser()) {
        setUser();
        return true;
    } else{
        return false;
    }
}