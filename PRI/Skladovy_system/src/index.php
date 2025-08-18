<?php
require_once 'prolog.php';

echo "Ahoj!";

if ($conn -> connect_errno) {
    echo "Failed to connect to MySQL: " . $conn -> connect_error;
    exit();
} else {
    echo "Připojení je ok";
}
