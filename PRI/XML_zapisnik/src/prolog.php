<?php

// Adresáře
define('INC', __DIR__ . '/includes');
define('XML', __DIR__ . '/xml_files');
define('PHP', __DIR__ . '/php');
define('IMG', __DIR__ . '/images');

session_start();

# DOM users
libxml_use_internal_errors(true);
$dom_users = new DOMDocument();
$dom_users->load(XML . '/users.xml');

$dom_notes = new DOMDocument();
$dom_notes->load(XML . '/notes.xml');
libxml_clear_errors();

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

function logout(): bool
{
    if (isUser()) {
        setJmeno();
        return true;
    } else{
        return false;
    }
}