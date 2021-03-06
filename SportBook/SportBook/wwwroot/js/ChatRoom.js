﻿document.addEventListener('DOMContentLoaded', function () {

    // Get the user name and store it to prepend to messages.
    var username = '@currentUser.Username'; //generateRandomName();

    // Set initial focus to message input box.
    var messageInput = document.getElementById('message');
    messageInput.focus();

    function createMessageEntry(encodedName, encodedMsg) {
        var entry = document.createElement('div');
        entry.classList.add("message-entry");
        entry.classList.add("d-flex");
        entry.classList.add("justify-content-start");
        entry.classList.add("mb-4");
        if (encodedName === "_SYSTEM_") {
            entry.innerHTML = encodedMsg;
            entry.classList.add("text-center");
            entry.classList.add("system-message");
        } else if (encodedName === "_BROADCAST_") {
            entry.classList.add("text-center");
            entry.innerHTML = `<div class="text-center broadcast-message">${encodedMsg}</div>`;
        } else if (encodedName === username) {
            entry.innerHTML = `<div class="message-avatar pull-right" style="font-weight: bold">${encodedName}</div>` +
                `<div class="message-content pull-right">${encodedMsg}<div>`;
        } else {
            entry.innerHTML = `<div class="message-avatar pull-left" style="font-weight: bold">${encodedName}</div>` +
                `<div class="message-content pull-left">${encodedMsg}<div>`;
        }
        return entry;
    }

    function bindConnectionMessage(connection) {
        var messageCallback = function (name, message) {
            if (!message) return;
            // Html encode display name and message.
            var encodedName = name;
            var encodedMsg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            var messageEntry = createMessageEntry(encodedName, encodedMsg);

            var messageBox = document.getElementById('messages');
            messageBox.appendChild(messageEntry);
            messageBox.scrollTop = messageBox.scrollHeight;
        };
        // Create a function that the hub can call to broadcast messages.
        connection.on('sendMessage', messageCallback);
        connection.on('echo', messageCallback);
        connection.onclose(onConnectionError);
    }

    function onConnected(connection) {
        console.log('connection started');
        connection.send('sendMessage', '_SYSTEM_', username + ' JOINED');
        document.getElementById('sendmessage').addEventListener('click', function (event) {
            // Call the sendMessage method on the hub.
            if (messageInput.value) {
                connection.send('sendMessage', username, messageInput.value);
            }

            // Clear text box and reset focus for next comment.
            messageInput.value = '';
            messageInput.focus();
            event.preventDefault();
        });
        document.getElementById('message').addEventListener('keypress', function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                document.getElementById('sendmessage').click();
                return false;
            }
        });
        document.getElementById('echo').addEventListener('click', function (event) {
            // Call the echo method on the hub.
            connection.send('echo', username, messageInput.value);

            // Clear text box and reset focus for next comment.
            messageInput.value = '';
            messageInput.focus();
            event.preventDefault();
        });
    }

    function onConnectionError(error) {
        if (error && error.message) {
            console.error(error.message);
        }
        var modal = document.getElementById('myModal');
        modal.classList.add('in');
        modal.style = 'display: block;';
    }

    var connection = new signalR.HubConnectionBuilder()
        .withUrl('/chat')
        .build();
    bindConnectionMessage(connection);
    connection.start()
        .then(function () {
            onConnected(connection);
        })
        .catch(function (error) {
            console.error(error.message);
        });
    //connection.closed()
    //    .then(function () {
    //        onDisconnectedAsync("Disconnected");
    //    });
});