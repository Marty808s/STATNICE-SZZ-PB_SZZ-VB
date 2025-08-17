<?php
require 'prolog.php';
requireLogin();

$id = isset($_GET['id']) ? trim((string)$_GET['id']) : '';
$action = isset($_GET['action']) ? trim((string)$_GET['action']) : '';

if ($id === '') {
    header('Location: /index.php');
    exit;
}

$my_note = null;

function findNoteById(DOMDocument $doc, int $noteId): ?DOMElement {
	foreach ($doc->getElementsByTagName('note') as $note) {
		$idEl = $note->getElementsByTagName('id')->item(0);
		if ($idEl && (int) trim($idEl->textContent) === $noteId) {
			return $note;
		}
	}
	return null;
}

$my_note = findNoteById($dom_notes, (int) $id);

function setTagText(DOMDocument $doc, DOMElement $note, string $tag, string $value): void {
    $el = $note->getElementsByTagName($tag)->item(0);
    if (!$el) {
        $el = $doc->createElement($tag);
        $note->appendChild($el);
    }
    while ($el->firstChild) { $el->removeChild($el->firstChild); }
    $el->appendChild($doc->createTextNode($value));
}

// Server-side zpracování akcí (uložit / smazat)
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $submitAction = $_POST['submit_action'] ?? '';
    $noteId = isset($_POST['id']) ? (int) $_POST['id'] : (int) $id;
    $target = findNoteById($dom_notes, $noteId);

    if (!$target) {
        header('Location: /index.php');
        exit;
    }

    if ($submitAction === 'save') {
        $title = trim((string)($_POST['title'] ?? ''));
        $category = trim((string)($_POST['category'] ?? ''));
        $description = trim((string)($_POST['description'] ?? ''));
        $state = trim((string)($_POST['state'] ?? ''));

        $allowedCategories = ['Práce','Škola','Osobní'];
        $allowedStates = ['Zahájené','Nezahájené','Dokončené'];
        if (!in_array($category, $allowedCategories, true)) { $category = 'Osobní'; }
        if (!in_array($state, $allowedStates, true)) { $state = 'Nezahájené'; }

        setTagText($dom_notes, $target, 'title', $title);
        setTagText($dom_notes, $target, 'category', $category);
        setTagText($dom_notes, $target, 'description', $description);
        setTagText($dom_notes, $target, 'state', $state);

        file_put_contents(XML . '/notes.xml', $dom_notes->saveXML(), LOCK_EX);
        header('Location: /index.php');
        exit;
    }

    if ($submitAction === 'delete') {
        $parent = $target->parentNode;
        if ($parent) {
            $parent->removeChild($target);
            file_put_contents(XML . '/notes.xml', $dom_notes->saveXML(), LOCK_EX);
        }
        header('Location: /index.php');
        exit;
    }
}

function getValues(DOMElement $note): array {
    $get = function (string $tag) use ($note): string {
        $n = $note->getElementsByTagName($tag)->item(0);
        return $n ? trim($n->textContent) : '';
    };
    return [
        'id' => $get('id'),
        'title' => $get('title'),
        'category' => $get('category'),
        'description' => $get('description'),
        'state' => $get('state'),
    ];
}


include INC . '/def_html.php';
include INC . '/nav.php';

$note_data = getValues($my_note);

?>

<div class="m-4">
    <h1 class="text-3xl">
        Úkol: <?= htmlspecialchars($note_data['title']) ?>
    </h1>

    <form method="post" id="form" note_id="<?= htmlspecialchars($id) ?>" class="border border-gray-300 w-full mt-4 mb-4 shadow-md p-2 bg-white rounded">
        <input type="hidden" name="id" value="<?= htmlspecialchars($id) ?>">
        <div class="w-full flex flex-col gap-4">
            <?php 
                echo createTextField("title", $note_data);
                echo createDropDown("category", $note_data, 'category');
                echo createTextArea("description", $note_data);
                echo createDropDown("state", $note_data, 'state');
            ?>
        </div>

        <div class="w-full flex justify-start gap-4">
            <button type="submit" name="submit_action" value="save" class="px-4 py-2 w-32 rounded text-white bg-blue-400 border border-blue-700 hover:bg-blue-600">
                Uložit
            </button>

            <button type="submit" name="submit_action" value="delete" class="px-4 py-2 w-32 rounded text-white bg-red-400 border border-red-700 hover:bg-red-600" onclick="return confirm('Opravdu smazat tuto poznámku?');">
                Vymazat
            </button>
        </div>

    </form>
</div>
