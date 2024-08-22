let selectedRoomId = null;
let chat = null;

function enterRoom(roomId) {
    selectedRoomId = roomId;
    document.getElementById("select-room").style.display = "none";
    document.getElementById("chat-ui").style.display = "flex";

    chat = $.connection.chatHub;
    chat.client.addChatMessage = function (user, message) {
        const chatArea = document.getElementById("chat-area");

        const userElement = document.createElement("strong");
        userElement.innerText = user;
        chatArea.appendChild(userElement);

        const messageElement = document.createElement("span");
        messageElement.innerText = ": " + message;
        chatArea.appendChild(messageElement);

        const lineBreak = document.createElement("br");
        chatArea.appendChild(lineBreak);

        chatArea.scrollTo(0, chatArea.scrollHeight);
    };
    
    $.connection.hub.qs = { "auth-token": "test-key-1" };
    $.connection.hub.start().done(function () {
        chat.server.joinRoom(selectedRoomId);

        const chatMessage = document.getElementById("chat-message");
        chatMessage.removeAttribute("disabled");
        chatMessage.focus();

        const chatButton = document.getElementById("chat-button");
        chatButton.removeAttribute("disabled");

        const chatName = document.getElementById("chat-name");
        chatName.removeAttribute("disabled");
        chatName.value = chat.connection.id;
    });
}

function sendMessage() {
    const chatMessage = document.getElementById("chat-message");
    if (!chatMessage.value) {
        return;
    }

    chat.server.sendMessage(selectedRoomId, chatMessage.value);
    chatMessage.value = "";
    chatMessage.focus();
}

function changeName() {
    const chatName = document.getElementById("chat-name");
    chat.server.setName(selectedRoomId, chatName.value);
}

$(() => {
    const connection = $.connection('/users');
    connection.received(data => {
        document.title = "Users online: " + data;
    });
    connection.start();
});