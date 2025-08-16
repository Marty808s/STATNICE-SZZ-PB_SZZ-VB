<?php
require 'prolog.php';
include INC . '/def_html.php';
include INC . '/nav.php';
?>

<?php
    $authMode = $_GET['mode'] ?? ($_POST['mode'] ?? 'login');
    $isLoginMode = ($authMode !== 'register');

    $loginUsername = $_POST['login_username'] ?? '';
    $loginPassword = $_POST['login_password'] ?? '';

    $registerUsername = $_POST['register_username'] ?? '';
    $registerPassword = $_POST['register_password'] ?? '';
    $registerPasswordAgain = $_POST['register_password_again'] ?? '';


    # Zpracování ze strany serveru
    $errors = [];
    if ($_SERVER['REQUEST_METHOD'] === 'POST') {
        if ($isLoginMode) {
            if ($loginUsername === '') {
                $errors[] = 'Zadejte uživatelské jméno.';
            }
            if ($loginPassword === '') {
                $errors[] = 'Zadejte heslo.';
            }
            // ověření proti XML: vyhledat uživatele a ověřit hash hesla
            if (empty($errors)) {
                $foundUser = null;
                foreach ($dom_users->getElementsByTagName('user') as $userEl) {
                    $nameEl = $userEl->getElementsByTagName('username')->item(0);
                    if ($nameEl && $nameEl->textContent === $loginUsername) {
                        $foundUser = $userEl;
                        break;
                    }
                }
                if (!$foundUser) {
                    $errors[] = 'Uživatel neexistuje.';
                } else {
                    $hashEl = $foundUser->getElementsByTagName('password')->item(0);
                    $storedHash = $hashEl ? $hashEl->textContent : '';
                    if ($storedHash === '' || !password_verify($loginPassword, $storedHash)) {
                        $errors[] = 'Nesprávné přihlašovací údaje.';
                    } else {
                        setJmeno($loginUsername);
                        echo "<script>setTimeout(function() { window.location.href = '/'; }, 1000);</script>";
                        exit;
                    }
                }
            }
        } else {
            if ($registerUsername === '') {
                $errors[] = 'Zadejte uživatelské jméno.';
            }
            if ($registerPassword === '') {
                $errors[] = 'Zadejte heslo.';
            }
            if ($registerPasswordAgain === '') {
                $errors[] = 'Zadejte kontrolu hesla.';
            }
            if ($registerPassword !== '' && $registerPasswordAgain !== '' && $registerPassword !== $registerPasswordAgain) {
                $errors[] = 'Hesla se neshodují.';
            }
            // registrace do XML: kontrola duplicity, uložení hashe
            if (empty($errors)) {
                $exists = false;
                foreach ($dom_users->getElementsByTagName('user') as $userEl) {
                    $nameEl = $userEl->getElementsByTagName('username')->item(0);
                    if ($nameEl && $nameEl->textContent === $registerUsername) {
                        $exists = true;
                        break;
                    }
                }
                if ($exists) {
                    $errors[] = 'Uživatel již existuje.';
                } else {
                    // vytvoření uzlu <user>
                    $newUser = $dom_users->createElement('user');
                    $usernameEl = $dom_users->createElement('username', $registerUsername);
                    $passwordHash = password_hash($registerPassword, PASSWORD_DEFAULT);
                    $passwordEl = $dom_users->createElement('password', $passwordHash);
                    $createdAtEl = $dom_users->createElement('created_at', date(DATE_ATOM));
                    $newUser->appendChild($usernameEl);
                    $newUser->appendChild($passwordEl);
                    $newUser->appendChild($createdAtEl);
                    // připojit k <users>
                    if ($dom_users->documentElement === null) {
                        $root = $dom_users->createElement('users');
                        $dom_users->appendChild($root);
                    }
                    $dom_users->documentElement->appendChild($newUser);
                    $dom_users->formatOutput = true;
                    $dom_users->save(XML . '/users.xml');

                    setJmeno($registerUsername);
                    echo "<script>setTimeout(function() { window.location.href = '/'; }, 1000);</script>";
                    exit;
                }
            }
        }
    }
?>

<div class="m-4">
    <h1 class="text-2xl font-bold">
        Přihlášení
    </h1>

    <div class="w-full bg-gray-100 rounded mt-2 mb-2 p-2 border border-gray-400">
        <form class="m-4 mt-2" method="post">
            <input type="hidden" name="mode" value="<?= $isLoginMode ? 'login' : 'register' ?>" />

            <?php if (!empty($errors)): ?>
                <div class="bg-red-100 text-red-800 p-2 mb-3 rounded">
                    <?php foreach ($errors as $err): ?>
                        <div><?= htmlspecialchars($err, ENT_QUOTES, 'UTF-8') ?></div>
                    <?php endforeach; ?>
                </div>
            <?php endif; ?>

            <div class="flex gap-2 mb-4">
                <a href="?mode=login" class="px-3 py-1 rounded <?= $isLoginMode ? 'bg-blue-500 text-white' : 'bg-blue-100' ?>">Přihlášení</a>
                <a href="?mode=register" class="px-3 py-1 rounded <?= !$isLoginMode ? 'bg-yellow-500 text-white' : 'bg-yellow-100' ?>">Registrace</a>
            </div>

            <?php if ($isLoginMode): ?>
                <div id="login-fields">
                    <div class="mb-4">
                        <label for="login-username" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Uživatelské jméno</label>
                        <input type="text" id="login-username" name="login_username" value="<?= htmlspecialchars($loginUsername, ENT_QUOTES, 'UTF-8') ?>" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg py-1 px-4" placeholder="Uživatelské jméno" required />
                    </div>

                    <div class="mb-4">
                        <label for="login-password" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Heslo</label>
                        <input type="password" id="login-password" name="login_password" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg py-1 px-4" placeholder="Heslo" required />
                    </div>
                </div>
            <?php else: ?>
                <div id="register-fields">
                    <div class="mb-4">
                        <label for="register-username" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Uživatelské jméno</label>
                        <input type="text" id="register-username" name="register_username" value="<?= htmlspecialchars($registerUsername, ENT_QUOTES, 'UTF-8') ?>" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg py-1 px-4" placeholder="Uživatelské jméno" />
                    </div>

                    <div class="mb-4">
                        <label for="register-password" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Heslo</label>
                        <input type="password" id="register-password" name="register_password" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg py-1 px-4" placeholder="Heslo" />
                    </div>

                    <div class="mb-4">
                        <label for="register-password-again" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Heslo znovu</label>
                        <input type="password" id="register-password-again" name="register_password_again" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg py-1 px-4" placeholder="Heslo znovu" />
                    </div>
                </div>
            <?php endif; ?>

            <div class="w-full flex justify-center ml-auto">
                <button type="submit" class="w-full <?= $isLoginMode ? 'bg-blue-500 hover:bg-blue-700' : 'bg-yellow-500 hover:bg-yellow-700'  ?> text-white py-1 px-2 rounded">
                    <?= $isLoginMode ? 'Přihlásit se' : 'Registrovat' ?>
                </button>
            </div>

        </form>
    </div>
    
    
</div>

<?php
include INC . '/footer.php';
include INC . '/def_html_end.php';
