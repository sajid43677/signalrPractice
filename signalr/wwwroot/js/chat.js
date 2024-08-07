"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("group-message-text").hidden = true;
document.getElementById("groupmsg").hidden = true;
document.getElementById("group-message-text").disabled = true;
document.getElementById("groupmsg").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    if (user == "System") { li.textContent = message; }
    else { li.textContent = user + " says: " + message; }
});

document.getElementById("join-group").addEventListener("click", function (event) {
    var user = document.getElementById("user-name").value;
    var groupName = document.getElementById("group-name").value;
    /*connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    connection.invoke("OnConnectedAsync").catch(function (err) {
        return console.error(err.toString());
    });*/
    try {
        connection.invoke("JoinGroup", groupName, user);
        document.getElementById("group-message-text").hidden = false;
        document.getElementById("groupmsg").hidden = false;
        document.getElementById("group-message-text").disabled = false;
        document.getElementById("groupmsg").disabled = false;
    }
    catch (e) {
        console.log(e);
    }
    event.preventDefault();
});

document.getElementById("groupmsg").addEventListener("click", function (event) {
    var user = document.getElementById("user-name").value;
    var message = document.getElementById("group-message-text").value;
    var groupName = document.getElementById("group-name").value;
    connection.invoke("SendMessageToGroup", groupName, user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("leave-group").addEventListener("click", function (event) {
    var user = document.getElementById("user-name").value;
    var groupName = document.getElementById("group-name").value;
    try {
        connection.invoke("LeaveGroup", groupName, user);
        document.getElementById("group-message-text").disabled = true;
        document.getElementById("groupmsg").disabled = true;
        document.getElementById("group-message-text").hidden = true;
        document.getElementById("groupmsg").hidden = true;
    }
    catch (e) {
        console.log(e);
    }
    event.preventDefault();
});