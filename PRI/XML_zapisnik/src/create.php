<?php
require 'prolog.php';
requireLogin();
include INC . '/def_html.php';
include INC . '/nav.php';

//FORM INSERT
// Zpracování vytvoření nové noty z formuláře (POST name="form_create")
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['form_create'])) {
    // Přečti hodnoty z formuláře
    $title = trim((string)($_POST['title'] ?? ''));
    $category = trim((string)($_POST['category'] ?? ''));
    $description = trim((string)($_POST['description'] ?? ''));
    $state = trim((string)($_POST['state'] ?? ''));

    // Jednoduchá validace proti XSD výčtům
    $allowedCategories = ['Práce', 'Škola', 'Osobní'];
    $allowedStates = ['Zahájené', 'Nezahájené', 'Dokončené'];
    if (!in_array($category, $allowedCategories, true)) { $category = 'Osobní'; }
    if (!in_array($state, $allowedStates, true)) { $state = 'Nezahájené'; }

    // Najdi kořen
    $root = $dom_notes->getElementsByTagName('notes')->item(0);
    if ($root) {

        $new_id = getNewId($dom_notes);
        // Sestav nový <note>
        $noteEl = $dom_notes->createElement('note');
        $idEl = $dom_notes->createElement('id', $new_id);
        $titleEl = $dom_notes->createElement('title', $title);
        $categoryEl = $dom_notes->createElement('category', $category);
        $descriptionEl = $dom_notes->createElement('description', $description);
        $stateEl = $dom_notes->createElement('state', $state);

        $noteEl->appendChild($idEl);
        $noteEl->appendChild($titleEl);
        $noteEl->appendChild($categoryEl);
        $noteEl->appendChild($descriptionEl);
        $noteEl->appendChild($stateEl);

        $root->appendChild($noteEl);

        // Ulož změny a přesměruj
        file_put_contents(XML . '/notes.xml', $dom_notes->saveXML(), LOCK_EX);
        echo "Note je úspěšně vytvořne!";
        echo "<script>setTimeout(function() { window.location.href = '/index.php'; }, 1000);</script>";
        exit;
    }
}


// XML INSERT
if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_FILES["xmlFile"])) {
    libxml_use_internal_errors(true);
    $targetDirectory = XML . '/upload/';
    if (!is_dir($targetDirectory)) {
        @mkdir($targetDirectory, 0775, true);
    }
    $targetFile = $targetDirectory . basename($_FILES["xmlFile"]["name"]);

    $fileType = strtolower(pathinfo($targetFile, PATHINFO_EXTENSION));
    
    if ($fileType != "xml") {
        echo '<div class="text-red-700">Omlouváme se, jsou povoleny pouze XML soubory.</div>';
        libxml_use_internal_errors(false);
        return;
    }

    if (!move_uploaded_file($_FILES['xmlFile']['tmp_name'], $targetFile)) {
        echo '<div class="text-red-700">Nahrání souboru se nezdařilo.</div>';
        libxml_use_internal_errors(false);
        return;
    }

    $dom_upload = new DOMDocument();

    if (!$dom_upload->load($targetFile)) {
        echo '<div class="text-red-700">Soubor nelze načíst jako XML.</div>';
        foreach (libxml_get_errors() as $err) {
            echo '<div class="text-red-700">' . htmlspecialchars($err->message) . '</div>';
        }
        libxml_use_internal_errors(false);
        return;
    }

    if (!$dom_upload->schemaValidate(XML . '/notes.xsd')) {
        echo '<div class="text-red-700">Soubor neodpovídá schématu notes.xsd.</div>';
        foreach (libxml_get_errors() as $err) {
            echo '<div class="text-red-700">' . htmlspecialchars($err->message) . '</div>';
        }
        libxml_use_internal_errors(false);
        return;
    }
    else {
        echo "<p>Je to ok!</p>";
        // zápis
        $import_notes = $dom_upload -> getElementsByTagName('note');
        $added = 0;
        foreach($import_notes as $note) {
            appendNote($note, $dom_notes);
            $added++;
        }
        // Ulož změny do cílového notes.xml
        file_put_contents(XML . '/notes.xml', $dom_notes->saveXML(), LOCK_EX);
        echo "<div class=\"text-green-700\">Vše vloženo. Přidáno: " . (int)$added . " poznámek.</div>";
        echo "<script>setTimeout(function() { window.location.href = '/index.php'; }, 1000);</script>";
    }

}
    
    libxml_use_internal_errors(false);

   
    // just dummy dict pro funkce - nechci je rozšiřovat
    $my_dummy_dict = [
        'id' => (string) getNewId($dom_notes),
        'title' => 'Zde název',
        'category' => 'default',
        'description' => 'Zde Popis',
        'state' => 'default',
    ]

?>
<div class="m-4">
    <h1 class="text-3xl">
        Vytvoření importem
    </h1>
    <div class="w-full mt-4 mb-4">
        <?php // action= -> odesílá formulář na jiný skript?>
        <form action="/create.php" method="post" class="cursor-pointer" enctype="multipart/form-data">
            <div class="flex flex-row gap-4 mb-3 w-md rounded border border-gray-400 hover:bg-gray-200 py-2 px-2">
                <label class="cursor-pointer" for="xmlFile">Vyberte XML soubor:</label>
                <input type="file" class="ml-auto cursor-pointer" name="xmlFile" id="xmlFile" accept=".xml">
            </div>
            <button type="submit" class="px-4 py-2 border border-gray-400 rounded bg-blue-300 hover:bg-blue-500">
                Validovat a vložit
            </button>
        </form>
    </div>

    <h1 class="text-3xl">
        Vytvoření formulářem
    </h1>

    <form method="post" id="form" note_id="<?= htmlspecialchars($id) ?>" class="border border-gray-300 w-full mt-4 mb-4 shadow-md p-2 bg-white rounded">
        <input type="hidden" name="id" value="<?= htmlspecialchars($id) ?>">
        <div class="w-full flex flex-col gap-4">
            <?php 
                echo createTextField("title", $my_dummy_dict);
                echo createDropDown("category", $my_dummy_dict, 'category');
                echo createTextArea("description", $my_dummy_dict);
                echo createDropDown("state", $my_dummy_dict, 'state');
            ?>
        </div>

        <div class="w-full flex justify-start gap-4">
            <button type="submit" name="form_create" value="save" class="px-4 py-2 w-32 rounded text-white bg-blue-400 border border-blue-700 hover:bg-blue-600">
                Vytvořit
            </button>

        </div>

    </form>


</div>

<?php
libxml_clear_errors();
