<?php
$userName = isUser() ? getName() : '';
?>

<div class="w-full flex bg-blue-200 py-8 border border-gray-400">
    <div class="justify-start ml-8">
        <p>Název appky</p>
    </div>

    <?php if (isUser()) : ?>
            <div class="ml-4 bg-blue-300 px-4 rounded">
                <p>Uživatel: <?= htmlspecialchars($userName) ?></p>
            </div>
    <?php endif ?>

    <div class="ml-auto">
        <ul class="flex gap-4 m-0 p-0 mr-8">
            <li>
                <a class="px-3 py-2 rounded hover:bg-blue-300" href="index.php">
                   Úkoly
                </a>

                <a class="px-3 py-2 rounded hover:bg-blue-300" href="#">
                   Item 1
                </a>

                <a class="px-3 py-2 rounded hover:bg-blue-300" href="logout.php">
                   Odhlásit se
                </a>
            </li>
        </ul>
    </div>
</div>