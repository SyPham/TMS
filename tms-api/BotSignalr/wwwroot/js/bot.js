"use strict";
const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://10.4.4.224:106/working-management-hub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
start();
function start() {
    connection.start().then(function () {
        checkServer();
        connection.on('ReceiveCheckAlert', (conId) => {
        });
        connection.on('UserConnected', (conId) => {
        });
        connection.on('UserDisconnected', (conId) => {
        });
    }).catch(function (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    });
}
// Start the connection.
//start();

function checkAlert() {
    connection.invoke("CheckAlert", "10000").catch(function (err) {
        return console.error(err.toString());
    });
}
function checkServer() {
    if (connection.connectionState === "Connected") {
        setInterval(() => {
            console.log('Ping: ', new Date().toLocaleDateString() + ' ' + new Date().toLocaleTimeString());
            checkAlert();
        }, 30000);
    }
}
function myStopFunction() {
    clearInterval(intervalSignalr);
}
//async function start() {
//    try {
//        await connection.start();
//        console.log("bot connected");
//        console.log("State: ", connection.connectionState);
//        checkServer();
       
//    } catch (err) {
//        console.log(err);
//        setTimeout(() => start(), 5000);
//    }
//};
//function registerEvent() {
//    connection.on('ReceiveCheckAlert', (users) => {
//        console.log("ReceiveCheckAlert");

//    });
//    connection.on('UserConnected', (conId) => {
//        console.log("UserConnected");

//    });
//    connection.on('UserDisconnected', (conId) => {
//        console.log("UserDisconnected");

//    });
//}
