<?php

// Adresáře
define('INC', __DIR__ . '/includes');
define('XML', __DIR__ . '/xml_files');
define('PHP', __DIR__ . '/php');
define('IMG', __DIR__ . '/images');

session_start();

//práce s uživatelem
function setJmeno($jmeno = '')
{
    if ($jmeno)
        $_SESSION['jmeno'] = $jmeno;
    else
        unset($_SESSION['jmeno']);
}

function isUser(): bool
{
    return isset($_SESSION['jmeno']);
}

function isAdmin(): bool
{
    return $_SESSION['jmeno'] === 'admin';
}

function getName(): string
{
    return $_SESSION['jmeno'];
}

function requireLogin(): void
{
    if (!isUser()) {
        header('Location: /login.php');
        exit;
    }
}