<?php
require 'prolog.php';
requireLogin();
include INC . '/def_html.php';
include INC . '/nav.php';

$notes_to_render = $dom_notes;
$selectedCategory = isset($_GET['category']) ? trim(string: (string)$_GET['category']) : '';
$selectedState = isset($_GET['state']) ? trim((string)$_GET['state']) : '';
if ($selectedCategory === 'default') { $selectedCategory = ''; }
if ($selectedState === 'default') { $selectedState = ''; }
?>

<?php

    function renderNote(DOMElement $note): string {
        // Helper pro bezpečné načtení textu vnořeného elementu a escapování do HTML
        $getText = function (string $tagName) use ($note): string {
            $node = $note->getElementsByTagName($tagName)->item(0);
            $text = $node ? trim($node->textContent) : '';
            return htmlspecialchars($text);
        };

        $title = $getText('title');
        $category = $getText('category');
        $description = $getText('description');
        $state = $getText('state');
        $id = $getText('id');

        // Barva štítku dle stavu
        $stateClass = 'bg-slate-100 text-slate-800';
        if ($state === 'Hotové') {
            $stateClass = 'bg-emerald-100 text-emerald-800';
        } elseif ($state === 'Zahájené') {
            $stateClass = 'bg-amber-100 text-amber-800';
        } elseif ($state === 'Nové') {
            $stateClass = 'bg-sky-100 text-sky-800';
        }

        // HTML karta s Tailwind utilitami
        return <<<HTML
        <div id="note-{$id}" data-note-id="{$id}" class="rounded border border-slate-200 bg-white shadow-sm p-4 cursor-pointer mt-2" onclick="window.location.href='/php/note_action.php?note_id={$id}'">
            <h2 class="text-lg font-semibold text-slate-900">{$title}</h2>
            <div class="mt-2 flex flex-wrap items-center gap-2 text-sm">
                <span class="inline-flex items-center rounded bg-slate-100 text-slate-700 px-2 py-1 font-medium">{$category}</span>
                <span class="inline-flex items-center rounded px-2 py-1 font-medium {$stateClass}">{$state}</span>
            </div>
            <p class="mt-3 text-slate-700 leading-relaxed">{$description}</p>
        </div>
        HTML;
    }
?>

<div class="m-4">
    <h1 class="text-3xl">
        Úkoly
    </h1>

    <div class="w-full flex bg-gray-100 rounded mt-2 mb-2 p-2 border gap-4 border-gray-400">
        <select id="kategorie" class="border border-gray-400 px-2 py-2 bg-gray-100 rounded" placeholder="Kategorie" onchange="handleChange(event)">
            <option value="default" <?= $selectedCategory === '' ? 'selected' : '' ?>>Vyberte kategorii</option>
            <option value="Práce" <?= $selectedCategory === 'Práce' ? 'selected' : '' ?>>Práce</option>
            <option value="Škola" <?= $selectedCategory === 'Škola' ? 'selected' : '' ?>>Škola</option>
            <option value="Osobní" <?= $selectedCategory === 'Osobní' ? 'selected' : '' ?>>Osobní</option>
        </select>

        <select id="stav" class="border border-gray-400 px-2 py-2 bg-gray-100 rounded" placeholder="Stav" onchange="handleChange(event)">
            <option value="default" <?= $selectedState === '' ? 'selected' : '' ?>>Vyberte stav</option>
            <option value="Zahájené" <?= $selectedState === 'Zahájené' ? 'selected' : '' ?>>Zahájené</option>
            <option value="Nezahájené" <?= $selectedState === 'Nezahájené' ? 'selected' : '' ?>>Nezahájené</option>
            <option value="Dokončené" <?= $selectedState === 'Dokončené' ? 'selected' : '' ?>>Dokončené</option>
        </select>
    </div>

    <div id="notes" class="w-full mt-2 mb-2">
    <?php 
        // výpis s ohledem na filtry v URL
        $notes = $notes_to_render->getElementsByTagName('note');
        foreach ($notes as $note) {
            $cat = $note->getElementsByTagName('category')->item(0)?->textContent ?? '';
            $st = $note->getElementsByTagName('state')->item(0)?->textContent ?? '';
            if ($selectedCategory !== '' && $cat !== $selectedCategory) { continue; } //skipuj
            if ($selectedState !== '' && $st !== $selectedState) { continue; } //skipuj
            echo renderNote($note); //renderuj
        }
    ?>
    </div>

</div>

<script>
function handleChange() {
    const category = document.getElementById('kategorie').value;
    const state = document.getElementById('stav').value;
    const params = new URLSearchParams();
    if (category && category !== 'default') params.set('category', category);
    if (state && state !== 'default') params.set('state', state);
    const q = params.toString();
    window.location.href = q ? (`/index.php?${q}`) : '/index.php';
}
</script>

<?php
include INC . '/footer.php';
include INC . '/def_html_end.php';