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
    <h1 class="text-2xl font-bold">
        Hello world!
    </h1>
</div>


<?php
include INC . '/footer.php';
include INC . '/def_html_end.php';