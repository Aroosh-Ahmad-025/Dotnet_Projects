"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var PCount = 1;
//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
connection.on("ReceiveMessage", function (user, message) {
    var li = $("#messagesList");
    var li2 = $("#messagesList2");
   
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    if (PCount == 1) {
        li.append(`<br/>${message}`);
        PCount++;
    }
    else {
        li.append(`<br/>${message}`);
    }
    
    li2.html(`<i class="mdi mdi-check"></i>${message}`);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementsById("UserInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

