<?php
require_once __DIR__ . '/../prolog.php';

$noteId = isset($_GET['note_id']) ? (int) $_GET['note_id'] : 0;
if ($noteId <= 0) {
    header('Location: /index.php');
    exit;
}

header('Location: /detail.php?id=' . $noteId);
exit;   
