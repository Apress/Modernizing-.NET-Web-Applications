let selectedRoomId = null;
let chat = null;

async function enterRoom(roomId) {
    selectedRoomId = roomId;
    document.getElementById("select-room").style.display = "none";
    document.getElementById("chat-ui").style.display = "flex";

    chat = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/ChatHub", {
            headers: { "X-Api-Key": "test-key-1" }
        })
        .configureLogging(signalR.LogLevel.Information)
        .build();

    chat.on("addChatMessage", (user, message) => {
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
    });
    await chat.start();

    await chat.invoke("joinRoom", selectedRoomId);

    const chatMessage = document.getElementById("chat-message");
    chatMessage.removeAttribute("disabled");
    chatMessage.focus();

    const chatButton = document.getElementById("chat-button");
    chatButton.removeAttribute("disabled");

    const chatName = document.getElementById("chat-name");
    chatName.removeAttribute("disabled");
    chatName.value = chat.connectionId;
}

async function sendMessage() {
    const chatMessage = document.getElementById("chat-message");
    if (!chatMessage.value) {
        return;
    }

    await chat.invoke("sendMessage", selectedRoomId, chatMessage.value);
    chatMessage.value = "";
    chatMessage.focus();
}

async function changeName() {
    const chatName = document.getElementById("chat-name");
    await chat.invoke("setName", selectedRoomId, chatName.value);
}

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/Users")
    .build()
connection.on("ProcessMessage", data => {
    document.title = "Users online: " + data;
});
connection.start();
