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

// práce s uživatelem
function setUser(string $jmeno = ''): void
{
    global $dom_users;
    global $users_notes;

    if ($jmeno !== '') {
        $_SESSION['jmeno'] = $jmeno;

        // najdi ID uživatele podle username v users.xml
        $id = findUserIdByUsername($dom_users, $jmeno);
        if ($id !== null) {
            $_SESSION['id'] = $id;
        } else {
            unset($_SESSION['id']);
        }

    } else {
        unset($_SESSION['jmeno']);
        unset($_SESSION['id']);
    }
}

function appendNote(DOMElement $noteFromUpload, DOMDocument $targetDoc): void {
    $root = $targetDoc->getElementsByTagName('notes')->item(0);
    if (!$root) return;

    // klíčové: import do cílového DOM (deep = true)
    $imported = $targetDoc->importNode($noteFromUpload, true);
    $root->appendChild($imported);
}

function findUserIdByUsername(DOMDocument $usersDoc, string $username): ?int
{
    foreach ($usersDoc->getElementsByTagName('user') as $user) {
        $usernameEl = $user->getElementsByTagName('username')->item(0);
        if ($usernameEl && trim($usernameEl->textContent) === $username) {
            $idEl = $user->getElementsByTagName('id')->item(0);
            if ($idEl) {
                return (int) trim($idEl->textContent);
            }
            return null;
        }
    }
    return null;
}

function txt (DOMElement $el, string $tag): string {
    $node = $el->getElementsByTagName($tag)->item(0);
    return $node ? htmlspecialchars(trim($node->textContent)) : '';
};

function createTextField(string $tag, array $data): string {
    $value = htmlspecialchars($data[$tag] ?? '');
    $name = htmlspecialchars($tag ?? '');
    return <<<HTML
        <div class="flex flex-col gap-2 mt-2 mb-2">
            <p>$tag</p>
            <input 
                type="text" 
                name="{$name}"
                placeholder="{$tag}"
                class="w-full rounded border border-gray-400 px-2 py-1"
                value="{$value}"
                placeholder="{$name}"
            >
        <div>
    HTML;
}

function createTextArea(string $tag, array $data): string {
    $value = htmlspecialchars($data[$tag] ?? '');
    $name = htmlspecialchars($tag ?? '');
    return <<<HTML
        <div class="flex flex-col gap-2 mt-2 mb-2">
            <p>$tag</p>
            <textarea 
                name="{$name}"
                placeholder="{$tag}"
                class="w-full rounded border border-gray-400 px-2 py-1"
                rows="4"
            >{$value}</textarea>
        </div>
    HTML;
}

function createDropDown(string $tag, array $data, string $type): string {
    $current = $data[$tag] ?? '';
    $name = htmlspecialchars($tag ?? '');
    $options = [];
    if ($type === 'category') {
        $options = ['Práce', 'Škola', 'Osobní'];
    } elseif ($type === 'state') {
        $options = ['Zahájené', 'Nezahájené', 'Dokončené'];
    }
    $optsHtml = '';
    foreach ($options as $opt) {
        $sel = ($current === $opt) ? ' selected' : '';
        $val = htmlspecialchars($opt);
        $optsHtml .= "<option value=\"{$val}\"{$sel}>{$val}</option>";
    }
    // heredoc přístup
    return <<<HTML
        <div class="flex flex-col gap-2 mt-2 mb-2">
            <p>$tag</p>
            <select 
                name="{$name}"
                class="w-full rounded border border-gray-400 px-2 py-1"
            >
                {$optsHtml}
            </select>
        </div>
    HTML;
}


function getNewId(DOMDocument $dom_notes): int {
    $maxId = 0;
    foreach ($dom_notes->getElementsByTagName('note') as $n) {
        $idEl = $n->getElementsByTagName('id')->item(0);
        if ($idEl) {
            $val = (int) trim($idEl->textContent);
            if ($val > $maxId) { $maxId = $val; }
        }
    }
    $newId = $maxId + 1;
    return $newId;
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
        setUser();
        return true;
    } else{
        return false;
    }
}