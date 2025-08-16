<?php
require 'prolog.php';
requireLogin();
include INC . '/def_html.php';
include INC . '/nav.php';
?>

<?php
    // obsah pro přihlášené uživatele zde...
?>

<div class="m-4">
    <h1 class="text-3xl">
        Úkoly
    </h1>

    <div class="w-full bg-gray-100 rounded mt-2 mb-2 p-2 border border-gray-400">
        <p>Zde budou zobrazeny úkoly</p>

    </div>

</div>


<?php
include INC . '/footer.php';
include INC . '/def_html_end.php';