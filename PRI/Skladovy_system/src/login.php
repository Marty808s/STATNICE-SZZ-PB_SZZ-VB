<?php
require 'prolog.php';
include INC . '/def_html.php';
include INC . '/nav.php';
?>

<?php
    $loginUsername = $_POST['login_username'] ?? '';
    $loginPassword = $_POST['login_password'] ?? '';



    # Zpracování ze strany serveru
    $errors = [];
    if ($_SERVER['REQUEST_METHOD'] === 'POST') {
        
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
                <div id="login-fields" class="flex-row">
                    <div class="mb-4">
                        <label for="login-username" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Uživatelské jméno</label>
                        <input type="text" id="login-username" name="login_username" value="<?= htmlspecialchars($loginUsername, ENT_QUOTES, 'UTF-8') ?>" class="w-md bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg py-1 px-4" placeholder="Uživatelské jméno" required />
                    </div>

                    <div class="mb-4">
                        <label for="login-password" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Heslo</label>
                        <input type="password" id="login-password" name="login_password" class="w-md bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg py-1 px-4" placeholder="Heslo" required />
                    </div>
                </div>

            <div class="w-md flex justify-center mr-auto">
                <button type="submit" class="w-full bg-blue-500 hover:bg-blue-700 text-white py-1 px-2 rounded">
                    Přihlaste se
                </button>
            </div>

        </form>
    </div>
    
    
</div>

<?php
include INC . '/footer.php';
include INC . '/def_html_end.php';
