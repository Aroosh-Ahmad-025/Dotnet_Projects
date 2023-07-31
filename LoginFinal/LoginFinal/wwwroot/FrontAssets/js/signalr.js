const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then(() => console.log('connected!'))
    .catch(console.error);