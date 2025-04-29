<?php
// Налаштування підключення до бази даних
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unityaccess";

// Створення з'єднання
$conn = new mysqli($servername, $username, $password, $dbname);

// Перевірка з'єднання
if ($conn->connect_error) {
    die("Помилка підключення: " . $conn->connect_error);
}

// Отримання даних з POST запиту
$playerId = $_POST['playerId'];
$itemId = $_POST['itemId'];
$quantity = $_POST['quantity'];

// Якщо кількість 0 - видаляємо предмет
if ($quantity <= 0) {
    $sql = "DELETE FROM player_items WHERE player_id = ? AND item_id = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("ii", $playerId, $itemId);
} else {
    // Інакше оновлюємо кількість предметів
    $sql = "UPDATE player_items SET quantity = ? WHERE player_id = ? AND item_id = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("iii", $quantity, $playerId, $itemId);
}

// Виконання запиту та відправка відповіді
if ($stmt->execute()) {
    echo "Success";
} else {
    echo "Error: " . $stmt->error;
}

// Закриття з'єднань
$stmt->close();
$conn->close();
?> 